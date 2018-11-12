using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Web.Areas.Admin.Controllers;
using WebBaseFrame.Models;

namespace Web.FileResize
{
    public partial class CameraSave : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string result = string.Empty;
            int intBigHeight;
            int intBigWidth;
            string strBigData;
            int intSmallHeight;
            int intSmallWidth;
            string strSmallData;
            try
            {
                Session.Remove("ArticImg"); 
                string path = Server.MapPath("~") + @"upload\avatar\";
                DirHelper.CheckFolder(path);
                intBigHeight = int.Parse(Request.Form["BigHeight"].ToString());
                intBigWidth = int.Parse(Request.Form["BigWidth"].ToString());
                strBigData = Request.Form["BigData"].ToString();
                string bmpBigJpg =DateTime.Now.ToString("yyyyMMddHHmmss") + "_w" + intBigWidth + "_h" + intBigHeight + ".jpg";
                SaveBmp(BuildBitmap(intBigWidth, intBigHeight, strBigData), path + "\\" + bmpBigJpg);

                intSmallHeight = int.Parse(Request.Form["SmallHeight"].ToString());
                intSmallWidth = int.Parse(Request.Form["SmallWidth"].ToString());
                strSmallData = Request.Form["SmallData"].ToString();
                string bmpSmallJpg = DateTime.Now.ToString("yyyyMMddHHmmss") + "_t" + intBigWidth + "_h" + intBigWidth + ".jpg";
                SaveBmp(BuildBitmap(intSmallWidth, intSmallHeight, strSmallData), path + "\\" + bmpSmallJpg);

                #region 缩略图
                string avatar = "/upload/avatar/" + bmpBigJpg;///图片地址
               
                Session["ArticImg"] = avatar;

                Response.Write("1");
                
                #endregion
            }
            catch
            {
                Response.Write("0");
            }
          
            Response.End();
          
        
        }

        public System.Drawing.Bitmap BuildBitmap(int width, int height, string strBmp)
        {
            System.Drawing.Bitmap tmpBmp = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            string[] StmpBmp = strBmp.Split(',');
            int pos = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tmpBmp.SetPixel(x, y, System.Drawing.Color.FromArgb(int.Parse(StmpBmp[pos], System.Globalization.NumberStyles.HexNumber)));
                    pos++;
                }
            }
            return tmpBmp;
        }
        public void SaveBmp(System.Drawing.Bitmap bmp, string filePath)
        {
            bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }


    }
}