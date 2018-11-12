using O2S.Components.PDFRender4NET;
/************************************************************************************
 * Copyright (c)2015 All Rights Reserved.
 * CLR版本： 4.0.30319.34014
 *机器名称：ED6971
 *公司名称：
 *命名空间：Common
 *文件名：  PDFHelper
 *版本号：  V1.0.0.0
 *唯一标识：87950d4f-7819-411e-9cf4-412e0c519c8f
 *当前的用户域：SH
 *创建人：  Li.Wen
 *电子邮箱：yswenli@outlook.com
 *创建时间：2015/7/3 18:45:52
 *描述：
 *=====================================================================
 *修改标记
 *修改时间：2015/7/3 18:45:52
 *修改人： Li.Wen
 *版本号： V1.0.0.0
 *描述：
/************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    public class PDFHelper
    {
        public enum PDFDefinition
        {
            One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10
        }



        /// <summary>
        /// 将PDF文档转换为图片的方法
        /// </summary>
        /// <param name="pdfInputPath">PDF文件路径</param>
        /// <param name="imageOutputPath">图片输出路径</param>
        public static void ToImage(string pdfInputPath, string imageOutputPath)
        {
            string imageName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            PDFHelper.ToImage(pdfInputPath, imageOutputPath, imageName, 1, 100, ImageFormat.Png, PDFDefinition.Five);
        }



        /// <summary>
        /// 将PDF文档转换为图片的方法
        /// </summary>
        /// <param name="pdfInputPath">PDF文件路径</param>
        /// <param name="imageOutputPath">图片输出路径</param>
        /// <param name="imageName">生成图片的名字</param>
        /// <param name="startPageNum">从PDF文档的第几页开始转换</param>
        /// <param name="endPageNum">从PDF文档的第几页开始停止转换</param>
        /// <param name="imageFormat">设置所需图片格式</param>
        /// <param name="definition">设置图片的清晰度，数字越大越清晰</param>
        public static void ToImage(string pdfInputPath, string imageOutputPath,
            string imageName, int startPageNum, int endPageNum, ImageFormat imageFormat, PDFDefinition definition)
        {
            using (PDFFile pdfFile = PDFFile.Open(pdfInputPath))
            {

                if (!Directory.Exists(imageOutputPath))
                {
                    Directory.CreateDirectory(imageOutputPath);
                }

                // validate pageNum
                if (startPageNum <= 0)
                {
                    startPageNum = 1;
                }

                if (endPageNum > pdfFile.PageCount)
                {
                    endPageNum = pdfFile.PageCount;
                }

                if (startPageNum > endPageNum)
                {
                    int tempPageNum = startPageNum;
                    startPageNum = endPageNum;
                    endPageNum = startPageNum;
                }

                // start to convert each page
                for (int i = startPageNum; i <= endPageNum; i++)
                {
                    using (Bitmap pageImage = pdfFile.GetPageImage(i - 1, 56 * (int)definition))
                    {
                        pageImage.Save(imageOutputPath + imageName + i.ToString() + "." + imageFormat.ToString(), imageFormat);
                    }
                }

            }
        }

    }
}
