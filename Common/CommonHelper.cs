using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public   class CommonHelper
    {
        /// <summary>
        /// 通过规格计算尺寸
        /// </summary>
        /// <param name="Spec"></param>
        /// <param name="dsizeA"></param>
        /// <param name="dsizeB"></param>
        /// <returns></returns>
        public static string GetSizeAB(string Spec, out decimal dsizeA, out decimal dsizeB)
        {
            #region 规格数据
            dsizeA = 0M;
            dsizeB = 0M;
            if (Spec == null)
            {
                return "规格数据异常";
            }
            var sizes = Spec.Split(new char[] { '*', 'X' }, StringSplitOptions.RemoveEmptyEntries);
            if (sizes.Length != 2)
            {
                return "规格数据异常";
            }
            var sizeA = sizes[0];
            var sizeB = sizes[1];
            if (!decimal.TryParse(sizeA, out dsizeA) || !decimal.TryParse(sizeB, out dsizeB))
            {
                return "规格数据异常";
            }
            #endregion
            return null;
        }
    }
}
