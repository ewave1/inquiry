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
    /// 表面物性
    /// 颜色 
    /// </summary>
    public  class MaterialFeatureModel
    {

        //[ColumnMapping("序号", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Id { get; set; }

        [ColumnMapping("材质", ColumnType =ReflectionColumnType.PrimaryKey)]
        [MinLength(2)]
        [MaxLength(12)]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "材质")]
        public string MaterialCode { get; set; }


        [ColumnMapping("硬度", ColumnType = ReflectionColumnType.PrimaryKey)]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "硬度")]
        public int Hardness { get; set; }


        [ColumnMapping(new string[] { "特殊性","颜色","表面物性","特性"})]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "物性或颜色")]
        public string Name { get; set; }

        public int MaterialId { get; set; }

        [ColumnMapping("系数")]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "系数")]
        public decimal Discount { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }

        /// <summary>
        /// 材料物性= 0 
        /// 表面物性 = 1 
        /// </summary>
        public MATERIALTYPE Type { get; set; }
    }
     
}
