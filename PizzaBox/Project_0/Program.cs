namespace Project_0
{
    class Program
    {
        static void Main(string[] args)
        {
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
}
