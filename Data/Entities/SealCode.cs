using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public  class SealCode
    {

        [Key]
        public string Code { get; set; }

        public decimal SizeA { get; set; }
        public decimal SizeB { get; set; }
        public decimal CalSize { get; set; }
    }
}
