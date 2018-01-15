using System;
using System.Collections.Generic;
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

        public int Id { get; set; }
        public int MaterialId { get; set; }

        /// <summary>
        /// 线径
        /// </summary>
        public decimal SizeB { get; set; }

        /// <summary>
        /// 孔数
        /// </summary>
        public int Hours { get; set; }


        public DateTime UpdateTime { get; set; }

        public int? UpdateUser { get; set; }
    }
}
