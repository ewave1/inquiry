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

        #region 材质


        #endregion

        #region 物性，顔色

        #endregion

        #region 比重

        #endregion

        #region 孔数

        #endregion

        #region 时数

        #endregion

        #region 效率

        #endregion


    }
}