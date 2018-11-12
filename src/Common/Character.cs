using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace Common
{
    /// <summary>
    /// 字符串处理加强辅助类
    /// </summary>
    public class Character
    {
        ///   <summary>   
        ///   去除所有HTML标记   
        ///   </summary>
        public static string ClearHtml(string strHtml)
        {
            if (!string.IsNullOrEmpty(strHtml))
            {
                strHtml = strHtml.Replace("\r\n", "");
                strHtml = Regex.Replace(strHtml, @"<\/?[^>]*>", "", RegexOptions.Multiline);
            }
            return strHtml;
        }
        /// <summary>
        /// 清除脚本>script<
        /// </summary>
        /// <param name="fHtmlString"></param>
        /// <returns></returns>
        public static string ClearScript(string fHtmlString)
        {
            if (!string.IsNullOrEmpty(fHtmlString))
            {
                fHtmlString = Regex.Replace(fHtmlString, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            }
            else
            {
                fHtmlString = "";
            }
            return fHtmlString;
        }
        /// <summary>
        /// 加强版去HTML、Script、LINK、Style等
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>  
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本     
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            //删除HTML     
            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            Htmlstring = regex.Replace(Htmlstring, "");
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");

            return Htmlstring;
        }


        ///   <summary>   
        ///   去除样式，并设置字体的大小 
        ///   </summary>
        public static string ClearHtmlSetFontSize(string strHtml, int fontsize)
        {
            if (!string.IsNullOrEmpty(strHtml))
            {
                strHtml = Regex.Replace(strHtml, @"<P>", "~", RegexOptions.Multiline);
                strHtml = Regex.Replace(strHtml, @"<p>", "~", RegexOptions.Multiline);
                strHtml = Regex.Replace(strHtml, @"</P>", "/~", RegexOptions.Multiline);
                strHtml = Regex.Replace(strHtml, @"</p>", "/~", RegexOptions.Multiline);
                strHtml = Regex.Replace(strHtml, @"<BR>", "^", RegexOptions.Multiline);
                strHtml = Regex.Replace(strHtml, @"<br>", "^", RegexOptions.Multiline);
                strHtml = Regex.Replace(strHtml, @"<br />", "^", RegexOptions.Multiline);
                strHtml = Regex.Replace(strHtml, @"<BR />", "^", RegexOptions.Multiline);
                strHtml = Regex.Replace(strHtml, @"<BR />", "^", RegexOptions.Multiline);
                strHtml = Regex.Replace(strHtml, @"<\/?[^>]*>", "", RegexOptions.Multiline);
                strHtml = Regex.Replace(strHtml, @"/~", "</P>", RegexOptions.Multiline);
                strHtml = Regex.Replace(strHtml, @"~", "<P>", RegexOptions.Multiline);
                strHtml = strHtml.Replace("^", "<br/>");
            }
            if (strHtml != string.Empty)
            {
                strHtml = "<span style='font-size:" + fontsize + "pt;'>" + strHtml + "</span>";
            }
            return strHtml;
        }
        /// <summary>
        /// 把指定的datatable某列拼成为1,2,3,4,5....
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="RowName"></param>
        /// <returns></returns>
        public static string DataTableToString(DataTable dt, string RowName, string _char)
        {
            string str = "";
            string _c = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str += _c + dt.Rows[i][RowName];
                _c = _char;
            }
            return str;
        }
        /// <summary>
        /// 把string[]转换成string 如["1","2","3"...] to "1,2,3..."
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ArrayToString(string[] list)
        {
            string str = "";
            for (int i = 0; i < list.Length; i++)
            {
                str += "," + list[i];
            }
            return str.Substring(1);
        }

        /// <summary>
        /// 把datatable转换成string[]
        /// </summary>
        /// <param name="p_Table"></param>
        /// <param name="RowName"></param>
        /// <returns></returns>
        public static string[] DataTableToArray(DataTable p_Table, string RowName)
        {
            string[] _ReturnText = new string[p_Table.Rows.Count];

            for (int i = 0; i != p_Table.Rows.Count; i++)
            {
                _ReturnText[i] = p_Table.Rows[i][RowName].ToString();
            }
            return _ReturnText;
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="obj">截取的对象</param>
        /// <param name="i">截取的长度</param>
        /// <returns>返回截取对象＋。。。</returns>
        public static string SubString(object obj, int length)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                return "";
            }
            length = length * 2;
            string temp = ClearHtml(obj.ToString());
            int j = 0;
            int k = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (Regex.IsMatch(temp.Substring(i, 1), @"[\u4e00-\u9fa5]+"))
                {
                    j += 2;
                }
                else
                {
                    j += 1;
                }
                if (j <= length)
                {
                    k += 1;
                }
                if (j >= length)
                {
                    return temp.Substring(0, k) + "...";
                }
            }
            return temp;
        }

        #region 输出json
        /// <summary>
        /// 输出json
        /// </summary>
        public static string StringToJson(string key, string value)
        {
            return "{ \"" + key + "\":\"" + value + "\"}";
        }
        /// <summary>
        /// 将dataTable转换成Json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder JsonString = new StringBuilder();
            //Exception Handling        
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                    }
                    /*end Of String*/
                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region 内容分页
        /// <summary>
        /// 文章内容自动分页
        /// </summary>
        public static string ContentByPage(out string spanPage, string Body, int? Size, int? CurrentPage, string Url)
        {
            int m_intPageSize = Size ?? 10000;//文章每页大小
            int m_intCurrentPage = CurrentPage ?? 1;//设置第一页为初始页
            int m_intTotalPage = 0;
            int m_intArticlelength = Body.Length;//文章长度
            string m_strRet = "";
            spanPage = "";
            if (m_intPageSize < m_intArticlelength)
            {//如果每页大小大于文章长度时就不用分页了
                if (m_intArticlelength % m_intPageSize == 0)
                {//set total pages count 
                    m_intTotalPage = m_intArticlelength / m_intPageSize;
                }
                else
                {//if the totalsize
                    m_intTotalPage = m_intArticlelength / m_intPageSize + 1;
                }
                try
                {
                    if (m_intCurrentPage > m_intTotalPage)
                    {
                        m_intCurrentPage = m_intTotalPage;
                    }
                }
                catch
                {
                    m_intCurrentPage = CurrentPage ?? 1;
                }
                //set the page content 设置获取当前页的大小
                m_intPageSize = m_intCurrentPage < m_intTotalPage ? m_intPageSize : (m_intArticlelength - m_intPageSize * (m_intCurrentPage - 1));
                m_strRet = Body.Substring(m_intPageSize * (m_intCurrentPage - 1), m_intPageSize);
                string m_strPageInfo = "<p></p>";
                if (m_intTotalPage > 1)
                {
                    for (int i = 1; i <= m_intTotalPage; i++)
                    {
                        if (i == m_intCurrentPage)
                        {
                            m_strPageInfo += "<b>" + i + "</b>｜";
                        }
                        else
                        {
                            m_strPageInfo += "<a href=\"" + Url + "page=" + i + "\">" + i + "</a>｜";
                        }
                    }
                }
                if (m_intCurrentPage > 1)
                {
                    m_strPageInfo = " <a href=\"" + Url + "page=" + (m_intCurrentPage - 1) + "\">上一页 </a>" + m_strPageInfo;
                }

                if (m_intCurrentPage < m_intTotalPage)
                {
                    m_strPageInfo += " <a href=\"" + Url + "page=" + (m_intCurrentPage + 1) + "\">下一页 </a>";
                }
                //输出显示各个页码
                spanPage = m_strPageInfo;
            }
            return m_strRet;
        }
        /// <summary>
        /// 根据分隔符自动分页
        /// </summary>
        public static string ContentByPage(out string spanPage, string Body, string strSplit, int? CurrentPage, string Url)
        {
            string m_strRet = "";
            spanPage = "";
            //设置第一页为初始页
            int m_intCurrentPage = CurrentPage ?? 1;
            //设置显示页数
            int m_intTotalPage = StringSplit(Body, strSplit).Length;

            string[] strContent = StringSplit(Body, strSplit);

            if (m_intCurrentPage > m_intTotalPage)
                m_intCurrentPage = m_intTotalPage;
            else if (m_intCurrentPage < 1)
                m_intCurrentPage = 1;

            m_strRet += strContent[m_intCurrentPage - 1].ToString();
            string m_strPageInfo = "";

            if (m_intTotalPage > 1)
            {
                for (int i = 1; i <= m_intTotalPage; i++)
                {
                    if (i == m_intCurrentPage)
                    {
                        m_strPageInfo += "[" + i + "]";
                    }
                    else
                    {
                        m_strPageInfo += " <a href=\"" + Url + "page=" + i + "\">[" + i + "] </a> ";
                    }
                }
            }
            if (m_intCurrentPage > 1)
            {
                m_strPageInfo = " <a href=\"" + Url + "page=" + (m_intCurrentPage - 1) + "\">上一页 </a>" + m_strPageInfo;
            }

            if (m_intCurrentPage < m_intTotalPage)
            {
                m_strPageInfo += " <a href=\"" + Url + "page=" + (m_intCurrentPage + 1) + "\">下一页 </a>";
            }
            //输出显示各个页码 
            spanPage = " <p> </p>" + m_strPageInfo;

            return m_strRet;
        }
        #endregion

        #region 将字符串分割成数组

        /// <summary>
        /// 将字符串分割成数组
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="strSplit"></param>
        /// <returns></returns>
        private static string[] StringSplit(string strSource, string strSplit)
        {
            string[] strtmp = new string[1];
            int index = strSource.IndexOf(strSplit, 0);
            if (index < 0)
            {
                strtmp[0] = strSource;
                return strtmp;
            }
            else
            {
                strtmp[0] = strSource.Substring(0, index);
                return StringSplit(strSource.Substring(index + strSplit.Length), strSplit, strtmp);
            }
        }
        /// <summary>
        /// 采用递归将字符串分割成数组
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="strSplit"></param>
        /// <param name="attachArray"></param>
        /// <returns></returns>
        private static string[] StringSplit(string strSource, string strSplit, string[] attachArray)
        {
            string[] strtmp = new string[attachArray.Length + 1];
            attachArray.CopyTo(strtmp, 0);
            int index = strSource.IndexOf(strSplit, 0);
            if (index < 0)
            {
                strtmp[attachArray.Length] = strSource;
                return strtmp;
            }
            else
            {
                strtmp[attachArray.Length] = strSource.Substring(0, index);
                return StringSplit(strSource.Substring(index + strSplit.Length), strSplit, strtmp);
            }
        }
        #endregion

        #region ========加密========

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "litianping");
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========解密========


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "litianping");
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion

        #region Base64加密、解密
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string EncryptBase64(string code)
        {
            string encode = "";
            byte[] bytes = Encoding.UTF8.GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string DecryptBase64(string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }
        #endregion

        #region MD5加密、密码比较
        /// <summary>
        /// 字符串MD5加密
        /// </summary>
        /// <param name="inputedPassword"></param>
        /// <returns></returns>
        public static string EncrytPassword(string inputedPassword)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(inputedPassword, "MD5");
        }
        /// <summary>
        /// 密码比较
        /// </summary>
        /// <param name="inputedPassword"></param>
        /// <param name="currentPassword"></param>
        /// <returns></returns>
        public static bool VerifyPassword(string inputedPassword, string currentPassword)
        {
            string encryptedPassword = EncrytPassword(inputedPassword);
            return (encryptedPassword == currentPassword.ToUpper()) ? true : false;
        }
        #endregion

        #region 半角/全角 字符转换
        /// <summary>
        /// 全角=>半角
        /// </summary>
        /// <param name="src"></param>
        /// <returns>半角字符串</returns>
        public static string SBCToDBC(string src)
        {
            if (src == null || src == string.Empty)
                return string.Empty;

            char[] c = src.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            return new string(c);
        }
        /// <summary>
        /// 半角=>全角
        /// </summary>
        /// <param name="src"></param>
        /// <returns>全角字符串</returns>
        public static string DBCToSBC(string src)
        {
            if (src == null || src == string.Empty)
                return string.Empty;

            char[] c = src.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 0)
                    {
                        b[0] = (byte)(b[0] - 32);
                        b[1] = 255;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            return new string(c);
        }
        #endregion

        #region  生成制定长度的随机数
        /// <summary>
        /// 生成制定长度的随机数
        /// </summary>
        /// <param name="type">all:包括数字、大小写字母；num：0~9数字；lower：小写字母；upper：大写字母；lower&upper:大小写字母</param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(string type, int length)
        {
            string allChar = "";
            if (type == "all")
                allChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            else if (type == "num")
                allChar = "0,1,2,3,4,5,6,7,8,9";
            else if (type == "lower")
                allChar = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            else if (type == "upper")
                allChar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            else if (type == "lower&upper")
                allChar = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(allCharArray.Length);

                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }

            return RandomCode;
        }
        #endregion

        #region 防止sql注入
        public static string ReplaceSqlKey(string text, int maxlength)
        {
            text = text.ToLower().Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (text.Length > maxlength)
                text = text.Substring(0, maxlength);

            text = Regex.Replace(text, "'", "");
            text = Regex.Replace(text, "\r\n", "");
            text = Regex.Replace(text, ";", "");
            return text;
        }
        #endregion

        #region 计算时间差值
        public static string DiffDate(DateTime dt1, DateTime dt2)
        {

            string dateDiff = null;
            try
            {
                TimeSpan ts1 = new TimeSpan(dt1.Ticks);
                TimeSpan ts2 = new TimeSpan(dt2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                if (ts.Days <= 3)
                {
                    if (ts.Seconds > 0)
                    {
                        dateDiff = ts.Seconds.ToString() + "秒";
                    }
                    if (ts.Minutes > 0)
                    {
                        dateDiff = ts.Minutes.ToString() + "分钟" + dateDiff;
                    }
                    if (ts.Hours > 0)
                    {
                        dateDiff = ts.Hours.ToString() + "小时" + dateDiff;
                    }
                    if (ts.Days > 0)
                    {
                        dateDiff = ts.Days.ToString() + "天" + dateDiff;
                    }
                    dateDiff += "前";
                }
                else
                {
                    dateDiff = dt2.ToString("yyyy-MM-dd");
                }
            }
            catch
            {

            }
            return dateDiff;
        }
        #endregion

        /// <summary>
        /// 生成随访数
        /// </summary>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <returns></returns>
        public static string Random(int min, int max)
        {
            return new Random((int)DateTime.Now.Ticks).Next(min, max).ToString();
        }
        /// <summary>
        /// 给地址栏参数格式化
        /// </summary>
        public static string UrlEscape(string str)
        {
            return Microsoft.JScript.GlobalObject.escape(str);
        }

        /// <summary>
        /// 根据日期获取今天是星期几
        /// </summary>
        public static string DayOfWeek(DateTime? datetime)
        {
            string week = string.Empty;
            if (datetime == null)
                week = "";
            else
            {
                int i = datetime.Value.DayOfWeek.GetHashCode();
                switch (i)
                {
                    case 1:
                        week = "星期一";
                        break;
                    case 2:
                        week = "星期二";
                        break;
                    case 3:
                        week = "星期三";
                        break;
                    case 4:
                        week = "星期四";
                        break;
                    case 5:
                        week = "星期五";
                        break;
                    case 6:
                        week = "星期六";
                        break;
                    case 7:
                        week = "星期天";
                        break;
                }
            }
            return week;
        }

        /// <summary>
        /// Int转成日期类型
        /// </summary>
        public static DateTime? IntToDateTime(int? second)
        {
            if (second == null)
                return null;
            else
            {
                return DateTime.Parse("1970-1-1").AddSeconds((int)second);
            }
        }
        /// <summary>
        /// 日期类型转成Int
        /// </summary>
        public static int? DateTimeToInt(DateTime? datetime)
        {
            if (datetime == null)
                return null;
            else
            {
                TimeSpan ts1 = new TimeSpan(datetime.Value.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime.Parse("1970-1-1").Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                return (int)ts.TotalSeconds;
            }
        }
        #region 汉字转拼音
        //定义拼音区编码数组  
        private static int[] getValue = new int[]  
        {  
               -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,  
               -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,  
               -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,  
               -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,  
               -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,  
               -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,  
               -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,  
               -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,  
               -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,  
               -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,  
               -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,  
               -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,  
               -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,  
               -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,  
               -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,  
               -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,  
               -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,  
               -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,  
               -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,  
               -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,  
               -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,  
               -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,  
               -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,  
               -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,  
               -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,  
               -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,  
               -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,  
               -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,  
               -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,  
               -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,  
               -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,  
               -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,  
               -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254  
           };

        //定义拼音数组  
        private static string[] getName = new string[]  
        {  
               "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",  
               "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",  
               "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",  
               "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",  
               "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",  
               "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",  
               "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",  
               "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",  
               "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",  
               "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",  
               "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",  
               "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",  
               "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",  
               "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",  
               "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",  
               "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",  
               "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",  
               "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",  
               "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",  
               "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",  
               "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",  
               "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",  
               "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",  
               "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",  
               "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",  
               "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",  
               "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",  
               "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",  
               "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",  
               "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",  
               "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",  
               "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",  
               "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"  
          };

        /// <summary>  
        /// 汉字转换成全拼的拼音  
        /// </summary>  
        /// <param name="Chstr">汉字字符串</param>  
        /// <returns>转换后的拼音字符串</returns>  
        public string convertCh(string Chstr)
        {
            Regex reg = new Regex("^[\u4e00-\u9fa5]$");//验证是否输入汉字  
            byte[] arr = new byte[2];
            string pystr = "";
            int asc = 0, M1 = 0, M2 = 0;
            char[] mChar = Chstr.ToCharArray();//获取汉字对应的字符数组  
            for (int j = 0; j < mChar.Length; j++)
            {
                //如果输入的是汉字  
                if (reg.IsMatch(mChar[j].ToString()))
                {
                    arr = System.Text.Encoding.Default.GetBytes(mChar[j].ToString());
                    M1 = (short)(arr[0]);
                    M2 = (short)(arr[1]);
                    asc = M1 * 256 + M2 - 65536;
                    if (asc > 0 && asc < 160)
                    {
                        pystr += mChar[j];
                    }
                    else
                    {
                        switch (asc)
                        {
                            case -9254:
                                pystr += "Zhen"; break;
                            case -8985:
                                pystr += "Qian"; break;
                            case -5463:
                                pystr += "Jia"; break;
                            case -8274:
                                pystr += "Ge"; break;
                            case -5448:
                                pystr += "Ga"; break;
                            case -5447:
                                pystr += "La"; break;
                            case -4649:
                                pystr += "Chen"; break;
                            case -5436:
                                pystr += "Mao"; break;
                            case -5213:
                                pystr += "Mao"; break;
                            case -3597:
                                pystr += "Die"; break;
                            case -5659:
                                pystr += "Tian"; break;
                            default:
                                for (int i = (getValue.Length - 1); i >= 0; i--)
                                {
                                    if (getValue[i] <= asc) //判断汉字的拼音区编码是否在指定范围内  
                                    {
                                        pystr += getName[i];//如果不超出范围则获取对应的拼音  
                                        break;
                                    }
                                }
                                break;
                        }
                    }
                }
                else//如果不是汉字  
                {
                    pystr += mChar[j].ToString();//如果不是汉字则返回  
                }
            }
            return pystr;//返回获取到的汉字拼音  
        }
        #endregion


        #region 人民币转中文
        /// <summary>   
        /// 人民币转中文   
        /// </summary>   
        /// <param name="num">金额</param>   
        /// <returns>返回大写形式</returns>   
        public static string ConvertRMB(decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字   
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字   
            string str3 = "";    //从原num值中取出的值   
            string str4 = "";    //数字的字符串形式   
            string str5 = "";  //人民币大写金额形式   
            int i;    //循环变量   
            int j;    //num的值乘以100的字符串长度   
            string ch1 = "";    //数字的汉语读法   
            string ch2 = "";    //数字位的汉字读法   
            int nzero = 0;  //用来计算连续的零值是几个   
            int temp;            //从原num值中取出的值   

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数   
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式   
            j = str4.Length;      //找出最高位   
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分   

            //循环取出每一位需要转换的值   
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值   
                temp = Convert.ToInt32(str3);      //转换为数字   
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时   
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位   
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上   
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整”   
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }

        #endregion
    }
}
