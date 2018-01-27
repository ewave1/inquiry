using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ImportHistory
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FileUrl { get; set; }

        public IMPORTTYPE FileType { get; set; }

        public string User { get; set; }

        public DateTime ImportTime { get; set; }
    }

    public enum IMPORTTYPE
    {
        库存=1,
        物性 = 2,
        颜色  = 3,
        比重= 4,
        孔数= 5,
        生产效率 = 6,
        其它=7        
    }
}
