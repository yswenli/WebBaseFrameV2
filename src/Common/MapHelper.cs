using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
namespace Common
{
    /// <summary>
    /// 地图类
    /// </summary>
    public class MapHelper
    {
        #region 属性、变量
        private static double PI = 3.14159265;
        const double x_pi = 3.14159265358979324 * 3000.0 / 180.0;
        private static double EARTH_RADIUS = 6378137;
        private static double RAD = Math.PI / 180.0;
        //static double logdeviation = 1.0000568461567492425578691530827;//经度偏差
        //static double latdeviation = 1.0002012762190961772159526495686;//纬度偏差
        static double[] TableX = new double[660 * 450];
        static double[] TableY = new double[660 * 450];
        public static string ak = System.Configuration.ConfigurationManager.AppSettings["BMapAK"];
        private class MapJson
        {
            public string ERROR { get; set; }
            public string X { get; set; }
            public string Y { get; set; }
        }
        /// <summary>
        /// 百度地图坐标转换返回的Json对象类
        /// </summary>
        public class Map
        {
            public double? X { get; set; }
            public double? Y { get; set; }
        }
        /// <summary>
        /// 地图类型枚举
        /// </summary>
        public enum MapEnum
        {
            GPS = 0,
            GOOGLE = 2,
            BAIDU = 4
        }
        #endregion

        #region 坐标互转
        /// <summary>
        /// 坐标互转
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="from">待转换的坐标类型</param>
        /// <param name="to">最终转换成的坐标类型</param>
        /// <returns></returns>
        public static Map ConvertAll(double lng, double lat, MapEnum from, MapEnum to)
        {
            //google 坐标转百度链接   //http://api.map.baidu.com/ag/coord/convert?from=2&to=4&x=&y=
            //gps坐标的type=0
            //google坐标的type=2
            //baidu坐标的type=4
            string path = "http://api.map.baidu.com/ag/coord/convert?from=" + from.GetHashCode() + "&to=" + to.GetHashCode() + "&x=" + lng + "+&y=" + lat;

            //使用http请求获取转换结果、得到返回的结果
            string res = HttpHelper.GET(path).ToString();
            //处理结果
            MapJson json = JsonHelper.JsonDeserialize<MapJson>(res);
            Map map = new Map();
            if (json.X != null)
            {
                byte[] outputb = Convert.FromBase64String(json.X);
                map.X = Convert.ToDouble(Encoding.Default.GetString(outputb));
            }
            if (json.Y != null)
            {
                byte[] outputb = Convert.FromBase64String(json.Y);
                map.Y = Convert.ToDouble(Encoding.Default.GetString(outputb));
            }
            return map;
        }
        /// <summary>
        /// 转成百度坐标
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="from">待转换的坐标类型</param>
        /// <returns></returns>
        public static Map Convert2BaiDu(double lng, double lat, MapEnum from)
        {
            return ConvertAll(lng, lat, from, MapEnum.BAIDU);
        }
        /// <summary>
        /// GPS坐标转成百度坐标
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        public static Map Gps2BaiDu(double lng, double lat)
        {
            return Convert2BaiDu(lng, lat, MapEnum.GPS);
        }
        /// <summary>
        /// 谷歌、火星坐标转成百度坐标
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        public static Map Google2BaiDu(double lng, double lat)
        {
            return Convert2BaiDu(lng, lat, MapEnum.GOOGLE);
        }
        /// <summary>
        /// 百度坐标转成谷歌、火星坐标
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        public static Map BaiDu2Google(double lng, double lat)
        {
            Map map = new Map();
            double x = lng - 0.0065, y = lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
            map.X = z * Math.Cos(theta);
            map.Y = z * Math.Sin(theta);
            return map;
        }
        /// <summary>
        /// 百度坐标转成GPS坐标
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        public static Map BaiDu2GPS(double lng, double lat)
        {
            Map map = ConvertAll(lng, lat, MapEnum.GPS, MapEnum.BAIDU);
            map.X = 2 * lng - map.X;
            map.Y = 2 * lat - map.Y;
            return map;
        }

