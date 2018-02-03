using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    /// <summary>
    /// 询价
    /// </summary>
    public class InquiryModelRequest
    {
        public InquiryModelRequest()
        {
        }

        /// <summary>
        /// 编码
        /// </summary>  

        [MaxLength(50)]
        [Display(Name = "编码")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "内径")]
        /// <summary>
        /// 内径
        /// </summary> 
        public decimal SizeA { get; set; }

        [Required]
        [Display(Name = "线径")]
        /// <summary>
        /// 线径
        /// </summary> 
        public decimal SizeB { get; set; }

        [Required]
        [Display(Name = "客户类型")]
        /// <summary>
        /// 工厂或贸易商
        /// </summary>
        public string Factory { get; set; }


        [Required]
        [Display(Name = "客户级别")]
        public string CustomerLevel { get; set; }

        [Required]
        [Display(Name = "数量")]
        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }


         
        [Display(Name = "材料编号")]
        public int MaterialId { get; set; }


        [Required]
        [Display(Name = "材质")]
        public string MaterialCode { get; set; }


        [Required]
        [Display(Name = "硬度")]

        public int Hardness { get; set; }

        [Required]
        [Display(Name = "材料物性")]

        public string Material1 { get; set; }

        [Required]
        [Display(Name = "表面物性")]
        public string Material2 { get; set; }



        [Required]
        [Display(Name = "颜色")]
        public string Color { get; set; }


    }
}
