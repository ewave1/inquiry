using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{ 
    /// <summary>
    /// 孔数
    /// </summary>
    public   class MaterialHole
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }

        /// <summary>
        /// 外径=SizeA+SizeB
        /// </summary>
        public decimal SizeC { get; set; }

        /// <summary>
        /// 孔数
        /// </summary>
        public int HoleCount { get; set; }


        public DateTime UpdateTime { get; set; }

        public int? UpdateUser { get; set; }
    }
}
