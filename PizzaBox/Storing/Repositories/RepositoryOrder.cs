using System;
using System.Collections.Generic;
using System.Text;
using Domain.Interfaces;
using Domain.Models;
using System.Linq;
using Domain;

namespace Storing.Repositories
{
    public class RepositoryOrder : IRepository<Orders>
    {
        PizzaDBContext pdb;
        public RepositoryOrder()
        {
            pdb = new PizzaDBContext();
        }
        public RepositoryOrder(PizzaDBContext pdb)
        {
            this.pdb = pdb ?? throw new ArgumentNullException(nameof(pdb));
        }

        public Orders Addp(Orders p)
        {
            pdb.Orders.Add(p);
            pdb.SaveChanges();

            var a = pdb.Orders.FirstOrDefault(d => d.OrderTime == p.OrderTime && d.User == p.User && d.Store == p.Store);
            Console.WriteLine($"Added Order to Table 'Orders'");
            return a;
        }

        public void Deletep(Orders p)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Orders> Getp(Users b)
        {
            var query = from a in pdb.Orders where(a.UserId == b.UserId)
                        select Mapper.MapOrder(a);
            return query;
        }
        public IEnumerable<Orders> Getp(Stores b)
        {
            var query = from a in pdb.Orders
                        where (a.StoreId == b.StoreId)
                        select Mapper.MapOrder(a);
            return query;
        }

        public void Modifyp(Orders p)
        {
            throw new NotImplementedException();
        }
        public Orders AccessP(Orders p)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Orders> Getp()
        {
            throw new NotImplementedException();
        }
    }
}
