using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InquiryDemo.Controllers
{
    /// <summary>
    /// 库存管理
    /// </summary>
    public class StorageController : Controller
    {
        // GET: Storage
        public ActionResult Index()
        {
            return View();
        }
    }
}