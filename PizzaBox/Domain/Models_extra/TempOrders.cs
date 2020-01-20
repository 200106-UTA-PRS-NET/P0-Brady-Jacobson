using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models_extra
{
    public class TempOrders
    {
        private double Cost;
        //if allPizza.Count > 100, no.
        List<Pizza> allPizza = new List<Pizza>();
        private DateTime ordered;

        //Unnecessary?
        private User customer;

        public void CalculateCost()
        {
            double amount = 0.00;
            foreach(Pizza p in allPizza)
            {
                amount += p.Cost;
            }
            Cost = amount;
        }
    }
}
