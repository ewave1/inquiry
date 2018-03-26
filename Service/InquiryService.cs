using Data.Entities;
using Data.Models;
using IServices;
using SmartSSO.Entities;
using SmartSSO.Services.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using PagedList.Mvc;
using Common;

namespace Services
{

    public sealed class InquiryService : ServiceContext, IInquiryService
    {

        public  InquiryModelRequest Get(int? id)
        {
            var log =  DbContext.InquiryLog.Where(v => v.Id == id).FirstOrDefault();
            if (log == null)
                return null;
            return new InquiryModelRequest
            {
                Code = log.Code,
                Color = log.Color,
                CustomerLevel = log.CustomerLevel,
                MaterialCode = log.MaterialCode,
                Factory = log.Factory,
                Hardness = log.Hardness,
                Material1 = log.Material1,
                Material2 = log.Material2,
                MaterialId =log.MaterialId,
                Number = log.Number,
                SizeA = log.SizeA,
                SizeB = log.SizeB
            };
        }
        public IPagedList<InquiryLog> GetAll(ManageUser user, string CreateUser, DateTime timeStart, DateTime timeEnd, int pageIndex)
        {
            if (user == null)
                return null;
            DbContext.InquiryLog.OrderBy(p => p.CreateTime).ToPagedList(10, 10);;
            if (user != null && user.IsAdmin)
            {

                return DbContext.InquiryLog.Where(v=>v.CreateTime>=timeStart&& v.CreateTime< timeEnd)
                    .OrderByDescending(p => p.CreateTime).ToPagedList(pageIndex, Const.PageSize);

            }
            else
                return DbContext.InquiryLog.Where(v => v.User == user.UserName&& v.CreateTime >= timeStart && v.CreateTime < timeEnd)
                       .OrderByDescending(p => p.CreateTime).ToPagedList(pageIndex, Const.PageSize);
        }

        public RepResult<InquiryLog> Create(InquiryModelRequest model, string User)
        {
            var user = DbContext.ManageUser.Find(User);
            if (user == null)
                return new RepResult<InquiryLog> { Code = -2, Msg = "请重新登陆" };
            if (string.IsNullOrEmpty( model.MaterialCode ))
                return new RepResult<InquiryLog> { Code = -1, Msg = "材料不能为空" };

            var material = DbContext.Material.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness).FirstOrDefault();
            if (material==null)
                return new RepResult<InquiryLog> { Code = -1, Msg = "原料数据找不到，请导入原料数据" };

            if (model.Number == 0)
                return new RepResult<InquiryLog> { Code = -1, Msg = "数量不能为0" };
            //var sc = DbContext.SealCode.Find(model.Code);
            //if (sc == null)
            //    return new RepResult<InquiryLog> { Code = -1, Msg = "请选择编码或者输入尺寸" };
          
            //if (model.SizeA < 1M || model.SizeA > 570M)
            //    return new RepResult<InquiryLog> { Code = -1, Msg = "内径必须在1-570范围内！" };
            //if (model.SizeB < 1M || model.SizeB > 10M)
            //    return new RepResult<InquiryLog> { Code = -1, Msg = "线径必须在1-10范围内，请重新输入！" };
            //SizeA
            //SizeB
            //Gravity
            var gravity = DbContext.MaterialGravity.Where(v => v.Color == model.Color && v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness).FirstOrDefault();
            if(gravity==null)
                return new RepResult<InquiryLog> { Code = -1, Msg = "比重数据找不到，请先导入比重数据" };
            //userate,badrate

            var materialRate = DbContext.MaterialRate.Where(v => v.SizeB <  model.SizeB && v.SizeB2 >= model.SizeB).FirstOrDefault();
            if (materialRate == null)
                return new RepResult<InquiryLog> { Code = -1, Msg = "不良率数据找不到，请先导入不良率数据" };

            //特殊材料
            MaterialFeature material1;
            if (model.Material1 == "Normal")
                material1 = new MaterialFeature { Discount = 1 };
            else 
            material1 = DbContext.MaterialFeature.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness&&v.Type== MATERIALTYPE.材料物性&&v.Name==model.Material1).FirstOrDefault();
            if (material1 == null)
                return new RepResult<InquiryLog> { Code = -1, Msg = "特殊材料数据找不到，请先导入特殊材料(材料物性)数据" };

