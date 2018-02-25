using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 材质
    /// </summary>
    public  class Material
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Code 如EP
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 如70 
        /// </summary>
        public int Hardness { get; set; }

        /// <summary>
        /// 显示 如EP70(
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 特殊件折扣
        /// </summary>
        public decimal SpecialDiscount { get; set; }


        /// <summary>
        /// 每个材质有一个默认的硬度
        /// 
        /// </summary>
        public bool IsDefault { get; set; } = false;

        public string Remark { get; set; }

        public decimal Price { get; set; }


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
