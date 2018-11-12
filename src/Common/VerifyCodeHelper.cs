
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class VerifyCodeHelper
    {
        private static byte[] randb = new byte[4];
        private static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

        private static Font[] fonts = {
                                         new Font(new FontFamily("Times New Roman"), 18 + Next(4),System.Drawing.FontStyle.Bold),
                                         new Font(new FontFamily("Georgia"), 18 + Next(4),System.Drawing.FontStyle.Bold),
                                         new Font(new FontFamily("Arial"), 18 + Next(4),System.Drawing.FontStyle.Bold),
                                         new Font(new FontFamily("Comic Sans MS"), 18 + Next(4),System.Drawing.FontStyle.Bold)};
        /// <summary>
        /// 获取验证码
        /// </summary>
        public static void ChangeCode()
        {
            string checkCode = Character.RandomString("num", 4);
            System.Web.HttpContext.Current.Session["verifyCode"] = checkCode;
        }
        /// <summary>
        /// 获取验证码图像
        /// </summary>
        public static void ResponseImage()
        {
            using (MemoryStream ms = VerifyCodeHelper.GetImageStream())
            {
                System.Web.HttpContext.Current.Response.ClearContent();
                System.Web.HttpContext.Current.Response.ContentType = "image/Png";
                System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            }
        }

        /// <summary>
        /// 获取验证码图像
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="bgcolor"></param>
        /// <param name="textcolor"></param>
        public static void ResponseImage(int width, int height, Color bgcolor, int textcolor)
        {
            string checkCode = Character.RandomString("num", 4);
            System.Web.HttpContext.Current.Session["verifyCode"] = checkCode;
            using (MemoryStream ms = VerifyCodeHelper.GetImageStream(checkCode, 100, 40, Color.White, 1))
            {
                System.Web.HttpContext.Current.Response.ClearContent();
                System.Web.HttpContext.Current.Response.ContentType = "image/Png";
                System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            }
        }


        /// <summary>
        /// 获取验证码图像流
        /// </summary>
        /// <returns></returns>
        public static void GenerateImage(string code, int width, int height, Color bgcolor, int textcolor)
        {


            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            Graphics g = Graphics.FromImage(bitmap);
            Rectangle rect = new Rectangle(0, 0, width, height);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.Clear(bgcolor);

            int fixedNumber = textcolor == 2 ? 60 : 0;
            //创建干扰线
            SolidBrush drawBrush = new SolidBrush(Color.FromArgb(Next(100), Next(100), Next(100)));
            for (int x = 0; x < 2; x++)
            {
                Pen linePen = new Pen(Color.FromArgb(Next(150) + fixedNumber, Next(150) + fixedNumber, Next(150) + fixedNumber), 1);
                g.DrawLine(linePen, new PointF(0.0F + Next(20), 0.0F + Next(height)), new PointF(0.0F + Next(width), 0.0F + Next(height)));
            }


            Matrix m = new Matrix();
            for (int x = 0; x < code.Length; x++)
            {
                m.Reset();
                m.RotateAt(Next(30) - 15, new PointF(Convert.ToInt64(width * (0.10 * x)), Convert.ToInt64(height * 0.5)));
                g.Transform = m;
                drawBrush.Color = Color.FromArgb(Next(150) + fixedNumber + 20, Next(150) + fixedNumber + 20, Next(150) + fixedNumber + 20);
                PointF drawPoint = new PointF(0.0F + Next(4) + x * 20, 3.0F + Next(3));
                g.DrawString(Next(1) == 1 ? code[x].ToString() : code[x].ToString().ToUpper(), fonts[Next(fonts.Length - 1)], drawBrush, drawPoint);
                g.ResetTransform();
            }


            //扭曲验证码
            double distort = Next(5, 10) * (Next(10) == 1 ? 1 : -1);

            using (Bitmap copy = (Bitmap)bitmap.Clone())
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int newX = (int)(x + (distort * Math.Sin(Math.PI * y / 84.5)));
                        int newY = (int)(y + (distort * Math.Cos(Math.PI * x / 54.5)));
                        if (newX < 0 || newX >= width)
                            newX = 0;
                        if (newY < 0 || newY >= height)
                            newY = 0;
                        bitmap.SetPixel(x, y, copy.GetPixel(newX, newY));
                    }
                }
            }

            drawBrush.Dispose();
            g.Dispose();

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ContentType = "image/Gif";
            System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
        }


        /// <summary>
        /// 获取验证码图像流
        /// </summary>
        /// <returns></returns>
        public static MemoryStream GetImageStream()
        {
            string checkCode = Character.RandomString("num", 4);
            System.Web.HttpContext.Current.Session["verifyCode"] = checkCode;
            return VerifyCodeHelper.GetImageStream(checkCode, 100, 40, Color.White, 1);
        }
        /// <summary>
        /// 获取验证码图像流
        /// </summary>
        /// <param name="code"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="bgcolor"></param>
        /// <param name="textcolor"></param>
        /// <returns></returns>
        public static MemoryStream GetImageStream(string code, int width, int height, Color bgcolor, int textcolor)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            using (Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    Rectangle rect = new Rectangle(0, 0, width, height);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.Clear(bgcolor);
                    int fixedNumber = textcolor == 2 ? 60 : 0;
                    //创建干扰线
                    using (SolidBrush drawBrush = new SolidBrush(Color.FromArgb(Next(100), Next(100), Next(100))))
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            Pen linePen = new Pen(Color.FromArgb(Next(150) + fixedNumber, Next(150) + fixedNumber, Next(150) + fixedNumber), 1);
                            g.DrawLine(linePen, new PointF(0.0F + Next(20), 0.0F + Next(height)), new PointF(0.0F + Next(width), 0.0F + Next(height)));
                        }


                        Matrix m = new Matrix();
                        for (int x = 0; x < code.Length; x++)
                        {
                            m.Reset();
                            m.RotateAt(Next(30) - 15, new PointF(Convert.ToInt64(width * (0.10 * x)), Convert.ToInt64(height * 0.5)));
                            g.Transform = m;
                            drawBrush.Color = Color.FromArgb(Next(150) + fixedNumber + 20, Next(150) + fixedNumber + 20, Next(150) + fixedNumber + 20);
                            PointF drawPoint = new PointF(0.0F + Next(4) + x * 20, 3.0F + Next(3));
                            g.DrawString(Next(1) == 1 ? code[x].ToString() : code[x].ToString().ToUpper(), fonts[Next(fonts.Length - 1)], drawBrush, drawPoint);
                            g.ResetTransform();
                        }

                        //扭曲验证码
                        double distort = Next(5, 10) * (Next(10) == 1 ? 1 : -1);

                        using (Bitmap copy = (Bitmap)bitmap.Clone())
                        {
                            for (int y = 0; y < height; y++)
                            {
                                for (int x = 0; x < width; x++)
                                {
                                    int newX = (int)(x + (distort * Math.Sin(Math.PI * y / 84.5)));
                                    int newY = (int)(y + (distort * Math.Cos(Math.PI * x / 54.5)));
                                    if (newX < 0 || newX >= width)
                                        newX = 0;
                                    if (newY < 0 || newY >= height)
                                        newY = 0;
                                    bitmap.SetPixel(x, y, copy.GetPixel(newX, newY));
                                }
                            }
                        }
                    }
                }
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            }
            return ms;
        }

        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        private static int Next(int max)
        {
            rand.GetBytes(randb);
            int value = BitConverter.ToInt32(randb, 0);
            value = value % (max + 1);
            if (value < 0)
                value = -value;
            return value;
        }

        /**/
        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        private static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }
    }
}