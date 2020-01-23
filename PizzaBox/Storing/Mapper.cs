using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    class Mapper
    {
        public static Models.Stores MapStore(Models.Stores s)
        {
            return new Models.Stores()
            {
                StoreId = s.StoreId,
                StoreName = s.StoreName,
                StoreCode = s.StoreCode
            };
        }

        public static Models.Users MapUser(Models.Users u)
        {
            return new Models.Users()
            {
                UserId = u.UserId,
                UserName = u.UserName,
                UserCode = u.UserCode,
                StoreId = u.StoreId,
                StoreTime = u.StoreTime
            };
        }

        public static Models.Orders MapOrder(Models.Orders o)
        {
            return new Models.Orders()
            {
                OrderId = o.OrderId,
                StoreId = o.StoreId,
                UserId = o.UserId,
                PizzaAmount = o.PizzaAmount,
                Cost = o.Cost,
                OrderTime = o.OrderTime
            };
        }

        public static Models.Pizzas MapPizza(Models.Pizzas p)
        {
            return new Models.Pizzas()
            {
                PizzaId = p.PizzaId,
                OrderId = p.OrderId,
                Crust = p.Crust,
                Size = p.Size,
                PizzaCost = p.PizzaCost
            };
        }
    }
}