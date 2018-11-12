using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Common
{
    /// <summary>
    /// 将指定的网页导出成图片
    /// </summary>
    public class HtmlToJpgHelper
    {
        string URL;
        int Height;
        int Width;
        int IHeight;
        int IWidth;
        Bitmap image;
        public HtmlToJpgHelper(string url, int width, int height, int iwidth, int iheight)
        {
            this.Height = height;
            this.Width = width;
            this.URL = url;
            this.IWidth = iwidth;
            this.IHeight = iheight;
            Thread th = new Thread(new ThreadStart(DrawBitmap));
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            while (this.image == null)
            {
                Thread.Sleep(200);
            }
        }
        private void DrawBitmap()
        {
            WebBrowser MyBrowser = new WebBrowser();
            MyBrowser.ScrollBarsEnabled = false;
            MyBrowser.Size = new Size(this.Width, this.Height);
            MyBrowser.Navigate(this.URL);
            while (MyBrowser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            Bitmap myBitmap = new Bitmap(Width, Height);
            Rectangle DrawRect = new Rectangle(0, 0, Width, Height);
            MyBrowser.DrawToBitmap(myBitmap, DrawRect);
            System.Drawing.Image imgOutput = myBitmap;
            System.Drawing.Image oThumbNail = new Bitmap(this.IWidth, this.IHeight, imgOutput.PixelFormat);
            Graphics g = Graphics.FromImage(oThumbNail);
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            Rectangle oRectangle = new Rectangle(0, 0, this.IWidth, this.IHeight);
            g.DrawImage(imgOutput, oRectangle);
            this.image = (Bitmap)oThumbNail;
            imgOutput.Dispose();
            imgOutput = null;
            MyBrowser.Dispose();
            MyBrowser = null;
        }

        public void WriteImage()
        {
            MemoryStream ms = new MemoryStream();
            this.image.Save(ms, ImageFormat.Jpeg);
            byte[] buffer = new byte[ms.Length];
            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(buffer, 0, buffer.Length);
            string Filename = DateTime.Now.ToString("yyyyMMddhhmmss");
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;   filename=" + Filename + ".jpg");
            System.Web.HttpContext.Current.Response.ContentType = "image/jpeg";
            System.Web.HttpContext.Current.Response.BinaryWrite(buffer);
            System.Web.HttpContext.Current.Response.End();
        }
    }

}