using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models_extra

{
    public class Store //: HistoryViewer
    {
        //Use repositories for most of this.
        private string name;
        private string passcode;
        //Figure out how to list the sales... Use Database... List<Sales>?
        private Dictionary<string, double> inventory = new Dictionary<string, double>();
        private List<User> userBase = new List<User>();
        Stack<TempOrders> myOrders = new Stack<TempOrders>();

        public void viewOrders()
        {

        }
    }
}
