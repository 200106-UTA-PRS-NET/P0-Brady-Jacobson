using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Users
    {
        public Users()
        {
            Orders = new HashSet<Orders>();
        }

        public Users(string name, string code)
        {
            UserName = name;
            UserCode = code;
            Orders = new HashSet<Orders>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public int? StoreId { get; set; }
        public DateTime? StoreTime { get; set; }

        public virtual Stores Store { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
