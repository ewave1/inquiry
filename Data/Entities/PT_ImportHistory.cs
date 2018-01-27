using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 导入主表
    /// </summary>
    public class PT_ImportHistory
    {
        [Key]
        public int Id { get; set; } 
        public DateTime ImportTime { get; set; } 
        public FILETYPE ImportType { get; set; }


         
        public string User { get; set; }  
        public int TotalCount { get; set; } 
        public int SuccessCount { get; set; } 
        public bool Status { get; set; } 
        public string ImportFileName { get; set; } 
        public string ImportFile { get; set; } 
        public string ReturnFile { get; set; }
    }

    public enum SuccessENUM
    {
        导入失败 = 0,
        导入成功 = 1,
        数据重复 = 2
    }
}
