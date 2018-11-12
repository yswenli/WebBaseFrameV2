using System;
using System.Web.Mvc;
using WebBaseFrame.Models;
using WEF.MvcPager;

namespace Web.Areas.Admin.Controllers
{
    public class MemberController : BaseController
    {

        public ActionResult Index(int? pageIndex, int? pageSize, Member entity)
        {
            try
            {
                MemberRepository ml = new MemberRepository();

                PagedList<Member> page = ml.Search(entity).GetPagedList(pageIndex ?? PageIndex, pageSize ?? PageSize, Order, By);

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
            return View(new Member());
        }

        [HttpPost]
        public ActionResult Create(int[] kids, FormCollection formCollection)
        {
            try
            {
                MemberRepository ml = new MemberRepository();

                Member obj = new Member() { CreateDate = DateTime.Now, CreateUserID = ID, IsDeleted = false };

                UpdateModel(obj);
                obj.Password = Common.Character.EncrytPassword(obj.PwdNotMD5);
                bool result = false;
                result = ml.Insert(obj) > 0 ? true : false;
                return result ? Content(ContentIcon.Succeed + "|保存成功|/admin/Member/Index") : Content(ContentIcon.Error + "|保存失败");
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
                MemberRepository ml = new MemberRepository();

                Member obj = ml.GetMember(id);

                return View(obj);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult Edit(int[] kids, int id, FormCollection formCollection)
        {
            try
            {
                MemberRepository ml = new MemberRepository();

                Member obj = ml.GetMember(id);

                UpdateModel(obj);

                obj.LastUpdateDate = DateTime.Now;

                obj.LastUpdateUserID = ID;

                obj.Password = Common.Character.EncrytPassword(obj.PwdNotMD5);

                bool result = ml.Update(obj) > 0 ? true : false;

                int mid = obj.ID;

                return result ? Content(ContentIcon.Succeed + "|保存成功") : Content(ContentIcon.Error + "|保存失败");
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
                MemberRepository ml = new MemberRepository();
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
                MemberRepository ml = new MemberRepository();
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
    }
}


