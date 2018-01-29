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
            var material = DbContext.Material.Find(model.MaterialId);
            if (material == null)
                return new RepResult<InquiryLog> { Code = -1, Msg = "材料不能为空" };
            if (model.Number == 0)
                return new RepResult<InquiryLog> { Code = -1, Msg = "数量不能为0" };
            var sc = DbContext.SealCode.Find(model.Code);
            if (sc == null)
                return new RepResult<InquiryLog> { Code = -1, Msg = "请选择编码或者输入尺寸" };
            var product = DbContext.Product.Where(v => v.Code == sc.Code && v.MaterialId == material.Id).FirstOrDefault();
            if (product == null)
                return new RepResult<InquiryLog> { Code = -1, Msg = "当前产品不存在，请重新输入！" };
            if (model.SizeA < 1M || model.SizeB < 1M)
                return new RepResult<InquiryLog> { Code = -1, Msg = "内径或线径小于 1，请重新输入！" };
            if (model.SizeA > 200M || model.SizeB > 8M)
                return new RepResult<InquiryLog> { Code = -1, Msg = "内径大于200或线径大于8，请重新输入！" };


            var user = DbContext.ManageUser.Find(User);
            if (user == null)
                return new RepResult<InquiryLog> { Code = -2, Msg = "请重新登陆" };
            var factory = DbContext.DiscountSet.Where(v=>v.Type== DisCountType.FACTORY&&v.Name== model.Factory).FirstOrDefault();
            var discount = factory.Discount * user.Discount;

            //表面物性
            var m1 = DbContext.DiscountSet.Where(v => v.Type == DisCountType.材料物性 && v.Name == model.Material1).FirstOrDefault();
            var m2 = DbContext.DiscountSet.Where(v => v.Type == DisCountType.表面物性 && v.Name == model.Material2).FirstOrDefault();
            discount = discount * ((m1 == null) ? 1 : m1.Discount) * ((m2 == null) ? 1 : m2.Discount);

            //判断是否特殊件
            var special = DbContext.DiscountSet.Where(v=>v.Type== DisCountType.Other).FirstOrDefault();
            if (product.SizeA != model.SizeA || product.SizeB != model.SizeB)
                discount = factory.Discount * (special == null ? 1 : special.Discount);
            discount = Math.Round(discount, 2);
            var price = product.Price * ( discount);
            price = Math.Round(price, 3);
            var totalprice = Math.Round(price * model.Number, 2);
            var log = new InquiryLog
            {
                CreateTime = DateTime.Now,
                Code = model.Code,
                Factory = model.Factory,
                MaterialId = model.MaterialId,
                Material = material.MaterialCode,
                Number = model.Number,
                SizeA = sc.SizeA,
                SizeB = sc.SizeB,
                discount = discount,
                User = User,
                Price = price,
                TotalPrice = price * model.Number,


            };

            DbContext.InquiryLog.Add(log);

            DbContext.SaveChanges();

            var info = string.Format("折扣：{0}，单价：{1}，总价：{2}", discount, price, price * model.Number);

            return new RepResult<InquiryLog> { Data = log, Msg = info };
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
        public List<Material> Materials() => DbContext.Material.ToList();

        /// <summary>
        /// 编码
        /// </summary>
        /// <returns></returns>
        public List<SealCode> SealCodes() => DbContext.SealCode.ToList();

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