        /// <summary>
        /// GPS坐标转成火星坐标
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        public static Map GPS2Google(double lng, double lat)
        {
            Map map = new Map();
            LoadText();

            int i, j, k;
            double x1, y1, x2, y2, x3, y3, x4, y4, xtry, ytry, dx, dy;
            double t, u;

            xtry = lng;
            ytry = lat;

            for (k = 0; k < 10; ++k)
            {
                // 只对中国国境内数据转换
                if (xtry < 72 || xtry > 137.9 || ytry < 10 || ytry > 54.9)
                {
                    return map;
                }

                i = (int)((xtry - 72.0) * 10.0);
                j = (int)((ytry - 10.0) * 10.0);

                x1 = TableX[GetID(i, j)];
                y1 = TableY[GetID(i, j)];
                x2 = TableX[GetID(i + 1, j)];
                y2 = TableY[GetID(i + 1, j)];
                x3 = TableX[GetID(i + 1, j + 1)];
                y3 = TableY[GetID(i + 1, j + 1)];
                x4 = TableX[GetID(i, j + 1)];
                y4 = TableY[GetID(i, j + 1)];

                t = (xtry - 72.0 - 0.1 * i) * 10.0;
                u = (ytry - 10.0 - 0.1 * j) * 10.0;

                dx = (1.0 - t) * (1.0 - u) * x1 + t * (1.0 - u) * x2 + t * u * x3 + (1.0 - t) * u * x4 - xtry;
                dy = (1.0 - t) * (1.0 - u) * y1 + t * (1.0 - u) * y2 + t * u * y3 + (1.0 - t) * u * y4 - ytry;

                xtry = (xtry + xtry - dx) / 2.0;
                ytry = (ytry + ytry - dy) / 2.0;
            }

            map.X = xtry;
            map.Y = ytry;

            return map;
        }
        private static int GetID(int I, int J)
        {
            return I + 660 * J;
        }
        private static void LoadText()
        {
            using (StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~") + "\\Lib\\MapHelper.txt"))
            {
                string s = sr.ReadToEnd();
                string[] lines = s.Split('\n');

                Match MP = Regex.Match(s, "(\\d+)");

                int i = 0;
                while (MP.Success)
                {
                    if (i % 2 == 0)
                    {
                        TableX[i / 2] = Convert.ToDouble(MP.Value) / 100000.0;
                    }
                    else
                    {
                        TableY[(i - 1) / 2] = Convert.ToDouble(MP.Value) / 100000.0;
                    }
                    i++;
                    MP = MP.NextMatch();
                }
            }
        }
        #endregion

        #region 根据坐标获取地址
        /// <summary>
        /// 根据百度坐标获取地址
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        public static string XY2Address(double lng, double lat)
        {
            string res = HttpHelper.GET("http://api.map.baidu.com/geocoder/v2/?ak=" + ak + "&location=" + lat + "," + lng + "&output=xml&pois=0").ToString();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(res);
            XmlNode note = xmlDoc.SelectSingleNode("//GeocoderSearchResponse//result//formatted_address");
            return note.InnerText;
        }
        #endregion

        #region 根据地址获取坐标
        /// <summary>
        /// 根据地址返回百度坐标，默认在上海市搜索
        /// </summary>
        public static Map Address2BXBY(string address)
        {
            return Address2BXBY(address, "ShangHai");
        }
        /// <summary>
        /// 根据地址返回百度坐标
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="city">搜索在城市，如：上海市</param>
        /// <returns></returns>
        public static Map Address2BXBY(string address, string city)
        {
            Map json = new Map();
            string url = "http://api.map.baidu.com/geocoder/v2/?ak=" + ak + "&output=xml&address=" + address + "&city=" + city;
            string res = HttpHelper.GET(url).ToString();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(res);
            XmlNode note = xmlDoc.SelectSingleNode("//GeocoderSearchResponse//result//location//lng");
            if (!string.IsNullOrEmpty(note.InnerText))
                json.X = Convert.ToDouble(note.InnerText);
            note = xmlDoc.SelectSingleNode("//GeocoderSearchResponse//result//location//lat");
            if (!string.IsNullOrEmpty(note.InnerText))
                json.Y = Convert.ToDouble(note.InnerText);

            return json;
        }
        /// <summary>
        /// 根据地址返回火星坐标，默认在上海市搜索
        /// </summary>
        public static Map Address2GXGY(string address)
        {
            Map map = Address2BXBY(address, "ShangHai");
            return BaiDu2Google((double)map.X, (double)map.Y);
        }
        /// <summary>
        /// 根据地址返回火星坐标
        /// </summary>
        public static Map Address2GXGY(string address, string city)
        {
            Map map = Address2BXBY(address, city);
            return BaiDu2Google((double)map.X, (double)map.Y);
        }
        /// <summary>
        /// 根据地址返回GPS坐标，默认在上海市搜索
        /// </summary>
        public static Map Address2XY(string address)
        {
            Map map = Address2BXBY(address, "ShangHai");
            return BaiDu2GPS((double)map.X, (double)map.Y);
        }
        /// <summary>
        /// 根据地址返回GPS坐标，默认在上海市搜索
        /// </summary>
        public static Map Address2XY(string address, string city)
        {
            Map map = Address2BXBY(address, city);
            return BaiDu2GPS((double)map.X, (double)map.Y);
        }
        #endregion

        #region 计算距离
        /// <summary>
        /// 计算2坐标的距离，此方法适合短距离计算
        /// </summary>
        /// <param name="lng1">经度1</param>
        /// <param name="lat1">纬度1</param>
        /// <param name="lng2">经度2</param>
        /// <param name="lat2">纬度2</param>
        /// <returns></returns>
        public static double GetDistance(double lng1, double lat1, double lng2, double lat2)
        {
            double radLat1 = lat1 * RAD;
            double radLat2 = lat2 * RAD;
            double a = radLat1 - radLat2;
            double b = (lng1 - lng2) * RAD;
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }
        #endregion

        #region 查找附近的
        /// <summary>
        /// 查找附近
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="raidus">查找范围,单位为米(m)</param>
        /// <returns>返回最小坐标，最大坐标</returns>
        public static double[] GetAround(double lng, double lat, int raidus)
        {
            Double latitude = lat;
            Double longitude = lng;
            Double degree = (24901 * 1609) / 360.0;
            double raidusMile = raidus;
            Double dpmLat = 1 / degree;
            Double radiusLat = dpmLat * raidusMile;
            Double minLat = latitude - radiusLat;
            Double maxLat = latitude + radiusLat;
            Double mpdLng = degree * Math.Cos(latitude * (PI / 180));
            Double dpmLng = 1 / mpdLng;
            Double radiusLng = dpmLng * raidusMile;
            Double minLng = longitude - radiusLng;
            Double maxLng = longitude + radiusLng;
            return new double[] { minLng, minLat, maxLng, maxLat };
        }
        #endregion
    }
}
