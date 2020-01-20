using System;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Domain.Models;
using System.Linq;
using Storing.Repositories;
using System.Collections.Generic;

namespace Project_0
{

    class Program
    {
        public static  Pizzas Hawaiian = new Pizzas("Original","8",6.00m);
        public static Pizzas BBQ = new Pizzas("Stuffed", "16", 9.49m);
        public static Pizzas American = new Pizzas("Original", "12", 7.00m);
        public static Pizzas Canadian = new Pizzas("Stuffed", "8", 7.50m);
        public static Pizzas Italian = new Pizzas("Thin crust", "8", 7.50m);
        public static Pizzas Rich = new Pizzas("Stuffed", "12", 130.00m);

        public static Dictionary<String, Decimal> priceLibrary = new Dictionary<string, Decimal>();

        
        public static void PizzaOrdering(Users u, Stores s, PizzaDBContext pdb)
        {
            Orders currentOrder = new Orders(s.StoreId, u.UserId, 0, 0.00m);
            //TO DO Make it dictionary to keep track of "premade" vs "Custom"
            List<Pizzas> pizzaList = new List<Pizzas>();

            var RepoOrder = new Storing.Repositories.RepositoryOrder(pdb);
            var RepoPizza = new Storing.Repositories.RepositoryPizza(pdb);
            var RepoUser = new Storing.Repositories.RepositoryUser(pdb);

            string size;
            string crust;
            bool check = false;
            Console.WriteLine("At least one pizza must be chosen before an order can be placed. \n " +
                "No more than 100 pizzas per order can be made. \n" +
                "The price cannot exceed 250 dollars.");
            while (!check)
            {
                Console.WriteLine("Type 'Back' to cancel your order.");
                Console.WriteLine("Please type 'Preset' to order a preset pizza or type 'Custom' to build your pizza from scratch.'");
                if (currentOrder.PizzaAmount > 0)
                {
                    Console.WriteLine($"You have {currentOrder.PizzaAmount} pizzas ordered at a cost of ${currentOrder.Cost}.");
                    Console.WriteLine("Type 'Place order' to submit your order.");
                }
                string choice = Console.ReadLine();

                //TO DO: Add a way to remove pizzas/ view all pizzas.
                if (choice == "Back")
                    return;

                else if (choice == "Place order")
                {
                    if (currentOrder.PizzaAmount < 0)
                        Console.WriteLine("At least one pizza must be chosen before an order can be placed.");
                    else if (currentOrder.PizzaAmount > 100)
                    {
                        Console.WriteLine($"No more than 100 Pizzas per order can be made. You have {currentOrder.PizzaAmount} pizzas lined up.");
                    }
                    else if (currentOrder.Cost > 250.00m)
                    {
                        Console.WriteLine($"The cost of the order cannot exceed 250 dollars. It currently costs ${currentOrder.Cost}.");
                    }
                    else
                    {
                        //To Do: Preview order here.
                        //To Do Toppings as well.
                        Console.WriteLine("Your order will now be placed.");
                        currentOrder.OrderTime = DateTime.Now;
                        Orders IDOrder = RepoOrder.Addp(currentOrder);
                        if(IDOrder == null)
                        {
                            Console.WriteLine("Something went wrong.");
                        }
                        else
                        {
                            foreach (Pizzas p in pizzaList)
                            {
                                p.OrderId = IDOrder.OrderId;
                                RepoPizza.Addp(p);
                            }
                        }
                        u.StoreId = s.StoreId;
                        u.StoreTime = currentOrder.OrderTime;
                        RepoUser.Modifyp(u);
                        Console.WriteLine("Order Placed. Returning to User menu.");
                        return;
                    }
                }
                else if (choice == "Preset")
                {
                    bool check3 = false;
                    Pizzas current;
                    while (!check3)
                    {
                        Console.WriteLine("Please Type one of the following options for your pizza.\n" +
                            "'Hawaiian', 'BBQ', 'American', 'Italian', 'Rich', and 'Canadian'.");
                        string preset = Console.ReadLine();

                        if (preset == "Hawaiian")
                            current = Hawaiian;
                        else if (preset == "BBQ")
                            current = BBQ;
                        else if (preset == "American")
                            current = American;
                        else if (preset == "Canadian")
                            current = Canadian;
                        else if (preset == "Italian")
                            current = Italian;
                        else if (preset == "Rich")
                            current = Rich;
                        else
                            current = null;
                        if (current != null)
                        {
                            Console.WriteLine($"Adding {preset} Pizza to order list.");
                            currentOrder.PizzaAmount = currentOrder.PizzaAmount+1;
                            currentOrder.Cost += current.PizzaCost;
                            pizzaList.Add(current);
                            check3 = true;
                        }
                    }
                }
                else if (choice == "Custom")
                {
                    bool check2 = false;
                    decimal price = 0m;
                    crust = "";
                    size = "";
                    while (!check2)
                    {
                        Console.WriteLine("Type one of the following options for your pizza size.\n" +
                    "'8', '12', or '16'");
                        size = Console.ReadLine();
                        if (!priceLibrary.ContainsKey(size))
                            Console.WriteLine("Please input a proper size.");
                        else
                        {
                            price += priceLibrary[size];
                            check2 = true;
                        }
                    }

                    while (check2)
                    {
                        Console.WriteLine("Now please type one of the following crust types for your pizza. \n" +
                            "'Original', 'Thin crust', or 'Stuffed'");
                        crust = Console.ReadLine();
                        if (!priceLibrary.ContainsKey(crust))
                            Console.WriteLine("Please input a proper crust type");
                        else
                        {
                            price += priceLibrary[crust];
                            check2 = false;
                        }
                    }
                    Pizzas custom = new Pizzas(crust, size, price);
                    Console.WriteLine($"Adding Custom Pizza to order list.");
                    currentOrder.PizzaAmount++;
                    currentOrder.Cost += custom.PizzaCost;
                    pizzaList.Add(custom);
                }
                else
                {
                    Console.WriteLine("Please type a valid command.");
                }
            }
            return;
        }

