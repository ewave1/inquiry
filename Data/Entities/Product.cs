using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 系统中的产品
    /// </summary>
    public  class Product
    {
        /// <summary>
        /// 自增
        /// </summary>
        [Key] 
        public int Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Code { get; set; }

        /// <summary>
        /// 内径
        /// </summary>
        [Required] 
        public decimal SizeA { get; set; }

        /// <summary>
        /// 线径
        /// </summary>
        [Required]
        public decimal SizeB { get; set; }


        public string Material { get; set; }
        public int MaterialId { get; set; }

        public decimal Price { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; }
    }
     
}
