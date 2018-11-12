using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Common
{
    public class UploadHepler
    {
        #region 对象
        static string fileupload="upload";
        /// <summary>
        /// 获得图片上传目录
        /// </summary>
        public static string Path
        {
            get
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~") + fileupload + @"\";
                DirHelper.CheckFolder(path);
                return path;
            }
        }        
        #endregion

        #region 方法
        /// <summary>
        /// 获取图片上传上来后返回的httpurl
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string AttachmentUrl(object path)
        {
            string str = path.ToString();
            if (str != "")
            {
                str =  "/" + fileupload + @"/" + path.ToString();
            }
            return str.Replace("\\", "/");
        }
        /// <summary>
        /// 获得附件的ico图标
        /// </summary>
        /// <param name="path"></param>
        /// <param name="_fix"></param>
        /// <returns></returns>
        public static string AttachmentIcoUrl(object path,string _fix)
        {
            string str = path.ToString();
            string fix=str.Substring(str.LastIndexOf(".")+1).ToLower();
            if (str != "")
            {
                if (fix == "jpg" || fix == "gif" || fix == "png" || fix == "bmp")
                    str = UrlHelper.HostUrl + "/" + fileupload + @"/" + path.ToString();
                else
                {
                    str = UrlHelper.HostUrl + "/areas/admin/content/img/ext/" + fix + @"." + _fix;
                    if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath("~") + @"\" + str.Replace("/", "\\")))
                    {
                        str = UrlHelper.HostUrl + "/areas/admin/content/img/ext/blank." + _fix;
                    }
                }
            }
            return str.Replace("\\", "/");
        }
        /// <summary>
        /// 获取缩略图的url
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="ThumbnailName">缩略图名称</param>
        /// <returns></returns>
        public static string AttachmentThumbnailUrl(string name, object path, string ThumbnailName)
        {
            string str = path.ToString();
            return str.Replace("\\", "/");
        }
        /// <summary>
        /// 返回附件的绝对地址
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string AttachmentPath(object obj)
        {
            string path = obj.ToString();
            if (path != "")
            {
                path = Path + obj;
            }
            return path;
        }
        ///<summary> 
        /// 生成缩略图 
        /// </summary> 
        /// <param name="originalImagePath">源图路径（物理路径）</param> 
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param> 
        /// <param name="width">缩略图宽度</param> 
        /// <param name="height">缩略图高度</param> 
        /// <param name="mode">生成缩略图的方式HW/W/H/CUT</param>     
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode.ToUpper())
            {
                case "HW"://指定高宽缩放（可能变形）                 
                    break;
                case "W"://指定宽，高按比例                     
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "CUT"://指定高宽裁减（不变形）                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
             new Rectangle(x, y, ow, oh),
             GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图 
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 创建水印
        /// </summary>
        /// <param name="imgpath"></param>
        /// <param name="WaterMarkPicPath"></param>
        /// <param name="newFilePath"></param>
        public static void WatermarkImage(string imgpath, string WaterMarkPicPath,out string newFilePath)
        {
            newFilePath = "";
            if (System.IO.File.Exists(imgpath) && System.IO.File.Exists(WaterMarkPicPath))
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(imgpath);
                Graphics picture = Graphics.FromImage(image);
                Image watermark = new Bitmap(WaterMarkPicPath);
                int _width = image.Width;
                int _height = image.Height;
                int _watermarkPosition = 9;
                ImageAttributes imageAttributes = new ImageAttributes();
                ColorMap colorMap = new ColorMap();

                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                ColorMap[] remapTable = { colorMap };

                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                float[][] colorMatrixElements = {
                                                 new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                             };

                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                int xpos = 0;
                int ypos = 0;
                int WatermarkWidth = 0;
                int WatermarkHeight = 0;
                double bl = 1d;
                //计算水印图片的比率
                //取背景的1/4宽度来比较
                if ((_width > watermark.Width * 4) && (_height > watermark.Height * 4))
                {
                    bl = 1;
                }
                else if ((_width > watermark.Width * 4) && (_height < watermark.Height * 4))
                {
                    bl = Convert.ToDouble(_height / 4) / Convert.ToDouble(watermark.Height);

                }
                else

                    if ((_width < watermark.Width * 4) && (_height > watermark.Height * 4))
                    {
                        bl = Convert.ToDouble(_width / 4) / Convert.ToDouble(watermark.Width);
                    }
                    else
                    {
                        if ((_width * watermark.Height) > (_height * watermark.Width))
                        {
                            bl = Convert.ToDouble(_height / 4) / Convert.ToDouble(watermark.Height);

                        }
                        else
                        {
                            bl = Convert.ToDouble(_width / 4) / Convert.ToDouble(watermark.Width);

                        }

                    }

                WatermarkWidth = Convert.ToInt32(watermark.Width * bl);
                WatermarkHeight = Convert.ToInt32(watermark.Height * bl);


                if (_watermarkPosition == 0)
                    _watermarkPosition = (new Random().Next(1, 9));
                switch (_watermarkPosition)
                {
                    case 1:// 顶部居左 
                        xpos = 10;
                        ypos = 10;
                        break;
                    case 2://顶部居中
                        xpos = _width / 2 - WatermarkWidth / 2;
                        ypos = 10;
                        break;
                    case 3://顶部居右
                        xpos = _width - WatermarkWidth - 10;
                        ypos = 10;
                        break;
                    case 4://左部居左
                        xpos = 10;
                        ypos = _height / 2 - WatermarkHeight / 2;
                        break;
                    case 5://左部居中
                        xpos = _width / 2 - WatermarkWidth / 2;
                        ypos = _height / 2 - WatermarkHeight / 2;
                        break;
                    case 6://左部居右
                        xpos = _width - WatermarkWidth - 10;
                        ypos = _height / 2 - WatermarkHeight / 2 - 10;
                        break;
                    case 7://底部居左
                        xpos = 10;
                        ypos = _height - WatermarkHeight - 10;
                        break;
                    case 8://底部居中
                        xpos =  _width / 2 - WatermarkWidth / 2;;
                        ypos = _height - WatermarkHeight - 10;
                        break;
                    case 9:// 底部居右
                        xpos = _width - WatermarkWidth - 10;
                        ypos = _height - WatermarkHeight - 10;
                        break;
                    
                }

                picture.DrawImage(watermark, new Rectangle(xpos, ypos, WatermarkWidth, WatermarkHeight), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);


                watermark.Dispose();
                imageAttributes.Dispose();
                string path = imgpath.Substring(0, imgpath.LastIndexOf(@"\") + 1);
                string name = imgpath.Substring(imgpath.LastIndexOf(@"\") + 1);
                newFilePath = path + "wm_" + name;
                image.Save(newFilePath);
                image.Dispose();
            }
        }

        /// <summary>
        /// 获得附件的大小KB
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string AttachmentLength(object obj)
        {
            string str = "";
            string path = Path + obj.ToString();
            if (System.IO.File.Exists(path))
            {
                System.IO.FileStream fs = System.IO.File.OpenRead(path);
                str = ConvertAttachmentLength((double)(fs.Length));
            }
            return str;
        }
        /// <summary>
        /// 计算附件大小
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ConvertAttachmentLength(double length)
        {
            double KBCount = 1024;
            double MBCount = KBCount * 1024;
            double GBCount = MBCount * 1024;
            double TBCount = GBCount * 1024;
            double size = length;
            int roundCount = 2;
            string str = "";
            if (KBCount > size)
            {
                str = Math.Round(size, roundCount) + "B";
            }
            else if (MBCount > size)
            {
                str = Math.Round(size / KBCount, roundCount) + "KB";
            }
            else if (GBCount > size)
            {
                str = Math.Round(size / MBCount, roundCount) + "MB";
            }
            else if (TBCount > size)
            {
                str = Math.Round(size / GBCount, roundCount) + "GB";
            }
            else
            {
                str = Math.Round(size / TBCount, roundCount) + "TB";
            }
            return str;
        }
        /// <summary>
        /// 获得附件的大小KB
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string AttachmentLength(int? length)
        {
            double KBCount = 1024;
            double MBCount = KBCount * 1024;
            double GBCount = MBCount * 1024;
            double TBCount = GBCount * 1024;
            double size = double.Parse(length.ToString());
            string str = "";
            int roundCount = 2;
            if (KBCount > size)
            {
                str = Math.Round(size, roundCount) + "B";
            }
            else if (MBCount > size)
            {
                str = Math.Round(size / KBCount, roundCount) + "KB";
            }
            else if (GBCount > size)
            {
                str = Math.Round(size / MBCount, roundCount) + "MB";
            }
            else if (TBCount > size)
            {
                str = Math.Round(size / GBCount, roundCount) + "GB";
            }
            else
            {
                str = Math.Round(size / TBCount, roundCount) + "TB";
            }

            return str;
        }
        #endregion
    }
}
