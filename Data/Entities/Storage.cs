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
        /// 材料
        /// </summary>
        public string Material { get; set; }
        public int MaterialId { get; set; }

        [Display(Name = "材料物性")]

        public string Material1 { get; set; }

        [Display(Name = "表面物性")]
        public string Material2 { get; set; }

        [Display(Name = "颜色")]

        public string Color { get; set; }

        [Display(Name = "硬度")]
        public int Hardness { get; set; }


        /// <summary>
        /// 库存数
        /// </summary>
        public int Number { get; set; } = 0;


        public DateTime UpdateTime { get; set; }

        public int? UpdateUser { get; set; }
    }
}
