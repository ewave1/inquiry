using Data.Entities;
using Data.Models;
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
        PagedResult<MaterialFeature> GetMaterialFeatures();

        void ImportMaterialFeatures(string User);
        void DeleteMatialFeature(Material material);

        PagedResult<MaterialGravity> GetMaterialGravities();
        void ImportMaterialGravity(string User);
        void DeleteMatialGravity(Material material);


        PagedResult<MaterialHole> GetMaterialHoles();

        void ImportMaterialHole(string User);
        void DeleteMatialHole(Material material);


        PagedResult<MaterialHour> GetMaterialHours();

        void ImportMaterialHour(string User);
        void DeleteMatialHour(Material material);


        PagedResult<MaterialRate> GetMaterialRates();

        void ImportMaterialRate(string User);
        void DeleteMatialRate(Material material);



    }
}
