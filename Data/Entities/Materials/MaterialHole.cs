using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Key]
        public int Id { get; set; }
        public int MaterialId { get; set; }


        public string MaterialCode { get; set; }
        public int Hardness { get; set; }

        /// <summary>
        /// 外径=SizeA+SizeB
        /// </summary>
        public decimal SizeC { get; set; } = 0;


        /// <summary>
        /// 外径2
        /// </summary>
        public decimal? SizeC2 { get; set; }
        /// <summary>
        /// 比率:基础孔数* Rate=孔数
        /// </summary>
        public decimal Rate { get; set; }


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
