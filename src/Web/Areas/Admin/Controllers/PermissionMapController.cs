using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WebBaseFrame.Models;
using WEF.MvcPager;

namespace Web.Areas.Admin.Controllers
{
    public class PermissionMapController : BaseController
    {

        public ActionResult Index(int? pageIndex, int? pageSize, PermissionMap entity)
        {
            try
            {
                PermissionMapRepository ml = new PermissionMapRepository();
                entity.IsDeleted = false;
                PagedList<PermissionMap> page = ml.Search(entity).GetPagedList(pageIndex ?? PageIndex, pageSize ?? PageSize, Order, By);
                if (page != null && page.TotalItemCount > 0)
                {
                    foreach (var item in page)
                    {
                        if (item.Menu.ID == 0)
                        {
                            PermissionDataRepository pl = new PermissionDataRepository();
                            var plts = pl.Search().Where(b => b.PID == item.ID).ToList();
                            if (plts != null)
                            {
                                pl.Deletes(plts);
                            }
                            ml.Delete(item);
                        }
                    }
                    page = ml.Search(entity).GetPagedList(pageIndex ?? PageIndex, pageSize ?? PageSize, Order, By);
                }
                IList<PermissionMap> objs = page;

                if (Request.IsAjaxRequest())

                    return PartialView("_Index", objs);

                return View(objs);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }

        public ActionResult Create()
        {
            ViewData["ForeachMenuByOption"] = ForeachMenuByOption(0, 0);
            ViewData["ForeachPermissionByOption"] = ForeachPermissionByOption(0, 0, 1);
            return View(new PermissionMap());
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            try
            {
                PermissionMapRepository ml = new PermissionMapRepository();

                PermissionMap obj = new PermissionMap() { CreateDate = DateTime.Now, CreateUserID = ID, IsDeleted = false };

                UpdateModel(obj);

                bool result = ml.Insert(obj) > 0 ? true : false;

                return result ? Content(ContentIcon.Succeed + "|操作成功|/Admin/PermissionMap/Index") : Content(ContentIcon.Error + "|操作失败");
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
                PermissionMapRepository ml = new PermissionMapRepository();

                PermissionMap obj = ml.GetPermissionMap(id);

                ViewData["ForeachMenuByOption"] = ForeachMenuByOption(obj.MID, 0);
                ViewData["ForeachPermissionByOption"] = ForeachPermissionByOption(obj.PID, 0, 1);

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
                PermissionMapRepository ml = new PermissionMapRepository();

                PermissionMap obj = ml.GetPermissionMap(id);

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

        [HttpPost]
        public ActionResult Delete(int? id, FormCollection collection)
        {
            try
            {
                PermissionMapRepository ml = new PermissionMapRepository();
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
                PermissionMapRepository ml = new PermissionMapRepository();

                PermissionMap obj = ml.GetPermissionMap(id);

                return View(obj);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }


        private string ForeachMenuByOption(int? id, int? parentID)
        {
            StringBuilder html = new StringBuilder();
            List<Menu> menulist = new MenuRepository().Search().Where(b => b.ParentID == parentID).ToList();
            var i = 0;
            foreach (var item in menulist)
            {
                string selected = "";
                if (id == item.ID)
                    selected = "selected";
                if (item.ParentID == 0 || item.ParentID.ToString() == "")
                    html.Append("<option value='" + item.ID + "' " + selected + "> " + item.Name + " </option>");
                else
                {
                    StringBuilder nbsp = new StringBuilder();
                    for (int j = 0; j < item.Level * 2; j++)
                    {
                        nbsp.Append("&nbsp;");
                    }
                    if (i == menulist.Count() - 1)
                    {
                        html.Append("<option value='" + item.ID + "' " + selected + "> " + nbsp.ToString() + "└ " + item.Name + " </option>");
                    }
                    else
                    {
                        html.Append("<option value='" + item.ID + "' " + selected + "> " + nbsp.ToString() + "├ " + item.Name + " </option>");
                    }
                }
                i++;
                html.Append(ForeachMenuByOption(id, item.ID));
            }
            return html.ToString();
        }

        private string ForeachPermissionByOption(int? id, int? parentID, int level)
        {
            StringBuilder html = new StringBuilder();
            List<PermissionMap> menulist = new PermissionMapRepository().Search().Where(b => b.PID == parentID).ToList();
            var i = 0;
            foreach (var item in menulist)
            {
                string selected = "";
                if (id == item.ID)
                    selected = "selected";
                if (item.PID == 0 || item.PID == null)
                    html.Append("<option value='" + item.ID + "' " + selected + "> " + item.Name + " </option>");
                else
                {
                    StringBuilder nbsp = new StringBuilder();
                    for (int j = 0; j < level * 2; j++)
                    {
                        nbsp.Append("&nbsp;");
                    }
                    if (i == menulist.Count() - 1)
                    {
                        html.Append("<option value='" + item.ID + "' " + selected + "> " + nbsp.ToString() + "└ " + item.Name + " </option>");
                    }
                    else
                    {
                        html.Append("<option value='" + item.ID + "' " + selected + "> " + nbsp.ToString() + "├ " + item.Name + " </option>");
                    }
                }
                i++;
                html.Append(ForeachPermissionByOption(id, item.ID, level + 1));
            }
            return html.ToString();
        }


        #region 树控件
        /// <summary>
        /// 树控件
        /// </summary>
        public ActionResult _Treeview(int ID, int Type)
        {
            ViewData["ID"] = ID;
            ViewData["Type"] = Type;
            return PartialView();
        }
        /// <summary>
        /// ajax获取权限结构
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 10, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult AjaxGetMap()
        {
            return Content(GetMap());
        }
        #region 权限结构具体方法
        List<Menu> mlts = new List<Menu>();
        List<PermissionMap> plts = new List<PermissionMap>();
        protected string GetMap()
        {
            string permissionTree = "";
            MenuRepository ml = new MenuRepository();
            mlts = ml.Search().Where(b => b.IsDeleted == false).ToList();
            PermissionMapRepository pml = new PermissionMapRepository();
            plts = pml.GetList(1, 1000);
            if (mlts != null)
            {
                permissionTree = GetMenuStr(new Menu() { ID = 0 });
            }
            return permissionTree;
        }
        /// <summary>
        /// 获取权限菜单的结构
        /// </summary>
        /// <param name="mt"></param>
        /// <returns></returns>
        public string GetMenuStr(Menu mt)
        {
            string str = "";
            var ms = mlts.Where(b => b.ParentID == mt.ID).OrderBy(b => b.Sort);
            if (ms != null)
            {
                foreach (var item in ms)
                {
                    str += "<li><label>" + item.Name + "</label>";
                    //递归子项
                    var childStr = GetMenuStr(item);
                    //权限结构
                    string ptStr = string.Empty;
                    var ps = plts.Where(b => b.MID == item.ID).OrderBy(b => b.SortID).ToList();
                    //菜单中有子项的
                    if (!string.IsNullOrEmpty(childStr))
                    {
                        str += "<ul>" + childStr;
                        ptStr = "";
                        if (ps != null && ps.Count > 0)
                        {
                            foreach (var sitem in ps)
                            {
                                ptStr += "<li><label><input type='checkbox' name='pData' value='" + sitem.ID + "' />" + sitem.Name + "</label>";
                                var sptStr = GetMapStr(new PermissionMap() { MID = item.ID });
                                //权限结构中有子项的
                                if (string.IsNullOrEmpty(sptStr))
                                {
                                    ptStr += sptStr + "</li>";
                                }
                                //不含子项的
                                else
                                {
                                    ptStr += "</li>";
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(ptStr))
                        {
                            str += "</ul></li>";
                        }
                        else
                        {
                            str += ptStr + "</ul></li>";
                        }
                    }
                    //菜单中不包含子项的
                    else
                    {
                        ptStr = "";
                        if (ps != null && ps.Count > 0)
                        {
                            ptStr += "<ul>";
                            foreach (var sitem in ps)
                            {
                                ptStr += "<li><label><input type='checkbox' name='pData' value='" + sitem.ID + "' />" + sitem.Name + "</label>";
                                var sptStr = GetMapStr(new PermissionMap() { MID = item.ID });
                                //权限结构中有子项的
                                if (string.IsNullOrEmpty(sptStr))
                                {
                                    ptStr += sptStr + "</li>";
                                }
                                //不含子项的
                                else
                                {
                                    ptStr += "</li>";
                                }
                            }
                            ptStr += "</ul>";
                        }
                        if (string.IsNullOrEmpty(ptStr))
                        {
                            str += "</li>";
                        }
                        else
                        {
                            str += ptStr + "</li>";
                        }
                    }
                }
            }
            return str;
        }
        public string GetMapStr(PermissionMap pt)
        {
            string str = "";
            var ps = plts.Where(b => b.PID == pt.ID).OrderBy(b => b.SortID);
            if (ps != null)
            {
                str = "<ul>";
                foreach (var item in ps)
                {
                    str += "<li><label><input type='checkbox' name='p" + item.ID + "' pid='p" + item.PID + "' />" + item.Name + "</label>";
                    var childStr = GetMapStr(item);
                    if (!string.IsNullOrEmpty(childStr))
                    {
                        str += "<ul>" + childStr + "</ul></li>";
                    }
                    else
                    {
                        str += "</li>";
                    }
                }
                str += "</ul>";
            }
            return str;
        }
        #endregion
        //
        #endregion
    }
}


