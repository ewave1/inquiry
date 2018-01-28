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


        [ColumnMapping("序号", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Id { get; set; }

        [ColumnMapping("材料")]
        public string MaterialCode { get; set; }
        public int MaterialId { get; set; }

        [ColumnMapping("线径")]
        /// <summary>
        /// 线径
        /// </summary>
        public decimal SizeB { get; set; }

        [ColumnMapping("时数")]
        /// <summary>
        /// 时数
        /// </summary>
        public int Hours { get; set; }


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
