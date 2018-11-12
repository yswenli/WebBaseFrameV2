using Libraries;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Mvc;
using WebBaseFrame.Models;
using WEF;

namespace Web.Areas.Admin.Controllers
{
    public class AjaxController : Controller
    {
        bool _IsMSSQL = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsMSSQL"]);

        /// <summary>
        /// 导出CSV文件
        /// </summary>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult _CSV(string tableHtml)
        {
            string path = Server.MapPath("~/upload/csvTemp/");
            string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            Common.DirHelper.CheckFolder(path);
            using (FileStream fs = new FileStream(path + fileName, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.WriteLine(tableHtml);
                    sw.Close();
                }
            }
            return Content(fileName);
        }

        /// <summary>
        /// 验证
        /// </summary>
        public ActionResult _Validator(int? id)
        {
            string t = Request.QueryString["t"];
            if (string.IsNullOrEmpty(t))
                return Content("1");
            else
            {
                string r = Request.QueryString["clientid"];
                bool b = validatorExists(t, r, Request.QueryString[r], id);
                return Content(b ? "0" : "1");
            }
        }
        /// <summary>
        /// 验证某条记录是否存在
        /// </summary>
        /// <param name="table"></param>
        /// <param name="row"></param>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool validatorExists(string table, string row, string value, int? id)
        {
            table = Common.Character.ReplaceSqlKey(table, int.MaxValue);
            row = Common.Character.ReplaceSqlKey(row, int.MaxValue);
            value = Common.Character.ReplaceSqlKey(value, int.MaxValue);
            string where = id > 0 ? " and id<>" + id : "";
            string sql = "select count('') from " + table + " where " + row + "='" + value + "'" + where;
            return Convert.ToInt32(new DBContext().ExecuteScalar(sql)) == 0 ? false : true;
        }
        /// <summary>
        /// 自动完成
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public ActionResult autoComplete(string words, string table, string field)
        {
            string result = "";
            if (!string.IsNullOrEmpty(words))
            {
                words = words.Replace("'", "").Replace("%", "").Replace("*", "").Replace(" ", "");
            }
            try
            {
                string sql = "select top 10 [" + field + "] from [" + table + "] where [" + field + "] like '%" + words + "%'";
                if (_IsMSSQL)
                {
                    sql = "select top 10 [" + field + "] from [" + table + "] where [" + field + "] like '%" + words + "%'";
                }
                else
                {
                    sql = "select [" + field + "] from [" + table + "] where [" + field + "] like '%" + words + "%' limit 0,10";
                }
                var dt = new DBContext().ExecuteDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        result += "<div style='cursor:pointer;height:25px;line-height:25px;margin:3px;background:#aaa;padding-left:12px;overflow:hidden;'>" + (item[0] == null ? "" : item[0]) + "</div>";
                    }
                }
            }
            catch { }
            return Content(result);
        }
        /// <summary>
        /// 验证随机码
        /// </summary>
        /// <returns></returns>
        public ActionResult verifyCode()
        {
            return File(Common.VerifyCodeHelper.GetImageStream().ToArray(), "image/png");
        }
        /// <summary>
        /// 按钮是否显示
        /// </summary>
        /// <param name="titleStr"></param>
        /// <param name="urlStr"></param>
        /// <returns></returns>
        public ActionResult HasPermission(string titleStr, string urlStr)
        {
            string result = "";
            var titleArray = titleStr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var urlArray = urlStr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < titleArray.Length; i++)
            {
                if (PermissionHelper.IsPast(titleArray[i], urlArray[i]) == true)
                {
                    result += "1,";
                }
                else
                {
                    result += "0,";
                }
            }
            result = result.Substring(0, result.Length - 1);
            return Content(result);
        }
        /// <summary>
        /// 对Content目录是否有操作权限
        /// </summary>
        /// <returns></returns>
        public ActionResult HasOperatePermission()
        {
            string result = "no";
            try
            {
                var path = Server.MapPath("/Content/");
                if (Common.DirHelper.HasOperatePermission(path)) result = "yes";
            }
            catch { }
            return Content(result);

        }

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProvinces()
        {
            string result = "<option value=\"\">请选择</option>";
            AreaRepository al = new AreaRepository();
            var alts = al.Search().Where(b => b.Depth == 0 && b.IsDeleted == false).ToList();
            if (alts != null)
            {
                foreach (var item in alts)
                {
                    result += "<option value=\"" + item.ID + "\">" + item.CName + "</option>";
                }
            }
            return Content(result);
        }
        /// <summary>
        /// 根据省份获取城市
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCitys(int? province)
        {
            string result = "<option value=\"\">请选择</option>";
            if (province != null && province > 0)
            {
                AreaRepository al = new AreaRepository();

                var alts = al.Search().Where(b => b.ParentID == province && b.Depth == 1 && b.IsDeleted == false).ToList();

                if (alts != null)
                {
                    foreach (var item in alts)
                    {
                        result += "<option value=\"" + item.ID + "\">" + item.CName + "</option>";
                    }
                }
            }
            return Content(result);
        }


        /// <summary>
        /// 用户是否已存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public ActionResult IsExist(string username)
        {
            string result = "1";
            var member = new MemberRepository().Search().Where(b => b.UserName == username && b.IsDeleted == false).FirstDefault();
            if (member != null && member.ID > 0)
            {
                result = "0";
            }
            return Content(result);
        }
    }
}
