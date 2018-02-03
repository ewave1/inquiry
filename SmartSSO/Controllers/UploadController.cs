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
            var uploadFile =_iservice.UploadFile(user?.UserName, Request,fileType); 

            return Json(uploadFile);

        }

        public ActionResult Delete(int id = 0)
        {
            _iservice.Delete(id);

            return RedirectToAction("Index");
        }


        /// <summary>
        /// 模板列表
        /// </summary>
        /// <returns></returns>
        public ActionResult TemplateList()
        {
            var lst = new List<TemplateModel>();
            int id = 1;
            lst.Add(new TemplateModel { Id = id++, FileName = "库存模板",Url = "/Content/Template/库存模板.xlsx" });
            lst.Add(new TemplateModel { Id = id++, FileName = "比重模板", Url = "/Content/Template/比重模板.xlsx" });
            lst.Add(new TemplateModel { Id = id++, FileName = "不良率模板", Url = "/Content/Template/不良率模板.xlsx" });
            lst.Add(new TemplateModel { Id = id++, FileName = "基础孔数模板", Url = "/Content/Template/基础孔数模板.xlsx" });
            lst.Add(new TemplateModel { Id = id++, FileName = "孔数模板", Url = "/Content/Template/孔数模板.xlsx" });
            lst.Add(new TemplateModel { Id = id++, FileName = "生产效率模板", Url = "/Content/Template/生产效率模板.xlsx" });
            lst.Add(new TemplateModel { Id = id++, FileName = "特殊配方模板", Url = "/Content/Template/特殊配方模板.xlsx" });
            lst.Add(new TemplateModel { Id = id++, FileName = "特殊处理外模板", Url = "/Content/Template/特殊处理外模板.xlsx" });
            lst.Add(new TemplateModel { Id = id++, FileName = "颜色模板", Url = "/Content/Template/颜色模板.xlsx" });
            lst.Add(new TemplateModel { Id = id++, FileName = "原料模板", Url = "/Content/Template/原料模板.xlsx" });
            return View(lst);
        }
    }
}