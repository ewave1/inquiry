using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 时效
    /// </summary>
    public  class MaterialHourModel
    {


        //[ColumnMapping("序号", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Id { get; set; }

        [ColumnMapping("材料", ColumnType = ReflectionColumnType.PrimaryKey)]
        public string MaterialCode { get; set; }

        [ColumnMapping("硬度", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Hardness { get; set; }
        public int MaterialId { get; set; }

        [ColumnMapping("线径1", ColumnType = ReflectionColumnType.PrimaryKey)]
        /// <summary>
        /// 线径
        /// </summary>
        public decimal SizeB { get; set; }

        [ColumnMapping("线径2")]
        public decimal SizeB2 { get; set; }

        [ColumnMapping("每小时模数")]
        /// <summary>
        /// 时数
        /// </summary>
        public int MosInHour { get; set; }


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
