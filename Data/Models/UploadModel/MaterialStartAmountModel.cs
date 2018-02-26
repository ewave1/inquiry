using Common;
using Data.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{

    public class MaterialStartAmountModel
    { 
        public int Id { get; set; }

        [ColumnMapping("外径1", ColumnType = ReflectionColumnType.PrimaryKey)]
        [DataType(DataType.Text)]
        [Display(Name = "外径1")]
        [Required]
        /// <summary>
        /// 外径
        /// </summary>
        public decimal SizeC { get; set; }

        [ColumnMapping("外径2")]
        [DataType(DataType.Text)]
        [Display(Name = "外径2")] 
        public decimal? SizeC2 { get; set; }

        [ColumnMapping("起订金额", ColumnType = ReflectionColumnType.PrimaryKey)]
        [DataType(DataType.Text)]
        [Display(Name = "起订金额")]
        [Required]
        /// <summary>
        /// 起订金额
        /// </summary>
        public decimal StartAmount { get; set; }


        [ColumnMapping("库存类型", ColumnType = ReflectionColumnType.PrimaryKey)]
        [DataType(DataType.Text)]
        [Display(Name = "库存类型")]
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        /// <summary>
        /// 库存类型
        /// </summary>
        public StorageTypeEnum StorageType { get; set; }


        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }

}
