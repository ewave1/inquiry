using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 利用率和不良第
    /// </summary>
    public class MaterialRate
    {
        [Key]
        public int Id { get; set; }
        public int MaterialId { get; set; }

        /// <summary>
        /// 线径
        /// </summary>
        public decimal SizeB { get; set; }

        /// <summary>
        /// 利用率
        /// </summary>
        public decimal UseRate { get; set; }

        /// <summary>
        /// 不良率
        /// </summary>
        public decimal BadRate { get; set; }


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
