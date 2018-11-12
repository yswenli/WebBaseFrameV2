using System;
using System.Web.Mvc;
using WebBaseFrame.Models;
using WEF.MvcPager;

namespace Web.Areas.Admin.Controllers
{
    public class CustomPageController : BaseController
    {

        public ActionResult Index(int? pageIndex, int? pageSize)
        {
            try
            {
                CustomPageRepository ml = new CustomPageRepository();
                var entity = new CustomPage();
                entity.IsDeleted = false;
                PagedList<CustomPage> page = ml.Search(entity).GetPagedList(pageIndex ?? PageIndex, pageSize ?? PageSize, Order, By);
                if (Request.IsAjaxRequest())
                    return PartialView("_Index", page);
                return View(page);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }

        public ActionResult Create()
        {
            return View(new CustomPage());
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            try
            {
                CustomPageRepository ml = new CustomPageRepository();

                CustomPage obj = new CustomPage() { CreateDate = DateTime.Now, CreateUserID = ID, IsDeleted = false };

                UpdateModel(obj);

                bool result = ml.Insert(obj) > 0 ? true : false;

                return result ? Content(ContentIcon.Succeed + "|操作成功|/admin/CustomPage/Index") : Content(ContentIcon.Error + "|操作失败");
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
                CustomPageRepository ml = new CustomPageRepository();

                CustomPage obj = ml.GetCustomPage(id);

                return View(obj);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult Edit(CustomPage obj, FormCollection formCollection)
        {
            try
            {
                CustomPageRepository ml = new CustomPageRepository();

                obj.LastUpdateDate = DateTime.Now;

                obj.LastUpdateUserID = ID;

                UpdateModel(obj);

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
                CustomPageRepository ml = new CustomPageRepository();
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
                CustomPageRepository ml = new CustomPageRepository();
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
                CustomPageRepository ml = new CustomPageRepository();

                CustomPage obj = ml.GetCustomPage(id);

                return View(obj);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }
        /// <summary>
        /// LayoutIt布局页面
        /// </summary>
        /// <returns></returns>
        public ActionResult LayoutIt(int? id)
        {
            CustomPage ct = new CustomPage();
            try
            {
                if (id != null && id > 0)
                {
                    ct = new CustomPageRepository().GetCustomPage(id ?? 0);
                }
            }
            catch { }
            return View(ct);
        }

        /// <summary>
        /// 获取保存的内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetContent(int id)
        {
            string EditContent = "";
            try
            {
                if (id > 0)
                {
                    EditContent = new CustomPageRepository().GetCustomPage(id).EditContent;
                }
            }
            catch { }
            return Content(EditContent);
        }
        /// <summary>
        /// 获取全部控制器
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetControllers()
        {
            string option = "<option value=\"\">--请选择--</option>";
            var clts = Common.DirHelper.GetFolders(Server.MapPath("~/") + "Views\\");
            if (clts != null && clts.Count > 0)
            {
                foreach (var item in clts)
                {
                    if (item.ToString() != "Shared")
                    {
                        option += "<option value=\"" + item.ToString() + "\">" + item.ToString() + "</option>";
                    }
                }
            }
            return Content(option);
        }

        //[HttpPost]
        public ActionResult IsExits(int ajaxType, string ajaxController, string ajaxAction)
        {
            var path = Server.MapPath("~/") + "Views\\" + ajaxController + "\\" + ajaxAction + ".cshtml";
            if (ajaxType == 0)
            {
                path = Server.MapPath("~/") + "Views\\Shared\\" + ajaxAction + ".cshtml";
            }
            if (Common.FileHelper.IsFileExists(path))
            {
                return Content("1");
            }
            return Content("0");
        }
        /// <summary>
        /// 生成页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="editcontent"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        //[HttpPost]
        public ActionResult CreateFile(int ajaxId, int ajaxType, string ajaxController, string ajaxAction, string ajaxEditcontent, string ajaxViewContent)
        {
            try
            {
                var path = Server.MapPath("~/") + "Views\\" + ajaxController + "\\" + ajaxAction + ".cshtml";
                if (ajaxType == 0)
                {
                    path = Server.MapPath("~/") + "Views\\Shared\\" + ajaxAction + ".cshtml";
                }
                CustomPage ct = new CustomPage();
                CustomPageRepository cc = new CustomPageRepository();
                if (ajaxId != 0)
                {
                    ct = cc.GetCustomPage(ajaxId);
                    ct.Type = ajaxType;
                    ct.Controller = ajaxController;
                    ct.Action = ajaxAction;
                    ct.Path = path;
                    ct.EditContent = ajaxEditcontent;
                    ct.LastUpdateDate = DateTime.Now;
                    ct.LastUpdateUserID = ID;
                    cc.Update(ct);
                }
                else
                {
                    ct.Type = ajaxType;
                    ct.Controller = ajaxController;
                    ct.Action = ajaxAction;
                    ct.Path = path;
                    ct.EditContent = ajaxEditcontent;
                    ct.CreateDate = DateTime.Now;
                    ct.CreateUserID = ID;
                    ct.LastUpdateDate = DateTime.Now;
                    ct.LastUpdateUserID = ID;
                    ct.IsDeleted = false;
                    cc.Insert(ct);
                }
                string htmlcontent = "@{\r\n    ViewBag.Title = \"" + ajaxAction + "\";\r\n    Layout = \"~/Views/Shared/_Layout.cshtml\";\r\n}";
                htmlcontent += ajaxViewContent;
                Common.FileHelper.WriteFile(path, htmlcontent);
                return Content("1");
            }
            catch { }
            return Content("0");
        }
    }
}


