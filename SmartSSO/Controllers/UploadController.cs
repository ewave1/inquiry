﻿using Data.Entities;
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
    /// 导入管理
    /// UploadFile管理
    /// </summary>
    public class UploadController : BaseController
    {


        #region 私有字段

        private readonly IInquiryService _inquiryService = UnityHelper.Instance.Unity.Resolve<IInquiryService>();

        private readonly IUploadService _iservice = UnityHelper.Instance.Unity.Resolve<IUploadService>();

        #endregion
        // GET: Import
        public ActionResult Index(string CreateUser, string timeStart, string timeEnd, FILETYPE fileType = FILETYPE.None, int page = 1)
        {
            var user = GetCurrentUser();

            var timeRange = SetTimeRange(timeStart, timeEnd);
            ViewBag.CreateUser = CreateUser;
            var result = _iservice.GetAll( CreateUser, timeRange.TimeStart, timeRange.TimeEnd,fileType, page);
            if (result == null)
                return RedirectToAction("Login", "Home");

            
            var lst = GetFileTypeList();

            ViewBag.FileTypes = lst;
            return View(result); 
        }

        [HttpPost]
        public ActionResult UploadFile( FILETYPE fileType= FILETYPE.其它)
        {
            var user = GetCurrentUser();
            var uploadFile =   _iservice.UploadFile(user?.UserName, Request,fileType); 

            return Json(uploadFile);

        }

        public ActionResult Delete(int id = 0)
        {
            _iservice.Delete(id);

            return RedirectToAction("Index");
        }


    }
}