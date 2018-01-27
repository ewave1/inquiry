using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{

    /// <summary>
    /// 导入明细
    /// </summary>
    public class PT_ImportHistoryDetail
    {
        [Key]
        public int Id { get; set; }
        public int ImportID { get; set; }
        public int? RelateID { get; set; }
        public SuccessENUM IsSuccess { get; set; }


        public string Json { get; set; }

        public string ErrorInfo { get; set; }
        //公司中文名称	中文简称	公司英文名称	英文简称	网站	地址	联系人名称	部门	职位	手机	邮箱	备注

    }

}
