using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;
using Domain.Interfaces;
using System.Linq;
using Domain;

namespace Storing.Repositories
{
    public class RepositoryToppings : IRepository<Toppings>
    {
        PizzaDBContext pdb;
        public RepositoryToppings()
        {
            pdb = new PizzaDBContext();
        }
        public RepositoryToppings(PizzaDBContext pdb)
        {
            this.pdb = pdb ?? throw new ArgumentNullException(nameof(pdb));
        }

        public Toppings Addp(Toppings p)
        {
            pdb.Toppings.Add(p);
            pdb.SaveChanges();
            return p;
        }

        public void Deletep(Toppings p)
        {
            throw new NotImplementedException();
        }

        public void Modifyp(Toppings p)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Toppings> Getp()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Toppings> Getp(Pizzas p)
        {
            var query = from a in pdb.Toppings
                        where (a.PizzaId == p.PizzaId)
                        select Mapper.MapTopping(a);
            return query;
        }
        public Toppings AccessP(Toppings p)
        {
            throw new NotImplementedException();
        }
    }
}
