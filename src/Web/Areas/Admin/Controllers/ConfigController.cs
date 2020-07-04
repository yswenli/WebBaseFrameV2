using Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebBaseFrame.Models;

namespace Web.Areas.Admin.Controllers
{
    public class ConfigController : BaseController
    {
        #region 站点配置
        public ActionResult Site()
        {
            try
            {
                SiteRepository ml = new SiteRepository();

                List<Site> list = ml.GetList(1, 20);
                Site obj = (list.Count > 0 ? list.First() : new Site());
                return View(obj);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Site(FormCollection formCollection)
        {
            try
            {
                bool result = false;

                int.TryParse(formCollection["id"], out int id);

                SiteRepository ml = new SiteRepository();

                var site= ml.Search().Where(b=>b.ID== id).First();

                if (site!=null)
                {
                    site.LastUpdateDate = DateTime.Now;

                    site.LastUpdateUserID = ID;

                    UpdateModel(site);

                    if (site.IsValidePermission == null || site.IsValidePermission == false)
                    {
                        site.IsFromFile = null;
                    }
                    result = ml.Update(site) > 0 ? true : false;
                }
                else
                {
                    site = new Site() { CreateDate = DateTime.Now, CreateUserID = ID, IsDeleted = false };

                    UpdateModel(site);

                    result = ml.Insert(site) > 0 ? true : false;
                }

                return result ? Content(ContentIcon.Succeed + "|保存成功") : Content(ContentIcon.Error + "|保存失败");
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }


        /// <summary>
        /// 设置开发人员选项开关
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetIsDeleloper(bool val)
        {
            string result = "";
            try
            {
                var site = SiteHelper.Default;
                site.IsDeleloper = val;
                SiteRepository sl = new SiteRepository();
                sl.Update(site);
                result = "ok";
            }
            catch { }
            return Content(result);
        }
        #endregion

        #region 邮箱配置

        public ActionResult Mail()
        {
            try
            {
                MailSettingRepository ml = new MailSettingRepository();

                List<MailSetting> list = ml.GetList(1, 20);
                MailSetting obj = (list.Count > 0 ? list.First() : new MailSetting());
                return View(obj);
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Mail(FormCollection formCollection)
        {
            try
            {
                bool result = false;
                MailSettingRepository ml = new MailSettingRepository();
                List<MailSetting> list = ml.GetList(1, 20);
                MailSetting obj = new MailSetting();
                if (list.Count > 0)
                {
                    obj.LastUpdateDate = DateTime.Now;

                    obj.LastUpdateUserID = ID;

                    UpdateModel(obj);

                    result = ml.Update(obj) > 0 ? true : false;
                }
                else
                {
                    obj = new MailSetting() { CreateDate = DateTime.Now, CreateUserID = ID, IsDeleted = false };

                    UpdateModel(obj);

                    result = ml.Insert(obj) > 0 ? true : false;
                }

                return result ? Content(ContentIcon.Succeed + "|保存成功") : Content(ContentIcon.Error + "|保存失败");
            }
            catch (Exception ex)
            {
                return Content(ContentIcon.Error + "|" + ErrorWirter(RouteData, ex.Message));
            }
        }
        #endregion
    }
}
