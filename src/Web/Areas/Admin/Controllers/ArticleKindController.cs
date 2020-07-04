using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WebBaseFrame.Models;
using WEF;
using WEF.Expressions;
using WEF.MvcPager;

namespace Web.Areas.Admin.Controllers
{
    public class ArticleKindController : BaseController
    {

        public ActionResult Index(int? pageIndex, int? pageSize, ArticleKind entity)
        {
            try
            {
                ArticleKindRepository ml = new ArticleKindRepository();

                var where = new Where<ArticleKind>();

                if (!string.IsNullOrEmpty(entity.Name))
                {
                    where.And(b => b.Name.Like(entity.Name));
                }

                PagedList<ArticleKind> page = ml.Search().Where(where).GetPagedList(pageIndex ?? PageIndex, pageSize ?? PageSize, Order, By);

                if (Request.IsAjaxRequest())
                    return PartialView("_Index", page);
                return View(page);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }

        public ActionResult Create(int? pid)
        {
            ViewData["GetParentOption"] = GetParentOption(0, 0);
            return View(new ArticleKind() { PID = pid ?? 0 });
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            try
            {
                ViewData["GetParentOption"] = GetParentOption(0, 0);
                ArticleKindRepository ml = new ArticleKindRepository();
                ArticleKind obj = new ArticleKind() { CreateDate = DateTime.Now, CreateUserID = ID, IsDeleted = false };
                UpdateModel(obj);
                bool result = ml.Insert(obj) > 0 ? true : false;
                return result ? Content(ContentIcon.Succeed + "|操作成功|/admin/ArticleKind/Index") : Content(ContentIcon.Error + "|操作失败");
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
                ArticleKindRepository ml = new ArticleKindRepository();
                ArticleKind obj = ml.GetArticleKind(id);
                return View(obj);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            try
            {
                ViewData["GetParentOption"] = GetParentOption(0, 0);

                ArticleKindRepository ml = new ArticleKindRepository();

                var obj = ml.GetArticleKind(id);

                UpdateModel(obj);

                obj.LastUpdateDate = DateTime.Now;

                obj.LastUpdateUserID = ID;

                bool result = ml.Update(obj) > 0 ? true : false;

                return result ? Content(ContentIcon.Succeed + "|操作成功") : Content(ContentIcon.Error + "|操作失败");
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
                ArticleKindRepository ml = new ArticleKindRepository();
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
                ArticleKindRepository ml = new ArticleKindRepository();
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
                ArticleKindRepository ml = new ArticleKindRepository();

                ArticleKind obj = ml.GetArticleKind(id);

                return View(obj);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
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

            List<ArticleKind> klist = new ArticleKindRepository().Search(new ArticleKind { PID = parentID }).GetPagedList(1, 1000, "SortID", true);

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
                    kind = new ArticleKindRepository().GetArticleKind(item.PID ?? 0).Name;
                }
                catch { }
                html.Append(string.Format(option, item.ID, name.ToString()));
                html.Append(GetParentOption(item.ID, level));
                i++;
            }
            return html.ToString();
        }
    }
}


