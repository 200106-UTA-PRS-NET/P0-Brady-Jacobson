//Brady Jacobson
//P0
//Pizza Box

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
        public static List<String> presetList = new List<String>();
        public static Dictionary<String, Decimal> priceLibraryS = new Dictionary<string, Decimal>();
        public static Dictionary<String, Decimal> priceLibraryC = new Dictionary<string, Decimal>();
        public static Dictionary<String, Decimal> toppingLibrary = new Dictionary<string, decimal>();


        //Allows a user to order a selection of pizza from a store 
        public static void PizzaOrdering(Users u, Stores s, PizzaDBContext pdb)
        {
            //Creates dictionaries to keep track of the order's parts before being pushed to the database.
            Orders currentOrder = new Orders(s.StoreId, u.UserId, 0, 0.00m);
            Dictionary<int,List<string>> vastToppings = new Dictionary<int, List<string>>();
            Dictionary<int,Pizzas> vastPizzas= new Dictionary<int,Pizzas>();
            int dictCounter = 0;

            var RepoTopping = new Storing.Repositories.RepositoryToppings(pdb);
            var RepoOrder = new Storing.Repositories.RepositoryOrder(pdb);
            var RepoPizza = new Storing.Repositories.RepositoryPizza(pdb);
            var RepoUser = new Storing.Repositories.RepositoryUser(pdb);

            string size;
            string crust;
            
            //The player is allowd to choose which action to perform regarding their order.
            bool check = false;
            Console.WriteLine("\nAt least one pizza must be chosen before an order can be placed. \n " +
                "No more than 100 pizzas per order can be made. \n" +
                "The price cannot exceed 250 dollars.");
            while (!check)
            {
                Console.WriteLine("\nType 'Back' to cancel your order. \nType 'Preview' to preview your order.");
                Console.WriteLine("Type 'Preset' to order a preset pizza. \nType 'Custom' to build your pizza from scratch.'");
                if (currentOrder.PizzaAmount > 0)
                {
                    Console.WriteLine($"Type 'Place order' to submit your order." +
                        $" You have {currentOrder.PizzaAmount} pizzas ordered at a cost of ${currentOrder.Cost}.");
                }
                string choice = Console.ReadLine();
                
                //Returns without placing an order.
                if (choice == "Back")
                    return;

                //Places the order after making sure the parameters are met.
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
                        //The unique order ID is created when an order record is added, which is used to create each pizza record.
                        Console.WriteLine("Your order will now be placed.");
                        currentOrder.OrderTime = DateTime.Now;
                        Orders IDOrder = RepoOrder.Addp(currentOrder);
                        if (IDOrder == null)
                        {
                            Console.WriteLine("\nSomething went wrong when placing the order.\n");
                        }
                        else
                        {
                            for(int up = 0; up < dictCounter; up++)
                            {
                                Pizzas p = vastPizzas[up];
                                p.OrderId = IDOrder.OrderId;
                                Pizzas p2 = RepoPizza.Addp(p);
                                Pizzas newp = RepoPizza.AccessP(p2);
                                List<string> tempTop = vastToppings[up];
                                //The toppings also use the new pizzaID to create the topping records.
                                foreach (string tt in tempTop)
                                {
                                    Toppings t = new Toppings();
                                    t.Topping = tt;
                                    t.PizzaId = newp.PizzaId;
                                    RepoTopping.Addp(t);
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
                }

                //The user will select a premade pizza to add to the order.
                else if (choice == "Preset")
                {
                    bool check3 = false;
                    Pizzas current;
                    List<string> pToppings = new List<string>(5);
                    while (!check3)
                    {
                        Console.WriteLine("\nType 'Back' to exit.");
                        Console.WriteLine("Please Type one of the following options for your pizza.");
                        foreach(String p in presetList)
                        {
                            Console.WriteLine($"'{p}'");
                        }
                        string preset = Console.ReadLine();
                        if(!presetList.Contains(preset))
                        {
                            if (preset == "Back")
                            {
                                current = null;
                                check3 = true;
                            }
                            else
                            {
                                Console.WriteLine("The pizza name you provided is not offered here. Please try again.");
                                current = null;
                            }
                        }
                        else if (preset == "Hawaiian")
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
                        else
                        {
                            Console.WriteLine("Error: The pizza name you provided is not offered here. Please try again.");
                            current = null;
                        }

                        if (current != null)
                        {
                            if (currentOrder.Cost + current.PizzaCost > 250m)
                                Console.WriteLine($"This pizza cannot be added. The cost, {currentOrder.Cost + current.PizzaCost}, exceeds the limit of $250.00.");
                            else if (currentOrder.PizzaAmount + 1 > 100)
                                Console.WriteLine($"This pizza cannot be added. The amount, {currentOrder.PizzaAmount + 1}, exceeds the limit of 100.");                          
                            else 
                            {
                                Console.WriteLine($"Adding {preset} Pizza to order list.");
                                currentOrder.PizzaAmount = currentOrder.PizzaAmount + 1;
                                currentOrder.Cost += current.PizzaCost;
                                vastPizzas.Add(dictCounter, current);
                                vastToppings.Add(dictCounter, pToppings);
                                dictCounter++;
                                check3 = true;
                            }
                        }
                    }
                }

                //The user will create their own pizza with unqiue toppings.
                else if (choice == "Custom")
                {
                    bool check2 = false;
                    decimal price = 0m;
                    crust = "";
                    size = "";
                    bool skipCustom = false;
                    while (!check2)
                    {
                        Console.WriteLine("\nType 'Back' to exit.");
                        Console.WriteLine("\nHere are the different sizes of pizza.");
                        foreach (string Sdict in priceLibraryS.Keys)
                            Console.WriteLine($"'{Sdict}' at cost {priceLibraryS[Sdict]}");
                        Console.WriteLine("\nType the desired size of pizza.");
                        size = Console.ReadLine();
                        if (size == "Back")
                        {
                            skipCustom = true;
                            check2 = true;
                        }
                        else if (!priceLibraryS.ContainsKey(size))
                            Console.WriteLine("Type a proper size of pizza.");
                        else
                        {
                            price += priceLibraryS[size];
                            check2 = true;
                        }
                    }

                    while (check2)
                    {
                        if (skipCustom != true)
                        {
                            Console.WriteLine("\nHere are the different types of crusts.");// \n" +
                            foreach (string Cdict in priceLibraryC.Keys)
                                Console.WriteLine($"'{Cdict}' at cost {priceLibraryC[Cdict]}");
                            Console.WriteLine("\nType the name of the crust you want.");
                            crust = Console.ReadLine();
                            if (crust == "Back")
                            {
                                skipCustom = true;
                                check2 = false;
                            }
                            else if (!priceLibraryC.ContainsKey(crust))
                                Console.WriteLine("Type a proper crust name.");
                            else
                            {
                                price += priceLibraryC[crust];
                                check2 = false;
                            }
                        }
                        else
                            check2 = false;
                    }
                    List<string> pToppings = new List<string>(5);
                    while (!check2)
                    {
                        Console.WriteLine("\nMarinara Sauce and Mozzarella Cheese are the default toppings. (both of these toppings are free).\n" +
                            "Type 'Yes' to replace these toppings with up to 5 of your own personal choice.\n" +
                            "Type 'No' to use the default toppings.");
                        string toppingPhase = Console.ReadLine();
                        if (toppingPhase == "Yes")
                        {
                            Console.WriteLine("\nHere is a list of available toppings and prices.");
                            foreach (string top in toppingLibrary.Keys)
                            {
                                Console.WriteLine($"'{top}' costs {toppingLibrary[top]}");
                            }
                            bool loopTopping = false;
                            Console.WriteLine("You must have at least 2 toppings, and no more than 5 toppings.\n" +
                                "Type 'Submit' to add your selected toppings. \nType 'Back' to use the default toppings.");
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
                                    Console.WriteLine("You cannot have over 5 toppings.\nType'Submit' to add the toppings. \nType 'Back' to use the default toppings.");
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
                            price += toppingLibrary[t];
                        Pizzas custom = new Pizzas(crust, size, price);
                        if (currentOrder.Cost + custom.PizzaCost > 250m)
                            Console.WriteLine($"\nThis pizza cannot be added. The cost, {currentOrder.Cost + custom.PizzaCost}, exceeds the limit of $250.00.");
                        else if (currentOrder.PizzaAmount + 1 > 100)
                            Console.WriteLine($"\nThis pizza cannot be added. The amount, {currentOrder.PizzaAmount + 1}, exceeds the limit of 100.");
                        else
                        {
                            Console.WriteLine($"\nAdding Custom Pizza to order list.");
                            currentOrder.PizzaAmount=currentOrder.PizzaAmount+1;
                            currentOrder.Cost = custom.PizzaCost + currentOrder.Cost;
                            vastToppings.Add(dictCounter, pToppings);
                            vastPizzas.Add(dictCounter, custom);
                            dictCounter++;
                        }
                    }
                }

                //We preview the completed pizzas before placing our order.
                else if (choice == "Preview")
                {
                    Console.WriteLine("\nPreview of all Pizzas in your order.");
                    for (int i = 0; i < dictCounter; i++)
                    {
                        Console.WriteLine($"    Pizza {i + 1} = Size: {vastPizzas[i].Size}, Crust: {vastPizzas[i].Crust}, Price: {vastPizzas[i].PizzaCost}");
                        string cumulative = "";
                        foreach (string t in vastToppings[i])
                        {
                            if (cumulative == "")
                                cumulative = $"{t}";
                            else
                                cumulative = cumulative + $", {t}";
                        }
                        Console.WriteLine($"Toppings include: {cumulative}.");
                    }
                }

                else
                    Console.WriteLine("Type a valid command.");
            }
            return;
        }

        //FindHistory will take an order and print out every pizza, including toppings, size, crust, and cost.
        public static bool FindHistory(List<String> templist, PizzaDBContext pdb )
        {
            var RepoPizza = new Storing.Repositories.RepositoryPizza(pdb);
            var RepoTopping = new Storing.Repositories.RepositoryToppings(pdb);
            Console.WriteLine("\nType the Order ID of the order you would like to better examine.\nType 'Back' to return.");
            string whatOrder = Console.ReadLine();
            if (whatOrder == "Back")
                return true;
            else if (templist.Contains(whatOrder))
            {
                var pizzasInOrder = RepoPizza.Getp(whatOrder);
                List<Pizzas> ongoingPizzas = new List<Pizzas>();
                foreach (var p in pizzasInOrder)
                {
                    ongoingPizzas.Add(p);
                }
                for (int i = 0; i < ongoingPizzas.Count(); i++)
                {
                    Console.WriteLine($"    Pizza {i + 1} = Size: {ongoingPizzas[i].Size}, Crust: {ongoingPizzas[i].Crust}, Price: {ongoingPizzas[i].PizzaCost}");
                    var toppingsInPizza = RepoTopping.Getp(ongoingPizzas[i]);
                    string cumulative = "";
                    foreach (var t in toppingsInPizza)
                    {
                        if (cumulative == "")
                            cumulative = $"{t.Topping}";
                        else
                            cumulative = cumulative + $", {t.Topping}";
                    }
                    Console.WriteLine($"Toppings include: {cumulative}.");
                }
                return false;
            }
            else
            {
                Console.WriteLine("\nThe Order ID is invalid or was not created for this user.");
                return false;
            }
        }

        //ObtainTime is used to record a timeframe for use when finding Sales
        public static int ObtainTime(string type, string example)
        {
            string result = "";
            int result2 = 0;
            bool timeLoop = false;
            while (!timeLoop)
            {
                Console.WriteLine($"\nType your desired {type}. Please format it like this: {example}");
                result = Console.ReadLine();
                if (result == "Back")
                    return -1;
                try
                {
                    result2 = Convert.ToInt32(result);
                    if (result2 == -1)
                        Console.Write($"Invalid {type}. Please try again.");
                    else
                        timeLoop = true;
                }
                catch
                {
                    Console.WriteLine($"Invalid {type}. Please try again.");
                }
            }
            return result2;
        }

        //The user is allowed to choose different options on how to interact with the Pizza box database.
        public static void UserDecision(Users u, PizzaDBContext pdb)
        {
            var RepoStore = new Storing.Repositories.RepositoryStore(pdb);
            bool Check = false;
            while (!Check)
            {
                Console.WriteLine($"\nType 'History' to view the users's order history. \nType 'Recent' to view your most recent pizza store. \n" +
                                  $"Type 'Order' to view locations and place an order. \nType 'Sign out' to sign out of account {u.UserName}.");
                string a = Console.ReadLine();

                //Order allows the user to order a pizza. The parameters are checked to make sure that we can properly order a pizza from a specific store.
                if (a == "Order")
                {
                    bool approve = false;
                    if (u.StoreTime != null)
                    {
                        DateTime moment = DateTime.Now;
                        TimeSpan TimeDifference = moment - (DateTime)u.StoreTime;
                        if (TimeDifference.TotalMinutes > 120)
                            approve = true;
                    }
                    else
                        approve = true;

                    if (approve == true)
                    {
                        var locations = RepoStore.Getp();
                        Console.WriteLine("\nHere are all the store locations to chooose from.");
                        foreach (var l in locations)
                        {
                            Console.WriteLine($"'{l.StoreName}' is the name of store {l.StoreId}.");
                        }
                        bool Check2 = false;
                        while (!Check2)
                        {
                            Console.WriteLine("\nType the name of the store you wish to order from. \nType 'Back' to return to the user menu.");
                            string z = Console.ReadLine();
                            if (z == "Back")
                                Check2 = true;
                            else
                            {
                                Stores y = RepoStore.Findp(z);
                                if (y == null)
                                    Console.WriteLine("Could not find provided name of location, make sure that it is spelled correctly.");
                                else
                                {
                                    if (u.StoreId == null || y.StoreId == u.StoreId)
                                    {
                                        PizzaOrdering(u, y, pdb);
                                        Check2 = true;
                                    }
                                    else if (y.StoreId != u.StoreId)
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
                                                Console.WriteLine($"You cannot order from another store until 24 hours have passed since ordering from the previous.");
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
                        Console.WriteLine($"Your most recent order was within 2 hours ago, at {u.StoreTime}. You must wait until it's been 2 hours.");
                }

                //History prints out all user orders and allows users to view any specific order in depth.
                else if (a == "History")
                {
                    var RepoOrder = new Storing.Repositories.RepositoryOrder(pdb);
                    var userOrders = RepoOrder.Getp(u);
                    List<string> templist = new List<string>();
                    Console.WriteLine($"\nHere are the orders made for user {u.UserName}.");
                    foreach (var o in userOrders)
                    {
                        Console.WriteLine($"Order '{o.OrderId}'. {o.PizzaAmount} pizzas ordered at {o.OrderTime} at a cost of {o.Cost}");
                        templist.Add(o.OrderId.ToString());
                    }
                    bool check3 = false;
                    if(templist.Count() == 0)
                    {
                        Console.WriteLine("There is currently no order history for this user.");
                        check3 = true;
                    }
                    while (!check3)
                        check3 = FindHistory(templist, pdb);  
                }

                //Recent prints out the time and location of the user's most recent order. This is helpful for the parameter checking.
                else if (a == "Recent")
                {
                    if (u.StoreId != null)
                    {
                        Stores temp = RepoStore.UseIDFindStore((int)u.StoreId);
                        Console.WriteLine($"\nYour most recent order was from store {temp.StoreName} at {u.StoreTime}");
                    }
                    else
                        Console.WriteLine("\nYou have not ordered pizza from a store yet. There is no 'most recent' store.");
                }
                
                //Returns to the log in menu.
                else if (a == "Sign out")
                {
                    Console.WriteLine("Returning now.");
                    return;
                }

                else
                    Console.WriteLine("Please insput a proper phrase, such as History, Recent, Order, or Sign out");
            }
        }

        //The store is able to choose a variety of options as well.
        public static void StoreDecision(Stores s, PizzaDBContext pdb)
        {
            var RepoOrder = new Storing.Repositories.RepositoryOrder(pdb);
            var RepoPizza = new Storing.Repositories.RepositoryPizza(pdb);
            var RepoUser = new Storing.Repositories.RepositoryUser(pdb);
            var RepoTopping = new Storing.Repositories.RepositoryToppings(pdb);
            bool Check = false;
            while (!Check) {
                Console.WriteLine($"\nType 'History' to view the store's order history.\nType 'Sales' to view this stores sales history. \n" +
                                  $"Type 'Inventory' to look at this store's inventory. \nType 'Users' to view which users go to this store. \n" +
                                  $"Type 'Sign out' to sign out of account {s.StoreName}.");
                string a = Console.ReadLine();

                //Shows the store's order history, and allows us to view any specific orders in greater depth.
                if (a == "History")
                {
                    var storeOrders = RepoOrder.Getp(s);
                    List<string> templist = new List<string>();
                    Console.WriteLine($"\nHere is the store {s.StoreName}'s orders.");
                    foreach (var o in storeOrders)
                    {
                        Console.WriteLine($"Order '{o.OrderId}'. {o.PizzaAmount} pizzas ordered at {o.OrderTime} at a cost of {o.Cost}");
                        templist.Add(o.OrderId.ToString());
                    }
                    bool check3 = false;
                    if (storeOrders.Count() == 0)
                    {
                        Console.WriteLine("\nThere is currently no history in regards to orders for this store.");
                        check3 = true;
                    }
                    while (!check3)
                        check3 = FindHistory(templist, pdb);
                }

                //Inventory prints out the inventory of the store.
                else if (a == "Inventory")
                {
                    Console.WriteLine($"\nHere is the inventory of {s.StoreName}");
                    foreach(string top in toppingLibrary.Keys)
                        Console.WriteLine($"Topping: {top}");
                    foreach(string crust in priceLibraryC.Keys)
                        Console.WriteLine($"Crust: {crust}");
                    foreach(string size in priceLibraryS.Keys)
                        Console.WriteLine($"Dough size: {size}");
                }

                //Sales prints out the cost of every sale made.
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
                        Console.WriteLine("\nType 'Month' to select orders by Month, or 'Day' to select orders by Day. \nType 'Back' to return to menu.");
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
                            Console.WriteLine("Invalid input. Please type 'Month', 'Day', or 'Back'");
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
                            check = true;
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
                            if (choice == 1)
                                Console.WriteLine($"\nNo sales were made during the day {month}/{day}/{year}.");
                            else
                                Console.WriteLine($"\nNo sales were made during the month {month}/{year}.");
                        }

                        else
                        {
                            if (choice == 1)
                                Console.WriteLine($"\nHere are the sales on the day {month}/{day}/{year}.");
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

                //Users allows us to see which users most recently used our store, and are therefore required to use our store for a 24 hour period.
                else if (a == "Users")
                {
                    var allUsers = RepoUser.Getp(s.StoreId);
                    int counter = 1;
                    Console.WriteLine($"\nHere are all users who have most recently used the store '{s.StoreName}':");
                    foreach(var u in allUsers)
                    {
                        Console.WriteLine($"User {counter}: {u.UserName} at {u.StoreTime}.");
                        counter++;
                    }
                }

                //Return to the log in screen.
                else if (a == "Sign out")
                {
                    Console.WriteLine("\nReturning now");
                    return;
                }

                else
                    Console.WriteLine("\nPlease input a proper phrase, such as History, Sales, Users, Inventory, or Sign out");
            }
        }

        //Log in allows us to log in or register as a store or user. Stores and users have unique menu items.
        public static void LogIn(PizzaDBContext pdb)
        {
            var RepoStore = new Storing.Repositories.RepositoryStore(pdb);
            var RepoUser = new Storing.Repositories.RepositoryUser(pdb);
            bool Check = false;
            while (!Check)
            {
                Console.WriteLine("\nType 'Sign in' to sign into an existing account. \nType 'Register' to register a new user or store.");
                String a = Console.ReadLine();
                if (a == "Sign in")
                {
                    while (!Check)
                    {
                        Console.WriteLine("\nType 'User' to sign in as a user.\nType 'Store' to sign in as a store.");
                        string a2 = Console.ReadLine();
                        if (a2 == "User")
                        {
                            while (!Check)
                            {
                                Console.WriteLine("\nType your personal username.");
                                string b = Console.ReadLine();
                                Console.WriteLine("\nType your password.");
                                string c = Console.ReadLine();
                                Users temp = new Users(b,c);
                                Users result = RepoUser.AccessP(temp);
                                if (result != null)
                                {
                                    Console.WriteLine("\n");
                                    Check = true;
                                    UserDecision(result, pdb);
                                }
                                else
                                {
                                    Console.WriteLine("Username or Password failed. \nType 'Yes' to try again. \nType anything else to return.");
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
                                Console.WriteLine("\nType your Store's username.");
                                string b = Console.ReadLine();
                                Console.WriteLine("\nType your password.");
                                string c = Console.ReadLine();
                                Stores temp = new Stores(b,c);
                                Stores result = RepoStore.AccessP(temp);
                                if (result != null)
                                {
                                    Console.WriteLine("\n");
                                    StoreDecision(result, pdb);
                                    Check = true;
                                }
                                else
                                {
                                    Console.WriteLine("\nUsername or Password failed. \nType 'Yes' to try again.");
                                    string retry = Console.ReadLine();
                                    if (retry != "Yes")
                                        Check = true;
                                }
                            }
                        }
                        else
                            Console.WriteLine("\nInvalid input. Please state Store or User.");
                    }
                }
                else if (a == "Register")
                {
                    while (!Check)
                    {
                        Console.WriteLine("\nType 'User' to register as a new user. \nType 'Store' to register as a new store.");
                        string b = Console.ReadLine();
                        if (b == "User")
                        {
                            string c = "";
                            string d = "";
                            bool Check4 = false;
                            Users tempUser = new Users();
                            while (!Check4)
                            {
                                Console.WriteLine("\nType your new username.");
                                c = Console.ReadLine();
                                if((c.Length > 50) || c == null)
                                    Console.WriteLine("Username invalid. Retype your new username. Make sure it is less than 50 letters.");
                                else
                                {
                                    Check4 = true;
                                    tempUser.UserName = c;
                                }
                            }
                            while(Check4)
                            {
                                Console.WriteLine("\nType your new password.");
                                d = Console.ReadLine();
                                if(d.Length > 50 || d==null)
                                    Console.WriteLine("Invalid password. Retype your password. Make sure it is 50 letters or less.");
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
                                    "Type 'Yes' to retry registration. \nType anything else to return.");
                                string final = Console.ReadLine();
                                if(final != "Yes")
                                    Check = true;
                            }
                        }
                        else if (b == "Store")
                        {
                            string c = "";
                            string d = "";
                            Stores tempStore = new Stores();
                            bool Check5 = false; 
                            while (!Check5)
                            {
                                Console.WriteLine("\nType your new store's username.");
                                c = Console.ReadLine();
                                if(c.Length > 50 || c == null)
                                    Console.WriteLine("\nName will not work. Please retype your username. Make sure it is 50 letters or less.");
                                else
                                {
                                    Check5 = true;
                                    tempStore.StoreName = c;
                                }
                            }
                            while (Check5)
                            {
                                Console.WriteLine("\nType your new store's password.");
                                d = Console.ReadLine();
                                if(d.Length > 50 || d == null)
                                    Console.WriteLine("\nInvalid password. Please retype password. Make sure it is 50 letters or less.");
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
                                Console.WriteLine("\nThe provided storename already exists in the database. Please provide an unused storename.\n" +
                                    "Type 'Yes' to retry registration. \nType anything else to return.");
                                string final = Console.ReadLine();
                                if (final != "Yes")
                                    Check = true;
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
            //Establishing the inventory libraries. 
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

            presetList.Add("Hawaiian");
            presetList.Add("BBQ");
            presetList.Add("American");
            presetList.Add("Canadian");
            presetList.Add("Italian");
            presetList.Add("Rich");

            //Establish a PizzaDBContext to interact with the database.
            PizzaDBContext pdb = Client.CreateContext.returnContext();
            bool check = false;
            while (!check)
                LogIn(pdb);
        }                  
    }
}