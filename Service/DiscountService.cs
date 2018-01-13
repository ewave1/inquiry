using Data.Models;
using IServices;
using SmartSSO.Services.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public  class DiscountService: ServiceContext, IDiscountService
    {
        public List<DiscountModel> GetDiscounts()
        {
            var lst =  DbContext.DiscountSet.ToList();

            return lst.Select(v => new DiscountModel {
                Name = v.Name,
                Discount=v.Discount,
                Remark=v.Remark,
                Type=v.Type 
            }).ToList();
        }

        public void SetDiscounts(List<DiscountModel> lst)
        {
            foreach(var dic  in lst)
            {
                DbContext.DiscountSet.RemoveRange(DbContext.DiscountSet.Where(v => v.Type == Data.Entities.DisCountType.Material));
             
                var item = DbContext.DiscountSet.Find(dic.Name);
                if (item == null)
                {
                    DbContext.DiscountSet.Add(new Data.Entities.DiscountSet
                    {
                        Discount = dic.Discount,
                        Name = dic.Name,
                        Remark = dic.Remark,
                        Type = dic.Type
                    });
                }
                else
                {
                    item.Discount = dic.Discount;
                    item.Remark = dic.Remark;
                } 
            }
            DbContext.SaveChanges();
        }



    }
}
