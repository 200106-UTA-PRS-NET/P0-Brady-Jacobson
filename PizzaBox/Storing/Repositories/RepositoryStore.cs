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
            string c = "";
            string d = "";
            bool check = false;
            while (!check)
            {
                Console.WriteLine("Type your new store's username.");
                c = Console.ReadLine();
                if (pdb.Stores.Any(e => e.StoreName == c) || c.Length > 50 || c == null)
                    Console.WriteLine("Name will not work. Please input an unused name that is 50 letters or less.");
                else
                    check = true;
            }
            while (check)
            {
                Console.WriteLine("Type your new store's password.");
                d = Console.ReadLine();
                if (d.Length > 50 || d == null)
                    Console.WriteLine("Invalid password. Please try a password that is 50 letters or less.");
                else
                    check = false;
            }
            Stores tempStore = new Stores();
            tempStore.StoreName = c;
            tempStore.StoreCode = d;
            pdb.Stores.Add(tempStore);
            pdb.SaveChanges();
            //TO DO: Replace with tempstore?
            var a = pdb.Stores.FirstOrDefault(d => d.StoreName == tempStore.StoreName);
            Console.WriteLine($"Added Store {c} to Table 'Stores'");
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
