using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Interview.Models;
using Interview.ViewModels;

namespace Interview.Controllers
{
    public class OrderListController : Controller
    {
        /// <summary>
        /// 產品清單首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            OrderListIndex orderListIndex = new OrderListIndex();
            OrderListViewModel orderListViewModel = new OrderListViewModel();
            orderListViewModel.OrderListData = orderListIndex.GetProduct().ToList();
            return View(orderListViewModel);
        }

        /// <summary>
        /// 收到要變更狀態的產品
        /// </summary>
        /// <param name="postData">每筆資料的內容</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProdusts(OrderListViewModel postData)
        {
            OrderListIndex orderListIndex = new OrderListIndex();
            for (int i = 0; i < postData.OrderListData.Count; i++) 
            {
                if (postData.OrderListData[i].Ischeck) 
                {
                    string result = orderListIndex.updateItemstatus(postData.OrderListData[i].ID, postData.OrderListData[i].account);
                    if (result != string.Empty)
                    {
                        return RedirectToAction("ProductError", "Home");
                    }
                }
            }
            return RedirectToAction("Index", "OrderList");
        }
    }
}