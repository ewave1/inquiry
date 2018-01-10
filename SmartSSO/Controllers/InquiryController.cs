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

namespace SmartSSO.Controllers
{
    /// <summary>
    /// 报价管理
    /// 主要是报价和报价的历史查询
    /// </summary>
    public class InquiryController : BaseController
    {
        #region 私有字段

        private readonly IInquiryService _inquiryService = UnityHelper.Instance.Unity.Resolve<IInquiryService>();


        #endregion
        // GET: Inquiry
        public ActionResult Index()
        {
            var user = GetCurrentUser();

            return View(_inquiryService.GetAll(user));
        }
        public ActionResult Create( )
        {
            ViewBag.Factories = GetFactories();
            ViewBag.Materials = GetMaterials();
            ViewBag.SealCodes = GetSealCodes(); 
            return View();
        }

        private List<SelectListItem> GetFactories()
        {
            var lst = _inquiryService.Factories().Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Name,

            }).ToList()
             ;
            if (lst.Count > 0)
                lst[0].Selected = true;
            return lst;
        }

        private List<SelectListItem> GetMaterials()
        {
            var lst = _inquiryService.Materials().Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString(),

            }).ToList()
             ;
            return lst;
        }

        private List<SelectListItem> GetSealCodes()
        {
            var lst = _inquiryService.SealCodes().Select(v => new SelectListItem
            {
                Text = v.Code,
                Value = v.Code.ToString(),

            }).ToList()
            ;
            lst.Insert(0, new SelectListItem { Selected = true, Text = "", Value = "" });
            return lst;
        }

        public ActionResult GetSealCode(string code, decimal? sizea, decimal? sizeb)
        {
            var sc = _inquiryService.GetSealCode(code, sizea, sizeb);

            return Json(new Data.Models.RepResult<SealCode>
            {
                Data = sc,
                Code = sc == null ? -1 : 0
            });
        }



        public ActionResult CreateInquiry(InquiryModelRequest model)
        {

            var user = Request.Cookies[UserAuthorizationAttribute.CookieUserName]?.Value;

            var result = _inquiryService.Create(model, user);

            return Json(result);
        }

        public ActionResult Delete(int id = 0)
        {
            _inquiryService.Delete(id);

            return RedirectToAction("Index");
        }

    }
}