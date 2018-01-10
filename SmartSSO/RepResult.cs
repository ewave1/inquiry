using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class RepResult
    { 
        public bool Success { get { return Code == 0; } }
         
        public int Code { get; set; }
         
        public string Msg { get; set; }

        public RepResult()
        {
        }

        public RepResult(int code, string msg)
        {
            Code = code;
            Msg = msg;
        }
    }

    public class RepResult<T> : RepResult
    { 
        public T Data { get; set; }
    }
}
