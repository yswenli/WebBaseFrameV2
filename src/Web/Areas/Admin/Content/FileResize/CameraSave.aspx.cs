using Common;
using Libraries;
using System;
using System.Drawing;
using WebBaseFrame.Models;

namespace Web.Areas.Admin.Content.FileResize
{
    public partial class CameraSave : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int intBigHeight;
            int intBigWidth;
            string strBigData;
            int intSmallHeight;
            int intSmallWidth;
            string strSmallData;
            try
            {
                string path = Server.MapPath("~") + @"upload\avatar\";
                DirHelper.CheckFolder(path);
                intBigHeight = int.Parse(Request.Form["BigHeight"].ToString());
                intBigWidth = int.Parse(Request.Form["BigWidth"].ToString());
                strBigData = Request.Form["BigData"].ToString();
                string bmpBigJpg = CurrentMember.ID + "_w" + intBigWidth + "_h" + intBigHeight + ".jpg";
                SaveBmp(BuildBitmap(intBigWidth, intBigHeight, strBigData), path + "\\" + bmpBigJpg);

                intSmallHeight = int.Parse(Request.Form["SmallHeight"].ToString());
                intSmallWidth = int.Parse(Request.Form["SmallWidth"].ToString());
                strSmallData = Request.Form["SmallData"].ToString();
                string bmpSmallJpg = CurrentMember.ID + "_w" + intSmallWidth + "_h" + intSmallHeight + ".jpg";
                SaveBmp(BuildBitmap(intSmallWidth, intSmallHeight, strSmallData), path + "\\" + bmpSmallJpg);

                #region 更新头像
                Member member = new MemberRepository().Search().Where(b => b.ID == CurrentMember.ID).First();
                member.Avatar = "/upload/avatar/" + bmpBigJpg;
                new MemberRepository().Update(member);
                Session.Remove("CurrentUser");
                #endregion

                Response.Write("1");
            }
            catch
            {
                Response.Write("0");
            }
            Response.End();

        }

        public System.Drawing.Bitmap BuildBitmap(int width, int height, string strBmp)
        {
            System.Drawing.Bitmap tmpBmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            string[] StmpBmp = strBmp.Split(',');
            int pos = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tmpBmp.SetPixel(x, y, Color.FromArgb(int.Parse(StmpBmp[pos], System.Globalization.NumberStyles.HexNumber)));
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