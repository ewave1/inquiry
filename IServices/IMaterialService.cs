using Data.Entities;
using Data.Models;
using PagedList;
using SmartSSO.Services.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public   interface IMaterialService
    {
        /// <summary>
        /// 获取所有的材质
        /// </summary>
        /// <returns></returns>
        List<Material> GetMaterialList();

        RepResult<Material> UpdateMaterial(MaterialModel material, string User);

        MaterialModel GetMaterial(int? id);
        bool DeleteMatial(int id);

        IPagedList<MaterialFeature> GetMaterialFeatures(int? MaterialId, int page);

        void ImportMaterialFeatures(string User);
        bool DeleteMatialFeature(int Id);


        RepResult<MaterialFeature> UpdateMaterialFeature(MaterialFeatureModel material, string User);

        MaterialFeatureModel GetMaterialFeature(int? id);

        IPagedList<MaterialGravity> GetMaterialGravities(int? MaterialId, int page);
        void ImportMaterialGravity(string User);
        bool DeleteMatialGravity(int Id);


        RepResult<MaterialGravity> UpdateMaterialGravity(MaterialGravityModel material, string User);

        MaterialGravityModel GetMaterialGravity(int? id);

        IPagedList<MaterialHole> GetMaterialHoles(int? MaterialId, int page);

        void ImportMaterialHole(string User);
        bool DeleteMatialHole(int Id);

        RepResult<MaterialHole> UpdateMaterialHole(MaterialHoleModel material, string User);

        MaterialHoleModel GetMaterialHole(int? id);

        IPagedList<MaterialHour> GetMaterialHours(int? MaterialId, int page);

        void ImportMaterialHour(string User);
        bool DeleteMatialHour(int Id);

        RepResult<MaterialHour> UpdateMaterialHour(MaterialHourModel material, string User);

        MaterialHourModel GetMaterialHoure(int? id);

        IPagedList<MaterialRate> GetMaterialRates(int? MaterialId, int page);

        void ImportMaterialRate(string User);
        bool DeleteMatialRate(int Id);

        RepResult<MaterialRate> UpdateMaterialRate(MaterialRateModel material, string User);

        MaterialRateModel GetMaterialRate(int? id);


    }
}
