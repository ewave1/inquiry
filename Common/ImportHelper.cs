using Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    /// <summary>
    /// 导入工具
    /// </summary>
    public static  class ImportHelper
    {
        /// <summary>
        /// 获取 Key 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<string> GetPrimaryKeys(Type type)
        {
            List<string> keys = new List<string>();
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (PropertyInfo p in propertyInfos)
            {

                object[] attrs = p.GetCustomAttributes(typeof(ColumnMappingAttribute), true);
                if (attrs == null || attrs.Length == 0)
                    continue;
                ColumnMappingAttribute columnMapping = (ColumnMappingAttribute)attrs[0];

                if (columnMapping.ColumnType == ReflectionColumnType.Identity || columnMapping.ColumnType == ReflectionColumnType.PrimaryKey)
                {
                    //keys.Add(columnMapping.Column);
                    keys.Add(p.Name);
                }

            }
            return keys;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private static object GetTargetValue(PropertyInfo p, DataRow row)
        {
            object[] attrs = p.GetCustomAttributes(typeof(ColumnMappingAttribute), true);
            if (attrs == null || attrs.Length == 0)
                return null;
            ColumnMappingAttribute attr = (ColumnMappingAttribute)attrs[0];
            if (!row.Table.Columns.Contains(attr.Column??""))
            {
                if(attr.AlterColumns!=null)
                {
                    foreach(var col in attr.AlterColumns)
                    {
                        if(row.Table.Columns.Contains(col))
                        {
                            attr.Column = col;
                            break;
                        }
                    }
                }
                if (!row.Table.Columns.Contains(attr.Column))
                    return null;
            }
            object sourceValue = row[attr.Column];
            if (sourceValue == DBNull.Value) sourceValue = null;
            Type sType = sourceValue == null ? null : sourceValue.GetType();
            Type vType = p.PropertyType;
            object targetValue = null;

            if (vType == typeof(int))
            {
                int v = 0;
                int.TryParse(sourceValue + string.Empty, out v);
                targetValue = v;
            }
            else if (vType == typeof(int?))
            {
                if (sourceValue != null)
                {
                    int v = 0;
                    if (int.TryParse(sourceValue + string.Empty, out v))
                        targetValue = v;
                }
            }
            else if (vType == typeof(string))
            {
                targetValue = sourceValue + string.Empty;
            }
            else if (vType == typeof(bool))
            {
                if (sType == typeof(bool))
                    targetValue = sourceValue;
                else if (sType == typeof(int))
                    targetValue = (int)sourceValue == 1;
                else
                {
                    bool v = false;
                    bool.TryParse(sourceValue + string.Empty, out v);
                    targetValue = v;
                }
            }
            else if (vType == typeof(bool?))
            {
                if (sourceValue != null)
                {
                    if (sType == typeof(bool))
                        targetValue = sourceValue;
                    else if (sType == typeof(int))
                        targetValue = (int)sourceValue == 1;
                    else
                    {
                        bool v = false;
                        if (bool.TryParse(sourceValue + string.Empty, out v))
                            targetValue = v;
                    }
                }
            }
            else if (vType == typeof(DateTime))
            {
                if (sType == typeof(DateTime))
                    targetValue = sourceValue;
                else
                {
                    DateTime v = DateTime.MinValue;
                    if (DateTime.TryParse(sourceValue + string.Empty, out v))
                        targetValue = v;
                }
            }
            else if (vType == typeof(DateTime?))
            {
                if (sourceValue != null)
                {
                    if (sType == typeof(DateTime))
                        targetValue = sourceValue;
                    else
                    {
                        DateTime v = DateTime.MinValue;
                        if (DateTime.TryParse(sourceValue + string.Empty, out v))
                            targetValue = v;
                    }
                }
            }
            else if (vType == typeof(double))
            {
                double v = 0;
                double.TryParse(sourceValue + string.Empty, out v);
                targetValue = v;
            }
            else if (vType == typeof(double?))
            {
                if (sourceValue != null)
                {
                    double v = 0;
                    if (double.TryParse(sourceValue + string.Empty, out v))
                        targetValue = v;
                }
            }
            else if (vType == typeof(decimal))
            {
                decimal v = 0;
                decimal.TryParse(sourceValue + string.Empty, out v);
                targetValue = v;
            }
            else if (vType == typeof(decimal?))
            {
                if (sourceValue != null)
                {
                    decimal v = 0;
                    if (decimal.TryParse(sourceValue + string.Empty, out v))
                        targetValue = v;
                }
            }
            //else
            //{
            //    object[] sAttrs = p.GetCustomAttributes(typeof(DataSerializableAttribute), true);
            //    if (sAttrs != null && sAttrs.Length > 0)
            //    {
            //        DataSerializableAttribute cdsAttr = sAttrs[0] as DataSerializableAttribute;
            //        if (sourceValue + string.Empty != "")
            //        {
            //            targetValue = CTool.DeserializeXml(cdsAttr.Type, sourceValue + string.Empty);
            //        }
            //    }
            //}
            return targetValue;
        }

        public static Object Get(Type type, DataRow row)
        {
            var obj = Activator.CreateInstance(type);

            List<PropertyInfo> pList = type.GetProperties().ToList();
            foreach (PropertyInfo p in pList)
            {
                var targetValue = GetTargetValue(p, row);
                if (targetValue == null)
                    continue;
                if (p.PropertyType == typeof(string))
                    targetValue = targetValue?.ToString().Trim();
                p.SetValue(obj, targetValue, null);
            }

            return obj;
        }


        public static List<PT_ImportHistoryDetail> AddToImport(Type type, int pid, string file)
        {

            var ds = ExcelHelper.ExcelToDataSet(file);
            List<PT_ImportHistoryDetail> lst = new List<PT_ImportHistoryDetail>();
            foreach (DataTable tbl in ds.Tables)
            {
                var keys = ImportHelper.GetPrimaryKeys(type);
                foreach (DataRow row in tbl.Rows)
                {
                    var detail = ImportHelper.Get(type, row);
                    var IsValid = true;
                    foreach (var key in keys)
                    {
                        var keyObj = type.GetProperty(key).GetValue(detail);
                        if (keyObj == null || string.IsNullOrEmpty(keyObj.ToString()))
                        {
                            IsValid = false;
                            break;
                        }
                    }
                    if (!IsValid)
                        continue;

                    var item = new PT_ImportHistoryDetail
                    {
                        Json = JsonConvert.SerializeObject(detail),
                        ImportID = pid
                    };

                    lst.Add(item);
                }
            }
            return lst;
        }

        public static void ExportToExcel(Type type, List<PT_ImportHistoryDetail> lst, string file)
        {
            var nlst = new List<Dictionary<string, dynamic>>();
            lst.ForEach(v =>
            {
                var dic = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(v.Json);
                dic["是否成功"] = v.IsSuccess.ToString();

                nlst.Add(dic);
            });
            ExcelHelper.WriteDicToExcel(file, nlst, true);
        }
    }
}
