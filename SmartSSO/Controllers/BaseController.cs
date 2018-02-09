using Data.Entities;
using Newtonsoft.Json;
using SmartSSO.Entities;
using SmartSSO.Filters;
using SmartSSO.Helpers;
using SmartSSO.Services;
using SmartSSO.Services.Impl;
using System;
using System.Collections.Generic;
using System.IO;
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
     
            var bag = filterContext.HttpContext.Request.QueryString["bag"];
            if (!string.IsNullOrEmpty(bag))
            {
                var bagDyn = JsonConvert.DeserializeObject<Dictionary< string, dynamic>>(bag);
                foreach(var item in bagDyn)
                {
                    if(!ViewData.ContainsKey(item.Key))
                    ViewData[item.Key] = item.Value;
                }
            }

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
        /// 查询的时段
        /// </summary>
        public class DateTimeRange
        {
            public DateTime  TimeStart { get; set; }
            public DateTime  TimeEnd { get; set; }
        }


        /// <summary>
        /// 文件类型s
        /// </summary>
        /// <returns></returns>
        protected List<SelectListItem> GetFileTypeList()
        {
            return new List<SelectListItem> {
                new SelectListItem{ Text = "所有",Value =FILETYPE.None.GetHashCode().ToString() },
                new SelectListItem{ Text = "其它",Value =FILETYPE.其它.GetHashCode().ToString() },
                new SelectListItem{ Text = "库存",Value =FILETYPE.库存.GetHashCode().ToString() },
                new SelectListItem{ Text = "孔数",Value =FILETYPE.孔数.GetHashCode().ToString() },
                new SelectListItem{ Text = "比重",Value =FILETYPE.比重.GetHashCode().ToString() },
                new SelectListItem{ Text = "表面物性",Value =FILETYPE.表面物性.GetHashCode().ToString() },
                new SelectListItem{ Text = "生产效率",Value =FILETYPE.生产效率.GetHashCode().ToString() },
                new SelectListItem{ Text = "颜色",Value =FILETYPE.颜色.GetHashCode().ToString() }
            };
        }
    }
}