            MaterialFeature material2 = null;
            if (model.Material2 == "Normal")
                material2 = new MaterialFeature { Discount = 1 }; 
            else 
            material2 = DbContext.MaterialFeature.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness && v.Type == MATERIALTYPE.表面物性 && v.Name == model.Material2).FirstOrDefault();
            if (material2 == null)
                return new RepResult<InquiryLog> { Code = -1, Msg = "特殊处理数据找不到，请先导入特殊处理(表面物性)数据" };
            var color = DbContext.MaterialFeature.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness && v.Type == MATERIALTYPE.颜色 && v.Name == model.Color).FirstOrDefault();
            if (color == null)
                return new RepResult<InquiryLog> { Code = -1, Msg = "颜色数据找不到，请先导入颜色数据" };

            //hour
            var hour = DbContext.MaterialHour.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness && v.SizeB <  model.SizeB && v.SizeB2 >= model.SizeB).FirstOrDefault();

            if (hour == null)
                return new RepResult<InquiryLog> { Code = -1, Msg = "生产效率数据找不到，请先导入生产效率数据" };
            //开模孔数
            var sizeC = model.SizeA + model.SizeB;
            var hole = DbContext.MaterialHole.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness && (v.SizeC <  sizeC && (v.SizeC2>=sizeC ||v.SizeC2 ==null) )).FirstOrDefault();

            //if (hole == null)
            //    return new RepResult<InquiryLog> { Code = -1, Msg = "开模孔数数据找不到，请先导入开模孔数数据" };
            //基础孔数
            //外径向上取整，查询基础孔数的数据
            var iSizeC = (int)Math.Ceiling(sizeC);
            var baseHole = DbContext.BaseHole.Where(v => v.SizeC == iSizeC).FirstOrDefault();
            if(baseHole==null)
                return new RepResult<InquiryLog> { Code = -1, Msg = "基础孔数数据找不到，请先导入基础孔数数据" };
            //查孔数时，向下取整
            int holeCnt = (int)Math.Floor(baseHole.HoleCount * (hole != null ? hole.Rate : 1));
            //profile 
            var profile = DbContext.DiscountSet.Where(v => v.Type == DisCountType.利润率).FirstOrDefault();
            var costByHour = DbContext.DiscountSet.Where(v => v.Type == DisCountType.每小时成本).FirstOrDefault();

            var price = CalUnitPrice(model.SizeA, model.SizeB, gravity.Gravity, materialRate.UseRate, materialRate.BadRate, material1.Discount, material2.Discount, color.Discount, material.Price*material.SpecialDiscount, costByHour.Discount, hour.MosInHour, holeCnt, profile.Discount);

            //库存(是否有模具）
            var storagesCnt = DbContext.MaterialStorage.Where(v =>v.SizeA==model.SizeA&&v.SizeB==model.SizeB).Count();
            var storage = DbContext.Storage.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness && v.SizeA == model.SizeA && v.SizeB == model.SizeB&& v.Material1==model.Material1&& v.Material2==model.Material2&& v.Color == model.Color).ToList().Sum(v=>v.Number);

            var factory = DbContext.DiscountSet.Where(v=>v.Type== DisCountType.FACTORY&&v.Name== model.Factory).FirstOrDefault();
            var customerLevel = DbContext.DiscountSet.Where(v => v.Type == DisCountType.客户级别 && v.Name == model.CustomerLevel).FirstOrDefault();
            //折扣
            var discount = factory.Discount * user.Discount* (customerLevel==null ?1:customerLevel.Discount);
             
            //判断是否特殊件
            var special = DbContext.DiscountSet.Where(v=>v.Type== DisCountType.Other&&v.Name== "特殊件").FirstOrDefault();

            //库存的折扣
            var storageType = StorageTypeEnum.无库存;//有模具无库存
            if (storage > 0)
                storageType = StorageTypeEnum.有库存;
            else if (storagesCnt == 0)
                storageType = StorageTypeEnum.无模具;

            //起订金额
            var startAmount =   DbContext.MaterialStartAmount.Where(v => v.StorageType == storageType && v.SizeC <  sizeC &&( v.SizeC2 >= sizeC||v.SizeC2==null)).FirstOrDefault();
            if(startAmount==null)
            {
                startAmount = DbContext.MaterialStartAmount.Where(v => v.StorageType ==  StorageTypeEnum.所有产品 && v.SizeC <= sizeC &&( v.SizeC2 > sizeC|| v.SizeC2==null)).FirstOrDefault();
            }


            var storageDiscount = DbContext.DiscountSet.Where(v => v.Type == DisCountType.库存级别&&v.Name==storageType.ToString()).FirstOrDefault();
            if(storageDiscount==null ) 
                return new RepResult<InquiryLog> { Code = -1, Msg = "库存折扣数据找不到，请联系维护人员" };
            discount = discount * storageDiscount.Discount;


            discount = Math.Round(discount, 2);

            price = price * discount;
            price = Math.Round(price, 3);
            //最低单价
            var minPrice = DbContext.DiscountSet.Where(v => v.Type == DisCountType.Other && v.Name == "最低单价").FirstOrDefault();
            if(minPrice==null)
            {
                minPrice = new DiscountSet {
                Type = DisCountType.Other,
                Name = "最低单价",
                Discount =  0.018M, 
                };
               // DbContext.DiscountSet.Add(minPrice); 
            }
            if (price < minPrice.Discount)
                price = minPrice.Discount;
            var totalprice = Math.Round(price  * model.Number, 2);
            var startAmountText = "";
            if (startAmount!=null && totalprice < startAmount.StartAmount)
            {
                startAmountText = "，起订金额：" + startAmount.StartAmount;
                totalprice = startAmount.StartAmount;
                price = Math.Round(totalprice / model.Number,3);
            }
            var log = new InquiryLog
            {
                CreateTime = DateTime.Now,
                Code = model.Code,
                Factory = model.Factory,
                MaterialId = model.MaterialId,
                MaterialCode = material.MaterialCode,
                Hardness = material.Hardness,
                Material1 = material1.Name,
                Material2 = material2.Name,
                Storage = storage.ToString(),
                Color = color.Name,
                CustomerLevel = customerLevel.Name,
                Number = model.Number,
                SizeA = model.SizeA,
                SizeB = model.SizeB,
                discount = discount,
                User = User,
                Price = price,
                TotalPrice = totalprice, 
            };

            DbContext.InquiryLog.Add(log);

            DbContext.SaveChanges();
            var storageText = "";
            if (storagesCnt == 0)
                storageText = "无模具";
            else 
                storageText = storage.ToString();

            var info = string.Format("单价：{0}，总价：{1}，库存：{2} {3}",   price, totalprice, storageText,startAmountText);

            return new RepResult<InquiryLog> { Data = log, Msg = info };
        }


        private static decimal CalUnitPrice(decimal SizeA,decimal SizeB,decimal Gravity,decimal UseRate,decimal BadRate,decimal Material1,decimal Material2,decimal Color,decimal MaterilaPrice,decimal CostByHour,int MosInHour,int HoleCount,decimal Rate)
        {
            return   ((((SizeA + SizeB) * 3.14159M * (3.14159M * SizeB / 2 * SizeB / 2)) / 1000 * Gravity * UseRate * BadRate / 1000) * Material1 * Color * MaterilaPrice + (CostByHour / MosInHour / HoleCount) * Material2) * Rate; 
        }
        public bool Delete(int id)
        {
            var model = DbContext.InquiryLog.Find(id);
             
            if (model != null)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }

        public RepResult<bool> RemoveData(DateTime start, DateTime end)
        {
            DbContext.Database.ExecuteSqlCommand(" delete from InquiryLogs where CreateTime>=@start and CreateTime <@end ", new MySql
                .Data.MySqlClient.MySqlParameter("@start", start), new MySql
                .Data.MySqlClient.MySqlParameter("@end", end)); 
            return new RepResult<bool> { Data =true };
        }
        /// <summary>
        /// 
        /// </summary>
        public List<DiscountSet> GetDiscountNames(DisCountType type) => DbContext.DiscountSet.Where(v=>v.Type== type).ToList();

        /// <summary>
        /// 材料
        /// </summary>
        public List<Material> Materials() => DbContext.Material.OrderBy(v=>v.Display).ToList();

        /// <summary>
        /// 编码
        /// </summary>
        /// <returns></returns>
        public List<SealCode> SealCodes() => DbContext.SealCode.OrderBy(v=>v.Code).ToList();

        public SealCode GetSealCode(string Code, decimal? SizeA, decimal? SizeB)
        {
            if (!string.IsNullOrWhiteSpace(Code))
            {
                var sc = DbContext.SealCode.Find(Code.Trim());
                return sc;
            }
            var nsc =  DbContext.SealCode.Where(v => v.SizeA == SizeA && v.SizeB == SizeB).FirstOrDefault();
            if(nsc==null)
            {
                var size = (SizeA + SizeB) * SizeB * SizeB;
                var lst = DbContext.SealCode .OrderBy(v=>v.CalSize).ToList();
                var min =Math.Abs( lst[0].CalSize - size??0);
                int i = 1;
                for( ; i < lst.Count(); i++)
                {
                    var newmin = Math.Abs(lst[i].CalSize - size ?? 0);
                    if (newmin < min)
                        min = newmin;
                    else
                        break;
                }
                return lst[i - 1];
            }
            return nsc;

        }
    }
}
