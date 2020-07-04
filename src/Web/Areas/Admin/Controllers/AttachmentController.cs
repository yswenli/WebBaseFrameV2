using Common;
using Libraries;
using System;
using System.Collections;
using System.Web.Mvc;
using WebBaseFrame.Models;
using WEF;
using WEF.MvcPager;

namespace Web.Areas.Admin.Controllers
{
    public class AttachmentController : BaseController
    {


        Attachment attachment = new Attachment();
        AttachmentRepository attachmentclass = new AttachmentRepository();

        public ActionResult Index(int? pageIndex, int? pageSize, Attachment entity)
        {
            try
            {
                AttachmentRepository ml = new AttachmentRepository();

                PagedList<Attachment> page = ml.Search(entity).GetPagedList(pageIndex ?? PageIndex, pageSize ?? PageSize, Order, By);

                if (Request.IsAjaxRequest())

                    return PartialView("_Index", page);

                return View(page);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }

        #region 附件列表
        public ActionResult List(int? pageIndex, int? pageSize)
        {
            string name = Request.QueryString["filename"];
            string begindate = Request.QueryString["start_uploadtime"];
            string enddate = Request.QueryString["end_uploadtime"];
            string ext = Request.QueryString["fileext"];
            PagedList<Attachment> page = attachmentclass.Search(new Attachment() { Name = name }).GetPagedList(pageIndex ?? 1, pageSize ?? 20, "ID", true);
            return View(page);
        }
        public ActionResult Dir(string dir)
        {
            ArrayList folderlist = DirHelper.GetFolders(Server.MapPath("~") + "/" + dir);
            ViewData["folderlist"] = folderlist;
            ArrayList filelist = DirHelper.GetFiles(Server.MapPath("~") + "/" + dir);
            ViewData["filelist"] = filelist;
            return View();
        }
        #endregion

        #region 附件上传
        /// <summary>
        /// 图库
        /// </summary>
        /// <returns></returns>
        public ActionResult AlbumList(int? pageIndex, int? pageSize = 16)
        {
            string name = Request.QueryString["filename"];
            string begindate = Request.QueryString["uploadtime"];
            string enddate = Request.QueryString["uploadtime"];
            string ext = Request.QueryString["fileext"];
            PagedList<Attachment> page = attachmentclass.Search(new Attachment() { Name = name }).GetPagedList(pageIndex ?? 1, pageSize ?? 20, "ID", true);
            return View(page);
        }
        /// <summary>
        /// 目录浏览
        /// </summary>
        /// <returns></returns>
        public ActionResult AlbumDir(string dir)
        {
            ArrayList folderlist = DirHelper.GetFolders(Server.MapPath("~") + "/" + dir);
            ViewData["folderlist"] = folderlist;
            ArrayList filelist = DirHelper.GetFiles(Server.MapPath("~") + "/" + dir);
            ViewData["filelist"] = filelist;
            return View();
        }

        #region 裁剪图片
        public ActionResult CropPic()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Crop(FormCollection collection)
        {
            System.IO.Stream stream = Request.InputStream;
            int width = Convert.ToInt32(Request.QueryString["width"]);
            int height = Convert.ToInt32(Request.QueryString["height"]);
            string filepath = Request.QueryString["file"].Substring(0, Request.QueryString["file"].IndexOf("?"));
            string filename = filepath.Substring(filepath.LastIndexOf("/") + 1);
            string newfilename = "thumb_" + width + "_" + height + "_" + filename;
            System.Drawing.Image img = null;
            img = System.Drawing.Image.FromStream(stream, false);
            string newfilepath = Server.MapPath("~") + @"upload\thumb\" + newfilename;
            img.Save(newfilepath, System.Drawing.Imaging.ImageFormat.Jpeg);
            return Content("/upload/thumb/" + newfilename);
        }
        #endregion

        public ActionResult SwfUpload(string args)
        {
            string[] arg = args.Split(',');
            string allowfiles = "10";
            string fix = "*.*";
            string filelength = "2";
            string showdir = "1";
            string watermark = "0";
            for (int i = 0; i < arg.Length; i++)
            {
                if (i == 0)//参数1：允许上传的附件个数
                    allowfiles = (arg[i] == "" ? allowfiles : arg[i]);
                else if (i == 1)//参数2：允许上传的附件格式
                {
                    fix = (arg[i] == "" ? fix : "*." + arg[i].Replace("|", ";*."));
                }
                else if (i == 2)//参数3：允许上传的附件大小
                {
                    filelength = (arg[i] == "" ? filelength : arg[i]);
                }
                else if (i == 3)//参数4：允许浏览目录
                {
                    showdir = (arg[i] == "" ? showdir : arg[i]);
                }
                else if (i == 4)//参数5：允许添加水印
                {
                    watermark = (arg[i] == "" ? watermark : arg[i]);
                }

            }
            ViewData["allowfiles"] = allowfiles;
            ViewData["fix"] = fix;
            ViewData["filelength"] = int.Parse(filelength) * 1024;
            ViewData["showdir"] = showdir;
            ViewData["watermark"] = watermark;
            return View();
        }

        [HttpPost]
        public ActionResult SwfUploading(FormCollection collection)
        {
            if (Request.Files.Count > 0)
            {
                string gID = DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd");
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var c = Request.Files[i];
                    if (c != null && c.ContentLength > 0)
                    {
                        int lastSlashIndex = c.FileName.LastIndexOf("\\");
                        string fileName = c.FileName.Substring(lastSlashIndex + 1, c.FileName.Length - lastSlashIndex - 1);
                        string fix = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();
                        string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        string rename = time + "." + fix;
                        string filepath = Common.UploadHepler.Path + gID;
                        string path = gID + "\\" + rename;

                        /*判断是否为办公文件，如果是则保存到指定目录转换成swf供在线播放*/
                        if (fix == "doc" || fix == "ppt" || fix == "xls" || fix == "pdf")
                        {
                            filepath = UploadHepler.Path + "doc";
                            path = "doc\\" + rename;
                            new DBContext().ExecuteNonQuery(@"insert into service_flashpaper_printe_queue(OriginalName,NewName,Created,IsConverted) values('" + rename + "','" + time + ".swf" + "',getdate(),0)");
                        }
                        DirHelper.CheckFolder(filepath);

                        string fullname = filepath + "\\" + rename;
                        c.SaveAs(fullname);

                        /*是否添加水印*/
                        string newFilePath = "";
                        if (SiteHelper.Default.WatermarkEnable == 1 && (fix == "jpg" || fix == "gif" || fix == "bmp" || fix == "png" || fix == "jepg"))
                        {
                            Common.UploadHepler.WatermarkImage(fullname, UploadHepler.Path.Replace(@"\upload\", "") + SiteHelper.Default.WatermarkImg.Replace("/", @"\"), out newFilePath);
                        }
                        if (newFilePath != "")
                        {
                            path = gID + "\\wm_" + rename;
                        }
                        attachment = new Attachment();
                        attachment.ID = int.Parse(string.IsNullOrEmpty(collection["aid"]) ? "0" : collection["aid"]);
                        attachment.Size = Common.UploadHepler.ConvertAttachmentLength((double)c.ContentLength);
                        attachment.FileName = fileName;
                        //attachment.attype = 2;
                        attachment.CreateDate = DateTime.Now;
                        attachment.CreateUserID = CurrentMember.ID;
                        attachment.Fix = fix;
                        //attachment.ssid = int.Parse(string.IsNullOrEmpty(collection["ssid"]) ? "0" : collection["ssid"]);
                        attachment.Url = path;
                        attachmentclass.Insert(attachment);
                    }
                }
            }
            return Content(attachment.ID + "," + UploadHepler.AttachmentUrl(attachment.Url) + "," + attachment.Fix);
        }
        #endregion

        #region 删除
        public ActionResult DeleteByName(string atname, string dir)
        {
            attachment = attachmentclass.Search().Where(b => b.Name == atname).First();
            Common.DirHelper.RemoveFile(Server.MapPath("~") + "\\" + dir + atname);
            if (attachment != null)
            {
                attachmentclass.Delete(attachment);
            }

            return Content("1");
        }
        public ActionResult Delete(int id)
        {
            attachment = attachmentclass.GetAttachment(id);
            if (attachment != null)
            {
                Common.DirHelper.RemoveFile(Common.UploadHepler.AttachmentPath(attachment.Url));
                attachmentclass.Delete(attachment);
            }
            return Content("1");
        }
        [HttpPost]
        public ActionResult Delete(FormCollection collection)
        {
            string[] atids = collection["aid[]"].Split(',');
            for (int i = 0; i < atids.Length; i++)
            {
                attachment = attachmentclass.GetAttachment(int.Parse(atids[i]));
                if (attachment != null)
                {
                    Common.DirHelper.RemoveFile(Common.UploadHepler.AttachmentPath(attachment.Url));
                    attachmentclass.Delete(attachment);
                }
            }
            return Content(ContentIcon.Succeed + "|保存成功");
        }
        #endregion

        #region 查询附件
        public ActionResult GetAttachmentName(string val, string index)
        {
            val = val.ToLower().Replace(Common.UrlHelper.HostUrl + "/upload/", "").Replace("/", "");
            object obj = new DBContext().ExecuteNonQuery("select top 1 atname from attachments where replace(url,'\\','')='" + val + "'");
            if (obj == null)
                obj = "";
            return Content(index + "," + obj.ToString());
        }
        #endregion
    }
}


