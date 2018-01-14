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

        public DateTime CreateTime { get; set; }

        public string Remark { get; set; }
        public int? CreateUser { get; set; }
    }
}
