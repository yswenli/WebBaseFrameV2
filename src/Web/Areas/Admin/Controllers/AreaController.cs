using System;
using System.Web.Mvc;
using WebBaseFrame.Models;
using WEF.MvcPager;

namespace Web.Areas.Admin.Controllers
{
    public class AreaController : BaseController
    {

        public ActionResult Index(int? pageIndex, int? pageSize, Area entity)
        {
            try
            {
                AreaRepository ml = new AreaRepository();
                entity.IsDeleted = false;
                PagedList<Area> page = ml.Search(entity).GetPagedList(pageIndex ?? PageIndex, pageSize ?? PageSize, Order, By);
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
            return View(new Area());
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            try
            {
                AreaRepository ml = new AreaRepository();

                Area obj = new Area() { CreateDate = DateTime.Now, CreateUserID = ID, IsDeleted = false };

                UpdateModel(obj);

                bool result = ml.Insert(obj) > 0 ? true : false;

                return result ? Content(ContentIcon.Succeed + "|操作成功|/Admin/Area/Index") : Content(ContentIcon.Error + "|操作失败");
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
                AreaRepository ml = new AreaRepository();

                Area obj = ml.GetArea(id);

                return View(obj);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult Edit(Area obj, FormCollection formCollection)
        {
            try
            {
                AreaRepository ml = new AreaRepository();

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
                AreaRepository ml = new AreaRepository();
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
                AreaRepository ml = new AreaRepository();

                Area obj = ml.GetArea(id);

                return View(obj);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }
    }
}


