using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 库存
    /// </summary>
    public  class Storage
    {

        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 内径
        /// </summary> 
        public decimal SizeA { get; set; }

        /// <summary>
        /// 线径
        /// </summary> 
        public decimal SizeB { get; set; }
         

        /// <summary>
        /// 材质
        /// </summary>
        public string MaterialCode { get; set; }

        public string MaterialDisplay { get; set; }

        [Display(Name = "硬度")]
        public int Hardness { get; set; }
        public int MaterialId { get; set; }

        [Display(Name = "材料物性")]

        public string Material1 { get; set; }

        [Display(Name = "表面物性")]
        public string Material2 { get; set; }

        [Display(Name = "颜色")]

        public string Color { get; set; }


        [Display(Name = "物料名称")]
        public string Name { get; set; }

        [Display(Name = "规格型号")]
        public string Spec { get; set; }

        [Display(Name = "批号")]
        public string BatchNo { get; set; }

        [Display(Name = "仓库代码")]
        public string Location { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 库存数
        /// </summary>
        public int Number { get; set; } = 0;


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
