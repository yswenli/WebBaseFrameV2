using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Libraries;
using Web.Areas.Manager.Helper;
using MvcPager;
using WEF.MvcPager;

namespace Web.Areas.Manager.Controllers
{

    /// <summary>
    /// 模板处理控制器
    /// </summary>
    public class TempleManagerController : BaseController
    {
        //
        // GET: /Manager/TempleManager/


        /// <summary>
        /// 模板列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Index(int? pageIndex, int? pageSize)
        {
            var filePath = Web.Areas.Manager.Helper.PathHelper.Templepath + "temp1\\";
            var fileList = Common.DirHelper.GetFiles(filePath);
            List<TempInfo> tlts = new List<TempInfo>();
            foreach (var item in fileList)
            {
                var tempStr = filePath + item.ToString();
                tempStr = tempStr.Substring(tempStr.LastIndexOf("\\") + 1);
                tempStr = tempStr.Substring(0, tempStr.LastIndexOf("."));
                TempInfo ti = new TempInfo(tempStr, filePath + item.ToString());
                tlts.Add(ti);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Index", tlts.ToPagedList(pageIndex ?? 1, pageSize ?? 10));
            }
            return View(tlts.ToPagedList(pageIndex ?? 1, pageSize ?? 10));
        }


        /// <summary>
        /// 创建模板
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            TempInfo ti = new TempInfo();
            return View(ti);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(TempInfo ti, string content)
        {
            bool result = false;
            try
            {
                if (Common.FileHelper.IsFileExists(ti.Path))
                {
                    Common.FileHelper.DeleteFile(ti.Path);
                }
                Common.FileHelper.WriteFile(ti.Path, content);
                result = true;
                return result ? Content(BaseController.ContentIcon.Succeed + "|操作成功|/Manager/TempleManager/Index") : Content(BaseController.ContentIcon.Error + "|操作失败");
            }
            catch (Exception ex)
            {
                return Content(BaseController.ContentIcon.Error + "|" + ex.Message);
            }
        }

        /// <summary>
        /// 编辑模板
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult Edit(string name)
        {
            TempInfo ti = new TempInfo(name);
            return View(ti);
        }


        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(TempInfo ti, string content)
        {
            bool result = false;
            try
            {
                if (Common.FileHelper.IsFileExists(ti.Path))
                {
                    Common.FileHelper.DeleteFile(ti.Path);
                }
                Common.FileHelper.WriteFile(ti.Path, content);
                result = true;
                return result ? Content(BaseController.ContentIcon.Succeed + "|操作成功|/Manager/TempleManager/Index") : Content(BaseController.ContentIcon.Error + "|操作失败");
            }
            catch (Exception ex)
            {
                return Content(BaseController.ContentIcon.Error + "|" + ex.Message);
            }
        }


        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult Delete(string name)
        {
            string result = "";
            TempInfo ti = new TempInfo(name);
            Common.FileHelper.DeleteFile(ti.Path);
            result = "ok";
            return Content(result);
        }

    }
}
