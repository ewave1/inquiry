using Data.Entities;
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

        Material UpdateMaterial(Material material, string User);

        void DeleteMatial(Material material);

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
