using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.UploadModel
{
    public  class BaseHoleModel
    {

        public int Id { get; set; }

        /// <summary>
        /// 外径=内径SizeA+线径SizeB
        /// </summary>
        [ColumnMapping("外径", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int SizeC { get; set; }

        [ColumnMapping("孔数")]
        /// <summary>
        /// 基础的孔数
        /// </summary>
        public int HoleCount { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
