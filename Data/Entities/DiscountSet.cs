using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 用于折扣表
    /// </summary>
    public  class DiscountSet
    {
        [Key]
        [MaxLength(50)]
        public string Name { get; set; }

        public decimal Discount { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public DisCountType Type { get; set; }
    }

    public enum DisCountType
    {
        FACTORY = 0,
        Other = 1 
    }
}
