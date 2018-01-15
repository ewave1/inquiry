using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 客户
    /// </summary>
    public  class Customer
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }

        public string ContactName { get; set; }

        public string ContactMobile { get; set; }

        /// <summary>
        /// DiscountSet 设置表Type = 客户级别
        /// </summary>
        public string CustomerLevel { get; set; }

        public DateTime CreateTime { get; set; }

        public string Remark { get; set; }
        public string CreateUser { get; set; }

        public DateTime? UpdateTime { get; set; }
        public string UpdateUser { get; set; }
    }
}
