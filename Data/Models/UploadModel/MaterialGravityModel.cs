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

        [ColumnMapping("序号", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Id { get; set; }

        [ColumnMapping("材料")]
        public string MaterialCode { get; set; }

        public int MaterialId { get; set; }



        [ColumnMapping("硬度")]
        public int Hardness { get; set; }

        [ColumnMapping("比重")]
        public int  Gravity { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
         
    }
}
