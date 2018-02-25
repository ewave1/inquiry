﻿using Data.Entities;
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
          
            if (model.SizeA < 1M || model.SizeB < 1M)
                return new RepResult<InquiryLog> { Code = -1, Msg = "内径或线径小于 1，请重新输入！" };
            if (model.SizeA > 200M || model.SizeB > 8M)
                return new RepResult<InquiryLog> { Code = -1, Msg = "内径大于200或线径大于8，请重新输入！" };
            //SizeA
            //SizeB
            //Gravity
            var gravity = DbContext.MaterialGravity.Where(v => v.Color == model.Color && v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness).FirstOrDefault();
            if(gravity==null)
                return new RepResult<InquiryLog> { Code = -1, Msg = "比重数据找不到，请先导入比重数据" };
            //userate,badrate

            var materialRate = DbContext.MaterialRate.Where(v => v.SizeB <= model.SizeB && v.SizeB2 > model.SizeB).FirstOrDefault();
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
            var hour = DbContext.MaterialHour.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness && v.SizeB <= model.SizeB && v.SizeB2 > model.SizeB).FirstOrDefault();

            if (hour == null)
                return new RepResult<InquiryLog> { Code = -1, Msg = "生产效率数据找不到，请先导入生产效率数据" };
            //开模孔数
            var sizeC = model.SizeA + model.SizeB;
            var hole = DbContext.MaterialHole.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness && (v.SizeC <= sizeC && (v.SizeC2>sizeC ||v.SizeC2 ==null) )).FirstOrDefault();

            if (hole == null)
                return new RepResult<InquiryLog> { Code = -1, Msg = "开模孔数数据找不到，请先导入开模孔数数据" };
            //基础孔数
            var baseHole = DbContext.BaseHole.Where(v => v.SizeC == (model.SizeA + model.SizeB)).FirstOrDefault();
            if(baseHole==null)
                return new RepResult<InquiryLog> { Code = -1, Msg = "基础孔数数据找不到，请先导入基础孔数数据" };

            int holeCnt = (int)Math.Round(baseHole.HoleCount * (hole != null ? hole.Rate : 1));
            //profile 
            var profile = DbContext.DiscountSet.Where(v => v.Type == DisCountType.利润率).FirstOrDefault();
            var costByHour = DbContext.DiscountSet.Where(v => v.Type == DisCountType.每小时成本).FirstOrDefault();

            var price = CalUnitPrice(model.SizeA, model.SizeB, gravity.Gravity, materialRate.UseRate, materialRate.BadRate, material1.Discount, material2.Discount, color.Discount, material.Price, costByHour.Discount, hour.MosInHour, holeCnt, profile.Discount);

            //库存(是否有模具）
            var storages = DbContext.Storage.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness && v.SizeA == model.SizeA && v.SizeB == model.SizeB).ToList();
            var storage = DbContext.Storage.Where(v => v.MaterialCode == model.MaterialCode && v.Hardness == model.Hardness && v.SizeA == model.SizeA && v.SizeB == model.SizeB&& v.Material1==model.Material1&& v.Material2==model.Material2&& v.Color == model.Color).ToList().Sum(v=>v.Number);

            var factory = DbContext.DiscountSet.Where(v=>v.Type== DisCountType.FACTORY&&v.Name== model.Factory).FirstOrDefault();
            var customerLevel = DbContext.DiscountSet.Where(v => v.Type == DisCountType.客户级别 && v.Name == model.CustomerLevel).FirstOrDefault();
            //折扣
            var discount = factory.Discount * user.Discount* (customerLevel==null ?1:customerLevel.Discount);
             
            //判断是否特殊件
            var special = DbContext.DiscountSet.Where(v=>v.Type== DisCountType.Other).FirstOrDefault();
        
            discount = Math.Round(discount, 2);

            price = Math.Round(price, 4);
            
            var totalprice = Math.Round(price * discount * model.Number, 2);
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
            if (storages.Count == 0)
                storageText = "无模具";
            else 
                storageText = storage.ToString();

            var info = string.Format("单价：{0}，总价：{1}，库存：{2}",   price, price * model.Number, storageText);

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
