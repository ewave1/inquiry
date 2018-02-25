using Data.Entities;
using Data.Models;
using PagedList;
using SmartSSO.Services.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IServices
{
    public   interface IMaterialService
    {
        /// <summary>
        /// 获取所有的材质
        /// </summary>
        /// <returns></returns>
        IPagedList<Material> GetMaterialList(int? MaterialId, int page);

        RepResult<Material> UpdateMaterial(MaterialModel material, string User);

        MaterialModel GetMaterial(int? id);


        List<NameValueModel> GetMaterialDetailData(string MaterialCode, int? Hardness, MATERIALMODELTYPE Type =  MATERIALMODELTYPE.Hardness);

        RepResult<bool> DeleteMatial(int id);


        RepResult<bool> RemoveAllMatertail();

        RepResult<Material> UploadMaterial(string User, HttpRequestBase Request);

        IPagedList<MaterialFeature> GetMaterialFeatures(int? MaterialId, MATERIALTYPE type, int page);

        RepResult<MaterialFeature> UploadMaterialFeature(string User, HttpRequestBase Request, MATERIALTYPE type = MATERIALTYPE.材料物性);
        RepResult<bool> DeleteMatialFeature(int Id);

        RepResult<bool> RemoveAllMatertailFeature(MATERIALTYPE? type);

        RepResult<MaterialFeature> UpdateMaterialFeature(MaterialFeatureModel material, string User);

        MaterialFeatureModel GetMaterialFeature(int? id);

        IPagedList<MaterialGravity> GetMaterialGravities(int? MaterialId, int page);
        RepResult<MaterialGravity> UploadMaterialGravity(string User, HttpRequestBase Request);
        RepResult<bool> DeleteMatialGravity(int Id);


        RepResult<MaterialGravity> UpdateMaterialGravity(MaterialGravityModel material, string User);

        MaterialGravityModel GetMaterialGravity(int? id);
        RepResult<bool> RemoveAllMatertailGravity();

        IPagedList<MaterialHole> GetMaterialHoles(int? MaterialId, int page);
        RepResult<MaterialHole> UploadMaterialHole(string User, HttpRequestBase Request);
        RepResult<bool> DeleteMatialHole(int Id);

        RepResult<MaterialHole> UpdateMaterialHole(MaterialHoleModel material, string User);

        MaterialHoleModel GetMaterialHole(int? id);


        RepResult<bool> RemoveAllMatertailHole();

        IPagedList<BaseHole> GetBaseHoles(  int page);
        RepResult<BaseHole> UploadBaseHole(string User, HttpRequestBase Request);
        RepResult<bool> DeleteBaseHole(int Id);

        RepResult<BaseHole> UpdateBaseHole(BaseHoleModel material, string User);

        BaseHoleModel GetBaseHole(int? id);
        RepResult<bool> RemoveAllBaseHole();


        IPagedList<MaterialHour> GetMaterialHours(int? MaterialId, int page);

        RepResult<MaterialHour> UploadMaterialHour(string User, HttpRequestBase Request);
        RepResult<bool> DeleteMatialHour(int Id);

        RepResult<MaterialHour> UpdateMaterialHour(MaterialHourModel material, string User);

        MaterialHourModel GetMaterialHour(int? id);
        RepResult<bool> RemoveAllMatertailHour();

        IPagedList<MaterialRate> GetMaterialRates(int? MaterialId, int page);

        RepResult<MaterialRate> UploadMaterialRate(string User, HttpRequestBase Request);
        RepResult<bool> DeleteMatialRate(int Id);

        RepResult<MaterialRate> UpdateMaterialRate(MaterialRateModel material, string User);

        MaterialRateModel GetMaterialRate(int? id);

        RepResult<bool> RemoveAllMatertailRate();

    }
}
