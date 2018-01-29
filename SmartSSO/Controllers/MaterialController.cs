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
        public ActionResult Index( int page = 1 )
        {  
            var result = _iservice.GetMaterialList( null,page );
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


        /// <summary>
        /// 上传基本数据
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadMaterial()
        {
            var user = GetCurrentUser();
            var uploadFile = _iservice.UploadMaterial(user?.UserName, Request);

            return Json(uploadFile);

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

        #region 物性，顔色


        // GET: Material
        public ActionResult MaterialFeatureList(int? MaterialId, MATERIALTYPE type = MATERIALTYPE.材料物性, int page = 1)
        {
            var result = _iservice.GetMaterialFeatures(MaterialId,type, page);
            if (result == null)
                return RedirectToAction("Login", "Home");
            ViewBag.Type = type.GetHashCode();
            ViewBag.Title = "特殊配方内列表";
            ViewBag.ColumnName = "特性";
            if (type == MATERIALTYPE.表面物性)
            {

                ViewBag.Title = "特殊处理外列表";
            }
            if (type == MATERIALTYPE.颜色)
            { 
                ViewBag.ColumnName = "颜色";
                ViewBag.Title = "颜色列表";
            }
            return View(result);
        }

        public ActionResult UpdateMaterialFeature(int? id, MATERIALTYPE type = MATERIALTYPE.材料物性)
        {
            var model = _iservice.GetMaterialFeature(id);
            if (model != null)
                type = model.Type;
            ViewBag.Type = type.GetHashCode();
            ViewBag.Title = "维护特殊配方内";
            ViewBag.ColumnName = "特性";
            if (type == MATERIALTYPE.表面物性)
            {
                ViewBag.Title = "维护特殊处理外";

            }
            if (type == MATERIALTYPE.颜色)
            { 
                ViewBag.ColumnName = "颜色";
                ViewBag.Title = "维护颜色数据";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public ActionResult UpdateMaterialFeature(MaterialFeatureModel model)
        {
            var user = GetCurrentUser();
            var result = _iservice.UpdateMaterialFeature(model, user?.UserName);
            if (result.Success)
                return RedirectToAction("MaterialFeatureList", new { type = result.Data?.Type });
            ModelState.AddModelError("_error", result.Msg);

            return View();
        }


        public ActionResult DeleteMaterialFeature(int id, MATERIALTYPE type = MATERIALTYPE.材料物性)
        {
            _iservice.DeleteMatialFeature(id);

            return RedirectToAction("MaterialFeatureList",new { type = type });

        }


        /// <summary>
        /// 上传孔数的数据
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadMaterialFeature(MATERIALTYPE type = MATERIALTYPE.材料物性)
        {
            var user = GetCurrentUser();
            var uploadFile = _iservice.UploadMaterialFeature(user?.UserName, Request,type);

            return Json(uploadFile);

        }

        #endregion

        #region 比重


        // GET: Material
        public ActionResult MaterialGravityList(int? MaterialId, int page = 1)
        {
            var result = _iservice.GetMaterialGravities(MaterialId, page);
            if (result == null)
                return RedirectToAction("Login", "Home");
            return View(result);
        }

        public ActionResult UpdateMaterialGravity(int? id)
        {
            var model = _iservice.GetMaterialGravity(id);

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public ActionResult UpdateMaterialGravity(MaterialGravityModel model)
        {
            var user = GetCurrentUser();
            var result = _iservice.UpdateMaterialGravity(model, user?.UserName);
            if (result.Success)
                return RedirectToAction("MaterialGravityList");
            ModelState.AddModelError("_error", result.Msg);

            return View();
        }


        public ActionResult DeleteMaterialGravity(int id)
        {
            _iservice.DeleteMatialGravity(id);

            return RedirectToAction("MaterialGravityList");

        }


        /// <summary>
        /// 上传孔数的数据
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadMaterialGravity()
        {
            var user = GetCurrentUser();
            var uploadFile = _iservice.UploadMaterialGravity(user?.UserName, Request);

            return Json(uploadFile);

        }
        #endregion


        #region 生产效率


        // GET: Material
        public ActionResult MaterialHourList(int? MaterialId, int page = 1)
        {
            var result = _iservice.GetMaterialHours(MaterialId, page);
            if (result == null)
                return RedirectToAction("Login", "Home");
            return View(result);
        }

        public ActionResult UpdateMaterialHour(int? id)
        {
            var model = _iservice.GetMaterialHour(id);

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public ActionResult UpdateMaterialHour(MaterialHourModel model)
        {
            var user = GetCurrentUser();
            var result = _iservice.UpdateMaterialHour(model, user?.UserName);
            if (result.Success)
                return RedirectToAction("MaterialHourList");
            ModelState.AddModelError("_error", result.Msg);

            return View();
        }


        public ActionResult DeleteMaterialHour(int id)
        {
            _iservice.DeleteMatialHour(id);

            return RedirectToAction("MaterialHourList");

        }


        /// <summary>
        /// 上传孔数的数据
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadMaterialHour()
        {
            var user = GetCurrentUser();
            var uploadFile = _iservice.UploadMaterialHour(user?.UserName, Request);

            return Json(uploadFile);

        }

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