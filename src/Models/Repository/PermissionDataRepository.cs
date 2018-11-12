using WEF;

namespace WebBaseFrame.Models
{
    public partial class PermissionDataRepository
    {
        /// <summary>
        /// 清除某人或某角色的权限记录
        /// </summary>
        /// <param name="mt"></param>
        /// <param name="type">type=0为角色，否则为用户</param>
        public void DeleteByMIDOrRID(int id, int type)
        {
            string sql = "";
            if (type == 0)
            {
                sql = "DELETE FROM [PermissionData] WHERE [RID]=" + id;
            }
            else
            {
                sql = "DELETE FROM [PermissionData] WHERE [MID]=" + id;
            }
            this.ExecuteSQL(sql);
        }
    }
}
