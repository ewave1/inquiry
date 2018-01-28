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
    /// 比重
    /// </summary>
    public class MaterialGravityModel
    {

        //[ColumnMapping("序号", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Id { get; set; }

        public int MaterialId { get; set; }
        [ColumnMapping("材质", ColumnType = ReflectionColumnType.PrimaryKey)]
        [MinLength(2)]
        [MaxLength(12)]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "材质")]
        public string MaterialCode { get; set; }

        [ColumnMapping("颜色", ColumnType = ReflectionColumnType.PrimaryKey)]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "颜色")]
        public string Color { get; set; }

        [ColumnMapping("硬度", ColumnType = ReflectionColumnType.PrimaryKey)]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "硬度")]
        public int Hardness { get; set; }

        [ColumnMapping("比重")]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "比重")]
        public decimal  Gravity { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
         
    }
}
