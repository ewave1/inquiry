using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 比重
    /// </summary>
    public class MaterialGravity
    {
        [Key]
        public int Id { get; set; }

        public int MaterialId { get; set; }

        public string Color { get; set; }

        public int Hardness { get; set; }

        public decimal  Gravity { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
         
    }
}
