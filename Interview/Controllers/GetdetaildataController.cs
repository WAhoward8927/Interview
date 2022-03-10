using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Interview.Models;
using Interview.ViewModels;

namespace Interview.Controllers
{
    public class GetdetaildataController : Controller
    {
        /// <summary>
        /// 取得指定產品的詳細資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(string id)
        {
            Getdetaildata getdetaildata = new Getdetaildata();
            DetailViewModel detailMainModel = new DetailViewModel();

            List<OrderListDetail> orderListDetails = new List<OrderListDetail>();
            orderListDetails = getdetaildata.GetDetails(id).ToList();
            detailMainModel.Details = orderListDetails;
            return View(detailMainModel);
        }
    }
}
