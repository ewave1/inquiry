using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public  class DiscountModel
    {
        public string Name { get; set; }

        public decimal Discount { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public DisCountType Type { get; set; }
    }
}
