using System.Text.RegularExpressions;
using System.Web;

namespace Common
{
    public class Validate
    {
        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegNumberDate = new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))(20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d|((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
        private static Regex RegEmail = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
        private static Regex RegMobile = new Regex(@"^(13|15|18|14)\d{9}$");

        #region 检查Request查询字符串的键值，是否是数字，最大长度限制

        /// <summary>
        /// 检查Request查询字符串的键值，是否是数字，最大长度限制
        /// </summary>
        /// <param name="req">Request</param>
        /// <param name="inputKey">Request的键值</param>
        /// <param name="maxLen">最大长度</param>
        /// <returns>返回Request查询字符串</returns>
        public static string FetchInputDigit(HttpRequest req, string inputKey, int maxLen)
        {
            string retVal = string.Empty;
            if (inputKey != null && inputKey != string.Empty)
            {
                retVal = req.QueryString[inputKey];
                if (null == retVal)
                    retVal = req.Form[inputKey];
                if (null != retVal)
                {
                    retVal = SqlText(retVal, maxLen);
                    if (!IsNumber(retVal))
                        retVal = string.Empty;
                }
            }
            if (retVal == null)
                retVal = string.Empty;
            return retVal;
        }
        #endregion

        #region 是否数字
        /// <summary>
        /// 是否数字
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string inputData)
        {
            Match m = RegNumber.Match(inputData);
            return m.Success;
        }
        #endregion

        #region 是否数字字符串 可带正负号
        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumberSign(string inputData)
        {
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }
        #endregion

        #region 是否是浮点数
        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(string inputData)
        {
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }
        #endregion

        #region 是否是浮点数 可带正负号
        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimalSign(string inputData)
        {
            Match m = RegDecimalSign.Match(inputData);
            return m.Success;
        }
        #endregion

        #region 判断是否是有效的日期
        /// <summary>
        /// 判断是否是有效的日期
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDateTime(string inputData)
        {
            Match m = RegNumberDate.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 检测是否有中文字符
        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsHasCHZN(string inputData)
        {
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 邮件地址
        /// <summary>
        /// 是否是邮件地址
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 手机号码
        /// <summary>
        /// 是否是手机号码
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsMobile(string inputData)
        {
            Match m = RegMobile.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 验证是否为小数
        /// <summary>
        /// 验证是否为小数
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidDecimal(string strIn)
        {

            return Regex.IsMatch(strIn, @"[0].\d{1,2}|[1]");
        }
        #endregion

        #region 验证是否为电话号码
        /// <summary>
        /// 验证是否为电话号码
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidTel(string strIn)
        {
            return Regex.IsMatch(strIn, @"(\d+-)?(\d{4}-?\d{7}|\d{3}-?\d{8}|^\d{7,8})(-\d+)?");
        }
        #endregion

        #region 验证是否为时间格式
        /// <summary>
        /// 验证是否为时间格式 
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidDate(string strIn)
        {
            return Regex.IsMatch(strIn, @"^2\d{3}-(?:0?[1-9]|1[0-2])-(?:0?[1-9]|[1-2]\d|3[0-1])(?:0?[1-9]|1\d|2[0-3]):(?:0?[1-9]|[1-5]\d):(?:0?[1-9]|[1-5]\d)$");
        }
        #endregion

        #region 验证后缀名
        /// <summary>
        /// 验证后缀名 使用|分隔如：gif|jpg
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidPostfix(string strIn, string fix)
        {
            return Regex.IsMatch(strIn, @"\.(?i:" + fix + ")$");
        }
        #endregion

        #region 验证字符长度
        /// <summary>
        /// 验证字符长度
        /// </summary>
        /// <param name="strIn">输入值</param>
        /// <param name="minlen">最小长度</param>
        /// <param name="maxlen">最大长度</param>
        /// <returns></returns>
        public static bool IsValidByte(string strIn, int minlen, int maxlen)
        {
            return Regex.IsMatch(strIn, @"^[a-z]{" + minlen + "," + maxlen + "}$");
        }
        #endregion

        #region 验证IP
        /// <summary>
        /// 验证IP 
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidIp(string strIn)
        {
            return Regex.IsMatch(strIn, @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        }
        #endregion

        #region 验证身份证
        /// <summary>
        /// 验证身份证
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsShenFenZheng(string strIn)
        {
            return Regex.IsMatch(strIn, @"/^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$/");
        }
        #endregion

        #region 验证是否为邮编
        /// <summary>
        /// 验证是否为邮编
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsZipCode(string strIn)
        {
            return Regex.IsMatch(strIn, @"^[1-9]\d{5}$");
        }
        #endregion

        #region 检查字符串最大长度，返回指定长度的串
        /// <summary>
        /// 检查字符串最大长度，返回指定长度的串
        /// </summary>
        /// <param name="sqlInput">输入字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>			
        public static string SqlText(string sqlInput, int maxLength)
        {
            if (sqlInput != null && sqlInput != string.Empty)
            {
                sqlInput = sqlInput.Trim();
                if (sqlInput.Length > maxLength)//按最大长度截取字符串
                    sqlInput = sqlInput.Substring(0, maxLength);
            }
            return sqlInput;
        }

        #endregion


    }
}
