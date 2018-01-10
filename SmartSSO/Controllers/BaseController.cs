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
    }
}