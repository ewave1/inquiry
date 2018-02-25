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
    /// 孔数
    /// </summary>
    public   class MaterialHoleModel
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


        [ColumnMapping("硬度", ColumnType = ReflectionColumnType.PrimaryKey)]

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "硬度")]
        public int Hardness { get; set; }

        [ColumnMapping("外径1", ColumnType = ReflectionColumnType.PrimaryKey)]

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "外径1(>=)")]
        /// <summary>
        /// 外径=SizeA+SizeB
        /// </summary>
        public decimal SizeC { get; set; } = 0;


        [ColumnMapping("外径2" )]

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "外径2(<)")]
        /// <summary>
        /// 外径=SizeA+SizeB
        /// </summary>
        public decimal? SizeC2 { get; set; }
        [ColumnMapping("系数")]

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "系数")]
        public decimal Rate { get; set; }
 


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
