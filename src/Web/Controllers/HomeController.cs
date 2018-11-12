using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            //Common.ExcelHelper excel = new Common.ExcelHelper();
            //excel.AddSheet<Models.Member>("Members", new Models.MemberRepository().GetMembers());
            //excel.AddSheet<Models.Member>("Members", new DBContext().ExecuteDataTableSql("select * from [Member]"));
            //excel.Export("");
            return View();
        }


        /// <summary>
        /// 示例
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration=int.MaxValue,Location=System.Web.UI.OutputCacheLocation.Any,NoStore=false)]
        public ActionResult Demo()
        {
            return View();
        }
    }
}
