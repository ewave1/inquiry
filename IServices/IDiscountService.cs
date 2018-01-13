using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IDiscountService
    {
        /// <summary>
        /// 查询折扣设置
        /// </summary>
        /// <returns></returns>
        List<DiscountModel> GetDiscounts();

        /// <summary>
        /// 设置折扣
        /// </summary>
        /// <param name="dics"></param>
        void SetDiscounts(List<DiscountModel> lst);
    }
}
