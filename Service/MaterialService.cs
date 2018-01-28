using Common;
using Data.Entities;
using Data.Models;
using IServices;
using PagedList;
using SmartSSO.Services.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MaterialService : ServiceContext, IMaterialService
    {

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
                Name = model.Name,
                Remark = model.Remark,
                SpecialDiscount = model.SpecialDiscount,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                UpdateUser = model.UpdateUser
            };
        }

        public RepResult<Material> UpdateMaterial(MaterialModel material, string User)
        {
            var original = DbContext.Material.Where(v => v.Name == material.Name).FirstOrDefault();
            if (original == null)
            {
                original = new Material {
                    Name = material.Name,
                    Display  =material.Display??material.Name,
                   
                };
                DbContext.Material.Add(original);
            }
            if (!string.IsNullOrEmpty(material.Display))
                original.Display = material.Display;
            if (!string.IsNullOrEmpty(material.Remark))
                original.Remark = material.Remark;
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


        public IPagedList<MaterialFeature> GetMaterialFeatures(int? MaterialId, int page)
        {
            return DbContext.MaterialFeature
                .Where(v => v.MaterialId == MaterialId || MaterialId == null)
                  .OrderByDescending(p => p.UpdateTime).ToPagedList(page, Const.PageSize);
        }

        public IPagedList<MaterialGravity> GetMaterialGravities( int? MaterialId,int page)
        {

            return DbContext.MaterialGravity
                .Where(v=>v.MaterialId == MaterialId || MaterialId==null)
                  .OrderByDescending(p => p.UpdateTime).ToPagedList(page,Const.PageSize);
        }

        public IPagedList<MaterialHole> GetMaterialHoles(int? MaterialId, int page)
        {
            return DbContext.MaterialHole
                .Where(v => v.MaterialId == MaterialId || MaterialId == null)
                  .OrderByDescending(p => p.UpdateTime).ToPagedList(page, Const.PageSize);
        }

        public IPagedList<MaterialHour> GetMaterialHours(int? MaterialId, int page)
        {
            return DbContext.MaterialHour
                .Where(v => v.MaterialId == MaterialId || MaterialId == null)
                  .OrderByDescending(p => p.UpdateTime).ToPagedList(page, Const.PageSize);
        }


        public IPagedList<MaterialRate> GetMaterialRates(int? MaterialId, int page)
        {
            return DbContext.MaterialRate
                .Where(v => v.MaterialId == MaterialId || MaterialId == null)
                  .OrderByDescending(p => p.UpdateTime).ToPagedList(page, Const.PageSize);
        }

        public void ImportMaterialFeatures(string User)
        {

            throw new NotImplementedException();
        }

        public void ImportMaterialGravity(string User)
        {
            throw new NotImplementedException();
        }

        public void ImportMaterialHole(string User)
        {
            throw new NotImplementedException();
        }

        public void ImportMaterialHour(string User)
        {
            throw new NotImplementedException();
        }

        public void ImportMaterialRate(string User)
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

        public RepResult<MaterialFeature> UpdateMaterialFeature(MaterialFeatureModel material, string User)
        {

            var original = DbContext.MaterialFeature.Where(v => v.Id==material.Id).FirstOrDefault();
            if (original == null)
            {
                original = new MaterialFeature
                {
                    MaterialId = material.MaterialId,
                    Discount = material.Discount,
                    

                };
                DbContext.MaterialFeature.Add(original);
            }
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
                MaterialId = model.MaterialId,
                Discount = model.Discount,
                Type = model.Type,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                UpdateUser = model.UpdateUser
            };
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
                Gravity = model.Gravity,
                Hardness = model.Hardness,
                MaterialId =model.MaterialId,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                UpdateUser = model.UpdateUser
            };
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
            original.HoleCount = material.HoleCount;
            original.MaterialId = material.MaterialId;
            original.SizeC = material.SizeC; 
            original.UpdateTime = DateTime.Now;
            original.UpdateUser = User;
            DbContext.SaveChanges();
            return new RepResult<MaterialHole> {Data = original };
        }

        public MaterialHoleModel GetMaterialHole(int? id)
        {
            var model = DbContext.MaterialHole.Find(id);
            if (model == null)
                return new MaterialHoleModel { };
            return new MaterialHoleModel
            {
                MaterialId = model.MaterialId,
                HoleCount = model.HoleCount,
                SizeC = model.SizeC,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                UpdateUser = model.UpdateUser
            };
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
            original.Hours = material.Hours;
            original.MaterialId = material.MaterialId;
            original.SizeB = material.SizeB; 
            original.UpdateTime = DateTime.Now;
            original.UpdateUser = User;
            DbContext.SaveChanges();
            return new RepResult<MaterialHour> { Data = original};
        }

        public MaterialHourModel GetMaterialHoure(int? id)
        {
            var model = DbContext.MaterialHour.Find(id);
            if (model == null)
                return new MaterialHourModel { };
            return new MaterialHourModel
            {
                Hours = model.Hours,
                MaterialId = model.MaterialId,
                SizeB = model.SizeB,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                UpdateUser = model.UpdateUser
            };
        }

        public RepResult<MaterialRate> UpdateMaterialRate(MaterialRateModel material, string User)
        {
            var original = DbContext.MaterialRate.Where(v => v.Id == material.Id).FirstOrDefault();
            if (original == null)
            {
                original = new MaterialRate
                {
                    MaterialId = material.MaterialId,  
                };
                DbContext.MaterialRate.Add(original);
            }
            original.MaterialId = material.MaterialId;
            original.SizeB = material.SizeB;
            original.UseRate = material.UseRate;
            original.UpdateTime = DateTime.Now;
            original.UpdateUser = User;
            DbContext.SaveChanges();
            return new RepResult<MaterialRate> { Data = original};
        }

        public MaterialRateModel GetMaterialRate(int? id)
        {
            var model = DbContext.MaterialRate.Find(id);
            if (model == null)
                return new MaterialRateModel { };
            return new MaterialRateModel
            {
                BadRate = model.BadRate,
                MaterialId = model.MaterialId,
                SizeB = model.SizeB,
                UseRate = model.UseRate,
                UpdateTime = model.UpdateTime,
                Id = model.Id,
                UpdateUser = model.UpdateUser
            };
        }
    }
}
