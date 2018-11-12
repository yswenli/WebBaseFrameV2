//
using Newtonsoft.Json;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Common
{
    /// <summary>
    /// json处理类
    /// </summary>
    public class JsonHelper
    {
        #region .net自带
        /// <summary>
        /// .net自带
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            if (jsonString.Replace("[]", "") == "")
                return string.Empty;
            return jsonString;
        }

        /// <summary>
        /// .net自带 JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
        #endregion

        #region newtonSoft
        /// <summary>
        /// newtonSoft
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string ObjToJson(object item)
        {
            try
            {
                return JsonConvert.SerializeObject(item);
            }
            catch(System.Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// newtonSoft
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T JsonToObj<T>(string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }
        #endregion

        # region dataTable转换成Json格式
        /// <summary>      
        /// dataTable转换成Json格式      
        /// </summary>      
        /// <param name="dt"></param>      
        /// <returns></returns>      
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                string c = "";
                jsonBuilder.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonBuilder.Append(c + "{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        jsonBuilder.Append("\"");
                        jsonBuilder.Append(dt.Columns[j].ColumnName);
                        jsonBuilder.Append("\":\"");
                        jsonBuilder.Append(dt.Rows[i][j].ToString());
                        jsonBuilder.Append("\",");
                    }
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    jsonBuilder.Append("}");
                    c = ",";
                }
                jsonBuilder.Append("]");
            }
            return jsonBuilder.ToString();
        }

        # endregion dataTable转换成Json格式

        # region DataSet转换成Json格式
        /// <summary>      
        /// DataSet转换成Json格式      
        /// </summary>      
        /// <param name="ds">DataSet</param>      
        /// <returns></returns>      
        public static string ToJson(DataSet ds)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {
                json.Append("{\"");
                json.Append(dt.TableName);
                json.Append("\":");
                json.Append(ToJson(dt));
                json.Append("}");
            }
            return json.ToString();
        }
        # endregion

        /// <summary>
        /// 键值对转换成JSON
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJson(string key, string value)
        {
            return "{\"" + key + "\":\"" + value.Replace("\"", "'") + "\"}";
        }
    }
}