        public static void UserDecision(Users u, PizzaDBContext pdb)
        {
            bool Check = false;
            while (!Check)
            {
                Console.WriteLine($"Type 'History' to view the users's order history, or type 'Recent' to view your most recent pizza store. \n" +
$"Type 'Order' to view locations and place an order and 'Sign out' to sign out of account {u.UserName}.");
                string a = Console.ReadLine();
                if (a == "Order")
                {
                    //Checks to ensure
                    bool approve = false;
                    if (u.StoreTime != null)
                    {
                        DateTime moment = DateTime.Now;
                        TimeSpan TimeDifference = moment - (DateTime)u.StoreTime;
                        if(TimeDifference.TotalMinutes > 120)
                        {
                            approve = true;
                        }
                    }
                    else
                        approve = true;

                    if (approve == true)
                    {
                        var RepoStore = new Storing.Repositories.RepositoryStore(pdb);
                        var locations = RepoStore.Getp();
                        foreach (var l in locations)
                        {
                            Console.WriteLine($"'{l.StoreName}' is the name of store {l.StoreId}.");
                        }
                        bool Check2 = false;
                        while (!Check2)
                        {
                            Console.WriteLine("Type the name of the store you wish to order from. Type 'Back' to return to the user menu.");
                            string z = Console.ReadLine();
                            if (z == "Back")
                            {
                                Check2 = true;
                            }
                            else
                            {
                                Stores y = RepoStore.Findp(z);
                                if (y == null)
                                {
                                    Console.WriteLine("Could not find provided name of location, make sure that it is spelled correctly.");
                                }
                                else
                                {
                                    if(u.StoreId == null || y.StoreId == u.StoreId)
                                    {
                                        PizzaOrdering(u, y, pdb);
                                        Check2 = true;
                                    }
                                    else if(y.StoreId != u.StoreId)
                                    {
                                        if (u.StoreTime != null)
                                        {
                                            DateTime moment = DateTime.Now;
                                            TimeSpan TimeDifference = moment - (DateTime)u.StoreTime;
                                            if (TimeDifference.TotalMinutes > 1440)
                                            {
                                                PizzaOrdering(u, y, pdb);
                                                Check2 = true;
                                            }
                                            else
                                            {
                                                Console.WriteLine($"You cannot order from another store until 24 hours have passed since ordering from the previous.");
                                            }
                                        }
                                    }
                                }
                            }
                        } 
                    }
                    else
                    {
                        Console.WriteLine($"Your most recent order was within 2 hours ago, at {u.StoreTime}. You must wait until it's been 2 hours.");
                    }
                }
                else if (a == "History")
                {
                    var RepoOrder = new Storing.Repositories.RepositoryOrder(pdb);
                    var userOrders = RepoOrder.Getp(u);
                    foreach (var o in userOrders)
                    {
                        Console.WriteLine($"Order {o.OrderId}. {o.PizzaAmount} pizzas ordered at {o.OrderTime} at a cost of {o.Cost}");
                    }
                }
                else if (a == "Recent")
                {
                    Console.WriteLine("Can't do inventory yet...");
                }
                else if (a == "Sign out")
                {
                    Console.WriteLine("Returning now");
                    return;
                }
                else
                    Console.WriteLine("Please input a proper phrase, such as History, Recent, Order, or Sign out");
            }
        }
        public static void StoreDecision(Stores s, PizzaDBContext pdb)
        {
            Console.WriteLine($"Type 'History' to view the store's order history, or type 'Sales' to view this stores sales history. \n" +
    $"Type 'Inventory' to look at this store's inventory, or type 'Users' to view which users go to this store. \n" +
    $"Type 'Sign out' to sign out of account {s.StoreName}.");
            bool Check = false;
            while (!Check) { 
                string a = Console.ReadLine();
                if (a == "History")
                {
                    var RepoOrder = new Storing.Repositories.RepositoryOrder(pdb);
                    var storeOrders = RepoOrder.Getp(s);
                    foreach (var o in storeOrders)
                    {
                        Console.WriteLine($"Order {o.OrderId}. {o.PizzaAmount} pizzas ordered at {o.OrderTime} at a cost of {o.Cost}");
                    }
                }
                else if (a == "Inventory")
                {
                    Console.WriteLine("Can't do inventory yet...");
                }
                else if (a == "Sales")
                {
                    Console.WriteLine("Can't do Sales yet...");
                }
                else if (a == "Users")
                {
                    Console.WriteLine("Can't do users yet...");
                }
                else if (a == "Sign out")
                {
                    Console.WriteLine("Returning now");
                    return;
                }
                else
                    Console.WriteLine("Please input a proper phrase, such as History, Sales, Users, Inventory, or Sign out");
            }
        }

