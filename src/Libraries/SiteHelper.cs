using System.Linq;
using WebBaseFrame.Models;

namespace Libraries
{
    public class SiteHelper
    {
        public static Site Default
        {
            get
            {
                return new SiteRepository().Search().First();
            }
        }
    }
}