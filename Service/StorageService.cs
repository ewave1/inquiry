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
    public class StorageService : ServiceContext, IStorageService
    {

        public IPagedList<Storage> GetAll(string CreateUser, DateTime timeStart, DateTime timeEnd, int pageIndex)
        {

            return DbContext.Storage.Where(v => v.UpdateTime >= timeStart && v.UpdateTime < timeEnd)
                .OrderByDescending(v => v.UpdateTime)
                .ToPagedList(pageIndex, Const.PageSize); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        public RepResult<Storage> UploadStorage(string User, HttpRequestBase Request)
        {

            UploadService service = new UploadService();
            var result = service.UploadFile(User, Request, FILETYPE.库存);
            if (!result.Success)//导入失败
                return new RepResult<Storage> { Msg = result.Msg, Code = result.Code };
            var file = result.Data;


            //将数据保存到数据库
            var importItem = new PT_ImportHistory
            {
                User = User,
                ImportTime = DateTime.Now,
                ImportType = FILETYPE.库存,
                ImportFile = file.LocalPath,
                ImportFileName = file.FileName,

            };
            DbContext.PT_ImportHistory.Add(importItem);
            DbContext.SaveChanges();
            var details = ImportHelper.AddToImport(typeof(StorageModel), importItem.Id, file.LocalPath);
            DbContext.PT_ImportHistoryDetail.AddRange(details);

            DbContext.Database.ExecuteSqlCommand(" update Storages set Number =0 ");
            foreach (var item in details)
            {
                //保存结果
                SaveImportResult(item, importItem, User);
            }
            DbContext.SaveChanges();
            return new RepResult<Storage> { Code = 0 };
        }
         
        /// <summary>
        /// 保存单笔结果 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="importItem"></param>
        /// <param name="User"></param>
        public void SaveImportResult(PT_ImportHistoryDetail item,PT_ImportHistory importItem,string User  )
        { 
            var relateItem  = JsonConvert.DeserializeObject<StorageModel>( item.Json);
            var storage = new Storage {
                Color = relateItem.Color,
                Hardness = relateItem.Hardness,
                MaterialCode = relateItem.MaterialCode,
                Material1 = relateItem.Material1,
                Material2 = relateItem.Material2,
                Number = relateItem.Number,
                SizeA = relateItem.SizeA,
                SizeB = relateItem.SizeB,
                UpdateTime = DateTime.Now,
                UpdateUser = User
            };

            DbContext.Storage.Add(storage);
            DbContext.SaveChanges();
            item.IsSuccess = SuccessENUM.导入成功;
            item.RelateID = storage.Id;
        } 

        public bool Delete(int id)
        {
            var model = DbContext.Storage.Find(id);
            if (model != null)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }
    }
}
