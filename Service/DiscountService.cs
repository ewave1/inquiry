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
        public Dictionary<string,decimal> GetDiscounts()
        {
            var lst =  DbContext.DiscountSet.ToList();
            var dic = new Dictionary<string, decimal>();
            lst.ForEach(v => dic.Add(v.Name, v.Discount));
            return dic;
        }

        public void SetDiscounts(Dictionary<string,decimal> dics)
        {
            foreach(var dic  in dics)
            {
                var item = DbContext.DiscountSet.Find(dic.Key);
                if (item == null)
                {
                    DbContext.DiscountSet.Add(new Data.Entities.DiscountSet { Discount = dic.Value, Name = dic.Key });
                }
                else
                    item.Discount = dic.Value;
            }
            DbContext.SaveChanges();
        }



    }
}
