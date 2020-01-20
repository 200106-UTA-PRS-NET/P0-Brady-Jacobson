using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;
using Domain.Interfaces;
using System.Linq;

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
            //TO DO: Update c and d's default values.
            string c = "";
            string d = "";
            bool check = false;
            while (!check) 
            {
                Console.WriteLine("Type your new username.");
                c = Console.ReadLine();
                if (pdb.Users.Any(e => e.UserName == c) || c.Length > 50 || c == null)
                    Console.WriteLine("Name will not work. Please input an unused name that is 50 letters or less.");
                else
                    check = true;
            }
            while (check) 
            {
                Console.WriteLine("Type your new password.");
                d = Console.ReadLine();
                if (d.Length > 50 || d == null)
                    Console.WriteLine("Invalid password. Please try a password that is 50 letters or less.");
                else
                    check = false;
            }
            Users tempUser = new Users();
            tempUser.UserName = c;
            tempUser.UserCode = d;
            pdb.Users.Add(tempUser);
            pdb.SaveChanges();
            var a = pdb.Users.FirstOrDefault(d => d.UserName == tempUser.UserName);
            Console.WriteLine($"Added User {c} to Table 'Users'");
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
