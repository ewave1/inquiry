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
    /// 
    /// </summary>
    public class MaterialController : BaseController
    {

        #region 私有字段
         

        private readonly IMaterialService _iservice = UnityHelper.Instance.Unity.Resolve<IMaterialService>();

        #endregion

        #region 基本资料
        // GET: Material
        public ActionResult Index( )
        {  
            var result = _iservice.GetMaterialList( );
            if (result == null)
                return RedirectToAction("Login", "Home"); 
            return View(result);
        }

        public ActionResult UpdateMaterial(int? id)
        {
            var model = _iservice.GetMaterial(id);

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public ActionResult UpdateMaterial(MaterialModel model)
        {
            var user = GetCurrentUser();
            var result = _iservice.UpdateMaterial(model, user?.UserName);
            if (result.Success)
                return RedirectToAction("Index");
            ModelState.AddModelError("_error", result.Msg);
             
            return View();
        }


        public ActionResult DeleteMaterial(int id)
        {
            _iservice.DeleteMatial(id);

            return RedirectToAction("Index");

        }

        #endregion


        #region 孔数


        // GET: Material
        public ActionResult MaterialHoleList(int ?MaterialId,int page=1)
        {
            var result = _iservice.GetMaterialHoles(MaterialId,page);
            if (result == null)
                return RedirectToAction("Login", "Home");
            return View(result);
        }

        public ActionResult UpdateMaterialHole(int? id)
        {
            var model = _iservice.GetMaterialHole(id);

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public ActionResult UpdateMaterialHole(MaterialHoleModel model)
        {
            var user = GetCurrentUser();
            var result = _iservice.UpdateMaterialHole(model, user?.UserName);
            if (result.Success)
                return RedirectToAction("MaterialHoleList");
            ModelState.AddModelError("_error", result.Msg);

            return View();
        }


        public ActionResult DeleteMaterialHole(int id)
        {
            _iservice.DeleteMatialHole(id);

            return RedirectToAction("MaterialHoleList");

        }


        /// <summary>
        /// 上传孔数的数据
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadMaterialHole()
        {
            var user = GetCurrentUser();
            var uploadFile = _iservice.UploadMaterialHole(user?.UserName, Request);

            return Json(uploadFile);

        }

         
        #endregion
        #region 材质

        public ActionResult MaterialFeatureDetail(int? materialId,int page)
        {
            var result = _iservice.GetMaterialFeatures(materialId,page);
            if (result == null)
                return RedirectToAction("Login", "Home");
            return View(result);
        }

        #endregion

        #region 物性，顔色

        #endregion

        #region 比重

        #endregion


        #region 时数

        #endregion

        #region 不良率


        // GET: Material
        public ActionResult MaterialRateList(int? MaterialId, int page = 1)
        {
            var result = _iservice.GetMaterialRates(MaterialId, page);
            if (result == null)
                return RedirectToAction("Login", "Home");
            return View(result);
        }

        public ActionResult UpdateMaterialRate(int? id)
        {
            var model = _iservice.GetMaterialRate(id);

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public ActionResult UpdateMaterialRate(MaterialRateModel model)
        {
            var user = GetCurrentUser();
            var result = _iservice.UpdateMaterialRate(model, user?.UserName);
            if (result.Success)
                return RedirectToAction("MaterialRateList");
            ModelState.AddModelError("_error", result.Msg);

            return View();
        }


        public ActionResult DeleteMaterialRate(int id)
        {
            _iservice.DeleteMatialRate(id);

            return RedirectToAction("MaterialHoleList");

        }


        /// <summary>
        ///  
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadMaterialRate()
        {
            var user = GetCurrentUser();
            var uploadFile = _iservice.UploadMaterialRate(user?.UserName, Request);

            return Json(uploadFile);

        }


        #endregion


    }
}