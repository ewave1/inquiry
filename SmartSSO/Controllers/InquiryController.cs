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

        private readonly IMaterialService _iService = UnityHelper.Instance.Unity.Resolve<IMaterialService>();


        #endregion
        // GET: Inquiry
        public ActionResult Index(string CreateUser, string timeStart, string timeEnd, int page = 1)
        {
            var user = GetCurrentUser();

            var timeRange = SetTimeRange(timeStart, timeEnd);

            ViewBag.CreateUser = CreateUser;
            var result = _inquiryService.GetAll(user, CreateUser, timeRange.TimeStart, timeRange.TimeEnd, page);
            if (result == null)
                return    RedirectToAction("Login", "Home");
            ViewBag.IsAdmin = user.IsAdmin;


            return View(result);
        }
        public ActionResult Create(int?id )
        {
            ViewBag.Factories = GetDiscountNames( DisCountType.FACTORY);
            //ViewBag.M1 = GetDiscountNames(DisCountType.材料物性);
            //ViewBag.M2 = GetDiscountNames(DisCountType.表面物性);
            ViewBag.Materials = getMaterials();
            ViewBag.SealCodes = GetSealCodes();
             
            ViewBag.CustomerLevels = GetDiscountNames(DisCountType.客户级别);
            if (id > 0)
            {
                var log =   _inquiryService.Get(id);

            }

            return View(new InquiryModelRequest { MaterialCode ="NBR" });
        }

        public ActionResult RemoveInquiryData(DateTime start, DateTime end)
        {
            var result = _inquiryService.RemoveData(start, end);
            return Json(result); 
        }


        /// <summary>
        /// 获取 材质
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> getMaterials()
        {
            var lst = _inquiryService.Materials().Select(v => v.MaterialCode).Distinct().ToList().Select(v => new SelectListItem
            {
                Text = v,
                Value = v,

            }).ToList()
             ;
            return lst;
        }

        public ActionResult GetMaterials()
        {
            var lst = getMaterials();

            return Json(lst);
        }

        /// <summary>
        /// 获取明细数据
        /// </summary>
        /// <param name="MaterialCode"></param>
        /// <param name="Hardness"></param>
        /// <param name="getType"></param>
        /// <returns></returns>
        public ActionResult GetMaterialData(string MaterialCode,int? Hardness,MATERIALMODELTYPE getType )
        {
            var lst = _iService.GetMaterialDetailData(MaterialCode, Hardness, getType);
            return Json(lst);
        }
         
        /// <summary>
        /// 获取 折扣表的数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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