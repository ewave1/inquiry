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
        public List<Material> GetMaterialList()
        {

            return DbContext.Material.ToList(); 
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
                Hardness = model.Hardness
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


        public bool DeleteMatial(int id)
        { 
            var model = DbContext.Material.Find(id);
            if (model != null)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }

        #endregion

        #region 特性


        public IPagedList<MaterialFeature> GetMaterialFeatures(int? MaterialId, int page)
        {
            return DbContext.MaterialFeature
                .Where(v => v.MaterialId == MaterialId || MaterialId == null)
                  .OrderByDescending(p => p.UpdateTime).ToPagedList(page, Const.PageSize);
        }

        public void ImportMaterialFeatures(string User)
        {

            throw new NotImplementedException();
        }


        public bool DeleteMatialFeature(int Id)
        {
            var model = DbContext.MaterialFeature.Find(Id);
            if (model != null)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }


        public RepResult<MaterialFeature> UpdateMaterialFeature(MaterialFeatureModel material, string User)
        {

            var original = DbContext.MaterialFeature.Where(v => v.Id == material.Id).FirstOrDefault();
            if (original == null)
            {
                original = new MaterialFeature
                {
                    MaterialId = material.MaterialId, 
                };
                DbContext.MaterialFeature.Add(original);
            }
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
                return new MaterialFeatureModel { };
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
            var storage = new MaterialGravity
            {
                UpdateTime = DateTime.Now,
                UpdateUser = User, 
                MaterialCode = relateItem.MaterialCode,
                Hardness = relateItem.Hardness, 
                Gravity = relateItem.Gravity,
                Color = relateItem.Color,
                MaterialId = material == null ? 0 : material.Id
            };


            DbContext.MaterialGravity.Add(storage);
            DbContext.SaveChanges();
            item.IsSuccess = SuccessENUM.导入成功;
            item.RelateID = storage.Id;
        }
        public bool DeleteMatialGravity(int Id)
        {
            var model = DbContext.MaterialGravity.Find(Id);
            if (model != null)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }

        public RepResult<MaterialGravity> UpdateMaterialGravity(MaterialGravityModel material, string User)
        {

            var original = DbContext.MaterialGravity.Where(v => v.Id == material.Id).FirstOrDefault();
            if (original == null)
            {
                original = new MaterialGravity
                {
                    MaterialId = material.MaterialId,

                };
                DbContext.MaterialGravity.Add(original);
            }
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
             
            var storage = new MaterialHole
            {                
                UpdateTime = DateTime.Now,
                UpdateUser = User,
                Hardness = relateItem.Hardness,
                HoleCount = relateItem.HoleCount,
                MaterialCode = relateItem.MaterialCode,
                Rate  = relateItem.Rate,
                SizeC = relateItem.SizeC,
                MaterialId = material==null?0:material.Id
            };


            DbContext.MaterialHole.Add(storage);
            DbContext.SaveChanges();
            item.IsSuccess = SuccessENUM.导入成功;
            item.RelateID = storage.Id;
        }

        public bool DeleteMatialHole(int Id)
        {
            var model = DbContext.MaterialHole.Find(Id);
            if (model != null)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }

        public RepResult<MaterialHole> UpdateMaterialHole(MaterialHoleModel material, string User)
        {
            var original = DbContext.MaterialHole.Where(v => v.Id == material.Id).FirstOrDefault();
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
                return new MaterialHoleModel { };
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
            var storage = new MaterialHour
            {
                UpdateTime = DateTime.Now,
                UpdateUser = User, 
                SizeB = relateItem.SizeB,
                SizeB2 = relateItem.SizeB2, 
                MaterialCode = relateItem.MaterialCode,
                Hardness = relateItem.Hardness,
                MosInHour = relateItem.MosInHour,
                MaterialId = material==null ?0:material.Id
            };


            DbContext.MaterialHour.Add(storage);
            DbContext.SaveChanges();
            item.IsSuccess = SuccessENUM.导入成功;
            item.RelateID = storage.Id;
        }

        public bool DeleteMatialHour(int Id)
        {
            var model = DbContext.MaterialHour.Find(Id);
            if (model != null)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }
        public RepResult<MaterialHour> UpdateMaterialHour(MaterialHourModel material, string User)
        {
            var original = DbContext.MaterialHour.Where(v => v.Id == material.Id).FirstOrDefault();
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
            original.SizeB2 = material.SizeB2; 
            original.MosInHour = material.MosInHour;
            original.MaterialId = material.MaterialId;
            original.SizeB = material.SizeB;
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
          

            var storage = new MaterialRate
            {
                UpdateTime = DateTime.Now,
                UpdateUser = User,
                BadRate = relateItem.BadRate,
                SizeB = relateItem.SizeB,
                SizeB2 = relateItem.SizeB2,
                UseRate = relateItem.UseRate,
                
            };


            DbContext.MaterialRate.Add(storage);
            DbContext.SaveChanges();
            item.IsSuccess = SuccessENUM.导入成功;
            item.RelateID = storage.Id;
        }

        public bool DeleteMatialRate(int Id)
        {
            var model = DbContext.MaterialRate.Find(Id);
            if (model != null)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }

        public RepResult<MaterialRate> UpdateMaterialRate(MaterialRateModel material, string User)
        {
            var original = DbContext.MaterialRate.Where(v => v.Id == material.Id).FirstOrDefault();
            if (original == null)
            {
                original = new MaterialRate();
                DbContext.MaterialRate.Add(original);
            }
            original.SizeB2 = material.SizeB2;
            original.SizeB = material.SizeB;
            original.UseRate = material.UseRate;
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
