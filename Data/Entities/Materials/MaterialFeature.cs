using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 表面物性
    /// 颜色 
    /// </summary>
    public  class MaterialFeature
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        public int MaterialId { get; set; }

        public string MaterialCode { get; set; }

        public int Hardness { get; set; }
        /// <summary>
        /// 显示的名称，比如颜色，或物性
        /// </summary>
        public string Name { get; set; }

        public decimal Discount { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }


        /// <summary>
        /// 每个材质是有默认的颜色 
        /// 默认的物性是Normal
        /// </summary>
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// 材料物性= 0 
        /// 表面物性 = 1
        ///  颜色 =2 , 
        /// </summary>
        public MATERIALTYPE Type { get; set; }
    }

    public enum MATERIALTYPE
    {
        材料物性= 0,
        表面物性 = 1 ,
        颜色 =2 , 
    }
}
