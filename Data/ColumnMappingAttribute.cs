using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    public class ColumnMappingAttribute : Attribute
    {
        public ColumnMappingAttribute()
        {
        }

        public ColumnMappingAttribute(string column)
        {
            Column = column;
        }

        public ColumnMappingAttribute(string column, ReflectionColumnType columnType)
        {
            Column = column;
            ColumnType = columnType;
        }

        public ColumnMappingAttribute(string column, bool canSave, bool languecode = false)
        {
            Column = column;
            CanSave = canSave;
            LangueCode = languecode;
        }

        public string Column { get; set; }

        bool _CanSet = true;
        public bool CanSave { get { return _CanSet; } set { _CanSet = value; } }

        public bool LangueCode { get; set; }

        public ReflectionColumnType ColumnType = ReflectionColumnType.Normal;
    }
    public enum ReflectionColumnType
    {
        Normal,
        /// <summary>
        /// 主键，int自增长
        /// </summary>
        Identity,
        /// <summary>
        /// 主键
        /// </summary>
        PrimaryKey
    }
}
