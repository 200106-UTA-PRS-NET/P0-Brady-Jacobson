using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models_extra
{
    public class Pizza
    {
        public String Name;
        private String Crust;
        private String Size;
        //Set to private, get/set (Encapsulate)
        public double Cost;
        private List<String> Toppings = new List<String>();

        public Pizza()
        {
            Name = "Custom";
            Crust = "Thin";
            Size = "12 inch";
            Toppings.Add("Cheese");
            Toppings.Add("Marinera");
            Cost = 2.50;
            //Add Cost Calculation WIth Dictionary 
        }

        public Pizza(String name, String c, String s)
        {
            Name = name;
            Crust = c;
            Size = s;
            Toppings.Add("Cheese");
            Toppings.Add("Marinera");
            Cost = 3.00;
        }
    }
}
