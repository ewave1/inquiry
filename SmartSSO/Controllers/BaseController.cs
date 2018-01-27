using SmartSSO.Entities;
using SmartSSO.Filters;
using SmartSSO.Helpers;
using SmartSSO.Services;
using SmartSSO.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyDemo.Controllers
{
    public class BaseController:Controller
    {

        private readonly IUserManageService _userManageService = new  UserManageService();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //if (filterContext.HttpContext.Request.IsAuthenticated)
            //{
            //    ViewData["user"] = User.Identity.Name;
            //}

            ViewBag.CurrentUser = GetCurrentUser();
            if (string.IsNullOrEmpty(UserName)&& ViewBag.CurrentUser!=null) 
            {
                string returnUrl = HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.AbsolutePath);
                filterContext.Result = Redirect("/Home/LoginOut?ReturnUrl=" + returnUrl);
            }
            base.OnActionExecuting(filterContext);
        }

        public string UserName
        {
            get
            {
                return Request.Cookies[UserAuthorizationAttribute.CookieUserName]?.Value; 
            }

        }
        public ManageUser GetCurrentUser()
        {
            return _userManageService.Get(UserName);
        }

        protected DateTimeRange SetTimeRange(string timeStart, string timeEnd)
        {
            DateTime _timeStart;
            DateTime _timeEnd;
            if (!string.IsNullOrEmpty(timeStart) && !string.IsNullOrEmpty(timeEnd))
            {
                ViewBag.timeStart = timeStart;
                ViewBag.timeEnd = timeEnd;
                timeEnd += " 23:59:59";
                _timeStart = DateTime.Parse(timeStart);
                _timeEnd = DateTime.Parse(timeEnd);
            }
            else
            {
                _timeEnd = DateTime.Now;
                _timeStart = _timeEnd.AddDays(-30);
                ViewBag.timeStart = string.Format("{0:yyyy-MM-dd}", _timeStart);
                ViewBag.timeEnd = string.Format("{0:yyyy-MM-dd}", _timeEnd);
            }

            return new DateTimeRange { TimeStart = _timeStart, TimeEnd = _timeEnd };
        }

        /// <summary>
        /// 默认一个月之内的
        /// </summary>
        public class DateTimeRange
        {
            public DateTime  TimeStart { get; set; }
            public DateTime  TimeEnd { get; set; }
        }
    }
}