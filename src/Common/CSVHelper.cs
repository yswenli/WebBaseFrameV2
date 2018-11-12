using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
namespace Common
{
    /// <summary>
    /// 导出到CSV
    /// </summary>
    public static class CSVHelper
    {


        public static void CSV(DataTable dt)
        {
            string Filename = DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";
            CSVHelper.CSV(Filename, dt);
        }

        public static void CSV(string filename, DataTable dt)
        {
            using (StringWriter sw = new StringWriter())
            {
                StringBuilder Str = new StringBuilder();
                char c = ',';
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    Str.Append(dt.Columns[i].Caption + c);
                }
                Str.Append("\r\n");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        try
                        {
                            Str.Append(ConvertToSaveCell(dt.Rows[i][j]));
                        }
                        catch
                        {
                            Str.Append(c);
                        }
                    }
                    Str.Append("\r\n");
                }
                sw.Write(System.Text.Encoding.UTF8.GetString(new byte[] { (byte)0xEF, (byte)0xBB, (byte)0xBF }));
                sw.Write(Str);
                var response = System.Web.HttpContext.Current.Response;
                response.Clear();
                response.BufferOutput = true;
                response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                response.ContentType = "application/ms-excel";
                response.ContentEncoding = System.Text.Encoding.UTF8;
                response.Write(sw);
                response.Flush();
                response.End();
            }
        }


        public static void CSV<T>(List<T> lts) where T : new()
        {
            string filename = DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";
            CSV(filename, lts);
        }

        public static void CSV<T>(string filename, List<T> lts) where T : new()
        {
            using (StringWriter sw = new StringWriter())
            {
                StringBuilder Str = new StringBuilder();
                char c = ',';
                //
                T t = new T();
                var properties = t.GetType().GetProperties();
                if (properties != null)
                {
                    foreach (var item in properties)
                    {
                        if (item.Name != "EntityState" && item.Name != "EntityKey")
                        {
                            Str.Append(item.Name + c);
                        }
                    }
                }
                Str.Append("\r\n");
                if (lts != null)
                {
                    foreach (var item in lts)
                    {
                        properties = item.GetType().GetProperties();
                        if (properties != null)
                        {
                            foreach (var ditem in properties)
                            {
                                if (ditem.Name != "EntityState" && ditem.Name != "EntityKey")
                                {
                                    var value = ditem.GetValue(item, null);
                                    if (value == null)
                                    {
                                        Str.Append("" + c);
                                    }
                                    else
                                    {
                                        DateTime result = new DateTime();
                                        if (DateTime.TryParse(value.ToString(), out result))
                                        {
                                            Str.Append(result.ToShortDateString() + c);
                                        }
                                        else
                                        {
                                            if (value == null || string.IsNullOrEmpty(value.ToString()))
                                            {
                                                Str.Append(c);
                                            }
                                            else
                                            {
                                                Str.Append(ConvertToSaveCell(value));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        Str.Append("\r\n");
                    }
                }
                sw.Write(System.Text.Encoding.UTF8.GetString(new byte[] { (byte)0xEF, (byte)0xBB, (byte)0xBF }));
                sw.Write(Str);
                var response = System.Web.HttpContext.Current.Response;
                response.Clear();
                response.BufferOutput = true;
                response.AddHeader("Content-Disposition", "attachment;   filename=" + filename);
                response.ContentType = "application/ms-excel";
                response.ContentEncoding = System.Text.Encoding.UTF8;
                response.Write(sw);
                response.Flush();
                response.End();
            }
        }

        /// <summary>
        /// 空格，双引号等照原输出，逗号换成中文
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static string ConvertToSaveCell(object cell)
        {
            var cellStr = "";
            if (cell != null) cellStr = cell.ToString();
            cellStr = cellStr.Replace(",", "，").Trim();
            cellStr = cellStr.Replace("\"", "\"\"");
            return "\"" + cellStr + "\"" + ",";
        }


        /// <summary>
        /// 将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="fileName">CSV文件路径</param>
        /// <returns>返回读取了CSV数据的DataTable</returns>
        public static DataTable GetData(string filePath)
        {
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }

            sr.Close();
            fs.Close();
            return dt;
        }
    }
}