        public static void LogIn(PizzaDBContext pdb)
        {
            var RepoStore = new Storing.Repositories.RepositoryStore(pdb);
            var RepoUser = new Storing.Repositories.RepositoryUser(pdb);
            bool Check = false;
            while (!Check)
            {
                Console.WriteLine("Type 'Sign in' to sign into an existing account. Type 'Register' to register a new user or store.");
                String a = Console.ReadLine();
                if (a == "Sign in")
                {
                    while (!Check)
                    {
                        Console.WriteLine("Type 'User' to sign in as a user. Type 'Store' to sign in as a store.");
                        string a2 = Console.ReadLine();
                        if (a2 == "User")
                        {
                            while (!Check)
                            {
                                Console.WriteLine("Type your personal username.");
                                string b = Console.ReadLine();
                                Console.WriteLine("Type your password.");
                                string c = Console.ReadLine();
                                Users temp = new Users();
                                temp.UserName = b;
                                temp.UserCode = c;
                                Users result = RepoUser.AccessP(temp);
                                if (result != null)
                                {
                                    Check = true;
                                    UserDecision(result, pdb);
                                }
                            }
                        }
                        else if (a2 == "Store")
                        {
                            while (!Check)
                            {
                                Console.WriteLine("Type your Store's username.");
                                string b = Console.ReadLine();
                                Console.WriteLine("Type your password.");
                                string c = Console.ReadLine();
                                Stores temp = new Stores();
                                temp.StoreName = b;
                                temp.StoreCode = c;
                                Stores result = RepoStore.AccessP(temp);
                                if (result != null)
                                {
                                    StoreDecision(result, pdb);
                                    Check = true;
                                }
                            }
                        }
                        else
                            Console.WriteLine("Please state Store or User.");
                    }
                }
                else if (a == "Register")
                {
                    while (!Check)
                    {
                        Console.WriteLine("Type 'User' to register as a new user. Type 'Store' to register as a new store.");
                        string b = Console.ReadLine();
                        if (b == "User")
                        {
                            Users temp = new Users();
                            temp = RepoUser.Addp(temp);
                            UserDecision(temp, pdb);
                            Check = true;
                        }
                        else if (b == "Store")
                        {
                            Stores temp = new Stores();
                            temp = RepoStore.Addp(temp);
                            StoreDecision(temp,pdb);
                            Check = true;
                        }
                        else
                            Console.WriteLine("Please input either 'User' or 'Store'.");
                    }
                }
                else
                    Console.WriteLine("Please input either 'Sign in' or 'Register'.");
            }
        }

