using Common;
using Data.Entities;
using Data.Models;
using IServices;
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
        public  List<NameValueModel >GetMaterialDetailData(string MaterialCode, int? Hardness, MATERIALMODELTYPE Type = MATERIALMODELTYPE.Hardness)
        {
            //获取硬度
            if (Type ==  MATERIALMODELTYPE.Hardness)
            {
                var hardness =    DbContext.Material.Where(v => v.MaterialCode == MaterialCode).Select(v => v.Hardness).Distinct().ToList();
                return hardness.Select(v => new NameValueModel {Name = v.ToString(),Value = v.ToString () }).ToList();
            }
            if(Type== MATERIALMODELTYPE.Material1)
            {
                var hardness = DbContext.MaterialFeature.Where(v => v.MaterialCode == MaterialCode&& v.Hardness== Hardness && v.Type== MATERIALTYPE.材料物性 ).Select(v => v.Name).Distinct().ToList();
                hardness.Insert(0, Const.MaterialNormal);
                return hardness.Select(v => new NameValueModel { Name = v.ToString(), Value = v.ToString() }).ToList();
            }
            if (Type == MATERIALMODELTYPE.Material2)
            {
                var hardness = DbContext.MaterialFeature.Where(v => v.MaterialCode == MaterialCode && v.Hardness == Hardness && v.Type == MATERIALTYPE.表面物性).Select(v => v.Name).Distinct().ToList();
                hardness.Insert(0, Const.MaterialNormal);
                return hardness.Select(v => new NameValueModel { Name = v.ToString(), Value = v.ToString() }).ToList();
            }
            if (Type == MATERIALMODELTYPE.Color)
            {
                var hardness = DbContext.MaterialFeature.Where(v => v.MaterialCode == MaterialCode && v.Hardness == Hardness && v.Type == MATERIALTYPE.颜色).Select(v => v.Name).Distinct().ToList();
                return hardness.Select(v => new NameValueModel { Name = v.ToString(), Value = v.ToString() }).ToList();
            }

            throw new NotImplementedException();

        }
        public IPagedList<Material> GetMaterialList(int? MaterialId, int page)
        {
            return DbContext.Material.OrderByDescending(v => v.UpdateTime).ToPagedList(page, Const.PageSize);
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
                Price = model.Price
            };
        }

        public RepResult<Material> UpdateMaterial(MaterialModel material, string User)
        {
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
          
            original.SpecialDiscount = material.SpecialDiscount;
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

             
            DbContext.SaveChanges();
            item.IsSuccess = SuccessENUM.导入成功;
            item.RelateID = material.Id;
        }




        #endregion

        #region 特性


        public IPagedList<MaterialFeature> GetMaterialFeatures(int? MaterialId, MATERIALTYPE type, int page)
        {
            return DbContext.MaterialFeature
                .Where(v => (v.MaterialId == MaterialId || MaterialId == null) && v.Type==type)
                  .OrderByDescending(p => p.UpdateTime).ToPagedList(page, Const.PageSize);
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
                    Type = type
                };
                DbContext.MaterialFeature.Add(feature);
            }
            else
            {
                feature.Discount = relateItem.Discount;
                feature.UpdateTime = DateTime.Now;
                feature.UpdateUser = User;
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
                UpdateUser = model.UpdateUser
            };
        }
       
        #endregion

        #region 比重

        public IPagedList<MaterialGravity> GetMaterialGravities( int? MaterialId,int page)
        { 
            return DbContext.MaterialGravity
                .Where(v=>v.MaterialId == MaterialId || MaterialId==null)
                  .OrderByDescending(p => p.UpdateTime).ToPagedList(page,Const.PageSize);
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

        public IPagedList<BaseHole> GetBaseHoles(  int page)
        {
            if (page == -1)
                return DbContext.BaseHole.OrderBy(v=>v.SizeC).ToPagedList(1, 9999);
            return DbContext.BaseHole 
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

        public IPagedList<MaterialHole> GetMaterialHoles(int? MaterialId, int page)
        {
            return DbContext.MaterialHole
                .Where(v => v.MaterialId == MaterialId || MaterialId == null)
                  .OrderByDescending(p => p.UpdateTime).ToPagedList(page, Const.PageSize);
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

            var baseHole = DbContext.BaseHole.Where(v => v.SizeC == relateItem.SizeC).FirstOrDefault();

            if(hole==null)
            {
                 hole = new MaterialHole
                {
                    UpdateTime = DateTime.Now,
                    UpdateUser = User,
                    Hardness = relateItem.Hardness,
                    HoleCount = relateItem.HoleCount,
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
                hole.HoleCount = relateItem.HoleCount;
                hole.Rate = relateItem.Rate;

            }
            if (baseHole != null && relateItem.HoleCount == 0)
                hole.HoleCount =Convert.ToInt32( baseHole.HoleCount * hole.Rate);
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
            original.HoleCount = material.HoleCount;
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
                HoleCount = model.HoleCount,
                SizeC = model.SizeC,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                UpdateUser = model.UpdateUser
            };
        }
        #endregion
        
        #region 模数 生产效率

        public IPagedList<MaterialHour> GetMaterialHours(int? MaterialId, int page)
        {
            return DbContext.MaterialHour
                .Where(v => v.MaterialId == MaterialId || MaterialId == null)
                  .OrderByDescending(p => p.UpdateTime).ToPagedList(page, Const.PageSize);
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

        public IPagedList<MaterialRate> GetMaterialRates(int? MaterialId, int page)
        {
            return DbContext.MaterialRate 
                  .OrderByDescending(p => p.UpdateTime).ToPagedList(page, Const.PageSize);
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

         
    }
}
