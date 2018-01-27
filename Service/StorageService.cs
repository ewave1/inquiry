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

        public RepResult<Storage> UploadStorage(string User, HttpRequestBase Request)
        {
            return ImportFile(User, Request, FILETYPE.库存);
        }

        private RepResult<Storage> ImportFile(string User, HttpRequestBase Request,FILETYPE filetype)
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
                ImportType = filetype,
                ImportFile = file.LocalPath,
                ImportFileName = file.FileName,
               
            };
            DbContext.PT_ImportHistory.Add(importItem);
            DbContext.SaveChanges();
            var type = GetImportFileType(filetype);
            var details =     ImportHelper.AddToImport(type, importItem.Id, file.LocalPath);
            DbContext.PT_ImportHistoryDetail.AddRange(details); 
            DbContext.SaveChanges();
            foreach(var item in details)
            {
                //保存结果
                SaveImportResult(item, importItem, User);
            }

            return new RepResult<Storage> { }; 
        }

        public void SaveImportResult(PT_ImportHistoryDetail item,PT_ImportHistory importItem,string User)
        {

        }
        private Type GetImportFileType(FILETYPE filetype)
        {
            var type = typeof(StorageModel);

            return type;
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
