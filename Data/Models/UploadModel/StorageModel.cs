using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    /// <summary>
    /// 库存
    /// </summary>
    public class StorageModel
    {
         
        public int Id { get; set; }
        ///// <summary>
        ///// 内径
        ///// </summary> 
        //[ColumnMapping("内径")]
        //public decimal SizeA { get; set; }

        ///// <summary>
        ///// 线径
        ///// </summary> 
        //[ColumnMapping("线径")]
        //public decimal SizeB { get; set; }


        ///// <summary>
        ///// 材料
        ///// </summary>
        //[ColumnMapping("材质")]
        //public string MaterialCode { get; set; }
        //public int MaterialId { get; set; }

        //[Display(Name = "材料物性")]

        //[ColumnMapping("材料物性")]
        //public string Material1 { get; set; }

        //[Display(Name = "表面物性")]
        //[ColumnMapping("表面物性")]
        //public string Material2 { get; set; }

        //[Display(Name = "颜色")]

        //[ColumnMapping("颜色")]
        //public string Color { get; set; }

        //[Display(Name = "硬度")]

        //[ColumnMapping("硬度")]
        //public int Hardness { get; set; }


        [ColumnMapping("物料名称", ColumnType = ReflectionColumnType.PrimaryKey)]
        [Display(Name = "物料名称")]
        public string Name { get; set; }

        [ColumnMapping("规格型号")]
        [Display(Name = "规格型号")]
        public string Spec { get; set; }

        [ColumnMapping("批号")]
        [Display(Name = "批号")]
        public string BatchNo { get; set; }

        [ColumnMapping("仓库代码")]
        [Display(Name = "仓库代码")]
        public string Location { get; set; }

        [ColumnMapping("备注")]
        [Display(Name = "备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 库存数
        /// </summary>
        [ColumnMapping("综合数量")]
        public string Number { get; set; }  


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
