﻿using Common;
using Data.Entities;
using Data.Models;
using IServices;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PagedList;
using SmartSSO.Services.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Services
{
    public class MaterialService : ServiceContext, IMaterialService
    {

        #region 基本资料

        /// <summary>
        /// 获取 材料的基础明细数据
        /// </summary>
        /// <param name="MaterialCode"></param>
        /// <param name="Hardness"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public  List<NameValueModel >GetMaterialDetailData(string MaterialCode, int? Hardness, MATERIALMODELTYPE Type = MATERIALMODELTYPE.Hardness,string selData = null)
        {
            //获取硬度
            if (Type ==  MATERIALMODELTYPE.Hardness)
            {
                var hardness =    DbContext.Material.Where(v => v.MaterialCode == MaterialCode).Select(v => new { v.Hardness ,v.IsDefault}).Distinct().OrderBy(v=>v.Hardness).ToList();
                var lst =  hardness.Select(v => new NameValueModel {Name = v.Hardness.ToString(), IsDefault=v.IsDefault,Value = v.Hardness.ToString () }).ToList();

                var oldItem = lst.Where(v => v.IsDefault == true).FirstOrDefault();
                var selItem = lst.Where(v => v.Name == selData).FirstOrDefault();
                if(selItem!=null)
                {
                    if(oldItem!=null)
                    oldItem.IsDefault = false;
                    selItem.IsDefault = true;
                }

                return lst;
            }
            if(Type== MATERIALMODELTYPE.Material1)
            {
                var hardness = DbContext.MaterialFeature.Where(v => v.MaterialCode == MaterialCode&& v.Hardness== Hardness && v.Type== MATERIALTYPE.材料物性 ).Select(v => v.Name).Distinct().OrderBy(v => v).ToList();
                //hardness.Insert(0, Const.MaterialNormal);
                var lst = hardness.Select(v => new NameValueModel { Name = v.ToString(), Value = v.ToString() }).ToList();
                var selItem = lst.Where(v => v.Name == selData).FirstOrDefault();
                if (selItem != null)
                { 
                    selItem.IsDefault = true;
                }
                return lst;
            }
            if (Type == MATERIALMODELTYPE.Material2)
            {
                var hardness = DbContext.MaterialFeature.Where(v => v.MaterialCode == MaterialCode && v.Hardness == Hardness && v.Type == MATERIALTYPE.表面物性).Select(v => v.Name).Distinct().OrderBy(v => v).ToList();
                //hardness.Insert(0, Const.MaterialNormal);
                var lst =  hardness.Select(v => new NameValueModel { Name = v.ToString(), Value = v.ToString() }).ToList();
                var selItem = lst.Where(v => v.Name == selData).FirstOrDefault();
                if (selItem != null)
                {
                    selItem.IsDefault = true;
                }
                return lst;
            }
            if (Type == MATERIALMODELTYPE.Color)
            {
                var hardness = DbContext.MaterialFeature.Where(v => v.MaterialCode == MaterialCode && v.Hardness == Hardness && v.Type == MATERIALTYPE.颜色).Select(v =>new { v.Name, v.IsDefault }).Distinct().OrderBy(v => v.Name).ToList();
                var lst =  hardness.Select(v => new NameValueModel { Name = v.Name.ToString(),IsDefault = v.IsDefault, Value = v.Name.ToString() }).ToList();

                var oldItem = lst.Where(v => v.IsDefault == true).FirstOrDefault();
                var selItem = lst.Where(v => v.Name == selData).FirstOrDefault();
                if (selItem != null)
                {
                    if (oldItem != null)
                        oldItem.IsDefault = false;
                    selItem.IsDefault = true;
                }
                return lst;
            }

            throw new NotImplementedException();

        }
        public IPagedList<Material> GetMaterialList(DateTime dateStart, DateTime dateEnd, int? MaterialId, int page)
        {
            return DbContext.Material
                .Where(v=>v.UpdateTime>=dateStart&&v.UpdateTime<=dateEnd)
                .OrderBy(v => v.MaterialCode)
                .ThenBy(v=>v.Hardness) 
                .ToPagedList(page, Const.PageSize);
        }

        public   MaterialModel GetMaterial(int? id)
        {
            var model =     DbContext.Material.Find(id);
            if (model == null)
                return new MaterialModel {  };
            return new MaterialModel {
                Display = model.Display,
                MaterialCode = model.MaterialCode,
                Remark = model.Remark,
                SpecialDiscount = model.SpecialDiscount,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                UpdateUser = model.UpdateUser,
                Hardness = model.Hardness,
                Price = model.Price,
                IsDefault = model.IsDefault ,
                Code = model.Code,
            };
        }

        public RepResult<Material> UpdateMaterial(MaterialModel material, string User)
        {

            if (material.IsDefault)
            {
                DbContext.Database.ExecuteSqlCommand(" update Materials set IsDefault=0 where MaterialCode=@MaterialCode", new MySql.Data.MySqlClient.MySqlParameter("@MaterialCode", material.MaterialCode));
            }
            var original = DbContext.Material.Where(v => v.Id == material.Id).FirstOrDefault();
            if (original == null)
            {
                original = new Material ();
                DbContext.Material.Add(original);
            }
            if (!string.IsNullOrEmpty(material.Display))
                original.Display = material.Display;
            if (!string.IsNullOrEmpty(material.Remark))
                original.Remark = material.Remark;
            original.Hardness = material.Hardness;
            original.MaterialCode = material.MaterialCode;
            original.Price = material.Price;
            original.SpecialDiscount = material.SpecialDiscount;
            original.IsDefault = material.IsDefault ;
            original.Code = material.Code;
            original.UpdateTime = DateTime.Now;
            original.UpdateUser = User;
            DbContext.SaveChanges();


            return new RepResult<Material> { Data = original} ;
        }


        public RepResult<bool> DeleteMatial(int id)
        { 
            var model = DbContext.Material.Find(id);
            //判断是否已经使用
            if (model != null)
            {

                if (DbContext.MaterialFeature.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness).Count() > 0)
                    return new RepResult<bool> {  Code = -1,Msg = "当前数据已经被引用"};
                if (DbContext.MaterialGravity.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness).Count() > 0)
                    return new RepResult<bool> { Code = -1, Msg = "当前数据已经被引用" };
                if (DbContext.MaterialHole.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness).Count() > 0)
                    return new RepResult<bool> { Code = -1, Msg = "当前数据已经被引用" };
                if (DbContext.MaterialHour.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness).Count() > 0)
                    return new RepResult<bool> { Code = -1, Msg = "当前数据已经被引用" };
                if (DbContext.Storage.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness).Count() > 0)
                    return new RepResult<bool> { Code = -1, Msg = "当前数据已经被引用" };
                if (DbContext.InquiryLog.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness).Count() > 0)
                    return new RepResult<bool> { Code = -1, Msg = "当前数据已经被引用" };
                

                DbContext.Entry(model).State = EntityState.Deleted;
                return new RepResult<bool> { Data = DbContext.SaveChanges() > 0 };
            }
            return new RepResult<bool> { Code = -2, Msg = "数据不存在" };
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        public RepResult<Material> UploadMaterial(string User, HttpRequestBase Request)
        {

            UploadService service = new UploadService();
            var result = service.UploadFile(User, Request, FILETYPE.原料);
            if (!result.Success)//导入失败
                return new RepResult<Material> { Msg = result.Msg, Code = result.Code };
            var file = result.Data;


            //将数据保存到数据库
            var importItem = new PT_ImportHistory
            {
                User = User,
                ImportTime = DateTime.Now,
                ImportType = FILETYPE.原料,
                ImportFile = file.LocalPath,
                ImportFileName = file.FileName,

            };
            DbContext.PT_ImportHistory.Add(importItem);
            DbContext.SaveChanges();
            var details = ImportHelper.AddToImport(typeof(MaterialModel), importItem.Id, file.LocalPath);
            DbContext.PT_ImportHistoryDetail.AddRange(details);

            foreach (var item in details)
            {
                //保存结果
                SaveImportMaterailResult(item, importItem, User);
            }
            DbContext.SaveChanges();
            return new RepResult<Material> { Code = 0 };
        }

        /// <summary>
        /// 保存单笔结果 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="importItem"></param>
        /// <param name="User"></param>
        public void SaveImportMaterailResult(PT_ImportHistoryDetail item, PT_ImportHistory importItem, string User)
        {
            var relateItem = JsonConvert.DeserializeObject<MaterialModel>(item.Json);

            var material = DbContext.Material.Where(v => v.MaterialCode == relateItem.MaterialCode && v.Hardness == relateItem.Hardness).FirstOrDefault();

            if (material == null)
            {
                material = new Material { MaterialCode = relateItem.MaterialCode,Hardness = relateItem.Hardness};
                DbContext.Material.Add(material);
            }
            material.Price = relateItem.Price;
            if (!string.IsNullOrEmpty(relateItem.Display))
                material.Display = relateItem.Display;

            if(!string.IsNullOrEmpty(relateItem.Remark))
            material.Remark = relateItem.Remark;

            if (relateItem.SpecialDiscount > 0)
                material.SpecialDiscount = relateItem.SpecialDiscount;

            if (!string.IsNullOrEmpty(relateItem.Display))
                material.Display = relateItem.Display;
            material.IsDefault = relateItem.IsDefault  ;

            material.Code = relateItem.Code;
            material.UpdateTime = DateTime.Now;
            material.UpdateUser = User;
            DbContext.SaveChanges();
            item.IsSuccess = SuccessENUM.导入成功;
            item.RelateID = material.Id;
        }




        #endregion

        #region 特性


        public IPagedList<MaterialFeature> GetMaterialFeatures(DateTime dateStart, DateTime dateEnd, int? MaterialId, MATERIALTYPE type, int page)
        {
            return DbContext.MaterialFeature 
                .Where(v =>  v.UpdateTime >= dateStart && v.UpdateTime <= dateEnd&&(v.MaterialId == MaterialId || MaterialId == null) && v.Type==type)
                .OrderBy(v=>v.MaterialCode).ThenBy(v=>v.Hardness)
                .ThenBy(v=>v.Name) 
                  .ThenByDescending(p => p.UpdateTime).ToPagedList(page, Const.PageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        public RepResult<MaterialFeature> UploadMaterialFeature(string User, HttpRequestBase Request, MATERIALTYPE type = MATERIALTYPE.材料物性)
        {

            UploadService service = new UploadService();
            var fileType = FILETYPE.材料物性;
            if (type == MATERIALTYPE.表面物性)
                fileType = FILETYPE.表面物性;
            if (type == MATERIALTYPE.颜色)
                fileType = FILETYPE.颜色;
            var result = service.UploadFile(User, Request, fileType);
            if (!result.Success)//导入失败
                return new RepResult<MaterialFeature> { Msg = result.Msg, Code = result.Code };
            var file = result.Data;


            //将数据保存到数据库
            var importItem = new PT_ImportHistory
            {
                User = User,
                ImportTime = DateTime.Now,
                ImportType = fileType,
                ImportFile = file.LocalPath,
                ImportFileName = file.FileName,

            };
            DbContext.PT_ImportHistory.Add(importItem);
            DbContext.SaveChanges();
            var details = ImportHelper.AddToImport(typeof(MaterialFeatureModel), importItem.Id, file.LocalPath);
            DbContext.PT_ImportHistoryDetail.AddRange(details);

            foreach (var item in details)
            {
                //保存结果
                SaveImportMaterailFeatureResult(item, importItem, User,type );
            }
            DbContext.SaveChanges();
            return new RepResult<MaterialFeature> { Code = 0 };
        }

        /// <summary>
        /// 保存单笔结果 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="importItem"></param>
        /// <param name="User"></param>
        public void SaveImportMaterailFeatureResult(PT_ImportHistoryDetail item, PT_ImportHistory importItem, string User, MATERIALTYPE type )
        {
            var relateItem = JsonConvert.DeserializeObject<MaterialFeatureModel>(item.Json);

            var material = DbContext.Material.Where(v => v.MaterialCode == relateItem.MaterialCode && v.Hardness == relateItem.Hardness).FirstOrDefault();

            var feature = DbContext.MaterialFeature.Where(v => v.MaterialCode == relateItem.MaterialCode && v.Hardness == relateItem.Hardness && v.Type == type && v.Name == relateItem.Name).FirstOrDefault();
            if(feature==null)
            {
                feature = new MaterialFeature
                {
                    UpdateTime = DateTime.Now,
                    UpdateUser = User,
                    MaterialCode = relateItem.MaterialCode,
                    Hardness = relateItem.Hardness,
                    MaterialId = material == null ? 0 : material.Id,
                    Discount = relateItem.Discount,
                    Name = relateItem.Name,
                    IsDefault = relateItem.IsDefault ,
                    Code = relateItem.Code,
                    Type = type
                };
                DbContext.MaterialFeature.Add(feature);
            }
            else
            {
                feature.Discount = relateItem.Discount;
                feature.UpdateTime = DateTime.Now;
                feature.UpdateUser = User;
                feature.IsDefault = relateItem.IsDefault  ;
                feature.Code = relateItem.Code;
            }

            DbContext.SaveChanges();
            item.IsSuccess = SuccessENUM.导入成功;
            item.RelateID = feature.Id;
        }



        public RepResult<bool> DeleteMatialFeature(int Id)
        {
            var model = DbContext.MaterialFeature.Find(Id);
            if (model != null)
            {
                if (DbContext.InquiryLog.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness &&
                (v.Material1==model.Name && model.Type== MATERIALTYPE.材料物性 
                || v.Material2 == model.Name && model.Type == MATERIALTYPE.表面物性
                || v.Color == model.Name && model.Type == MATERIALTYPE.颜色
                )
                ).Count() > 0)
                    return new RepResult<bool> { Code = -1, Msg = "当前数据已经被引用" };
                if (DbContext.Storage.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness &&
                (v.Material1==model.Name && model.Type== MATERIALTYPE.材料物性 
                || v.Material2 == model.Name && model.Type == MATERIALTYPE.表面物性
                || v.Color == model.Name && model.Type == MATERIALTYPE.颜色
                )
                ).Count() > 0)
                    return new RepResult<bool> { Code = -1, Msg = "当前数据已经被引用" };
                DbContext.Entry(model).State = EntityState.Deleted;
                return new RepResult<bool> { Data = DbContext.SaveChanges() > 0 };
            }
            return new RepResult<bool> { Code = -1, Msg = "找不到数据" }; 
        }


        public RepResult<MaterialFeature> UpdateMaterialFeature(MaterialFeatureModel material, string User)
        {

            if (material.IsDefault)
            {
                DbContext.Database.ExecuteSqlCommand(" update MaterialFeatures set IsDefault=0 where MaterialCode=@MaterialCode and Type = @Type", new MySql.Data.MySqlClient.MySqlParameter("@MaterialCode", material.MaterialCode), new MySql.Data.MySqlClient.MySqlParameter("@Type", material.Type));
            }
            var original = DbContext.MaterialFeature.Where(v => v.Id == material.Id).FirstOrDefault();
            if (original == null)
            {
                //检查是否已经存在
                original=DbContext.MaterialFeature.Where(v => v.MaterialCode == material.MaterialCode && v.Hardness == material.Hardness && v.Type == material.Type && v.Name == material.Name).FirstOrDefault();

                if(original==null)
                {

                    original = new MaterialFeature
                    {
                        MaterialId = material.MaterialId,
                    };
                    DbContext.MaterialFeature.Add(original);
                }
            }
            original.Name = material.Name;
            original.MaterialCode = material.MaterialCode;
            original.Discount = material.Discount;
            original.Hardness = material.Hardness;
            original.MaterialId = material.MaterialId; 
            original.Type = material.Type;
            original.IsDefault = material.IsDefault ;
            original.Code = material.Code;
            original.UpdateTime = DateTime.Now;
            original.UpdateUser = User;
            DbContext.SaveChanges();
            return new RepResult<MaterialFeature> { Data = original };
        }

        public MaterialFeatureModel GetMaterialFeature(int? id)
        {
            var model = DbContext.MaterialFeature.Find(id);
            if (model == null)
                return null;
            return new MaterialFeatureModel
            {
                MaterialCode = model.MaterialCode,
                Hardness = model.Hardness,
                Name = model.Name,
                MaterialId = model.MaterialId,
                Discount = model.Discount,
                Type = model.Type,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                IsDefault = model.IsDefault  ,
                UpdateUser = model.UpdateUser,
                Code = model.Code,
            };
        }
       
        #endregion

        #region 比重

        public IPagedList<MaterialGravity> GetMaterialGravities(DateTime dateStart, DateTime dateEnd, int? MaterialId,int page)
        { 
            return DbContext.MaterialGravity
                .Where(v=> v.UpdateTime >= dateStart && v.UpdateTime <= dateEnd && (v.MaterialId == MaterialId || MaterialId==null))
                .OrderBy(v => v.MaterialCode).ThenBy(v => v.Hardness)
                .ThenBy(v=>v.Color)
                .ThenBy(v=>v.Gravity)
                  .ThenByDescending(p => p.UpdateTime).ToPagedList(page,Const.PageSize);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        public RepResult<MaterialGravity> UploadMaterialGravity(string User, HttpRequestBase Request)
        {

            UploadService service = new UploadService();
            var result = service.UploadFile(User, Request, FILETYPE.比重);
            if (!result.Success)//导入失败
                return new RepResult<MaterialGravity> { Msg = result.Msg, Code = result.Code };
            var file = result.Data;


            //将数据保存到数据库
            var importItem = new PT_ImportHistory
            {
                User = User,
                ImportTime = DateTime.Now,
                ImportType = FILETYPE.比重,
                ImportFile = file.LocalPath,
                ImportFileName = file.FileName, 
            };
            DbContext.PT_ImportHistory.Add(importItem);
            DbContext.SaveChanges();
            var details = ImportHelper.AddToImport(typeof(MaterialGravityModel), importItem.Id, file.LocalPath);
            DbContext.PT_ImportHistoryDetail.AddRange(details);

            foreach (var item in details)
            {
                //保存结果
                SaveImportMaterailGravityResult(item, importItem, User);
            }
            DbContext.SaveChanges();
            return new RepResult<MaterialGravity> { Code = 0 };
        }

        /// <summary>
        /// 保存单笔结果 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="importItem"></param>
        /// <param name="User"></param>
        public void SaveImportMaterailGravityResult(PT_ImportHistoryDetail item, PT_ImportHistory importItem, string User)
        {
            var relateItem = JsonConvert.DeserializeObject<MaterialGravityModel>(item.Json);

            var material = DbContext.Material.Where(v => v.MaterialCode == relateItem.MaterialCode && v.Hardness == relateItem.Hardness).FirstOrDefault();

            var gravity = DbContext.MaterialGravity.Where(v => v.MaterialCode == relateItem.MaterialCode && v.Hardness == relateItem.Hardness && v.Color == relateItem.Color).FirstOrDefault();
            if(gravity==null)
            {
                gravity = new MaterialGravity
                {
                    UpdateTime = DateTime.Now,
                    UpdateUser = User,
                    MaterialCode = relateItem.MaterialCode,
                    Hardness = relateItem.Hardness,
                    Gravity = relateItem.Gravity,
                    Color = relateItem.Color,
                    MaterialId = material == null ? 0 : material.Id
                };


                DbContext.MaterialGravity.Add(gravity);
            }
            else
            {
                gravity.Gravity = relateItem.Gravity;
                gravity.UpdateTime = DateTime.Now;
                gravity.UpdateUser = User;
            }

            
            DbContext.SaveChanges();
            item.IsSuccess = SuccessENUM.导入成功;
            item.RelateID = gravity.Id;
        }
        public RepResult<bool> DeleteMatialGravity(int Id)
        {
            var model = DbContext.MaterialGravity.Find(Id);
            if (model != null)
            {
                if (DbContext.InquiryLog.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness && v.Color== model.Color  ).Count() > 0)
                    return new RepResult<bool> { Code = -1, Msg = "当前数据已经被引用" };
                if (DbContext.Storage.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness && v.Color == model.Color).Count() > 0)
                    return new RepResult<bool> { Code = -1, Msg = "当前数据已经被引用" };
                DbContext.Entry(model).State = EntityState.Deleted;
                return new RepResult<bool> { Data = DbContext.SaveChanges() > 0 };
            }
            return new RepResult<bool> { Code = -1, Msg = "找不到数据" };
        }

        public RepResult<MaterialGravity> UpdateMaterialGravity(MaterialGravityModel material, string User)
        {

            var original = DbContext.MaterialGravity.Where(v => v.Id == material.Id).FirstOrDefault();

            if (original == null)
                //检查是否已经存在
                original = DbContext.MaterialGravity.Where(v => v.MaterialCode == material.MaterialCode && v.Hardness == material.Hardness && v.Color == material.Color).FirstOrDefault();
            if (original == null)
            {
                original = new MaterialGravity
                {
                    MaterialId = material.MaterialId,

                };
                DbContext.MaterialGravity.Add(original);
            }
            original.MaterialCode = material.MaterialCode;
            original.Color = material.Color;
            original.Hardness = material.Hardness;
            original.Gravity = material.Gravity;
            original.MaterialId = material.MaterialId;
            original.UpdateTime = DateTime.Now;
            original.UpdateUser = User;
            DbContext.SaveChanges();
            return new RepResult<MaterialGravity> { Data = original };
        }

        public MaterialGravityModel GetMaterialGravity(int? id)
        {
            var model = DbContext.MaterialGravity.Find(id);
            if (model == null)
                return new MaterialGravityModel { };
            return new MaterialGravityModel
            {
                MaterialCode = model.MaterialCode,
                Color = model.Color,
                Gravity = model.Gravity,
                Hardness = model.Hardness,
                MaterialId = model.MaterialId,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                UpdateUser = model.UpdateUser
            };
        }
        #endregion


        #region 基础孔数

        public IPagedList<BaseHole> GetBaseHoles(DateTime? dateStart, DateTime? dateEnd, int page)
        {
            if (page == -1)
                return DbContext.BaseHole .OrderBy(v=>v.SizeC).ToPagedList(1, 9999);
            return DbContext.BaseHole
                .Where(v => v.UpdateTime >= dateStart && v.UpdateTime <= dateEnd)
                  .OrderByDescending(p => p.SizeC).ToPagedList(page, Const.PageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        public RepResult<BaseHole> UploadBaseHole(string User, HttpRequestBase Request)
        {

            UploadService service = new UploadService();
            var result = service.UploadFile(User, Request, FILETYPE.孔数);
            if (!result.Success)//导入失败
                return new RepResult<BaseHole> { Msg = result.Msg, Code = result.Code };
            var file = result.Data;


            //将数据保存到数据库
            var importItem = new PT_ImportHistory
            {
                User = User,
                ImportTime = DateTime.Now,
                ImportType = FILETYPE.孔数,
                ImportFile = file.LocalPath,
                ImportFileName = file.FileName,

            };
            DbContext.PT_ImportHistory.Add(importItem);
            DbContext.SaveChanges();
            var details = ImportHelper.AddToImport(typeof(BaseHoleModel), importItem.Id, file.LocalPath);
            DbContext.PT_ImportHistoryDetail.AddRange(details);

            foreach (var item in details)
            {
                //保存结果
                SaveImportHoleResult(item, importItem, User);
            }
            DbContext.SaveChanges();
            return new RepResult<BaseHole> { Code = 0 };
        }

        /// <summary>
        /// 保存单笔结果 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="importItem"></param>
        /// <param name="User"></param>
        public void SaveImportHoleResult(PT_ImportHistoryDetail item, PT_ImportHistory importItem, string User)
        {
            var relateItem = JsonConvert.DeserializeObject<BaseHoleModel>(item.Json);
            var hole =  DbContext.BaseHole.Where(v => v.SizeC == relateItem.SizeC).FirstOrDefault();
            if (hole == null)
            {

                hole = new BaseHole
                {
                    UpdateTime = DateTime.Now,
                    UpdateUser = User,
                    HoleCount = relateItem.HoleCount,
                    SizeC = relateItem.SizeC,
                };

                DbContext.BaseHole.Add(hole);
            }
            else
            {
                hole.HoleCount = relateItem.HoleCount;
                hole.UpdateTime = DateTime.Now;
                hole.UpdateUser = User;

            }


            DbContext.SaveChanges();
            item.IsSuccess = SuccessENUM.导入成功;
            item.RelateID = hole.Id;
        }

        public RepResult<bool> DeleteBaseHole(int Id)
        {
            var model = DbContext.BaseHole.Find(Id);
            if (model != null)
            {
                if (DbContext.MaterialHole.Where(v => v.SizeC == model.SizeC).Count() > 0)
                    return new RepResult<bool> { Code = -1, Msg = "当前数据已经被引用" }; 
                DbContext.Entry(model).State = EntityState.Deleted;
                return new RepResult<bool> { Data = DbContext.SaveChanges() > 0 };
            }
            return new RepResult<bool> { Code = -1, Msg = "找不到数据" };
        }

        public RepResult<BaseHole> UpdateBaseHole(BaseHoleModel Base, string User)
        {
            var original = DbContext.BaseHole.Where(v => v.Id == Base.Id).FirstOrDefault();
            if (original == null) 
                //检查是否已经存在
                original = DbContext.BaseHole.Where(v => v.SizeC == Base.SizeC).FirstOrDefault();
                if (original == null)
            {
                original = new BaseHole
                { 
                };
                DbContext.BaseHole.Add(original);
            } 
            original.HoleCount = Base.HoleCount; 
            original.SizeC = Base.SizeC;
            original.UpdateTime = DateTime.Now;
            original.UpdateUser = User;
            DbContext.SaveChanges();
            return new RepResult<BaseHole> { Data = original };
        }

        public BaseHoleModel GetBaseHole(int? id)
        {
            var model = DbContext.BaseHole.Find(id);
            if (model == null)
                return new BaseHoleModel { };
            return new BaseHoleModel
            { 
                HoleCount = model.HoleCount,
                SizeC = model.SizeC,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                UpdateUser = model.UpdateUser
            };
        }
        #endregion

        #region 孔数

        public IPagedList<MaterialHole> GetMaterialHoles(DateTime dateStart, DateTime dateEnd, int? MaterialId, int page)
        {
            return DbContext.MaterialHole
                .Where(v => v.UpdateTime >= dateStart && v.UpdateTime <= dateEnd && (v.MaterialId == MaterialId || MaterialId == null))
                .OrderBy(v => v.MaterialCode).ThenBy(v => v.Hardness)
                .ThenBy(v=>v.SizeC)
                  .ThenByDescending(p => p.UpdateTime).ToPagedList(page, Const.PageSize);
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        public RepResult<MaterialHole> UploadMaterialHole(string User, HttpRequestBase Request)
        {

            UploadService service = new UploadService();
            var result = service.UploadFile(User, Request, FILETYPE.孔数);
            if (!result.Success)//导入失败
                return new RepResult<MaterialHole> { Msg = result.Msg, Code = result.Code };
            var file = result.Data;


            //将数据保存到数据库
            var importItem = new PT_ImportHistory
            {
                User = User,
                ImportTime = DateTime.Now,
                ImportType = FILETYPE.孔数,
                ImportFile = file.LocalPath,
                ImportFileName = file.FileName,

            };
            DbContext.PT_ImportHistory.Add(importItem);
            DbContext.SaveChanges();
            var details = ImportHelper.AddToImport(typeof(MaterialHoleModel), importItem.Id, file.LocalPath);
            DbContext.PT_ImportHistoryDetail.AddRange(details);

            foreach (var item in details)
            {
                //保存结果
                SaveImportMaterailHoleResult(item, importItem, User);
            }
            DbContext.SaveChanges();
            return new RepResult<MaterialHole> { Code = 0 };
        }

        /// <summary>
        /// 保存单笔结果 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="importItem"></param>
        /// <param name="User"></param>
        public void SaveImportMaterailHoleResult(PT_ImportHistoryDetail item, PT_ImportHistory importItem, string User)
        {
            var relateItem = JsonConvert.DeserializeObject<MaterialHoleModel>(item.Json);
            var material = DbContext.Material.Where(v => v.MaterialCode == relateItem.MaterialCode && v.Hardness == relateItem.Hardness).FirstOrDefault();

            var hole = DbContext.MaterialHole.Where(v => v.MaterialCode == relateItem.MaterialCode && v.Hardness == relateItem.Hardness && v.SizeC == relateItem.SizeC).FirstOrDefault();
             

            if(hole==null)
            {
                 hole = new MaterialHole
                {
                    UpdateTime = DateTime.Now,
                    UpdateUser = User,
                    Hardness = relateItem.Hardness,
                    SizeC2 = relateItem.SizeC2,
                    MaterialCode = relateItem.MaterialCode,
                    Rate = relateItem.Rate,
                    SizeC = relateItem.SizeC,
                    MaterialId = material == null ? 0 : material.Id
                };


                DbContext.MaterialHole.Add(hole);
            }
            else
            {
                hole.UpdateTime = DateTime.Now;
                hole.UpdateUser = User;
                hole.SizeC2 = relateItem.SizeC2;
                hole.Rate = relateItem.Rate;

            } 
            DbContext.SaveChanges();
            item.IsSuccess = SuccessENUM.导入成功;
            item.RelateID = hole.Id;
        }

        public RepResult<bool> DeleteMatialHole(int Id)
        {
            var model = DbContext.MaterialHole.Find(Id);
            if (model != null)
            {
                if (DbContext.InquiryLog.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness && (v.SizeA+ v.SizeB)== model.SizeC).Count() > 0)
                    return new RepResult<bool> { Code = -1, Msg = "当前数据已经被引用" };
                if (DbContext.Storage.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness && (v.SizeA + v.SizeB) == model.SizeC).Count() > 0)
                    return new RepResult<bool> { Code = -1, Msg = "当前数据已经被引用" };
                DbContext.Entry(model).State = EntityState.Deleted;
                return new RepResult<bool> { Data = DbContext.SaveChanges() > 0 };
            }
            return new RepResult<bool> { Code = -1, Msg = "找不到数据" };
        }

        public RepResult<MaterialHole> UpdateMaterialHole(MaterialHoleModel material, string User)
        {
            var original = DbContext.MaterialHole.Where(v => v.Id == material.Id).FirstOrDefault();
            if (original == null)
                //检查是否已经存在
                original = DbContext.MaterialHole.Where(v => v.MaterialCode == material.MaterialCode && v.Hardness == material.Hardness && v.SizeC == material.SizeC).FirstOrDefault();
            if (original == null)
            {
                original = new MaterialHole
                {
                    MaterialId = material.MaterialId,

                };
                DbContext.MaterialHole.Add(original);
            }
            original.MaterialCode = material.MaterialCode;
            original.Hardness = material.Hardness;
            original.Rate = material.Rate; 
            original.SizeC2 = material.SizeC2;
            original.MaterialId = material.MaterialId;
            original.SizeC = material.SizeC;
            original.UpdateTime = DateTime.Now;
            original.UpdateUser = User;
            DbContext.SaveChanges();
            return new RepResult<MaterialHole> { Data = original };
        }

        public MaterialHoleModel GetMaterialHole(int? id)
        {
            var model = DbContext.MaterialHole.Find(id);
            if (model == null)
                return new MaterialHoleModel { Rate=1 };
            return new MaterialHoleModel
            {
                MaterialCode = model.MaterialCode,
                Hardness = model.Hardness,
                Rate = model.Rate,
                MaterialId = model.MaterialId,
                SizeC2 = model.SizeC2,
                SizeC = model.SizeC,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                UpdateUser = model.UpdateUser
            };
        }
        #endregion
        
        #region 模数 生产效率

        public IPagedList<MaterialHour> GetMaterialHours(DateTime dateStart, DateTime dateEnd, int? MaterialId, int page)
        {
            return DbContext.MaterialHour
                .Where(v => v.UpdateTime >= dateStart && v.UpdateTime <= dateEnd && (v.MaterialId == MaterialId || MaterialId == null))
                .OrderBy(v => v.MaterialCode).ThenBy(v => v.Hardness)
                .ThenBy(v=>v.MosInHour)
                  .ThenByDescending(p => p.UpdateTime).ToPagedList(page, Const.PageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        public RepResult<MaterialHour> UploadMaterialHour(string User, HttpRequestBase Request)
        {

            UploadService service = new UploadService();
            var result = service.UploadFile(User, Request, FILETYPE.生产效率);
            if (!result.Success)//导入失败
                return new RepResult<MaterialHour> { Msg = result.Msg, Code = result.Code };
            var file = result.Data;


            //将数据保存到数据库
            var importItem = new PT_ImportHistory
            {
                User = User,
                ImportTime = DateTime.Now,
                ImportType = FILETYPE.生产效率,
                ImportFile = file.LocalPath,
                ImportFileName = file.FileName,

            };
            DbContext.PT_ImportHistory.Add(importItem);
            DbContext.SaveChanges();
            var details = ImportHelper.AddToImport(typeof(MaterialHourModel), importItem.Id, file.LocalPath);
            DbContext.PT_ImportHistoryDetail.AddRange(details);

            foreach (var item in details)
            {
                //保存结果
                SaveImportMaterailHourResult(item, importItem, User);
            }
            DbContext.SaveChanges();
            return new RepResult<MaterialHour> { Code = 0 };
        }

        /// <summary>
        /// 保存单笔结果 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="importItem"></param>
        /// <param name="User"></param>
        public void SaveImportMaterailHourResult(PT_ImportHistoryDetail item, PT_ImportHistory importItem, string User)
        {
            var relateItem = JsonConvert.DeserializeObject<MaterialHourModel>(item.Json);

            var material = DbContext.Material.Where(v => v.MaterialCode == relateItem.MaterialCode && v.Hardness == relateItem.Hardness).FirstOrDefault();
            var hour = DbContext.MaterialHour.Where(v => v.MaterialCode == relateItem.MaterialCode && v.Hardness == relateItem.Hardness && v.SizeB == relateItem.SizeB && v.SizeB2 == relateItem.SizeB2).FirstOrDefault();

            if(hour ==null)
            {
                hour = new MaterialHour
                {
                    UpdateTime = DateTime.Now,
                    UpdateUser = User,
                    SizeB = relateItem.SizeB,
                    SizeB2 = relateItem.SizeB2,
                    MaterialCode = relateItem.MaterialCode,
                    Hardness = relateItem.Hardness,
                    MosInHour = relateItem.MosInHour,
                    MaterialId = material == null ? 0 : material.Id
                };


                DbContext.MaterialHour.Add(hour);

            }
            else
            {
                hour.UpdateTime = DateTime.Now;
                hour.UpdateUser = User;
                hour.MosInHour = relateItem.MosInHour;
            }
            DbContext.SaveChanges();
            item.IsSuccess = SuccessENUM.导入成功;
            item.RelateID = hour.Id;
        }

        public RepResult<bool> DeleteMatialHour(int Id)
        {
            var model = DbContext.MaterialHour.Find(Id);
            if (model != null)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
                return new RepResult<bool> { Data = DbContext.SaveChanges() > 0 };
            }
            return new RepResult<bool> { Code = -1, Msg = "找不到数据" };
        }
        public RepResult<MaterialHour> UpdateMaterialHour(MaterialHourModel material, string User)
        {
            var original = DbContext.MaterialHour.Where(v => v.Id == material.Id).FirstOrDefault();
            if(original==null )
                //检查是否已经存在
                original = DbContext.MaterialHour.Where(v => v.MaterialCode == material.MaterialCode && v.Hardness == material.Hardness && v.SizeB == material.SizeB && v.SizeB2 == material.SizeB2).FirstOrDefault();
            if (original == null)
            {
                original = new MaterialHour
                {
                    MaterialId = material.MaterialId, 
                };
                DbContext.MaterialHour.Add(original);
            }
            original.Hardness = material.Hardness;
            original.MaterialCode = material.MaterialCode;
            original.SizeB = material.SizeB;
            original.SizeB2 = material.SizeB2; 
            original.MosInHour = material.MosInHour;
            original.MaterialId = material.MaterialId;
            original.UpdateTime = DateTime.Now;
            original.UpdateUser = User;
            DbContext.SaveChanges();
            return new RepResult<MaterialHour> { Data = original };
        }

        public MaterialHourModel GetMaterialHour(int? id)
        {
            var model = DbContext.MaterialHour.Find(id);
            if (model == null)
                return new MaterialHourModel { };
            return new MaterialHourModel
            {
                MaterialCode = model.MaterialCode,
                Hardness = model.Hardness,
                SizeB2 = model.SizeB2,
                MosInHour = model.MosInHour,
                MaterialId = model.MaterialId,
                SizeB = model.SizeB,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                UpdateUser = model.UpdateUser
            };
        }
        #endregion

        #region 不良率

        public IPagedList<MaterialRate> GetMaterialRates(DateTime dateStart, DateTime dateEnd, int? MaterialId, int page)
        {
            return DbContext.MaterialRate
                .Where(v => v.UpdateTime >= dateStart && v.UpdateTime <= dateEnd)
                  .OrderBy(p => p.SizeB).ToPagedList(page, Const.PageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        public RepResult<MaterialRate> UploadMaterialRate(string User, HttpRequestBase Request)
        {

            UploadService service = new UploadService();
            var result = service.UploadFile(User, Request, FILETYPE.不良率);
            if (!result.Success)//导入失败
                return new RepResult<MaterialRate> { Msg = result.Msg, Code = result.Code };
            var file = result.Data;


            //将数据保存到数据库
            var importItem = new PT_ImportHistory
            {
                User = User,
                ImportTime = DateTime.Now,
                ImportType = FILETYPE.不良率,
                ImportFile = file.LocalPath,
                ImportFileName = file.FileName,

            };
            DbContext.PT_ImportHistory.Add(importItem);
            DbContext.SaveChanges();
            var details = ImportHelper.AddToImport(typeof(MaterialRateModel), importItem.Id, file.LocalPath);
            DbContext.PT_ImportHistoryDetail.AddRange(details);

            foreach (var item in details)
            {
                //保存结果
                SaveImportMaterailRateResult(item, importItem, User);
            }
            DbContext.SaveChanges();
            return new RepResult<MaterialRate> { Code = 0 };
        }

        /// <summary>
        /// 保存单笔结果 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="importItem"></param>
        /// <param name="User"></param>
        public void SaveImportMaterailRateResult(PT_ImportHistoryDetail item, PT_ImportHistory importItem, string User)
        {
            var relateItem = JsonConvert.DeserializeObject<MaterialRateModel>(item.Json);

            var rate = DbContext.MaterialRate.Where(v => v.SizeB == relateItem.SizeB && v.SizeB2 == relateItem.SizeB2).FirstOrDefault();
            if(rate==null)
            { 
                rate = new MaterialRate
                {
                    UpdateTime = DateTime.Now,
                    UpdateUser = User,
                    BadRate = relateItem.BadRate,
                    SizeB = relateItem.SizeB,
                    SizeB2 = relateItem.SizeB2,
                    UseRate = relateItem.UseRate,

                };


                DbContext.MaterialRate.Add(rate);
            }
            else
            {
                rate.BadRate = relateItem.BadRate;
                rate.UseRate = relateItem.UseRate;
                rate.UpdateTime = DateTime.Now;
                rate.UpdateUser = User;
            }
            DbContext.SaveChanges();
            item.IsSuccess = SuccessENUM.导入成功;
            item.RelateID = rate.Id;
        }

        public RepResult<bool> DeleteMatialRate(int Id)
        {
            var model = DbContext.MaterialRate.Find(Id);
            if (model != null)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
                return new RepResult<bool> { Data = DbContext.SaveChanges() > 0 };
            }
            return new RepResult<bool> { Code = -1, Msg = "找不到数据" };
        }

        public RepResult<MaterialRate> UpdateMaterialRate(MaterialRateModel material, string User)
        {
            var original = DbContext.MaterialRate.Where(v => v.Id == material.Id).FirstOrDefault();
            if (original == null)
            {

                original = DbContext.MaterialRate.Where(v => v.SizeB == material.SizeB && v.SizeB2 == material.SizeB2).FirstOrDefault();
                if(original==null)
                { 
                    original = new MaterialRate();
                    DbContext.MaterialRate.Add(original);
                }
            }
            original.SizeB2 = material.SizeB2;
            original.SizeB = material.SizeB;
            original.UseRate = material.UseRate;
            original.BadRate = material.BadRate;
            original.UpdateTime = DateTime.Now;
            original.UpdateUser = User;
            DbContext.SaveChanges();
            return new RepResult<MaterialRate> { Data = original };
        }

        public MaterialRateModel GetMaterialRate(int? id)
        {
            var model = DbContext.MaterialRate.Find(id);
            if (model == null)
                return new MaterialRateModel { };
            return new MaterialRateModel
            {
                SizeB2 = model.SizeB2,
                BadRate = model.BadRate,
                SizeB = model.SizeB,
                UseRate = model.UseRate,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                UpdateUser = model.UpdateUser
            };
        }

        #endregion

        #region 起订金额

        public IPagedList<MaterialStartAmount> GetMaterialStartAmount(DateTime dateStart, DateTime dateEnd, int? MaterialId, int page)
        {
            return DbContext.MaterialStartAmount
                .Where(v => v.UpdateTime >= dateStart && v.UpdateTime <= dateEnd)
                .OrderBy(p=>p.StorageType)
                  .ThenBy(p => p.SizeC).ToPagedList(page, Const.PageSize);
        }

        public RepResult<MaterialStartAmount> UploadMaterialStartAmount(string User, HttpRequestBase Request)
        {
            UploadService service = new UploadService();
            var result = service.UploadFile(User, Request, FILETYPE.起订金额);
            if (!result.Success)//导入失败
                return new RepResult<MaterialStartAmount> { Msg = result.Msg, Code = result.Code };
            var file = result.Data;


            //将数据保存到数据库
            var importItem = new PT_ImportHistory
            {
                User = User,
                ImportTime = DateTime.Now,
                ImportType = FILETYPE.起订金额,
                ImportFile = file.LocalPath,
                ImportFileName = file.FileName,

            };
            DbContext.PT_ImportHistory.Add(importItem);
            DbContext.SaveChanges();
            var details = ImportHelper.AddToImport(typeof(MaterialStartAmountModel), importItem.Id, file.LocalPath);
            DbContext.PT_ImportHistoryDetail.AddRange(details);

            foreach (var item in details)
            {
                //保存结果
                SaveImportMaterailStartAmountResult(item, importItem, User);
            }
            DbContext.SaveChanges();
            return new RepResult<MaterialStartAmount> { Code = 0 };
        }

        /// <summary>
        /// 保存单笔结果 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="importItem"></param>
        /// <param name="User"></param>
        public void SaveImportMaterailStartAmountResult(PT_ImportHistoryDetail item, PT_ImportHistory importItem, string User)
        {
            var relateItem = JsonConvert.DeserializeObject<MaterialStartAmountModel>(item.Json);

            var rate = DbContext.MaterialStartAmount.Where(v =>v.StorageType==relateItem.StorageType&& v.SizeC == relateItem.SizeC && v.SizeC2 == relateItem.SizeC2).FirstOrDefault();
            if (rate == null)
            {
                rate = new MaterialStartAmount
                {
                    UpdateTime = DateTime.Now,
                    UpdateUser = User,
                    SizeC = relateItem.SizeC,
                    SizeC2 = relateItem.SizeC2,
                    StorageType = relateItem.StorageType,
                    StartAmount = relateItem.StartAmount,

                };


                DbContext.MaterialStartAmount.Add(rate);
            }
            else
            {
                rate.StartAmount = relateItem.StartAmount;
                rate.StorageType = relateItem.StorageType;
                rate.UpdateTime = DateTime.Now;
                rate.UpdateUser = User;
            }
            DbContext.SaveChanges();
            item.IsSuccess = SuccessENUM.导入成功;
            item.RelateID = rate.Id;
        }

        public RepResult<bool> DeleteMatialStartAmount(int Id)
        {
            var model = DbContext.MaterialStartAmount.Find(Id);
            if (model != null)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
                return new RepResult<bool> { Data = DbContext.SaveChanges() > 0 };
            }
            return new RepResult<bool> { Code = -1, Msg = "找不到数据" };
        }

        public RepResult<MaterialStartAmount> UpdateMaterialStartAmount(MaterialStartAmountModel material, string User)
        {
            var original = DbContext.MaterialStartAmount.Where(v => v.Id == material.Id).FirstOrDefault();
            if (original == null)
            {

                original = DbContext.MaterialStartAmount.Where(v => v.StorageType == material.StorageType && v.SizeC == material.SizeC && v.SizeC2 == material.SizeC2).FirstOrDefault();
                if (original == null)
                {
                    original = new MaterialStartAmount();
                    DbContext.MaterialStartAmount.Add(original);
                }
            }
            original.StorageType = material.StorageType;
            original.SizeC2 = material.SizeC2;
            original.SizeC = material.SizeC;
            original.StartAmount = material.StartAmount;
            original.UpdateTime = DateTime.Now;
            original.UpdateUser = User;
            DbContext.SaveChanges();
            return new RepResult<MaterialStartAmount> { Data = original };
        }

        public MaterialStartAmountModel GetMaterialStartAmount(int? id)
        {
            var model = DbContext.MaterialStartAmount.Find(id);
            if (model == null)
                return new MaterialStartAmountModel { };
            return new MaterialStartAmountModel
            {
                SizeC = model.SizeC,
                SizeC2 = model.SizeC2,
                StartAmount = model.StartAmount,
                StorageType = model.StorageType,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                UpdateUser = model.UpdateUser
            };
        }

        public RepResult<bool> RemoveAllMatertailStartAmount()
        {
            DbContext.Database.ExecuteSqlCommand("delete from materialStartamounts ");

            return new RepResult<bool> { Data = true };
        }
        #endregion 

        #region 模具

        public IPagedList<MaterialStorage> GetMaterialStorage(DateTime dateStart, DateTime dateEnd, int? MaterialId, int page)
        {
            return DbContext.MaterialStorage
                .Where(v => v.UpdateTime >= dateStart && v.UpdateTime <= dateEnd) 
                  .OrderBy(p => p.Spec).ToPagedList(page, Const.PageSize);
        }

        public RepResult<MaterialStorage> UploadMaterialStorage(string User, HttpRequestBase Request)
        {
            UploadService service = new UploadService();
            var result = service.UploadFile(User, Request, FILETYPE.模具);
            if (!result.Success)//导入失败
                return new RepResult<MaterialStorage> { Msg = result.Msg, Code = result.Code };
            var file = result.Data;


            //将数据保存到数据库
            var importItem = new PT_ImportHistory
            {
                User = User,
                ImportTime = DateTime.Now,
                ImportType = FILETYPE.模具,
                ImportFile = file.LocalPath,
                ImportFileName = file.FileName,

            };
            DbContext.PT_ImportHistory.Add(importItem);
            DbContext.SaveChanges();
            var details = ImportHelper.AddToImport(typeof(MaterialStorageModel), importItem.Id, file.LocalPath);
            DbContext.PT_ImportHistoryDetail.AddRange(details);

            foreach (var item in details)
            {
                //保存结果
                SaveImportMaterailStorageResult(item, importItem, User);
            }
            DbContext.SaveChanges();
            return new RepResult<MaterialStorage> { Code = 0 };
        }
         

        /// <summary>
        /// 保存单笔结果 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="importItem"></param>
        /// <param name="User"></param>
        public void SaveImportMaterailStorageResult(PT_ImportHistoryDetail item, PT_ImportHistory importItem, string User)
        {
            var relateItem = JsonConvert.DeserializeObject<MaterialStorageModel>(item.Json);

            var rate = DbContext.MaterialStorage.Where(v => v.BatchNo == relateItem.BatchNo).FirstOrDefault();
            #region 规格数据
            var dsizeA = 0M;
            var dsizeB = 0M;
            var errorInfo = CommonHelper. GetSizeAB(relateItem.Spec, out dsizeA, out dsizeB);
            if (!string.IsNullOrEmpty(errorInfo))
            {
                item.ErrorInfo = "规格数据异常";
                item.IsSuccess = SuccessENUM.导入失败;
                return;
            } 
            #endregion
            if (rate == null)
            {
                rate = new MaterialStorage
                {
                    UpdateTime = DateTime.Now,
                    UpdateUser = User,
                    BatchNo = relateItem.BatchNo, 
                    Location = relateItem.Location,
                    Spec = relateItem.Spec,
                    Spec2= relateItem.Spec2,
                    SizeA = dsizeA,
                    SizeB = dsizeB,
                    Remark = relateItem.Remark, 
                };


                DbContext.MaterialStorage.Add(rate);
            }
            else
            {
                rate.BatchNo = relateItem.BatchNo;
                rate.Location = relateItem.Location;
                rate.SizeA = dsizeA;
                rate.SizeB = dsizeB;
                rate.Remark = relateItem.Remark;
                rate.Spec = rate.Spec;
                rate.Spec2 = rate.Spec2;
                rate.UpdateTime = DateTime.Now;
                rate.UpdateUser = User;
            }
            DbContext.SaveChanges();
            item.IsSuccess = SuccessENUM.导入成功;
            item.RelateID = rate.Id;
        }

        public RepResult<bool> DeleteMatialStorage(int Id)
        {
            var model = DbContext.MaterialStorage.Find(Id);
            if (model != null)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
                return new RepResult<bool> { Data = DbContext.SaveChanges() > 0 };
            }
            return new RepResult<bool> { Code = -1, Msg = "找不到数据" };
        }

        public RepResult<MaterialStorage> UpdateMaterialStorage(MaterialStorageModel material, string User)
        {
            var original = DbContext.MaterialStorage.Where(v => v.Id == material.Id).FirstOrDefault();
            #region 规格数据
            var dsizeA = 0M;
            var dsizeB = 0M;
            var errorInfo = CommonHelper.GetSizeAB(material.Spec, out dsizeA, out dsizeB);
            if (!string.IsNullOrEmpty(errorInfo))
            {
                return new RepResult<MaterialStorage> {  Code  =-2,Msg = errorInfo};
            }
            #endregion
            if (original == null)
            {

                original = DbContext.MaterialStorage.Where(v => v.BatchNo == material.BatchNo).FirstOrDefault();
                if (original == null)
                {
                    original = new MaterialStorage();
                    DbContext.MaterialStorage.Add(original);
                }
            }
            original.BatchNo = material.BatchNo;
            original.Spec = material.Spec;
            original.Spec2 = material.Spec2;
            original.SizeA = dsizeA;
            original.SizeB = dsizeB;
            original.Remark = material.Remark;

            original.UpdateTime = DateTime.Now;
            original.UpdateUser = User;
            DbContext.SaveChanges();
            return new RepResult<MaterialStorage> { Data = original };
        }

        public MaterialStorageModel GetMaterialStorage(int? id)
        {
            var model = DbContext.MaterialStorage.Find(id);
            if (model == null)
                return new MaterialStorageModel { };
            return new MaterialStorageModel
            {
                Spec = model.Spec,
                Spec2 = model.Spec2,
                BatchNo = model.BatchNo,
                Location = model.Location,
                Remark = model.Remark ,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                UpdateUser = model.UpdateUser
            };
        }

        public RepResult<bool> RemoveAllMatertailStorage()
        {
            DbContext.Database.ExecuteSqlCommand("delete from materialStorages ");

            return new RepResult<bool> { Data = true };
        }
        #endregion

        #region 删除所有
        public RepResult<bool> RemoveAllMatertail()
        {
            if (DbContext.MaterialFeature.Count() > 0)
                return new RepResult<bool> { Code = -1, Msg = "请先清除其它基础数据" };
            if (DbContext.MaterialGravity.Count() > 0)
                return new RepResult<bool> { Code = -1, Msg = "请先清除其它基础数据" };
            if (DbContext.MaterialHole.Count() > 0)
                return new RepResult<bool> { Code = -1, Msg = "请先清除其它基础数据" };
            if (DbContext.MaterialHour.Count() > 0)
                return new RepResult<bool> { Code = -1, Msg = "请先清除其它基础数据" };
            if (DbContext.Storage.Count() > 0)
                return new RepResult<bool> { Code = -1, Msg = "请先清除其它基础数据" };
            if (DbContext.InquiryLog.Count() > 0)
                return new RepResult<bool> { Code = -1, Msg = "请先清除其它基础数据" };
            DbContext.Database.ExecuteSqlCommand("delete from materials ");

            return new RepResult<bool> { Data = true };
        }

        public RepResult<bool> RemoveAllMatertailFeature(MATERIALTYPE? type)
        {
            DbContext.Database.ExecuteSqlCommand("delete from materialfeatures where type=@type  ", new MySqlParameter("@type", type));

            return new RepResult<bool> { Data = true };
        }

        public RepResult<bool> RemoveAllMatertailGravity()
        {
            DbContext.Database.ExecuteSqlCommand("delete from  materialgravities ");

            return new RepResult<bool> { Data = true };
        }

        public RepResult<bool> RemoveAllMatertailHole()
        {
            DbContext.Database.ExecuteSqlCommand("delete from materialholes ");

            return new RepResult<bool> { Data = true };
        }

        public RepResult<bool> RemoveAllBaseHole()
        {
            DbContext.Database.ExecuteSqlCommand("delete from baseholes ");

            return new RepResult<bool> { Data = true };
        }

        public RepResult<bool> RemoveAllMatertailHour()
        {
            DbContext.Database.ExecuteSqlCommand("delete from materialhours ");

            return new RepResult<bool> { Data = true };
        }

        public RepResult<bool> RemoveAllMatertailRate()
        {
            DbContext.Database.ExecuteSqlCommand("delete from materialrates ");

            return new RepResult<bool> { Data = true };
        }
        #endregion
    }
}
