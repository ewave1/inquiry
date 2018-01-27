using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 比重
    /// </summary>
    public class MaterialGravityModel
    {

        [ColumnMapping("编号", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Id { get; set; }

        public int MaterialId { get; set; }

        public int Hardness { get; set; }

        public int  Gravity { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
         
    }
}
