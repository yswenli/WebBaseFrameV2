using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using WebBaseFrame.Models;
//

namespace Web.Areas.Admin.Controllers
{
    public class ArticleController : BaseController
    {
        ArticleRepository ml = new ArticleRepository();
        ArticleKindRepository kc = new ArticleKindRepository();

        public ActionResult Index(int? pageIndex, int? pageSize, Article entity)
        {
            try
            {
                entity.IsDeleted = false;
                var page = ml.Search(entity).GetPagedList(pageIndex ?? PageIndex, pageSize ?? PageSize, Order, By);
                if (Request.IsAjaxRequest())
                    return PartialView("_Index", page);
                return View(page);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }

        public ActionResult Create(int? kid)
        {
            Session.Remove("upaddress");
            ViewData["GetParentOption"] = GetParentOption(0, 0);
            return View(new Article() { KID = kid ?? 0 });
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            try
            {
                ViewData["GetParentOption"] = GetParentOption(0, 0);
                ArticleRepository ml = new ArticleRepository();
                Article obj = new Article() { CreateDate = DateTime.Now, CreateUserID = ID, IsDeleted = false, Thumbs = formCollection["Thumbs"], Files = (Session["upaddress"] == null ? "" : Session["upaddress"].ToString()) };
                UpdateModel(obj);
                obj.Files = string.IsNullOrEmpty(formCollection["upaddress"]) ? string.Empty : formCollection["upaddress"];

                bool result = ml.Insert(obj) > 0 ? true : false;

                return result ? Content(ContentIcon.Succeed + "|操作成功||1") : Content(ContentIcon.Error + "|操作失败");
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                ViewData["GetParentOption"] = GetParentOption(0, 0);

                ArticleRepository ml = new ArticleRepository();

                Article obj = ml.Search().Where(b => b.ID == id).First();

                return View(obj);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            try
            {

                ViewData["GetParentOption"] = GetParentOption(0, 0);
                ArticleRepository ml = new ArticleRepository();
                Article obj = ml.Search().Where(b => b.ID == id).First();
                UpdateModel(obj);
                obj.LastUpdateDate = DateTime.Now;
                obj.LastUpdateUserID = ID;
                obj.Files = string.IsNullOrEmpty(formCollection["upaddress"]) ? string.Empty : formCollection["upaddress"];
                bool result = ml.Update(obj) > 0 ? true : false;
                return result ? Content(ContentIcon.Succeed + "|操作成功||1") : Content(ContentIcon.Error + "|操作失败");
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                ArticleRepository ml = new ArticleRepository();
                if (id > 0)
                    ml.Delete(id);
                else
                {
                    return Content("未指定删除对象ID");
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content(ErrorWirter(RouteData, ex.Message));
            }
        }
        [HttpPost]
        public ActionResult Delete(int? id, FormCollection collection)
        {
            try
            {
                ArticleRepository ml = new ArticleRepository();
                if (id != null && id > 0)
                    ml.Delete(id ?? 0);
                else
                {
                    if (string.IsNullOrEmpty(collection["IDs"]))
                        return Content("未指定删除对象ID");
                    string[] ids = collection["IDs"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in ids)
                    {
                        ml.Delete(int.Parse(item));
                    }
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content(ErrorWirter(RouteData, ex.Message));
            }
        }
        public ActionResult Detail(int id)
        {
            try
            {
                ArticleRepository ml = new ArticleRepository();

                Article obj = ml.Search().Where(b => b.ID == id).First();

                return View(obj);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }
        /// <summary>
        /// 缓存菜单
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 10, Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult TreeView()
        {
            string html = "<ul id=\"browser\" class=\"filetree\"><li><span class=\"folder\">root</span>";
            ViewData["treeView"] = html + GetNotes(1) + "</li></ul>";
            return View();
        }
        private string GetNotes(int? pid)
        {
            string noteStr = "";
            ArticleKindRepository ml = new ArticleKindRepository();
            var klts = ml.GetListByPID(pid ?? 0);
            if (klts != null)
            {
                noteStr += "<ul>";
                foreach (var item in klts)
                {
                    if (ml.HasChilds(item.ID))
                    {
                        noteStr += "<li><span class=\"folder\" title=\"" + item.Name + "\" data-id=\"" + item.ID + "\">" + (item.Name.Length > 6 ? item.Name.Substring(0, 5) + ".." : item.Name) + "</span>" + GetNotes(item.ID) + "</li>";
                    }
                    else
                    {
                        noteStr += "<li><span class=\"file skip\" title=\"" + item.Name + "\" data-id=\"" + item.ID + "\">" + (item.Name.Length > 6 ? item.Name.Substring(0, 5) + ".." : item.Name) + "</span></li>";
                    }
                }
                noteStr += "</ul>";
            }
            return noteStr;
        }

        /// <summary>
        /// 递归得到类别列表
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public string GetParentOption(int? parentID, int level)
        {
            level++;
            string option = "<option value=\"{0}\">" + "{1}" + "</option>";
            StringBuilder html = new StringBuilder();
            List<ArticleKind> klist = new ArticleKindRepository().Search().Where(b => b.PID == parentID).OrderBy(b => b.SortID).ToList();
            var i = 0;
            foreach (var item in klist)
            {
                StringBuilder name = new StringBuilder();
                if (item.PID == 0 || item.PID.ToString() == "")
                    name.Append(item.Name);
                else
                {
                    StringBuilder nbsp = new StringBuilder();
                    for (int j = 0; j < level * 2; j++)
                    {
                        nbsp.Append("&nbsp;");
                    }

                    if (i == klist.Count() - 1)
                    {
                        name.Append(nbsp.ToString() + "└─  " + item.Name);
                    }
                    else
                    {
                        name.Append(nbsp.ToString() + "├─  " + item.Name);
                    }
                }
                string kind = "";
                try
                {
                    kind = new ArticleKindRepository().Search().Where(b => b.ID == item.PID).First().Name;
                }
                catch { }
                html.Append(string.Format(option, item.ID, name.ToString()));
                html.Append(GetParentOption(item.ID, level));
                i++;
            }
            return html.ToString();
        }

        #region 每次新增内容前先清除Session
        public void RemoveSession()
        {

            Session.Remove("ArticImg");
            // return Content(Session["ArticImg"].ToString());
        }
        #endregion


        #region ajax请求获得内容缩略图的路径
        [HttpPost]
        public ActionResult GetImg()
        {
            if (Session["ArticImg"] != null)
            {
                return Content(Session["ArticImg"].ToString());
            }
            else
            {
                return Content("-1");
            }
        }

        #endregion


        #region 内容附件上传
        [HttpPost]
        public ActionResult UpLoadFile()
        {
            try
            {
                string upaddress = string.Empty;
                var c = Request.Files[0];
                if (c == null && c.ContentLength <= 0)
                {
                    return Content(string.Empty);
                }
                else
                {
                    string fileName = c.FileName;
                    string newFilename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(0, 9) + fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.'));

                    string savepath = "/Upload/" + DateTime.Now.Date.ToString("yyyy") + "/" + DateTime.Now.Date.ToString("MMdd");
                    string path = Server.MapPath("~") + savepath;

                    if (!System.IO.File.Exists(path))
                    {
                        Common.DirHelper.CheckFolder(path);
                    }

                    c.SaveAs(path + "/" + newFilename);
                    upaddress += savepath + "/" + newFilename;
                    return Content(upaddress);

                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message.ToString());
            }
        }
        #endregion
    }
}


