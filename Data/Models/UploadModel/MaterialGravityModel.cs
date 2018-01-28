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

        //[ColumnMapping("序号", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Id { get; set; }

        public int MaterialId { get; set; }
        [ColumnMapping("材料", ColumnType = ReflectionColumnType.PrimaryKey)]
        public string MaterialCode { get; set; }

        [ColumnMapping("颜色", ColumnType = ReflectionColumnType.PrimaryKey)] 
        public string Color { get; set; }

        [ColumnMapping("硬度", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Hardness { get; set; }

        [ColumnMapping("比重")]
        public decimal  Gravity { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
         
    }
}
