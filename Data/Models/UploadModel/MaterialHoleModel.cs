using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{ 
    /// <summary>
    /// 孔数
    /// </summary>
    public   class MaterialHoleModel
    {

        //[ColumnMapping("序号", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Id { get; set; }

        public int MaterialId { get; set; }
        [ColumnMapping("材料", ColumnType = ReflectionColumnType.PrimaryKey)]
        public string MaterialCode { get; set; }


        [ColumnMapping("硬度", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Hardness { get; set; }

        [ColumnMapping("外径", ColumnType = ReflectionColumnType.PrimaryKey)]
        /// <summary>
        /// 外径=SizeA+SizeB
        /// </summary>
        public decimal SizeC { get; set; }

        [ColumnMapping("系数")]
        public decimal Rate { get; set; }

        [ColumnMapping("孔数")]
        /// <summary>
        /// 孔数
        /// </summary>
        public int HoleCount { get; set; }


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
