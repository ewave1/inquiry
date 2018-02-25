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
        public int Id { get; set; }

        /// <summary>
        /// 如EP
        /// </summary>
        [MinLength(2)]
        [MaxLength(12)]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "材质")]
        [ColumnMapping("材质", ColumnType = ReflectionColumnType.PrimaryKey)]
        /// <summary>
        /// Code 
        /// </summary>
        public string MaterialCode { get; set; }


        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "硬度")]
        [ColumnMapping("硬度", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Hardness { get; set; }
         
        /// <summary>
        /// 如EP70 
        /// </summary>
        [MaxLength(12)] 
        [DataType(DataType.Text)]
        [Display(Name = "编码")]
        [ColumnMapping("编码")]
        /// <summary>
        /// 显示 
        /// </summary>
        public string Display { get; set; }
          
        [DataType(DataType.Text)]
        [Display(Name = "特殊件折扣")]
        [ColumnMapping("特殊件折扣")]
        /// <summary>
        /// 特殊件折扣
        /// </summary>
        public decimal SpecialDiscount { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "单价")]
        [ColumnMapping("单价")]
        [Required]
        /// <summary>
        /// 特殊件折扣
        /// </summary>
        public decimal Price { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }


        [ColumnMapping("是否默认")]
        [Display(Name = "是否默认")]
        public bool IsDefault { get; set; }


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
