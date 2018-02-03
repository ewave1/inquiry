using Data.Entities;
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

           var result =  lst.GroupBy(v => v.Type).Select(u => new DiscountModel
            {
                Type =u.Key,
                Values =u.ToList().Select(x=>new SingleModel { Name = x.Name,Value = x.Discount}).ToList()
            }).ToList();

            //if(result.Where(v=>v.Type== DisCountType.材料物性).Count() == 0)
            //{ 
            //    result.Add(new DiscountModel {
            //        Type = DisCountType.材料物性,
            //        Values = new List<SingleModel> { new SingleModel { Name = "",Value = 0 } }
            //    });
                
            //}
            //if (result.Where(v => v.Type == DisCountType.表面物性).Count() == 0)
            //{
            //    result.Add(new DiscountModel
            //    {
            //        Type = DisCountType.表面物性,
            //        Values = new List<SingleModel> { new SingleModel { Name = "", Value = 0 } }
            //    });

            //}

            return result.ToList();
        }

        public void SetDiscounts(List<DiscountModel> lst)
        {
            DbContext.DiscountSet.RemoveRange(DbContext.DiscountSet.Where(v => 
            v.Type != Data.Entities.DisCountType.FACTORY&& v.Type!= DisCountType.Other));
            DbContext.SaveChanges();
            foreach ( var detail in lst )
            foreach(var dic  in detail.Values)
            {
                    if (string.IsNullOrWhiteSpace(dic.Name))
                        continue;
                var item = DbContext.DiscountSet.Where(v=>v.Name==dic.Name&&v.Type==detail.Type).FirstOrDefault();
                if (item == null)
                {
                    DbContext.DiscountSet.Add(new Data.Entities.DiscountSet
                    {
                        Discount = dic.Value,
                        Name = dic.Name.Trim(), 
                        Type = detail.Type
                    });
                }
                else  
                {
                    item.Discount = dic.Value; 
                } 
            }
            DbContext.SaveChanges();
        }



    }
}
