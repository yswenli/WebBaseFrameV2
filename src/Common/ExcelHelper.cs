//
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Common
{
    /// <summary>
    /// Excel97操作相关辅助类
    /// </summary>
    public class ExcelHelper
    {
        #region 静态简单方法
        /// <summary>
        /// 读取Excel中数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static DataTable GetData(string filePath, string sheetName)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(sheetName) || !File.Exists(filePath))
            {
                return new DataTable();
            }
            DataTable dt = new DataTable();
            string linkStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + filePath + "';Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
            OleDbDataAdapter da = new OleDbDataAdapter(@"select * from [" + sheetName + "$]", linkStr);
            da.Fill(dt);
            dt.AcceptChanges();
            da.Dispose();
            return dt;
        }

        /// <summary>
        /// 导出Excel文件，并自定义文件名 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="FileName"></param>
        public static void GetExcel(DataTable dataTable, string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || dataTable == null || dataTable.Rows.Count < 1)
            {
                return;
            }
            HttpContext curContext = HttpContext.Current;
            StringWriter strWriter = null;
            HtmlTextWriter htmlWriter = null;
            curContext.Response.Clear();
            curContext.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + ".xls");
            curContext.Response.ContentType = "application nd.ms-excel";
            curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
            curContext.Response.Charset = "GB2312";
            strWriter = new StringWriter();
            htmlWriter = new HtmlTextWriter(strWriter);
            GridView dgExport = new GridView();
            dgExport.DataSource = dataTable.DefaultView;
            dgExport.AllowPaging = false;
            dgExport.DataBind();
            dgExport.RenderControl(htmlWriter);
            curContext.Response.Write(strWriter.ToString());
            curContext.Response.Flush();
            curContext.Response.End();
        }


        /// <summary>
        /// 读取excel到dataset
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="firstRowAsHeader"></param>
        /// <param name="sheetCount"></param>
        /// <returns></returns>
        public static DataSet ExcelToDataSet(string excelPath, bool firstRowAsHeader, out int sheetCount)
        {
            using (DataSet ds = new DataSet())
            {
                using (FileStream fileStream = new FileStream(excelPath, FileMode.Open, FileAccess.Read))
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(fileStream);
                    HSSFFormulaEvaluator evaluator = new HSSFFormulaEvaluator(workbook);
                    sheetCount = workbook.NumberOfSheets;
                    for (int i = 0; i < sheetCount; ++i)
                    {
                        HSSFSheet sheet = workbook.GetSheetAt(i) as HSSFSheet;
                        DataTable dt = ExcelToDataTable(sheet, evaluator, firstRowAsHeader); ds.Tables.Add(dt);
                    }
                    return ds;
                }
            }
        }
        /// <summary>
        /// 读取excel到datatable
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string excelPath, string sheetName)
        {
            return ExcelToDataTable(excelPath, sheetName, true);
        }
        /// <summary>
        /// 读取excel到datatable
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="sheetName"></param>
        /// <param name="firstRowAsHeader"></param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string excelPath, string sheetName, bool firstRowAsHeader)
        {
            using (FileStream fileStream = new FileStream(excelPath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = null; IFormulaEvaluator evaluator = null; ISheet sheet = null;
                if (excelPath.EndsWith(".xls"))
                {
                    workbook = new HSSFWorkbook(fileStream);
                    evaluator = new HSSFFormulaEvaluator(workbook);
                    sheet = workbook.GetSheet(sheetName) as HSSFSheet;
                    if (sheet == null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    workbook = new XSSFWorkbook(fileStream);
                    evaluator = new XSSFFormulaEvaluator(workbook);
                    sheet = workbook.GetSheet(sheetName) as XSSFSheet;
                    if (sheet == null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                return ExcelToDataTable(sheet, evaluator, firstRowAsHeader);
            }
        }

        private static DataTable ExcelToDataTable(ISheet sheet, IFormulaEvaluator evaluator, bool firstRowAsHeader)
        {
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
                IRow firstRow = sheet.GetRow(0) as IRow; int cellCount = GetCellCount(sheet);
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
                    IRow row = sheet.GetRow(i) as IRow; DataRow dr = dt.NewRow();
                    FillDataRowByHSSFRow(row, evaluator, ref dr); dt.Rows.Add(dr);
                }
                dt.TableName = sheet.SheetName; return dt;
            }
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
                        DataRow dr = dt.NewRow(); dt.Rows.Add(dr);
                    }
                    for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i) as IRow; DataRow dr = dt.NewRow();
                        FillDataRowByHSSFRow(row, evaluator, ref dr); dt.Rows.Add(dr);
                    }
                }
                dt.TableName = sheet.SheetName; return dt;
            }
        }
        private static void FillDataRowByHSSFRow(IRow row, IFormulaEvaluator evaluator, ref DataRow dr)
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
                            case CellType.Blank: dr[j] = DBNull.Value; break;
                            case CellType.Boolean:
                                dr[j] = cell.BooleanCellValue; break;
                            case CellType.Numeric:
                                if (DateUtil.IsCellDateFormatted(cell))
                                {
                                    dr[j] = cell.DateCellValue;
                                }
                                else
                                {
                                    dr[j] = cell.NumericCellValue;
                                } break;
                            case CellType.String:
                                dr[j] = cell.StringCellValue; break;
                            case CellType.Error:
                                dr[j] = cell.ErrorCellValue; break;
                            case CellType.Formula:
                                cell = evaluator.EvaluateInCell(cell) as ICell; dr[j] = cell.ToString(); break;
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

        #region 非静态方法

        #region 变量
        /// <summary>
        /// 定义ExcelBook
        /// </summary>
        private IWorkbook workbook;
        /// <summary>
        /// 定义标题栏样式
        /// </summary>
        private ICellStyle styleTitle;
        #endregion

        /// <summary>
        /// 构造方法(将数据写入到excel)
        /// </summary>
        public ExcelHelper()
        {
            workbook = new HSSFWorkbook();//创建Workbook对象
            styleTitle = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;//加粗
            styleTitle.SetFont(font);
        }

        /// <summary>
        /// 向book中添加sheet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheetName"></param>
        /// <param name="data"></param>
        public void AddSheet<T>(string sheetName, IList<T> data)
        {
            if (string.IsNullOrEmpty(sheetName) || data == null || data.Count < 1)
            {
                return;
            }
            //创建工作表
            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow firstRow = sheet.CreateRow(0);
            //在工作表中添加一行
            PropertyInfo[] properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                ICell cell = firstRow.CreateCell(i);
                cell.CellStyle = styleTitle;
                cell.SetCellValue(properties[i].Name);
            }
            for (int i = 0; i < data.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1); //在工作表1中添加一行
                for (int j = 0; j < properties.Length; j++)
                {
                    var obj = properties[j].GetValue(data[i], null);
                    string value = obj == null ? "" : obj.ToString();
                    row.CreateCell(j).SetCellValue(value);
                }
            }
        }
        /// <summary>
        /// 向book中添加sheet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheetName"></param>
        /// <param name="data"></param>
        public void AddSheet(string sheetName, DataTable data)
        {
            if (string.IsNullOrEmpty(sheetName) || data == null || data.Rows.Count < 1)
            {
                return;
            }
            //创建工作表
            ISheet sheet = workbook.CreateSheet(sheetName);
            //在工作表中添加一行
            IRow firstRow = sheet.CreateRow(0);

            for (int i = 0; i < data.Columns.Count; i++)
            {
                ICell cell = firstRow.CreateCell(i);
                cell.CellStyle = styleTitle;
                cell.SetCellValue(data.Columns[i].ColumnName);
            }
            for (int i = 0; i < data.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1); //在工作表1中添加一行
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    var rowValue = data.Rows[i][j] == null ? "" : data.Rows[i][j].ToString();
                    row.CreateCell(j).SetCellValue(rowValue);
                }
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        public MemoryStream GetStream()
        {
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="fileName">未指定则自动按日期时间生成</param>
        public void Export(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.f").Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "").Replace(".", "") + ".xls";
            }
            var response = HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            response.ContentType = "application nd.ms-excel";
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Charset = "GB2312";
            using (var ms = this.GetStream())
            {
                response.BinaryWrite(ms.GetBuffer());
                ms.Flush();
            }
            response.Flush();
            response.End();
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        public void Export()
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.f").Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "").Replace(".", "") + ".xls";
            this.Export(fileName);
        }
        #endregion
    }

    public sealed class ColumnAttribute : Attribute
    {
        /// <summary>
        /// 类属性对应的列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 类属性是否参加拼写SQL语句
        /// </summary>
        public bool IsFilter { get; set; }

        /// <summary>
        /// 类属性是否主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 能否为空
        /// </summary>
        public bool CanNull { get; set; }

        /// <summary>
        /// 列值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="columnName">类属性对应的列名</param>
        /// <param name="isFilter">是否过滤</param>
        /// <param name="isPrimaryKey">是否是主键</param>
        /// <param name="canNull">是否可为空</param>
        public ColumnAttribute(string columnName, bool isFilter, bool isPrimaryKey, bool canNull)
        {
            ColumnName = columnName;
            IsFilter = isFilter;
            IsPrimaryKey = isPrimaryKey;
            if (isPrimaryKey)
            {
                canNull = false;
            }
            CanNull = canNull;
        }
    }

}
