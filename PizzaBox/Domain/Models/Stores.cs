using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Stores
    {
        public Stores()
        {
            Orders = new HashSet<Orders>();
            Users = new HashSet<Users>();
        }

        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreCode { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}
