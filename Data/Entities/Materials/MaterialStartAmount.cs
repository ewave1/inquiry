using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public   class MaterialStartAmount
    {
        [Key]
        public int Id { get; set; } 

        /// <summary>
        /// 外径
        /// </summary>
        public decimal SizeC { get; set; }
         
        public decimal? SizeC2 { get; set; }

        /// <summary>
        /// 起订金额
        /// </summary>
        public decimal StartAmount { get; set; }

        /// <summary>
        /// 库存类型
        /// </summary>
        public StorageTypeEnum StorageType { get; set; }


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }

    
}
