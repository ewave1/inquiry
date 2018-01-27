using Data.Entities;
using Data.Models;
using IServices;
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


        public PagedResult<MaterialFeature> GetMaterialFeatures()
        {
            return new PagedResult<MaterialFeature>
            {
                Result = DbContext.MaterialFeature
                  .OrderByDescending(p => p.UpdateTime).ToList()
            }; 
        }

        public PagedResult<MaterialGravity> GetMaterialGravities()
        {

            return new PagedResult<MaterialGravity>
            {
                Result = DbContext.MaterialGravity
                  .OrderByDescending(p => p.UpdateTime).ToList()
            }; 
        }

        public PagedResult<MaterialHole> GetMaterialHoles()
        {
            return new PagedResult<MaterialHole>
            {
                Result = DbContext.MaterialHole
                  .OrderByDescending(p => p.UpdateTime).ToList()
            }; 
        }

        public PagedResult<MaterialHour> GetMaterialHours()
        {
            return new PagedResult<MaterialHour>
            {
                Result = DbContext.MaterialHour
                  .OrderByDescending(p => p.UpdateTime).ToList()
            }; 
        }


        public PagedResult<MaterialRate> GetMaterialRates()
        {
            return new PagedResult<MaterialRate>
            {
                Result = DbContext.MaterialRate
                  .OrderByDescending(p => p.UpdateTime).ToList()
            }; 
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

        public void DeleteMatialFeature(Material material)
        {
            throw new NotImplementedException();
        }

        public void DeleteMatialGravity(Material material)
        {
            throw new NotImplementedException();
        }

        public void DeleteMatialHole(Material material)
        {
            throw new NotImplementedException();
        }

        public void DeleteMatialHour(Material material)
        {
            throw new NotImplementedException();
        }

        public void DeleteMatialRate(Material material)
        {
            throw new NotImplementedException();
        }
    }
}
