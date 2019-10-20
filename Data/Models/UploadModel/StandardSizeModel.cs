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
    /// 孔数
    /// </summary>
    public  class StandardSizeModel
    { 
        public int Id { get; set; }

        /// <summary>
        ///  
        /// </summary>
        [ColumnMapping("内径")]
        [Display(Name = "内径")]
        public decimal SizeA { get; set; }
        [ColumnMapping("线径")]
        [Display(Name = "线径")]

        public decimal SizeB { get; set; }
        [ColumnMapping("代码")]
        [Display(Name = "代码")]
        public string Code { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
