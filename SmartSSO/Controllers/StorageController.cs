using Data.Entities;
using IServices;
using MyDemo.Controllers;
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

namespace InquiryDemo.Controllers
{
    /// <summary>
    /// 库存管理
    /// </summary>
    public class StorageController : BaseController
    {

        #region 私有字段

        private readonly IInquiryService _inquiryService = UnityHelper.Instance.Unity.Resolve<IInquiryService>();

        private readonly IStorageService _iservice = UnityHelper.Instance.Unity.Resolve<IStorageService>();

        #endregion
        // GET: Storage
        public ActionResult Index(string CreateUser, string timeStart, string timeEnd,  int page = 1)
        {
            var user = GetCurrentUser();

            var timeRange = SetTimeRange(timeStart, timeEnd);
            ViewBag.CreateUser = CreateUser;
            ViewBag.Page = page;
            var result = _iservice.GetAll(CreateUser, timeRange.TimeStart, timeRange.TimeEnd,  page);
            if (result == null)
                return RedirectToAction("Login", "Home");

             
            return View(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFile(FILETYPE fileType = FILETYPE.其它)
        {
            var user = GetCurrentUser();
            var uploadFile = _iservice.UploadStorage(user?.UserName, Request);

            return Json(uploadFile);

        }

        public ActionResult Delete(int id)
        {
            _iservice.Delete(id);
            return RedirectToAction("Index");
        }


    }
}