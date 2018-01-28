using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 时效
    /// </summary>
    public  class MaterialHour
    {
        [Key]
        public int Id { get; set; }
        public int MaterialId { get; set; }

        public string MaterialCode { get; set; }
        public int Hardness { get; set; }
        /// <summary>
        /// 线径
        /// </summary>
        public decimal SizeB { get; set; }

        public decimal SizeB2 { get; set; }

        /// <summary>
        /// 每小时模数
        /// </summary>
        public int MosInHour { get; set; }


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
