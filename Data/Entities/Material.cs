using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 材质
    /// </summary>
    public  class Material
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }
    }
}
