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
    public class MaterialStorageModel
    {

        public int Id { get; set; }


        [ColumnMapping("模具编号", ColumnType = ReflectionColumnType.PrimaryKey)]
        [Display(Name = "模具编号")]
        public string BatchNo { get; set; }


        [ColumnMapping("规格")]
        [Display(Name = "规格")]
        public string Spec { get; set; }
        [ColumnMapping("模板规格")]
        [Display(Name = "模板规格")]
        public string Spec2 { get; set; }



        [ColumnMapping("储位")]
        [Display(Name = "储位")]
        public string Location { get; set; }

        [ColumnMapping("备注")]
        [Display(Name = "备注")]
        public string Remark { get; set; }


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