        static void Main(string[] args)
        {
            priceLibrary.Add("Thin crust",1.99m);
            priceLibrary.Add("Original", 1.00m);
            priceLibrary.Add("Stuffed", 2.50m);
            priceLibrary.Add("8", 5.00m);
            priceLibrary.Add("12", 6.00m);
            priceLibrary.Add("16", 6.99m);

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true,
                reloadOnChange: true);
            IConfigurationRoot configuration = configBuilder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<PizzaDBContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("PizzaDb"));
            var options = optionsBuilder.Options;
            PizzaDBContext pdb = new PizzaDBContext(options);
            bool check = false;
            while(!check)
                LogIn(pdb);
        }
           
          
            //MY PROJECT, BASICALLY EVERYTHING IS UP TO ME ... 
            //Give user the chance to Log In or Register. Only proceed if they eventually sign in. Sign in as a user or store.
            //Back space or Sign out button is always offered if Users or Stores change their minds.

            //Use Stacks (First in, Last out) to represent Order History. Maybe have Two sales lists to store orders by month and day.

            //Users are allowed to sign out, View order History, and Locate stores. (Pizza requires a store)
            //If users look for location, offer list of stores.(Eventually offer search or by state, but for now list is fine).
            //If user selects store, check history to see if they used another store within 24 hours.

            //If they have not used another store, they are given a chance to make an order. Offer a list of preset pizza's or a custom option.
            //If preset is selected, list of toppings and accessories are used to create this order. 
            //Users are allowed to alternate crust and size at this point. (Possibly toppings if time permits)
            //If user chooses custom, crust and size are determined by users (Required fields to progress). Maybe Topping (Cheese/Sauce default)

            //After a preset or custom pizza is picked, the cost is calculated and recorded.
            //Offer user a chance to order more pizzas (Eventually offer a quantity option). Also a chance to preview order(all pizzas w/ toppings).

            //Once an order is submitted/confirmed, if cost exceeds 250, alert user. If amount of pizza ever exceeds 100, alert user.
            //If order is satisfactory, and no others within 2 hours, add orders to store and user order history.
            //If first order in 24 hours, set location as mandatory store for 24 hours. 
            
            //If user selects History, we see list/stack of all previous orders. The top one is most recent.    

            //Stores are allowed to sign out, view their order history, their sales history, their inventory, and their userbase.
            //If select sales, stores can choose to view by month or day. 

            //If user or store decides to log out, go back to first screen and remove all unnecessary data. Require log-in/registration.
    }
}
