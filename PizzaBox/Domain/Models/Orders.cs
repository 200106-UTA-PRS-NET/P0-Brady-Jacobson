using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Orders
    {
        public Orders()
        {
            Pizzas = new HashSet<Pizzas>();
        }

        public Orders(int s, int u, int p, decimal price)
        {
            StoreId = s;
            UserId = u;
            PizzaAmount = p;
            Cost = price;
        }
        public int OrderId { get; set; }
        public int StoreId { get; set; }
        public int UserId { get; set; }
        public int PizzaAmount { get; set; }
        public decimal Cost { get; set; }
        public DateTime OrderTime { get; set; }

        public virtual Stores Store { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<Pizzas> Pizzas { get; set; }
    }
}
