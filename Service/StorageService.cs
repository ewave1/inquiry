﻿using Common;
using Data.Entities;
using Data.Models;
using IServices;
using Newtonsoft.Json;
using PagedList;
using SmartSSO.Services.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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

            DbContext.Database.ExecuteSqlCommand(" truncate table  Storages   ");
            //foreach (var item in details)
            //{
            //    //保存结果
            //    SaveImportResult(item, importItem, User);
            //}
            //DbContext.BulkInsert(details);
            SaveImportResult(importItem, details, User);
            DbContext.SaveChanges();
            DbContext.Database.ExecuteSqlCommand(@"update Materials a,Storages b 
set b.MaterialCode = a.MaterialCode,b.Hardness = a.Hardness
where  a.Display = b.MaterialDisplay and b.MaterialCode is null ;");
            return new RepResult<Storage> { Code = 0 };
        }
         
        public void SaveImportResult(PT_ImportHistory importItem,List<PT_ImportHistoryDetail> details, string User)
        {
            List<Storage> storages = new List<Storage>();
            List<StorageModel> modes = new List<StorageModel>();
            Stopwatch sw = Stopwatch.StartNew();
 
                foreach (var item in details)
            {
                var relateItem = JsonConvert.DeserializeObject<StorageModel>(item.Json);
                #region 规格数据
                var dsizeA = 0M;
                var dsizeB = 0M;
                var errorInfo = CommonHelper.GetSizeAB(relateItem.Spec, out dsizeA, out dsizeB);
                if (!string.IsNullOrEmpty(errorInfo))
                {
                    item.ErrorInfo = "规格数据异常";
                    item.IsSuccess = SuccessENUM.导入失败;
                    continue;
                }
                #endregion
                var storage = new Storage
                {
                    Name = relateItem.Name,
                    Spec = relateItem.Spec,
                    BatchNo = relateItem.BatchNo,
                    Location = relateItem.Location,
                    Remark = relateItem.Remark,
                    SizeA = dsizeA,
                    SizeB = dsizeB, 
                    UpdateTime = DateTime.Now,
                    UpdateUser = User
                };
                #region 物料名称
                var codes = relateItem.Name?.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (codes != null && codes.Length == 4)
                {
                    var material = codes[1];
                    storage.MaterialDisplay = material;
                    //var m = DbContext.Material.Where(v => v.Display == material).FirstOrDefault();
                    //if (m == null)
                    //{
                    //    storage.MaterialCode = material;
                    //}
                    //else
                    //{
                    //    storage.MaterialCode = m.MaterialCode;
                    //    storage.Hardness = m.Hardness;
                    //}
                    storage.Color = codes[2];
                    if (codes[3] == "Normal" || codes[3].Length != 4)
                    {
                        storage.Material1 = "Normal";
                        storage.Material2 = "Normal";
                    }
                    else
                    {
                        var m1 = codes[3].Substring(0, 2);
                        var m2 = codes[3].Substring(2, 2);
                        if (m1 != "00")
                        {
                            storage.Material1 = m1;
                        }
                        if (m2 != "00")
                            storage.Material2 = m2;
                    }
                }
                #endregion
                if (relateItem.Number != null && relateItem.Number.EndsWith("PCS"))
                {
                    var number = relateItem.Number.Substring(0, relateItem.Number.Length - 3);
                    int iNumber = 0;
                    if (int.TryParse(number, out iNumber))
                        storage.Number = iNumber;
                }
                storages.Add(storage);
            }
            var s2 = sw.ElapsedMilliseconds;
            Console.Write(s2);
            DbContext.BulkInsert(storages); 
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
            #region 规格数据
            var dsizeA = 0M;
            var dsizeB = 0M;
            var errorInfo = CommonHelper.GetSizeAB(relateItem.Spec, out dsizeA, out dsizeB);
            if (!string.IsNullOrEmpty(errorInfo))
            {
                item.ErrorInfo = "规格数据异常";
                item.IsSuccess = SuccessENUM.导入失败;
                return;
            }
            #endregion
            var storage = new Storage
            {
                Name = relateItem.Name,
                Spec = relateItem.Spec,
                BatchNo = relateItem.BatchNo,
                Location = relateItem.Location,
                Remark = relateItem.Remark,
                SizeA = dsizeA,
                SizeB = dsizeB,
                //Color = relateItem.Color,
                //Hardness = relateItem.Hardness,
                //MaterialCode = relateItem.MaterialCode,
                //Material1 = relateItem.Material1,
                //Material2 = relateItem.Material2,
                //Number = relateItem.Number,
                //SizeA = relateItem.SizeA,
                //SizeB = relateItem.SizeB,
                UpdateTime = DateTime.Now,
                UpdateUser = User
            };
            #region 物料名称
            var codes = relateItem.Name?.Split(new char[] {' ','\t' }, StringSplitOptions.RemoveEmptyEntries);
            if(codes!=null && codes.Length == 4)
            {
                var material = codes[1];
                storage.MaterialDisplay = material;
                var m =  DbContext.Material.Where(v => v.Display == material).FirstOrDefault();
                if(m==null )
                {
                    storage.MaterialCode = material;
                }
                else
                {
                    storage.MaterialCode = m.MaterialCode;
                    storage.Hardness = m.Hardness;
                }
                storage.Color = codes[2];
                if (codes[3] == "Normal"|| codes[3].Length!=4)
                {
                    storage.Material1 = "Normal";
                    storage.Material2 = "Normal";
                }
                else 
                {
                    var m1 = codes[3].Substring(0, 2);
                    var m2 = codes[3].Substring(2, 2);
                    if (m1 != "00")
                    {
                        storage.Material1 = m1;
                    }
                    if (m2 != "00")
                        storage.Material2 = m2;
                }
            }
            #endregion
            if(relateItem.Number!=null && relateItem.Number.EndsWith("PCS"))
            {
                var number = relateItem.Number.Substring(0, relateItem.Number.Length - 3);
                int iNumber = 0;
                if (int.TryParse(number, out iNumber))
                    storage.Number = iNumber;
            }

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
