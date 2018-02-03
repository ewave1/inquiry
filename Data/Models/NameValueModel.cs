using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class NameValueModel
    {
        public string Name { get; set; }

        public string Value { get; set; }


    }

    public enum MATERIALMODELTYPE
    {
        Hardness = 0,
        Material1 = 1,
        Material2 = 2,
        Color = 3 
    }
}
