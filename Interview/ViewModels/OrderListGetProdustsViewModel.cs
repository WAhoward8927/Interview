using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interview.Models;

namespace Interview.ViewModels
{
    public class OrderListViewModel
    {
        public List<OrderList> OrderListData { get; set; }
    }

    public class DetailViewModel 
    {
        public List<OrderListDetail> Details { get; set; }
    }

}