using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 自定义扩展系统方法（马甲）
    /// </summary>
    public static class SystemHelper
    {
        /// <summary>
        /// 格式化日期 yyyy-MM-dd
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string Format(this DateTime? dt)
        {
            return SystemHelper.Format(dt, "yyyy-MM-dd");
        }
        /// <summary>
        /// 格式化日期 yyyy-MM-dd
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string Format(this DateTime dt)
        {
            return SystemHelper.Format(dt, "yyyy-MM-dd");
        }

        /// <summary>
        /// 格式化日期
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string Format(this DateTime? dt, string format)
        {
            var str = "";
            if (dt != null && dt.HasValue)
            {
                str = dt.Value.ToString(format, DateTimeFormatInfo.InvariantInfo);
            }
            return str;
        }
        /// <summary>
        /// 格式化日期
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string Format(this DateTime dt, string format)
        {
            var str = "";
            if (dt != null)
            {
                str = dt.ToString(format, DateTimeFormatInfo.InvariantInfo);
            }
            return str;
        }

        /// <summary>
        /// 年龄
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int Age(this DateTime dt)
        {
            return SystemHelper.Age(dt, DateTime.Now);
        }
        public static int Age(this DateTime dt, DateTime date)
        {
            int result = date.Year - dt.Year;
            if (!(date.Month > dt.Month || (date.Month == dt.Month && date.Day >= dt.Day)))
            {
                result -= 1;
            }
            return result;
        }
        /// <summary>
        /// 年龄
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int Age(this DateTime? dt)
        {
            return SystemHelper.Age(dt, DateTime.Now);
        }
        public static int Age(this DateTime? dt, DateTime date)
        {
            if (dt.HasValue)
            {
                int result = DateTime.Now.Year - dt.Value.Year;
                if (!(date.Month > dt.Value.Month || (date.Month == dt.Value.Month && date.Day >= dt.Value.Day)))
                {
                    result -= 1;
                }
                return result;
            }
            return 0;
        }


        #region 根据年月日计算星期几(Label2.Text=CaculateWeekDay(2004,12,9);)
        /// <summary>
        /// 根据年月日计算星期几(Label2.Text=CaculateWeekDay(2004,12,9);)
        /// </summary>
        /// <param name="y">年</param>
        /// <param name="m">月</param>
        /// <param name="d">日</param>
        /// <returns></returns>
        public static string CaculateWeekDay(int y, int m, int d)
        {
            var dt = new DateTime(y, m, d);
            string weekstr = "";
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Sunday: weekstr = "星期日"; break;
                case DayOfWeek.Monday: weekstr = "星期一"; break;
                case DayOfWeek.Tuesday: weekstr = "星期二"; break;
                case DayOfWeek.Wednesday: weekstr = "星期三"; break;
                case DayOfWeek.Thursday: weekstr = "星期四"; break;
                case DayOfWeek.Friday: weekstr = "星期五"; break;
                case DayOfWeek.Saturday: weekstr = "星期六"; break;
            }
            return weekstr;
        }
        #endregion
        /// <summary>
        /// 获取星期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetWeek(this DateTime? dt)
        {
            string result = "";
            if (dt.HasValue)
            {
                result = CaculateWeekDay(dt.Value.Year, dt.Value.Month, dt.Value.Day);
            }
            return result;
        }
        /// <summary>
        /// 获取星期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetWeek(this DateTime dt)
        {
            return CaculateWeekDay(dt.Year, dt.Month, dt.Day);
        }

        /// <summary>
        /// 格式化Bool
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>        
        public static string Format(this bool? b)
        {
            var str = "";
            if (!b.HasValue)
            {
                str = "";
            }
            else if (b == true)
            {
                str = "✔";
            }
            else
            {
                str = "✘";
            }
            return str;
        }
        /// <summary>
        /// 格式化Bool
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string Format(this bool b)
        {
            var str = "";
            if (b == true)
            {
                str = "✔";
            }
            else
            {
                str = "✘";
            }
            return str;
        }

        /// <summary>
        /// List>T<随机乱序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> ToRandomList<T>(this List<T> list)
        {
            Random random = new Random();
            List<T> newList = new List<T>();
            foreach (T item in list)
            {
                newList.Insert(random.Next(newList.Count), item);
            }
            return newList;
        }


        /// <summary>
        /// 将CSV格式中的 3/21/2015 11:39:02 PM 转为日期
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(this string str)
        {
            DateTime dt = new DateTime();
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    var gArr = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    var dArr = gArr[0].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    var tArr = gArr[1].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                    int year = 0;
                    int month = 0;
                    int day = 0;

                    int hour = 0;
                    int minute = 0;
                    int second = 0;

                    year = int.Parse(dArr[2]);
                    month = int.Parse(dArr[0]);
                    day = int.Parse(dArr[1]);

                    hour = int.Parse(tArr[0]);
                    minute = int.Parse(tArr[1]);
                    second = int.Parse(tArr[2]);

                    if (gArr[2] == "PM")
                    {
                        hour += 12;
                        if (hour == 24) hour = 0;
                    }

                    dt = new DateTime(year, month, day, hour, minute, second);
                }
                catch { }
            }
            return dt;
        }

    }
}
