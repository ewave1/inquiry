using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class MaterialStorage
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

        [Display(Name = "规格")]
        public string Spec { get; set; }

        [Display(Name = "模板规格")]
        public string Spec2 { get; set; }

        [Display(Name = "模具编号")]
        public string BatchNo { get; set; }

        [Display(Name = "储位")]
        public string Location { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }
         


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
