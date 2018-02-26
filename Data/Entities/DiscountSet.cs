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
        public int Id { get; set; }
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
        FACTORY = 0,//只能修改折扣
        Other = 1, //只能修改折扣
     
        客户级别= 4,
        库存级别 = 5,
        利润率 =6,
        每小时成本 = 7
    }

    public enum StorageType
    {
        无模具,
        无库存,
        有库存,
    }
     
}
