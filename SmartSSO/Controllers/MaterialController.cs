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

        private readonly IInquiryService _inquiryService = UnityHelper.Instance.Unity.Resolve<IInquiryService>();

        #endregion

        #region 基本方法


        /// <summary>
        /// 获取 材质
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> GetMaterials()
        {
            var lst = _inquiryService.Materials().Select(v => v.MaterialCode).Distinct().ToList().Select(v => new SelectListItem
            {
                Text = v,
                Value = v,

            }).ToList()
             ;
            return lst;
        }

        /// <summary>
        /// 获取明细数据
        /// </summary>
        /// <param name="MaterialCode"></param>
        /// <param name="Hardness"></param>
        /// <param name="getType"></param>
        /// <returns></returns>
        public ActionResult GetMaterialData(string MaterialCode, int? Hardness, MATERIALMODELTYPE getType)
        {
            var lst = _iservice.GetMaterialDetailData(MaterialCode, Hardness, getType);
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

        #endregion

        #region 基本资料
        // GET: Material
        public ActionResult Index(int page = 1)
        {
            var result = _iservice.GetMaterialList(null, page);
            if (result == null)
                return RedirectToAction("Login", "Home");
            return View(result);
        }

        public ActionResult UpdateMaterial(int? id)
        {
            var model = _iservice.GetMaterial(id);

            var action = "维护";
            if (id == 0)
                action = "新增";
            ViewBag.Action = action;
            ViewBag.CommitName = model != null && model.Id > 0 ? "修改" : "创建";
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
            var result = _iservice.DeleteMatial(id);


            return Json(result);

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


        #region 基础孔数


        // GET: Base
        public ActionResult BaseHoleList(int? BaseId, int page = 1)
        {
            var result = _iservice.GetBaseHoles(page);
            if (result == null)
                return RedirectToAction("Login", "Home");
            return View(result);
        }

        public ActionResult LoadBaseHoles()
        {
            var lst = _iservice.GetBaseHoles(-1);

            return Json(lst);
        }

        public ActionResult UpdateBaseHole(int? id)
        {
            var model = _iservice.GetBaseHole(id);
            var action = "维护";
            if (id == 0)
                action = "新增";
            ViewBag.Action = action;
            ViewBag.CommitName = model != null && model.Id > 0 ? "修改" : "创建";
            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public ActionResult UpdateBaseHole(BaseHoleModel model)
        {
            var user = GetCurrentUser();
            var result = _iservice.UpdateBaseHole(model, user?.UserName);
            if (result.Success)
                return RedirectToAction("BaseHoleList");
            ModelState.AddModelError("_error", result.Msg);

            return View();
        }


        public ActionResult DeleteBaseHole(int id)
        {
            var result = _iservice.DeleteBaseHole(id);

            return Json(result);

        }


        /// <summary>
        /// 上传孔数的数据
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadBaseHole()
        {
            var user = GetCurrentUser();
            var uploadFile = _iservice.UploadBaseHole(user?.UserName, Request);

            return Json(uploadFile);

        }


        #endregion

        #region 孔数


        // GET: Material
        public ActionResult MaterialHoleList(int? MaterialId, int page = 1)
        {
            var result = _iservice.GetMaterialHoles(MaterialId, page);
            if (result == null)
                return RedirectToAction("Login", "Home");
            return View(result);
        }

        public ActionResult UpdateMaterialHole(int? id)
        {
            var model = _iservice.GetMaterialHole(id);

            var action = "维护";
            if (id == 0)
                action = "新增";
            ViewBag.Action = action;
            ViewBag.CommitName = model != null && model.Id > 0 ? "修改" : "创建";
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
            var result = _iservice.DeleteMatialHole(id);

            return Json(result);

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
            var result = _iservice.GetMaterialFeatures(MaterialId, type, page);
            if (result == null)
                return RedirectToAction("Login", "Home");
            ViewBag.Type = type.GetHashCode();
            ViewBag.Title = "材料物性";
            ViewBag.ColumnName = "特性";
            if (type == MATERIALTYPE.表面物性)
            {

                ViewBag.Title = "表面物性";
            }
            if (type == MATERIALTYPE.颜色)
            {
                ViewBag.ColumnName = "颜色";
                ViewBag.Title = "颜色";
            }
            ViewBag.ActionTitle = "新增" + ViewBag.Title;
            ViewBag.Title = ViewBag.Title + "列表";

            return View(result);
        }

        public ActionResult UpdateMaterialFeature(int? id, MATERIALTYPE type )
        {
            var model = _iservice.GetMaterialFeature(id);
            if (model != null)
                type = model.Type ;
            ViewBag.Type = type.GetHashCode();
            var action = "维护";
            if (id == 0)
                action = "新增";
            ViewBag.Title = "材料物性";
            ViewBag.ColumnName = "特性";
            if (type == MATERIALTYPE.表面物性 )
            {
                ViewBag.Title = "表面物性"; 
            }
            if (type == MATERIALTYPE.颜色 )
            {
                ViewBag.ColumnName = "颜色";
                ViewBag.Title = "颜色数据";
            }
            ViewBag.Title = action + ViewBag.Title;
            if (model == null)
                model = new MaterialFeatureModel { Type=type,Discount = 1 };
            ViewBag.CommitName = model != null && model.Id > 0 ? "修改" : "创建";
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
            var result = _iservice.DeleteMatialFeature(id);

            return Json(result);
            //     return RedirectToAction("MaterialFeatureList",new { type = type });

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
            var uploadFile = _iservice.UploadMaterialFeature(user?.UserName, Request, type);

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

            var action = "维护";
            if (id == 0)
                action = "新增"; 
            ViewBag.Action = action  ; 
            ViewBag.CommitName = model != null && model.Id > 0 ? "修改" : "创建";

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
            var result = _iservice.DeleteMatialGravity(id);


            return Json(result);
            //  return RedirectToAction("MaterialGravityList");

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

            var action = "维护";
            if (id == 0)
                action = "新增";
            ViewBag.Action = action;
            ViewBag.CommitName = model != null && model.Id > 0 ? "修改" : "创建";
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
            var result = _iservice.DeleteMatialHour(id);


            return Json(result);

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

            var action = "维护";
            if (id == 0)
                action = "新增";
            ViewBag.Action = action;
            ViewBag.CommitName = model != null && model.Id > 0 ? "修改" : "创建";
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
            var result = _iservice.DeleteMatialRate(id);

            return Json(result);

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