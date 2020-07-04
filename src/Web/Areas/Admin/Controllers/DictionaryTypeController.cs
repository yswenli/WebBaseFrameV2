using System;
using System.Web.Mvc;
using WebBaseFrame.Models;
using WEF;
using WEF.Expressions;
using WEF.MvcPager;

namespace Web.Areas.Admin.Controllers
{
    public class DictionaryTypeController : BaseController
    {
        public ActionResult Index(int? pageIndex, int? pageSize, DictionaryType entity)
        {
            try
            {
                DictionaryTypeRepository ml = new DictionaryTypeRepository();

                var where = new Where<DictionaryType>();

                where.And(b => b.ID == entity.ID);

                PagedList<DictionaryType> page = ml.Search().Where(where).GetPagedList(pageIndex ?? PageIndex, pageSize ?? PageSize, Order, By);

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
            return View(new DictionaryType());
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            try
            {
                DictionaryTypeRepository ml = new DictionaryTypeRepository();

                DictionaryType obj = new DictionaryType() { CreateDate = DateTime.Now, CreateUserID = ID, IsDeleted = false };

                UpdateModel(obj);

                bool result = ml.Insert(obj) > 0 ? true : false;

                return result ? Content(ContentIcon.Succeed + "|操作成功|/Admin/DictionaryType/Index") : Content(ContentIcon.Error + "|操作失败");
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
                DictionaryTypeRepository ml = new DictionaryTypeRepository();

                DictionaryType obj = ml.Search().Where(b => b.ID == id).First();

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
                DictionaryTypeRepository ml = new DictionaryTypeRepository();

                DictionaryType obj = ml.Search().Where(b => b.ID == id).First();

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
                DictionaryTypeRepository ml = new DictionaryTypeRepository();

                if (id != null && id > 0)
                {
                    var de = ml.Search().Where(b => b.ID == id).First();

                    ml.Delete(de);
                }
                else
                {
                    if (string.IsNullOrEmpty(collection["IDs"]))
                        return Content("未指定删除对象ID");
                    string[] ids = collection["IDs"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in ids)
                    {
                        var entity = ml.Search().Where(b => b.ID == int.Parse(item)).First();

                        ml.Delete(entity);
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
                DictionaryTypeRepository ml = new DictionaryTypeRepository();

                DictionaryType obj = ml.Search().Where(b => b.ID == id).First();

                return View(obj);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }
    }
}


