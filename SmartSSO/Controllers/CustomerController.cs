using IServices;
using SmartSSO.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;
using SmartSSO.Entities;
using SmartSSO.Extendsions;
using SmartSSO.Filters;
using SmartSSO.Models;
using SmartSSO.Services;
using SmartSSO.Services.Impl;
using Data.Models;
using Data.Entities;
using MyDemo.Controllers;
using System.Web.Routing;

namespace InquiryDemo.Controllers
{
    public class CustomerController : BaseController
    {
        #region 私有字段

        private readonly ICustomerService _iservice = UnityHelper.Instance.Unity.Resolve<ICustomerService>();
        private readonly IInquiryService _inquiryService = UnityHelper.Instance.Unity.Resolve<IInquiryService>();


        #endregion
        // GET: Inquiry
        public ActionResult Index()
        {
            //var user = GetCurrentUser();

            return View(_iservice.GetAll());
        }
        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {

            return base.BeginExecute(requestContext, callback, state);
        }
        /// <summary>
        /// viewbag定义放在这里
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        { 
            ViewData["CustomerLevels"] = GetDiscountNames(DisCountType.客户级别);
            base.OnActionExecuting(filterContext);
        }
        public ActionResult Create( int?id)
        {  
            var model = _iservice.Get(id);
            return View(model);
        }

        private List<SelectListItem> GetDiscountNames(DisCountType type)
        {
            var lst = _inquiryService.GetDiscountNames(type).Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Name,

            }).ToList()
             ;
            if (lst.Count > 0)
                lst[0].Selected = true;
            return lst;
        }


        [HttpPost]
        [ValidateModelState]
        public ActionResult Create(CustomerModel model)
        {
            var user = GetCurrentUser();
            var result = _iservice.Create(model,user?.UserName);
            if (result.Success)
                return RedirectToAction("Index");
            ModelState.AddModelError("_error", result.Msg);

            ViewData["CustomerLevels"] = GetDiscountNames(DisCountType.客户级别);
            return View();
        } 

        public ActionResult Delete(int id = 0)
        {
            _iservice.Delete(id);

            return RedirectToAction("Index");
        }
    }
}