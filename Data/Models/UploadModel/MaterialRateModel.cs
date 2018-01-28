using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 利用率和不良第
    /// </summary>
    public class MaterialRateModel
    {

        //[ColumnMapping("序号", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Id { get; set; }
         

        [ColumnMapping("线径1", ColumnType = ReflectionColumnType.PrimaryKey)]
        /// <summary>
        /// 线径
        /// </summary>
        public decimal SizeB { get; set; }
        [ColumnMapping("线径2", ColumnType = ReflectionColumnType.PrimaryKey)]
        /// <summary>
        /// 线径
        /// </summary>
        public decimal SizeB2 { get; set; }

        [ColumnMapping("效率")]
        /// <summary>
        /// 利用率
        /// </summary>
        public decimal UseRate { get; set; }

        [ColumnMapping("不良率")]
        /// <summary>
        /// 不良率
        /// </summary>
        public decimal BadRate { get; set; }


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
