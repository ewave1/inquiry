using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{

    public class SingleModel
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
    }

    public  class DiscountModel
    {
        public List<SingleModel> Values { get; set; }
        public bool IsCanAdd
        {
            get
            {
                if (Type == DisCountType.Other||Type== DisCountType.FACTORY)
                    return false;
                return true;
            }
        }

        public string IsReadOnly
        {
            get
            {
                if (IsCanAdd)
                    return "";
                return " readonly='readonly'";
            }
        }


        /// <summary>
        /// 类型
        /// </summary>
        public DisCountType Type { get; set; }
        public string Name
        {
            get
            {
                if (Type == DisCountType.FACTORY)
                    return "厂商";
                if (Type == DisCountType.Other)
                    return "其它";
                return Type.ToString();
            }
        }
         
         
    }
}
