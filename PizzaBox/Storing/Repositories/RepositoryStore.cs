using System;
using System.Collections.Generic;
using System.Text;
using Domain.Interfaces;
using Domain.Models;
using System.Linq;
using Domain;

namespace Storing.Repositories
{
    public class RepositoryStore : IRepository<Stores>
    {
        PizzaDBContext pdb;
        public RepositoryStore()
        {
            pdb = new PizzaDBContext();
        }
        public RepositoryStore(PizzaDBContext pdb)
        {
            this.pdb = pdb ?? throw new ArgumentNullException(nameof(pdb));
        }

        public Stores Addp(Stores p)
        {
            if (pdb.Stores.Any(e => e.StoreName == p.StoreName))
                return null;
            pdb.Stores.Add(p);
            pdb.SaveChanges();
            var a = pdb.Stores.FirstOrDefault(d => d.StoreName == p.StoreName);
            Console.WriteLine($"Added Store {a.StoreName} to Table 'Stores'");
            return a;
        }

        public void Deletep(Stores p)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Stores> Getp()
        {
            var query = from a in pdb.Stores
                        select Mapper.MapStore(a);
            return query;
        }

        public void Modifyp(Stores p)
        {
            throw new NotImplementedException();
        }

        public Stores AccessP(Stores p)
        {
            if (pdb.Stores.Any(d => d.StoreName == p.StoreName && d.StoreCode == p.StoreCode))
            {
                var a = pdb.Stores.FirstOrDefault(d => d.StoreName == p.StoreName && d.StoreCode == p.StoreCode);
                Console.WriteLine($"Logged in successfully to Store '{p.StoreName}'");
                return a;
            }
            else
            {
                Console.WriteLine("Username or password incorrect. Please try again.");
            }
            return null;
        }

        public Stores UseIDFindStore(int p)
        {
            if(pdb.Stores.Any(d=>d.StoreId == p))
            {
                var a = pdb.Stores.FirstOrDefault(d => d.StoreId == p);
                return a;
            }
            else
            {
                Console.WriteLine("Error: The store ID provided does not have an associated store. The store was possibly deleted.");
            }
            return null;
        }
        public Stores Findp(string name)
        {
            if(pdb.Stores.Any(d => d.StoreName == name))
            {
                var a = pdb.Stores.FirstOrDefault(d => d.StoreName == name);
                return a;
            }
            Console.WriteLine("Strange Error has occurred.");
            return null;
        }
    }
}
