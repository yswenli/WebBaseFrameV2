using System.Collections.Generic;
using System.Linq;

namespace WebBaseFrame.Models
{
    /// <summary>
    /// 自定义ArticleKind处理类
    /// </summary>
    public partial class ArticleKindRepository
    {
        public List<ArticleKind> GetListByPID(int pid)
        {
            return this.Search().Where(b => b.PID == pid).ToList();
        }

        public bool HasChilds(int id)
        {
            bool hasChilds = false;
            try
            {
                var klts = this.GetListByPID(id);
                if (klts != null && klts.Count > 0) hasChilds = true;
            }
            catch { }
            return hasChilds;
        }

        public List<int> GetListByName(string name)
        {
            try
            {
                return this.ExecuteSQL("select * from [ArticleKind] where [Name] like '%" + name + "%'").ToList<ArticleKind>().Select(b => b.ID).ToList();
            }
            catch { }
            return null;
        }
    }
}
