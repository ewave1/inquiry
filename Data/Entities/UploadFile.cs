using System;
using System.Collections.Generic;
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
        public int Id { get; set; }
        public string FileUrl { get; set; }

        public string FileName { get; set; }

        public string LocalPath { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }

        public DateTime  UpdateTime { get; set; }

        public int UpdateUser { get; set; }
    }
}
