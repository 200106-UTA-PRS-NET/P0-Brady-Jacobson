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
    }
}