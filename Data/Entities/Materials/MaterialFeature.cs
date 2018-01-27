﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 物性
    /// 颜色 
    /// </summary>
    public  class MaterialFeature
    {
        [Key]
        public int Id { get; set; }

        public int MaterialId { get; set; }

        public decimal Discount { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }

        /// <summary>
        /// 材料物性= 0 
        /// 表面物性 = 1
        /// 
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
