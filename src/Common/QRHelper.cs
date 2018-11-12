/************************************************************************************
 * Copyright (c)2015 All Rights Reserved.
 * CLR版本： 4.0.30319.34014
 *机器名称：ED6971
 *公司名称：
 *命名空间：Common
 *文件名：  QRHelper
 *版本号：  V1.0.0.0
 *唯一标识：bb136c29-5d32-40fd-8ef2-1434f25c0066
 *当前的用户域：SH
 *创建人：  li.wen
 *电子邮箱：ysyswenli@outlook.com
 *创建时间：2015/6/26 15:41:39
 *描述：
 *=====================================================================
 *修改标记
 *修改时间：2015/6/26 15:41:39
 *修改人： li.wen
 *版本号： V1.0.0.0
 *描述：
/************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

//
using Leigh.Wen.QRCode.Libs;
using System.Drawing.Imaging;

namespace Common
{
    /// <summary>
    /// 二维码生成类
    /// </summary>
    public class QRHelper
    {
        string _message;

        Color _foreColor;

        Color _backColor;

        string _iconFilePath;

        byte[] _QRContent;

        public QRHelper(string message)
            : this(message, Color.FromArgb(255, 70, 181, 208), Color.White, string.Empty)
        {

        }

        public QRHelper(string message, Color foreColor, Color backColor, string iconFilePath)
        {
            _message = message;
            _foreColor = foreColor;
            _backColor = backColor;
            _iconFilePath = iconFilePath;
        }

        /// <summary>
        /// 获取QR内容
        /// </summary>
        /// <returns></returns>
        public byte[] GetQRContent()
        {

            return GetQRContent(QRCodeEncoder.ENCODE_MODE.BYTE, 3, 6, QRCodeEncoder.ERROR_CORRECTION.M, ImageFormat.Png);
        }

        /// <summary>
        /// 获取QR内容
        /// </summary>
        /// <param name="model">编码模式</param>
        /// <param name="scale">比例：二维码图片大小</param>
        /// <param name="version">版本：决定图片复杂度及内容多少</param>
        /// <param name="correction">纠错</param>
        /// <param name="imageFormat"></param>
        /// <returns></returns>
        public byte[] GetQRContent(Leigh.Wen.QRCode.Libs.QRCodeEncoder.ENCODE_MODE model, int scale, int version, Leigh.Wen.QRCode.Libs.QRCodeEncoder.ERROR_CORRECTION correction, ImageFormat imageFormat)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();

                    qrCodeEncoder.QRCodeEncodeMode = model;

                    qrCodeEncoder.QRCodeScale = scale;

                    qrCodeEncoder.QRCodeVersion = version;

                    qrCodeEncoder.QRCodeErrorCorrect = correction;

                    qrCodeEncoder.QRCodeBackgroundColor = _backColor;

                    qrCodeEncoder.QRCodeForegroundColor = _foreColor;

                    System.Drawing.Bitmap bmp = qrCodeEncoder.Encode(_message);

                    if (bmp != null && bmp.Width > 0)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(_iconFilePath))
                            {
                                Bitmap iconBitMap = new Bitmap(_iconFilePath);
                                if (iconBitMap != null && iconBitMap.Width > 0)
                                {
                                    int left = (bmp.Width - iconBitMap.Width) / 2;
                                    int top = (bmp.Height - iconBitMap.Height) / 2;
                                    for (int i = 0; i < bmp.Width; i++)
                                    {
                                        for (int j = 0; j < bmp.Height; j++)
                                        {
                                            if (i >= left && i < left + iconBitMap.Width)
                                            {
                                                if (j >= top && j < top + iconBitMap.Height)
                                                {
                                                    var color = iconBitMap.GetPixel(i - left, j - top);

                                                    if (color.A != Color.Transparent.A)
                                                    {
                                                        bmp.SetPixel(i, j, iconBitMap.GetPixel(i - left, j - top));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                    bmp.Save(ms, imageFormat);
                }
                catch { }
                _QRContent = ms.ToArray();
                return _QRContent;
            }

        }


        /// <summary>
        /// 存储QR
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path)
        {
            using (var fs = File.Create(path))
            {
                fs.Write(_QRContent, 0, _QRContent.Length);
                fs.Flush();
            }
        }

    }
}
