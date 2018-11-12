using Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebBaseFrame.Models;
using WEF.MvcPager;

namespace Web.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {

        public ActionResult Index(int? pageIndex, int? pageSize, Role entity)
        {
            try
            {
                RoleRepository ml = new RoleRepository();

                PagedList<Role> page = ml.Search(entity).GetPagedList(pageIndex ?? PageIndex, pageSize ?? PageSize, Order, By);

                IList<Role> objs = page;

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
            return View(new Role());
        }

        [HttpPost]
        public ActionResult Create(int[] kids, FormCollection formCollection)
        {
            try
            {
                RoleRepository ml = new RoleRepository();

                Role obj = new Role() { CreateDate = DateTime.Now, CreateUserID = ID, IsDeleted = false };

                UpdateModel(obj);

                ml.Insert(obj);

                int rid = ml.Search(obj).First().ID;

                #region 更新引角色权限
                if (rid > 1)
                {
                    var pdl = new PermissionDataRepository();
                    pdl.DeleteByMIDOrRID(rid, 0);
                    var pmlts = new PermissionMapRepository().GetList(1, 1000);
                    if (pmlts != null)
                    {
                        string pData = formCollection["pData"];
                        foreach (var item in pmlts)
                        {
                            var pdt = new PermissionData()
                            {
                                PID = item.ID,
                                RID = rid,
                                HasPermission = false,
                                CreateUserID = CurrentMember.ID,
                                LastUpdateUserID = CurrentMember.ID,
                                CreateDate = DateTime.Now,
                                LastUpdateDate = DateTime.Now,
                                IsDeleted = false
                            };
                            if (!string.IsNullOrEmpty(pData))
                            {
                                try
                                {
                                    var pDataArr = pData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (pDataArr.Contains(item.ID.ToString()))
                                    {
                                        pdt.HasPermission = true;
                                    }
                                }
                                catch { }
                            }
                            pdl.Insert(pdt);
                        }
                    }
                }
                PermissionHelper permission = new PermissionHelper();
                permission.Write();
                #endregion

                return Content(ContentIcon.Succeed + "|保存成功|/admin/Role/Index");
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
                RoleRepository ml = new RoleRepository();

                Role obj = ml.GetRole(id);

                return View(obj);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, int[] kids, FormCollection formCollection)
        {
            try
            {
                RoleRepository ml = new RoleRepository();

                Role obj = ml.GetRole(id);

                UpdateModel(obj);

                obj.LastUpdateDate = DateTime.Now;

                obj.LastUpdateUserID = ID;

                bool result = ml.Update(obj) > 0 ? true : false;

                int rid = obj.ID;

                #region 更新引角色权限
                if (rid > 1)
                {
                    var pdl = new PermissionDataRepository();
                    pdl.DeleteByMIDOrRID(rid, 0);
                    var pmlts = new PermissionMapRepository().GetList(1,1000);
                    if (pmlts != null)
                    {
                        string pData = formCollection["pData"];
                        foreach (var item in pmlts)
                        {
                            var pdt = new PermissionData()
                            {
                                PID = item.ID,
                                RID = rid,
                                HasPermission = false,
                                CreateUserID = CurrentMember.ID,
                                LastUpdateUserID = CurrentMember.ID,
                                CreateDate = DateTime.Now,
                                LastUpdateDate = DateTime.Now,
                                IsDeleted = false
                            };
                            if (!string.IsNullOrEmpty(pData))
                            {
                                try
                                {
                                    var pDataArr = pData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (pDataArr.Contains(item.ID.ToString()))
                                    {
                                        pdt.HasPermission = true;
                                    }
                                }
                                catch { }
                            }
                            pdl.Insert(pdt);
                        }
                    }
                }
                PermissionHelper permission = new PermissionHelper();
                permission.Write();
                #endregion

                return result ? Content(ContentIcon.Succeed + "|保存成功") : Content(ContentIcon.Error + "|保存失败");
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            RoleRepository ml = new RoleRepository();
            try
            {
                if (id > 0)
                {
                    var mlts = new MemberRepository().Search().Where(b => b.RoleID == id && b.IsDeleted == false).ToList();
                    if (mlts != null && mlts.Count > 0)
                    {
                        return Content(ContentIcon.Error + "|当前角色下含有用户，请先将用户删除");
                    }
                    else
                    {
                        ml.Delete(id);
                        return Content("1");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(collection["IDs"]))
                        return Content("未指定删除对象ID");
                    string[] ids = collection["IDs"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in ids)
                    {
                        ml.Delete(int.Parse(item));
                    }
                    return Content("1");
                }
            }
            catch (Exception ex)
            {
                return Content(ErrorWirter(RouteData, ex.Message));
            }
        }

    }
}


