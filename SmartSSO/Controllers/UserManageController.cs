using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IServices;
using Microsoft.Practices.Unity;
using MyDemo.Controllers;
using SmartSSO.Entities;
using SmartSSO.Extendsions;
using SmartSSO.Filters;
using SmartSSO.Helpers;
using SmartSSO.Models;
using SmartSSO.Services;
using SmartSSO.Services.Impl;

namespace SmartSSO.Controllers
{
    [UserAuthorization]
    public class UserManageController : BaseController
    {
        #region 私有字段

        private readonly IUserManageService _userManageService = UnityHelper.Instance.Unity.Resolve<IUserManageService>();
        private readonly IDiscountService _discountService = UnityHelper.Instance.Unity.Resolve<IDiscountService>();


        #endregion

        #region action method

        public ActionResult Index()
        {
            return View(_userManageService.GetAll());
        }

        public ActionResult Create(string userName=null )
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var user = _userManageService.Get(userName);
                return View(new UserCreateRequest {
                    UserName = user.UserName,
                    Discount = user.Discount,
                    IsAdd = false,
                    IsAdmin = user.IsAdmin,
                    Nick  = user.Nick,
                    Password = user.UserPwd
                });
            } 
            return View(new UserCreateRequest {IsAdd = true });
        }
         
         


        [HttpPost]
        [ValidateModelState]
        public ActionResult Create(UserCreateRequest model)
        {
            var result = _userManageService.Create(model);
            if (result.Success)
                return RedirectToAction("Index");
            ModelState.AddModelError("_error", result.Msg);

            return View();
        }

        public ActionResult Delete(string username = "")
        {
            if (username.Length > 0)
            {
                _userManageService.Delete(username);
            }

            return RedirectToAction("Index");
        }



        #endregion

        #region 折扣设置

        public ActionResult DiscountSet()
        {  
            return View(_discountService.GetDiscounts());
        }

        [HttpPost]
        [ValidateModelState]
        public ActionResult DiscountSet(Dictionary<string,decimal> model)
        {
            return View();
        }

        #endregion
    }
}