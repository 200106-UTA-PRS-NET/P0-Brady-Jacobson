using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;
using Domain.Interfaces;

namespace Storing.Repositories
{
    public class RepositoryPizza : IRepository<Pizzas>
    {
        PizzaDBContext pdb;
        public RepositoryPizza()
        {
            pdb = new PizzaDBContext();
        }
        public RepositoryPizza(PizzaDBContext pdb)
        {
            this.pdb = pdb ?? throw new ArgumentNullException(nameof(pdb));
        }

        public Pizzas AccessP(Pizzas p)
        {
            throw new NotImplementedException();
        }

        public Pizzas Addp(Pizzas p)
        {
            pdb.Pizzas.Add(p);
            pdb.SaveChanges();
            Console.WriteLine($"Added Pizza to table 'Pizzas'");
            return p;
        }

        public void Deletep(Pizzas p)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Pizzas> Getp()
        {
            throw new NotImplementedException();
        }

        public void Modifyp(Pizzas p)
        {
            throw new NotImplementedException();
        }
    }
}
