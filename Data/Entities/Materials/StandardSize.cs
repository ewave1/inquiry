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
    public  class StandardSize
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public decimal SizeA { get; set; }

        public decimal SizeB { get; set; }
         
        public string Code { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
