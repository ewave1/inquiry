using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 上传的文件
    /// </summary>
    public class UploadFile
    {
        [Key]
        public int Id { get; set; }
        public string FileUrl { get; set; }

        public string FileName { get; set; }

        public string LocalPath { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public FILETYPE FileType { get; set; }

        public DateTime  UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }


    public enum FILETYPE
    {
        None = 0,//查询时使用，所有
        库存 = 1,
        物性 = 2,
        颜色 = 3,
        比重 = 4,
        孔数 = 5,
        生产效率 = 6,
        其它 = 7
    }
}
