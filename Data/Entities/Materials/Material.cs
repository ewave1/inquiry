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
        /// Code 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示 
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 特殊件折扣
        /// </summary>
        public decimal SpecialDiscount { get; set; }

        public string Remark { get; set; }


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
