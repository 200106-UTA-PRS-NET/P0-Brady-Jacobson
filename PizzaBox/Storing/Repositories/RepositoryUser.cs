using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;
using Domain.Interfaces;
using System.Linq;
using Domain;

namespace Storing.Repositories
{
    public class RepositoryUser : IRepository<Users>
    {
        PizzaDBContext pdb;
        public RepositoryUser()
        {
            pdb = new PizzaDBContext();
        }
        public RepositoryUser(PizzaDBContext pdb)
        {
            this.pdb = pdb??throw new ArgumentNullException(nameof(pdb));
        }

        public Users Addp(Users p)
        {
            if (pdb.Users.Any(e => e.UserName == p.UserName))
                return null;
            pdb.Users.Add(p);
            pdb.SaveChanges();
            var a = pdb.Users.FirstOrDefault(d => d.UserName == p.UserName);
            Console.WriteLine($"Added User {a.UserName} to Table 'Users'");
            return a;
        }
            

        public void Deletep(Users p)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Users> Getp()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Users> Getp(int p)
        {
            var query = from a in pdb.Users
                        where (a.StoreId == p)
                        select Mapper.MapUser(a);
            return query;
        }

        public void Modifyp(Users p)
        {
            if( pdb.Users.Any(a => a.UserId == p.UserId))
            {
                var u = pdb.Users.FirstOrDefault(a => a.UserId == p.UserId);
                u.StoreId = p.StoreId;
                u.StoreTime = p.StoreTime;
                pdb.Users.Update(u);
                pdb.SaveChanges();
            }
            else
            {
                Console.WriteLine("Could not change provided User.");
            }
            return;
        }

        public Users AccessP(Users p)
        {
            if (pdb.Users.Any(d => d.UserName == p.UserName && d.UserCode == p.UserCode))
            {
                var a = pdb.Users.FirstOrDefault(d => d.UserName == p.UserName && d.UserCode == p.UserCode);
                Console.WriteLine($"Logged in successfully to User '{p.UserName}'");
                return a;
            }
            else
            {
                Console.WriteLine("Username or password incorrect. Please try again.");
            }
            return null;
        }
    }
}
