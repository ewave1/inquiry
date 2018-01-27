using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    public static class ExcelHelper
    {
        public static void WriteDataToExceel(string fileName, DataSet ds, bool isOverideFile = true, params string[] SetColumns)
        {
            if (!isOverideFile && File.Exists(fileName))
                throw new Exception("File is Exists!");

            using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                try
                {
                    IWorkbook workbook = ReadToWorkBook(ds, fileName, SetColumns);
                    workbook.Write(stream);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }
        public static MemoryStream WriteDataToExcel(DataSet ds, string Extension = ".xls", params string[] SetColumns)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                IWorkbook workbook = ReadToWorkBook(ds, Extension);
                workbook.Write(memoryStream);

                workbook = null;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return memoryStream;
        }
        #region 按类型导出
        public static void WriteListToExcel<T>(string fileName, List<T> ds, bool isOverideFile = true, params string[] SetColumns) where T : new()
        {
            if (!isOverideFile && File.Exists(fileName))
                throw new Exception("File is Exists!");

            using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                try
                {
                    IWorkbook workbook = ReadToWorkBookByList<T>(ds, fileName, SetColumns);
                    workbook.Write(stream);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }
        private static IWorkbook ReadToWorkBookByList<T>(List<T> ds, string Extension, params string[] SetColumns) where T : new()
        {
            IWorkbook workbook = null;
            if (Extension.ToLower().EndsWith(".xls"))
                workbook = new HSSFWorkbook();
            else
                workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            IRow headerRow = sheet.CreateRow(0);
            var type = typeof(T);
            var ps = type.GetProperties();
            int idx = 0;
            foreach (var p in ps)
            {
                if (SetColumns != null && SetColumns.Length > 0 && !SetColumns.Contains(p.Name))
                    continue;
                string columnName = p.Name;

                headerRow.CreateCell(idx++).SetCellValue(columnName);
            }
            int rowIndex = 1;
            foreach (var t in ds)
            {
                IRow dataRow = sheet.CreateRow(rowIndex++);
                idx = 0;
                foreach (var p in ps)
                {
                    if (SetColumns != null && SetColumns.Length > 0 && !SetColumns.Contains(p.Name))
                        continue;
                    var value = p.GetValue(t, null);
                    if (value != null)
                        dataRow.CreateCell(idx++).SetCellValue(value.ToString());
                    else
                        dataRow.CreateCell(idx++).SetCellValue("");
                }
            }
            sheet = null;
            headerRow = null;
            return workbook;
        }
        #endregion

        #region 按dic
        public static void WriteDicToExcel(string fileName, List<Dictionary<string, dynamic>> ds, bool isOverideFile = true, params string[] SetColumns)
        {
            if (!isOverideFile && File.Exists(fileName))
                throw new Exception("File is Exists!");
            if (ds.Count == 0)
                throw new Exception("没有数据!");

            using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                try
                {
                    IWorkbook workbook = ReadToWorkBookByDic(ds, fileName, SetColumns);
                    workbook.Write(stream);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }
        private static IWorkbook ReadToWorkBookByDic(List<Dictionary<string, dynamic>> ds, string Extension, params string[] SetColumns)
        {
            IWorkbook workbook = null;
            if (Extension.ToLower().EndsWith(".xls"))
                workbook = new HSSFWorkbook();
            else
                workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            IRow headerRow = sheet.CreateRow(0);

            int idx = 0;
            foreach (var p in ds[0])
            {
                if (SetColumns != null && SetColumns.Length > 0 && !SetColumns.Contains(p.Key))
                    continue;
                string columnName = p.Key;

                headerRow.CreateCell(idx++).SetCellValue(columnName);
            }
            int rowIndex = 1;
            foreach (var t in ds)
            {
                IRow dataRow = sheet.CreateRow(rowIndex++);
                idx = 0;
                foreach (var p in t)
                {
                    if (SetColumns != null && SetColumns.Length > 0 && !SetColumns.Contains(p.Key))
                        continue;
                    var value = p.Value;
                    if (value != null)
                        dataRow.CreateCell(idx++).SetCellValue(value.ToString());
                    else
                        dataRow.CreateCell(idx++).SetCellValue("");
                }
            }
            sheet = null;
            headerRow = null;
            return workbook;
        }
        #endregion
        private static IWorkbook ReadToWorkBook(DataSet ds, string Extension, params string[] SetColumns)
        {
            IWorkbook workbook = null;
            if (Extension.ToLower().EndsWith(".xls"))
                workbook = new HSSFWorkbook();
            else
                workbook = new XSSFWorkbook();
            foreach (DataTable table in ds.Tables)
            {
                ISheet sheet = workbook.CreateSheet(table.TableName);
                int firstRow = 0;
                if (!string.IsNullOrEmpty(table.Namespace))
                {
                    IRow hRow = sheet.CreateRow(0);
                    hRow.CreateCell(0).SetCellValue(table.Namespace);
                    CellRangeAddress r = new CellRangeAddress(0, 0, 0, table.Columns.Count - 1);
                    sheet.AddMergedRegion(r);
                    firstRow = 1;
                }
                IRow headerRow = sheet.CreateRow(firstRow);
                foreach (DataColumn column in table.Columns)
                {
                    if (SetColumns != null && SetColumns.Length > 0 && !SetColumns.Contains(column.ColumnName))
                        continue;
                    string columnName = column.Caption;
                    if (string.IsNullOrEmpty(columnName))
                        columnName = column.ColumnName;
                    headerRow.CreateCell(column.Ordinal).SetCellValue(columnName);
                }
                int rowIndex = firstRow + 1;
                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in table.Columns)
                    {
                        if (SetColumns != null && SetColumns.Length > 0 && !SetColumns.Contains(column.ColumnName))
                            continue;
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }
                sheet = null;
                headerRow = null;
            }

            return workbook;
        }

        public static DataSet ExcelToDataSet(string excelPath, string CustomerHeader = null)
        {
            return ExcelToDataSet(excelPath, true, CustomerHeader);
        }
        public static DataSet ExcelToDataSet(string excelPath, bool firstRowAsHeader, string CustomerHeader = null)
        {
            int sheetCount;
            try
            {
                return ExcelToDataSet(excelPath, firstRowAsHeader, CustomerHeader, out sheetCount);
            }
            catch
            {
                return ExcelToDataSet(excelPath, firstRowAsHeader, CustomerHeader, out sheetCount, true);
            }
        }

        public static DataSet ExcelToDataSet(string excelPath, bool firstRowAsHeader, string CustomerHeader, out int sheetCount, bool isXlsx = false)
        {
            using (DataSet ds = new DataSet())
            {
                using (FileStream fileStream = new FileStream(excelPath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook;
                    IFormulaEvaluator evaluator;
                    GetWorkBook(excelPath, fileStream, out workbook, out evaluator, isXlsx);
                    sheetCount = workbook.NumberOfSheets;
                    for (int i = 0; i < sheetCount; ++i)
                    {
                        DataTable dt = ExcelToDataTable(workbook.GetSheetAt(i), evaluator, firstRowAsHeader, CustomerHeader);
                        ds.Tables.Add(dt);


                    }
                    return ds;
                }
            }
        }

        public static List<string> ExcelToNextValue(string excelPath, string CustomerHeader, bool isXlsx = false)
        {
            List<string> list = new List<string>();
            using (FileStream fileStream = new FileStream(excelPath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook;
                IFormulaEvaluator evaluator;
                GetWorkBook(excelPath, fileStream, out workbook, out evaluator, isXlsx);
                int sheetCount = workbook.NumberOfSheets;

                for (int i = 0; i < sheetCount; ++i)
                {
                    string res = ExcelToGetNextValueAsHeader(workbook.GetSheetAt(i), evaluator, CustomerHeader);
                    list.Add(res);

                }
                return list;
            }
        }

        public static DataTable ExcelToDataTable(string excelPath, string sheetName, bool firstRowAsHeader = true, string CustomerHeader = null)
        {
            using (FileStream fileStream = new FileStream(excelPath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook;
                IFormulaEvaluator evaluator;
                GetWorkBook(excelPath, fileStream, out workbook, out evaluator);
                return ExcelToDataTable(workbook.GetSheet(sheetName), evaluator, firstRowAsHeader, CustomerHeader);
            }
        }

        public static DataTable ExcelToDataTable(string excelPath, int pageIndex, bool firstRowAsHeader = true, string CustomerHeader = null)
        {
            using (FileStream fileStream = new FileStream(excelPath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook;
                IFormulaEvaluator evaluator;
                GetWorkBook(excelPath, fileStream, out workbook, out evaluator);
                if (pageIndex > workbook.NumberOfSheets)
                    throw new Exception("参数PageIndex>Excel页面总数!");
                return ExcelToDataTable(workbook.GetSheetAt(pageIndex), evaluator, firstRowAsHeader, CustomerHeader);
            }
        }

        private static void GetWorkBook(string excelPath, FileStream fileStream, out IWorkbook workbook, out IFormulaEvaluator evaluator, bool isUseXlsx = false)
        {
            workbook = null;
            evaluator = null;
            if (excelPath.ToLower().EndsWith(".xls") && !isUseXlsx)
            {
                workbook = new HSSFWorkbook(fileStream, true);
                evaluator = new HSSFFormulaEvaluator(workbook);
            }
            else
            {
                workbook = new XSSFWorkbook(fileStream);
                evaluator = new XSSFFormulaEvaluator(workbook);
            }
        }

        #region 内部方法
        private static DataTable ExcelToDataTable(ISheet sheet, IFormulaEvaluator evaluator, bool firstRowAsHeader, string CustomerHeader)
        {
            if (!string.IsNullOrWhiteSpace(CustomerHeader))
                return ExcelToDataTableCustomRowAsHeader(sheet, evaluator, CustomerHeader);
            if (firstRowAsHeader)
            {
                return ExcelToDataTableFirstRowAsHeader(sheet, evaluator);
            }
            else
            {
                return ExcelToDataTable(sheet, evaluator);
            }
        }
        private static DataTable ExcelToDataTableFirstRowAsHeader(ISheet sheet, IFormulaEvaluator evaluator)
        {
            using (DataTable dt = new DataTable())
            {
                IRow firstRow = sheet.GetRow(0) as IRow;
                int cellCount = GetCellCount(sheet);
                for (int i = 0; i < cellCount; i++)
                {
                    if (firstRow.GetCell(i) != null)
                    {
                        dt.Columns.Add(firstRow.GetCell(i).StringCellValue ?? string.Format("F{0}", i + 1), typeof(string));
                    }
                    else
                    {
                        dt.Columns.Add(string.Format("F{0}", i + 1), typeof(string));
                    }
                }
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i) as IRow;
                    DataRow dr = dt.NewRow();
                    FillDataRowByHSSFRow(row, evaluator, ref dr);
                    dt.Rows.Add(dr);
                }
                dt.TableName = sheet.SheetName;
                return dt;
            }
        }


        private static DataTable ExcelToDataTableCustomRowAsHeader(ISheet sheet, IFormulaEvaluator evaluator, string HeaderName)
        {
            using (DataTable dt = new DataTable())
            {
                int i = 0;
                int cellCount = GetCellCount(sheet);
                for (; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null)
                        continue;
                    Console.WriteLine(row.GetCell(0));
                    Console.WriteLine(row.GetCell(1));
                    if (row.GetCell(0) != null)
                    {
                        if (row.GetCell(0).StringCellValue.Equals(HeaderName))
                        {
                            for (int j = 0; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                    dt.Columns.Add(row.GetCell(j).StringCellValue ?? string.Format("F{0}", j + 1), typeof(string));
                                }
                                else
                                {
                                    dt.Columns.Add(string.Format("F{0}", j + 1), typeof(string));
                                }
                            }
                            break;
                        }
                    }

                }
                for (i++; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i) as IRow;
                    DataRow dr = dt.NewRow();
                    FillDataRowByHSSFRow(row, evaluator, ref dr);
                    dt.Rows.Add(dr);
                }
                dt.TableName = sheet.SheetName;
                return dt;
            }
        }

        private static string ExcelToGetNextValueAsHeader(ISheet sheet, IFormulaEvaluator evaluator, string HeaderName)
        {
            string res = "";
            int i = 0;
            int cellCount = GetCellCount(sheet);
            for (; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                Console.WriteLine(row.GetCell(0));
                Console.WriteLine(row.GetCell(1));

                if (row.GetCell(0).StringCellValue.Equals(HeaderName))
                {

                    var cell = row.GetCell(1);
                    switch (cell.CellType)
                    {
                        case CellType.Blank: //空数据类型处理
                            return "";
                        case CellType.String: //字符串类型
                            return cell.StringCellValue;
                        case CellType.Numeric: //数字类型                                   
                            if (DateUtil.IsValidExcelDate(cell.NumericCellValue))
                            {
                                return cell.DateCellValue.ToString("yyyy/MM/dd");
                            }
                            else
                            {
                                return cell.NumericCellValue.ToString();
                            }
                        default:
                            return "";
                    }

                }


            }
            return res;
        }

        private static DataTable ExcelToDataTable(ISheet sheet, IFormulaEvaluator evaluator)
        {
            using (DataTable dt = new DataTable())
            {
                if (sheet.LastRowNum != 0)
                {
                    int cellCount = GetCellCount(sheet);
                    for (int i = 0; i < cellCount; i++)
                    {
                        dt.Columns.Add(string.Format("F{0}", i), typeof(string));
                    }
                    for (int i = 0; i < sheet.FirstRowNum; ++i)
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.Add(dr);
                    }

                    for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i) as IRow;
                        DataRow dr = dt.NewRow();
                        FillDataRowByHSSFRow(row, evaluator, ref dr);
                        dt.Rows.Add(dr);
                    }
                }
                dt.TableName = sheet.SheetName;
                return dt;
            }
        }
        private static void FillDataRowByHSSFRow(IRow row, IFormulaEvaluator evaluator, ref DataRow dr)
        {
            if (row != null)
            {
                for (int j = 0; j < dr.Table.Columns.Count; j++)
                {
                    ICell cell = row.GetCell(j) as ICell;
                    if (cell != null)
                    {
                        switch (cell.CellType)
                        {
                            case CellType.Blank:
                                dr[j] = DBNull.Value;
                                break;
                            case CellType.Boolean:
                                dr[j] = cell.BooleanCellValue;
                                break;
                            case CellType.Numeric:
                                if (DateUtil.IsCellDateFormatted(cell))
                                    dr[j] = cell.DateCellValue;
                                else
                                    dr[j] = cell.NumericCellValue;
                                break;
                            case CellType.String:
                                dr[j] = cell.StringCellValue;
                                break;
                            case CellType.Error:
                                dr[j] = cell.ErrorCellValue;
                                break;
                            case CellType.Formula:
                                cell = evaluator.EvaluateInCell(cell) as ICell;
                                dr[j] = cell.ToString();
                                break;
                            default:
                                throw new NotSupportedException(string.Format("Catched unhandle CellType[{0}]", cell.CellType));
                        }
                    }
                }
            }
        }
        private static int GetCellCount(ISheet sheet)
        {
            int firstRowNum = sheet.FirstRowNum;
            int cellCount = 0;
            for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i)
            {
                IRow row = sheet.GetRow(i) as IRow;
                if (row != null && row.LastCellNum > cellCount)
                {
                    cellCount = row.LastCellNum;
                }
            }
            return cellCount;
        }

        #endregion
    }
}
