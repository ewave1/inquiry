﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 物性
    /// 颜色 
    /// </summary>
    public  class MaterialFeatureModel
    {

        //[ColumnMapping("序号", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Id { get; set; }

        [ColumnMapping("材料",ColumnType =ReflectionColumnType.PrimaryKey)]
        public string MaterialCode { get; set; }


        [ColumnMapping("硬度", ColumnType = ReflectionColumnType.PrimaryKey)]
        public int Hardness { get; set; }


        [ColumnMapping("物性或颜色")]
        public string Name { get; set; }

        public int MaterialId { get; set; }

        [ColumnMapping("系数")]
        public decimal Discount { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }

        /// <summary>
        /// 材料物性= 0 
        /// 表面物性 = 1 
        /// </summary>
        public MATERIALTYPE Type { get; set; }
    }
     
}
