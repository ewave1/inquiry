﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    /// <summary>
    /// 孔数
    /// </summary>
    public  class BaseHole
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 外径=内径SizeA+线径SizeB
        /// </summary>
        public int SizeC { get; set; }

        /// <summary>
        /// 基础的孔数
        /// </summary>
        public int HoleCount { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}