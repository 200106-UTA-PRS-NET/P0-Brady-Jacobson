using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models_extra
{
    public class User //: HistoryViewer
    {
        private String name;
        private String passcode;
        private Store mandatoryStore;
        private DateTime mandatoryTime;
        private DateTime recentOrder;
        Stack<TempOrders> myOrders = new Stack<TempOrders>();
        //Make it an interface
        public void viewOrders()
        {

        }

    }
}
