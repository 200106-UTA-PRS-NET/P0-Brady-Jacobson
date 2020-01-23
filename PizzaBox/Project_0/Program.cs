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
        public static Pizzas Hawaiian = new Pizzas("Original", "8", 6.00m);
        public static Pizzas BBQ = new Pizzas("Stuffed", "16", 9.49m);
        public static Pizzas American = new Pizzas("Original", "12", 7.00m);
        public static Pizzas Canadian = new Pizzas("Stuffed", "8", 7.50m);
        public static Pizzas Italian = new Pizzas("Thin crust", "8", 7.50m);
        public static Pizzas Rich = new Pizzas("Stuffed", "12", 130.00m);

        public static Dictionary<String, Decimal> priceLibraryS = new Dictionary<string, Decimal>();
        public static Dictionary<String, Decimal> priceLibraryC = new Dictionary<string, Decimal>();
        public static Dictionary<String, Decimal> toppingLibrary = new Dictionary<string, decimal>();


        public static void PizzaOrdering(Users u, Stores s, PizzaDBContext pdb)
        {
            Orders currentOrder = new Orders(s.StoreId, u.UserId, 0, 0.00m);
            //TO DO Make it dictionary to keep track of "premade" vs "Custom"
            //List<Pizzas> pizzaList = new List<Pizzas>();
            Dictionary<int,List<string>> vastToppings = new Dictionary<int, List<string>>();
            Dictionary<int,Pizzas> vastPizzas= new Dictionary<int,Pizzas>();
            int dictCounter = 0;
            var RepoTopping = new Storing.Repositories.RepositoryToppings(pdb);
            var RepoOrder = new Storing.Repositories.RepositoryOrder(pdb);
            var RepoPizza = new Storing.Repositories.RepositoryPizza(pdb);
            var RepoUser = new Storing.Repositories.RepositoryUser(pdb);

            string size;
            string crust;
            bool check = false;
            Console.WriteLine("\n");
            Console.WriteLine("At least one pizza must be chosen before an order can be placed. \n " +
                "No more than 100 pizzas per order can be made. \n" +
                "The price cannot exceed 250 dollars.");
            while (!check)
            {
                Console.WriteLine("\n");
                Console.WriteLine("Type 'Back' to cancel your order. Type 'Preview' to preview your order.");
                Console.WriteLine("Please type 'Preset' to order a preset pizza or type 'Custom' to build your pizza from scratch.'");
                if (currentOrder.PizzaAmount > 0)
                {
                    Console.WriteLine($"Type 'Place order' to submit your order." +
                        $" You have {currentOrder.PizzaAmount} pizzas ordered at a cost of ${currentOrder.Cost}.");
                }

                string choice = Console.ReadLine();

                //TO DO: Add a way to remove pizzas.
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
                        Console.WriteLine("Your order will now be placed.");
                        currentOrder.OrderTime = DateTime.Now;
                        Orders IDOrder = RepoOrder.Addp(currentOrder);
                        if (IDOrder == null)
                        {
                            Console.WriteLine("Something went wrong when placing the order.");
                        }
                        else
                        {
                            for(int up = 0; up < dictCounter; up++)
                            {
                                Console.WriteLine($"Pizza {up}");
                                Pizzas p = vastPizzas[up];
                                p.OrderId = IDOrder.OrderId;
                                Pizzas p2 = RepoPizza.Addp(p);
                                Pizzas newp = RepoPizza.AccessP(p2);
                                List<string> tempTop = vastToppings[up];
                                foreach (string tt in tempTop)
                                {
                                    Toppings t = new Toppings();
                                    t.Topping = tt;
                                    t.PizzaId = newp.PizzaId;
                                    RepoTopping.Addp(t);
                                }
                            }
                        }

                        u.StoreId = s.StoreId;
                        u.StoreTime = currentOrder.OrderTime;
                        RepoUser.Modifyp(u);
                        Console.WriteLine("Pizza order has been placed. User's recent store has been updated. Returning to User menu.");
                        Console.WriteLine("\n");
                        return;
                    }
                }
                else if (choice == "Preset")
                {
                    bool check3 = false;
                    Pizzas current;
                    while (!check3)
                    {
                        Console.WriteLine("Type 'Back' to exit.");
                        Console.WriteLine("Please Type one of the following options for your pizza.\n" +
                            "'Hawaiian', 'BBQ', 'American', 'Italian', 'Rich', and 'Canadian'.");
                        string preset = Console.ReadLine();
                        List<string> pToppings = new List<string>(5);
                        if (preset == "Hawaiian")
                        {
                            current = new Pizzas("Original","8",6.00m);
                            pToppings.Add("Cheddar Cheese");
                            pToppings.Add("Marinara Sauce");
                            pToppings.Add("Pineapple");
                        }
                        else if (preset == "BBQ")
                        {
                            current = new Pizzas("Stuffed", "16", 9.549m);
                            pToppings.Add("Cheddar Cheese");
                            pToppings.Add("BBQ Sauce");
                            pToppings.Add("Bacon");
                            pToppings.Add("Sausage");
                        }
                        else if (preset == "American")
                        {
                            current = new Pizzas("Original", "12", 7.00m);
                            pToppings.Add("Marinara Sauce");
                            pToppings.Add("Mozzarella Cheese");
                            pToppings.Add("Mango");
                        }
                        else if (preset == "Canadian")
                        {
                            current = new Pizzas("Stuffed", "8", 7.50m);
                            pToppings.Add("Olive");
                            pToppings.Add("Mozzarella Cheese");
                            pToppings.Add("Meat Sauce");
                        }
                        else if (preset == "Italian")
                        {
                            current = new Pizzas("Thin crust", "8", 7.50m);
                            pToppings.Add("Cheddar Cheese");
                            pToppings.Add("Mozzarella Cheese");
                        }
                        else if (preset == "Rich")
                        {
                            current = new Pizzas("Stuffed", "12", 130.00m);
                            pToppings.Add("Cheddar Cheese");
                            pToppings.Add("BBQ Sauce");
                            pToppings.Add("Marinara Sauce");
                            pToppings.Add("Mozzarella Cheese");
                            pToppings.Add("Meat Sauce");
                        }
                        else if (preset == "Back")
                        {
                            current = null;
                            check3 = true;
                        }
                        else
                        {
                            Console.WriteLine("The pizza name you provided is not offered here. Please try again.");
                            current = null;
                        }
                        if (current != null)
                        {
                            Console.WriteLine($"Adding {preset} Pizza to order list.");
                            currentOrder.PizzaAmount = currentOrder.PizzaAmount + 1;
                            currentOrder.Cost += current.PizzaCost;
                            //pizzaList.Add(current);
                            vastPizzas.Add(dictCounter, current);
                            vastToppings.Add(dictCounter, pToppings);
                            dictCounter++;
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
                    bool skipCustom = false;
                    while (!check2)
                    {
                        Console.WriteLine("Type 'Back' to exit.");
                        Console.WriteLine("Type one of the following options for your pizza size.");
                        foreach (string Sdict in priceLibraryS.Keys)
                        {
                            Console.WriteLine($"'{Sdict}' at cost {priceLibraryS[Sdict]}");
                        }
                        size = Console.ReadLine();
                        if (size == "Back")
                        {
                            skipCustom = true;
                            check2 = true;
                        }
                        else if (!priceLibraryS.ContainsKey(size))
                            Console.WriteLine("Please input a proper size.");
                        else
                        {
                            price += priceLibraryS[size];
                            check2 = true;
                            Console.WriteLine("\n");
                        }
                    }

                    while (check2)
                    {
                        if (skipCustom != true)
                        {
                            Console.WriteLine("Now please type one of the following crust types for your pizza.");// \n" +
                            foreach (string Cdict in priceLibraryC.Keys)
                            {
                                Console.WriteLine($"'{Cdict}' at cost {priceLibraryC[Cdict]}");
                            }
                            crust = Console.ReadLine();
                            if (crust == "Back")
                            {
                                skipCustom = true;
                                check2 = false;
                            }
                            else if (!priceLibraryC.ContainsKey(crust))
                                Console.WriteLine("Please input a proper crust type");
                            else
                            {
                                price += priceLibraryC[crust];
                                check2 = false;
                                Console.WriteLine("\n");
                            }
                        }
                        else
                        {
                            check2 = false;
                        }
                    }
                    List<string> pToppings = new List<string>(5);
                    while (!check2)
                    {
                        Console.WriteLine("Marinara Sauce and Mozzarella Cheese are the default toppings. (both of these toppings are free).\n" +
                            "If you would like to replace these toppings with up to 5 of your own personal choice, Type 'Yes' now.\n" +
                            "If these two toppings are find Type 'No' to skip the toppings phase.");
                        string toppingPhase = Console.ReadLine();
                        if (toppingPhase == "Yes")
                        {
                            Console.WriteLine("Here is a list of available toppings and prices.");
                            foreach (string top in toppingLibrary.Keys)
                            {
                                Console.WriteLine($"'{top}' costs {toppingLibrary[top]}");
                            }
                            bool loopTopping = false;
                            Console.WriteLine("You must have at least 2 toppings, and no more than 5 toppings.\n" +
                                "Type 'Submit' to add your selected toppings. Type 'Back' to use the default toppings.");
                            while (!loopTopping)
                            {
                                if (pToppings.Count < 5)
                                    Console.WriteLine($"Type the name of topping number {pToppings.Count + 1}.");
                                if (pToppings.Count >= 2)
                                    Console.WriteLine("Type 'Submit' to submit toppings.");
                                string numberTopping = Console.ReadLine();
                                if (numberTopping == "Back")
                                {
                                    pToppings.Clear();
                                    pToppings.Add("Marinara Sauce");
                                    pToppings.Add("Mozzarella Cheese");
                                    loopTopping = true;
                                    check2 = true;
                                }
                                else if (numberTopping == "Submit")
                                {
                                    if (pToppings.Count >= 2 && pToppings.Count <= 5)
                                    {
                                        loopTopping = true;
                                        check2 = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("You do not have the right amount of toppings. There must be at least 2 and no more than 5.");
                                    }
                                }
                                else if (pToppings.Count >= 5)
                                {
                                    Console.WriteLine("You cannot have over 5 toppings. You must type 'Submit' to add the toppings or 'Back' to use the default toppings.");
                                }
                                else if (!toppingLibrary.ContainsKey(numberTopping))
                                    Console.WriteLine("Please input a proper topping");
                                else
                                {
                                    pToppings.Add(numberTopping);
                                }
                            }

                        }
                        else if (toppingPhase == "No")
                        {
                            pToppings.Add("Marinara Sauce");
                            pToppings.Add("Mozzarella Cheese");
                            check2 = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                        }
                    }
                    if (skipCustom != true)
                    {
                        foreach (string t in pToppings)
                        {
                            price += toppingLibrary[t];
                        }
                        Pizzas custom = new Pizzas(crust, size, price);
                        Console.WriteLine($"Adding Custom Pizza to order list.");
                        currentOrder.PizzaAmount++;
                        currentOrder.Cost += custom.PizzaCost;
                       // pizzaList.Add(custom);
                        vastToppings.Add(dictCounter, pToppings);
                        vastPizzas.Add(dictCounter, custom);
                        dictCounter++;
                    }
                }
                else if (choice == "Preview")
                {
                    Console.WriteLine("Preview of all Pizzas in your order.");
                    int counter = 1;
                    foreach (Pizzas p in vastPizzas.Values)
                    {
                        Console.WriteLine($"Pizza {counter}: Size {p.Size}, Crust {p.Crust}, Price {p.PizzaCost}");
                        counter++;
                    }
                }
                else
                {
                    Console.WriteLine("Please type a valid command.");
                }
            }
            return;
        }

        public static int ObtainTime(string type, string example)
        {
            string result = "";
            int result2 = 0;
            bool timeLoop = false;
            while (!timeLoop)
            {
                Console.WriteLine($"Please provide the {type}. Please format it like this: '{example}'");
                result = Console.ReadLine();
                if (result == "Back")
                    return -1;
                try
                {
                    result2 = Convert.ToInt32(result);
                    timeLoop = true;
                }
                catch
                {
                    Console.WriteLine("Invalid Day. Please try again.");
                }
            }
            return result2;
        }
        public static void UserDecision(Users u, PizzaDBContext pdb)
        {
            var RepoStore = new Storing.Repositories.RepositoryStore(pdb);
            bool Check = false;
            while (!Check)
            {
                Console.WriteLine("\n");
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
                        var locations = RepoStore.Getp();
                        foreach (var l in locations)
                        {
                            Console.WriteLine($"'{l.StoreName}' is the name of store {l.StoreId}.");
                        }
                        Console.WriteLine("\n");
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
                                        else
                                        {
                                            Console.WriteLine("Error. If storeID is not null, Storetime should not be null either. But in this instance it is");
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
                    if (u.StoreId != null)
                    {
                        Stores temp = RepoStore.UseIDFindStore((int)u.StoreId);
                        Console.WriteLine($"Your most recent order was from store {temp.StoreName} at {u.StoreTime}");
                    }
                    else
                    {
                        Console.WriteLine("You have not ordered pizza from a store yet. There is no 'most recent' store.");
                    }
                }
                else if (a == "Sign out")
                {
                    Console.WriteLine("Returning now.");
                    return;
                }
                else
                    Console.WriteLine("Please insput a proper phrase, such as History, Recent, Order, or Sign out");
            }
        }
        public static void StoreDecision(Stores s, PizzaDBContext pdb)
        {
            var RepoOrder = new Storing.Repositories.RepositoryOrder(pdb);
            var RepoPizza = new Storing.Repositories.RepositoryPizza(pdb);
            var RepoUser = new Storing.Repositories.RepositoryUser(pdb);

            bool Check = false;
            while (!Check) {
                Console.WriteLine("\n");
                Console.WriteLine($"Type 'History' to view the store's order history, or type 'Sales' to view this stores sales history. \n" +
$"Type 'Inventory' to look at this store's inventory, or type 'Users' to view which users go to this store. \n" +
$"Type 'Sign out' to sign out of account {s.StoreName}.");
                string a = Console.ReadLine();
                if (a == "History")
                {
                    var storeOrders = RepoOrder.Getp(s);
                    List<string> tempList = new List<string>();
                    foreach (var o in storeOrders)
                    {
                        Console.WriteLine($"Order ID = '{o.OrderId}' placed at time {o.OrderTime}");
                        tempList.Add(o.OrderId.ToString());
                    }
                    bool check3 = false;
                    if (storeOrders.Count() == 0)
                    {
                        Console.WriteLine("There is currently no history in regards to orders for this store.");
                        check3 = true;
                    }
                    while (!check3)
                    {

                        Console.WriteLine("Please Type the Order ID of the order you would like to better examine. Type 'Back' to return.");
                        string whatOrder = Console.ReadLine();
                        if (whatOrder == "Back")
                        {
                            check3 = true;
                        }
                        else if (tempList.Contains(whatOrder))
                        {
                            var pizzasInOrder = RepoPizza.Getp(whatOrder);
                            int counter = 1;
                            foreach (var p in pizzasInOrder)
                            {
                                Console.WriteLine($"Pizza {counter}. Crust type {p.Crust}, Pizza size {p.Size}, Price{p.PizzaCost}");
                                counter++;
                            }
                            Console.WriteLine("\n");
                        }
                        else
                        {
                            Console.WriteLine("The Order ID is invalid or was not created in this store.");
                            Console.WriteLine("\n");
                        }
                    }
                }

                else if (a == "Inventory")
                {
                    Console.WriteLine($"Here is the inventory of {s.StoreName}");
                    foreach(string top in toppingLibrary.Keys)
                    {
                        Console.WriteLine($"Topping: {top}");
                    }
                    foreach(string crust in priceLibraryC.Keys)
                    {
                        Console.WriteLine($"Crust: {crust}");
                    }
                    foreach(string size in priceLibraryS.Keys)
                    {
                        Console.WriteLine($"Dough size: {size}");
                    }
                }
                else if (a == "Sales")
                {
                    bool check = false;
                    DateTime compareEarly = new DateTime();
                    int day = 0;
                    int month = 0;
                    int year = 0;
                    int choice = 0;
                    while (!check)
                    {
                        Console.WriteLine("Type 'Month' to select orders by Month, or 'Day' to select orders by Day. Type 'Back' to return to menu.");
                        string timePick = Console.ReadLine();
                        if (timePick == "Day")
                        {
                            choice = 1;
                            check = true;
                        }
                        else if (timePick == "Month")
                        {
                            choice = 2;
                            check = true;
                        }
                        else if (timePick == "Back")
                        {
                            choice = 0;
                            check = true;
                        }
                        else
                        {
                            Console.WriteLine("");
                        }
                    }

                    while (check)
                    {
                        if (choice == 0)
                            check = false;
                        else
                        {
                            year = ObtainTime("Year", "2019");
                            if (year == -1)
                                choice = 0;
                            else if (year > 0 && year <= 2100)
                                check = false;
                            else
                                Console.WriteLine("Invalid Year, please try again.");
                        }
                    }
                    while (!check)
                    {
                        if (choice == 0)
                            check = false;
                        else
                        {
                            month = ObtainTime("Month", "02");
                            if (month == -1)
                                choice = 0;
                            else if (month >= 1 && month <= 12)
                                check = true;
                            else
                                Console.WriteLine("Invalid Month, please try again.");
                        }
                    }

                    if(choice == 2)
                        compareEarly = new DateTime(year, month, 01);
                    else if (choice == 1)
                    {
                        check = true;
                        while (check)
                        {
                            day = ObtainTime("Day", "06");
                            if (day == -1)
                            {
                                choice = 0;
                                day = 1;
                                check = false;
                            }
                            else if (day >= 1 && day <= 28)
                                check = false;
                            else if (day == 29)
                            {
                                if (month != 2)
                                    check = false;
                                else
                                {
                                    if (year % 4 == 0)
                                        check = false;
                                    else
                                        Console.WriteLine("Invalid day. Feburary 29th is only valid on leap years.");
                                }
                            }
                            else if (day == 30)
                            {
                                if (month == 2)
                                    Console.WriteLine("Invalid day. The last day of February is not above the 29th");
                                else
                                    check = false;
                            }
                            else if (day == 31)
                            {
                                if (month == 9 || month == 4 || month == 6 || month == 11 || month == 2)
                                    Console.WriteLine($"Invalid day. {month} does not go to day {day}");
                                else
                                    check = false;
                            }
                            else
                                Console.WriteLine("Invalid Day. Please try again.");
                        }
                        compareEarly = new DateTime(year, month, day);
                    }

                    if (choice != 0)
                    {
                        var storeOrders = RepoOrder.Getp(compareEarly, s, choice);
                        int counter = 1;
                        if (storeOrders.Count() == 0)
                        {
                            Console.WriteLine("\nThere is no sales history at this store.");
                        }
                        else
                        {
                            if (choice == 1)
                                Console.WriteLine($"\nHere are the sales on the day {day}/{month}/{year}.");
                            else if (choice == 2)
                                Console.WriteLine($"\nHere are the sales on the month {month}/{year}.");
                            foreach (var o in storeOrders)
                            {
                                Console.WriteLine($"Sale {counter}. {o.PizzaAmount} pizzas ordered at {o.OrderTime} at a cost of {o.Cost}");
                                counter++;
                            }
                        }
                    }
                }

                else if (a == "Users")
                {
                    var allUsers = RepoUser.Getp(s.StoreId);
                    int counter = 1;
                    Console.WriteLine($"Here are all users who have most recently used the store '{s.StoreName}':");
                    foreach(var u in allUsers)
                    {
                        Console.WriteLine($"User {counter}: {u.UserName} at {u.StoreTime}.");
                        counter++;
                    }
                }
                else if (a == "Sign out")
                {
                    Console.WriteLine("Returning now \n");
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

                        Console.WriteLine("\n");
                        Console.WriteLine("Type 'User' to sign in as a user. Type 'Store' to sign in as a store.");
                        string a2 = Console.ReadLine();
                        if (a2 == "User")
                        {
                            while (!Check)
                            {

                                Console.WriteLine("\n");
                                Console.WriteLine("Type your personal username.");
                                string b = Console.ReadLine();

                                Console.WriteLine("\n");
                                Console.WriteLine("Type your password.");
                                string c = Console.ReadLine();
                                Users temp = new Users();
                                temp.UserName = b;
                                temp.UserCode = c;
                                Users result = RepoUser.AccessP(temp);
                                if (result != null)
                                {

                                    Console.WriteLine("\n");
                                    Check = true;
                                    UserDecision(result, pdb);
                                }
                                else
                                {
                                    Console.WriteLine("Username or Password failed. Type 'Yes' to try again. Type anything else to return.");
                                    string retry = Console.ReadLine();
                                    if (retry != "Yes")
                                        Check = true;
                                }
                            }
                        }
                        else if (a2 == "Store")
                        {
                            while (!Check)
                            {

                                Console.WriteLine("\n");
                                Console.WriteLine("Type your Store's username.");
                                string b = Console.ReadLine();

                                Console.WriteLine("\n");
                                Console.WriteLine("Type your password.");
                                string c = Console.ReadLine();
                                Stores temp = new Stores();
                                temp.StoreName = b;
                                temp.StoreCode = c;
                                Stores result = RepoStore.AccessP(temp);
                                if (result != null)
                                {
                                    Console.WriteLine("\n");
                                    StoreDecision(result, pdb);
                                    Check = true;
                                }
                                else
                                {
                                    Console.WriteLine("Username or Password failed. Type 'Yes' to try again.");
                                    string retry = Console.ReadLine();
                                    if (retry != "Yes")
                                        Check = true;
                                }
                            }
                        }
                        else
                            Console.WriteLine("Invalid input. Please state Store or User.");
                    }
                }
                else if (a == "Register")
                {
                    while (!Check)
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine("Type 'User' to register as a new user. Type 'Store' to register as a new store.");
                        string b = Console.ReadLine();
                        if (b == "User")
                        {
                            Console.WriteLine("\n");
                            string c = "";
                            string d = "";
                            bool Check4 = false;
                            Users tempUser = new Users();
                            while (!Check4)
                            {
                                Console.WriteLine("Type your new username.");
                                c = Console.ReadLine();
                                if((c.Length > 50) || c == null)
                                {
                                    Console.WriteLine("Username invalid. Retype your new username. Make sure it is less than 50 letters.");
                                }
                                else
                                {
                                    Check4 = true;
                                    tempUser.UserName = c;
                                }
                            }
                            while(Check4)
                            {
                                Console.WriteLine("Type your new password.");
                                d = Console.ReadLine();
                                if(d.Length > 50 || d==null)
                                {
                                    Console.WriteLine("Invalid password. Retype your password. Make sure it is 50 letters or less.");
                                }
                                else
                                {
                                    Check4 = false;
                                    tempUser.UserCode = d;
                                }
                            }
                            tempUser = RepoUser.Addp(tempUser);
                            if (tempUser != null)
                            {
                                UserDecision(tempUser, pdb);
                                Check = true;
                            }
                            else
                            {
                                Console.WriteLine("The provided username already exists in the database. Please provide an unused username.\n" +
                                    "Type 'Yes' to retry registration. Type anything else to return.");
                                string final = Console.ReadLine();
                                if(final != "Yes")
                                {
                                    Check = true;
                                }
                            }
                        }
                        else if (b == "Store")
                        {
                            Console.WriteLine("\n");
                            string c = "";
                            string d = "";
                            Stores tempStore = new Stores();
                            bool Check5 = false; 
                            while (!Check5)
                            {
                                Console.WriteLine("Type your new store's username.");
                                c = Console.ReadLine();
                                if(c.Length > 50 || c == null)
                                {
                                    Console.WriteLine("Name will not work. Please retype your username. Make sure it is 50 letters or less.");
                                }
                                else
                                {
                                    Check5 = true;
                                    tempStore.StoreName = c;
                                }
                            }
                            while (Check5)
                            {
                                Console.WriteLine("Type your new store's password.");
                                d = Console.ReadLine();
                                if(d.Length > 50 || d == null)
                                {
                                    Console.WriteLine("Invalid password. Please retype password. Make sure it is 50 letters or less.");
                                }
                                else
                                {
                                    Check5 = false;
                                    tempStore.StoreCode = d;
                                }
                            }
                            tempStore = RepoStore.Addp(tempStore);
                            if(tempStore != null)
                            {
                                StoreDecision(tempStore, pdb);
                                Check = true;
                            }
                            else
                            {
                                Console.WriteLine("The provided storename already exists in the database. Please provide an unused storename.\n" +
                                    "Type 'Yes' to retry registration. Type anything else to return.");
                                string final = Console.ReadLine();
                                if (final != "Yes")
                                {
                                    Check = true;
                                }
                            }
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
            priceLibraryC.Add("Thin crust",1.99m);
            priceLibraryC.Add("Original", 1.00m);
            priceLibraryC.Add("Stuffed", 2.50m);
            priceLibraryS.Add("8", 5.00m);
            priceLibraryS.Add("12", 6.00m);
            priceLibraryS.Add("16", 6.99m);

            toppingLibrary.Add("Marinara Sauce", 0.0m);
            toppingLibrary.Add("Mozzarella Cheese", 0.0m);
            toppingLibrary.Add("Meat Sauce", 1.0m);
            toppingLibrary.Add("Cheddar Cheese", 0.50m);
            toppingLibrary.Add("BBQ Sauce", 0.50m);

            toppingLibrary.Add("Sausage", 0.50m);
            toppingLibrary.Add("Pepperoni", 0.45m);
            toppingLibrary.Add("Bacon", 0.99m);
            toppingLibrary.Add("Olive", 239.99m);
            toppingLibrary.Add("Pineapple", 1.00m);

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
            while (!check)
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
