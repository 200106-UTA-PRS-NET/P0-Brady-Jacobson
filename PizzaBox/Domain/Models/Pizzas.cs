using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Pizzas
    {
        public Pizzas()
        {
            Toppings = new HashSet<Toppings>();
        }

        public Pizzas(string c, string s, decimal cost)
        {
            Crust = c;
            Size = s;
            PizzaCost = cost;
        }

        public int PizzaId { get; set; }
        public int OrderId { get; set; }
        public string Crust { get; set; }
        public string Size { get; set; }
        public decimal PizzaCost { get; set; }

        public virtual Orders Order { get; set; }
        public virtual ICollection<Toppings> Toppings { get; set; }
    }
}
