using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElFinder;

namespace Web.Areas.Admin.Controllers
{
    public class elFinderController : Controller
    {
        public string pathfolder = "/Upload/";
        public string urlfolder = "/Upload/";

        [ValidateInput(false)]
        public ActionResult Index(string folder, string subFolder, string content)
        {
            try
            {
                //不存在，则直接创建目录
                string meeting_folder = Server.MapPath(pathfolder + folder);
                if (!Directory.Exists(meeting_folder))
                {
                    Common.DirHelper.CheckFolder(meeting_folder);
                    //Common.DirHelper.CopyDirectory(Server.MapPath(pathContent), meeting_folder);
                }
                FileSystemDriver driver = new FileSystemDriver();
                var root = new Root(
                        new DirectoryInfo(meeting_folder),
                        "http://" + Request.Url.Authority + urlfolder + folder)
                {
                    IsReadOnly = false, // Can be readonly according to user's membership permission
                    Alias = urlfolder.Substring(1) + folder, // Beautiful name given to the root/home folder
                    MaxUploadSizeInKb = 500000 // Limit imposed to user uploaded file <= 500 KB
                };
                if (!string.IsNullOrEmpty(subFolder))
                {
                    root.StartPath = new DirectoryInfo(meeting_folder + "/" + subFolder);
                }
                driver.AddRoot(root);
                var connector = new Connector(driver);
                return connector.Process(this.HttpContext.Request, content);
            }
            catch
            {

            }
            return Content(string.Empty);
        }
        [ValidateInput(false)]
        public ActionResult SelectFile(string target)
        {
            try
            {
                FileSystemDriver driver = new FileSystemDriver();

                driver.AddRoot(
                    new Root(
                        new DirectoryInfo(pathfolder),
                        "http://" + Request.Url.Authority + urlfolder) { IsReadOnly = false });

                var connector = new Connector(driver);

                return Json(connector.GetFileByHash(target).FullName);
            }
            catch { }
            return Content(string.Empty);
        }
    }
}
