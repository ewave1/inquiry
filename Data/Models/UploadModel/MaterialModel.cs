using Common;
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
    public  class MaterialModel
    {
        [ColumnMapping("编号", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Id { get; set; }

        [MinLength(3)]
        [MaxLength(12)]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "编码")]
        /// <summary>
        /// Code 
        /// </summary>
        public string Name { get; set; }
         
        [MaxLength(12)] 
        [DataType(DataType.Text)]
        [Display(Name = "名称")]
        /// <summary>
        /// 显示 
        /// </summary>
        public string Display { get; set; }
         
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "特殊件折扣")]
        /// <summary>
        /// 特殊件折扣
        /// </summary>
        public decimal SpecialDiscount { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
