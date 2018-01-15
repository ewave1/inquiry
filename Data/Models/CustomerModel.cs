using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    /// <summary>
    /// 客户
    /// </summary>
    public sealed  class CustomerModel
    {

        public int Id { get; set; }

        [MinLength(3)]
        [MaxLength(12)]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "公司名称")]
        public string CompanyName { get; set; }

        [MinLength(3)]
        [MaxLength(12)]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "联系人")]
        public string ContactName { get; set; }

        [MinLength(6)]
        [MaxLength(12)]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "联系方式")]
        public string ContactMobile { get; set; }

        public DateTime CreateTime { get; set; }


        [MinLength(3)]
        [MaxLength(120)] 
        [DataType(DataType.Text)]
        [Display(Name = "备注")]
        public string Remark { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "客户级别")]
        public string CustomerLevel { get; set; }
    }
}
