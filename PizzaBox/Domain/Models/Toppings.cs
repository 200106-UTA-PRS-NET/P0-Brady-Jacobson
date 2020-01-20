using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Toppings
    {
        public int ToppingId { get; set; }
        public int PizzaId { get; set; }
        public string Topping { get; set; }

        public virtual Pizzas Pizza { get; set; }
    }
}
