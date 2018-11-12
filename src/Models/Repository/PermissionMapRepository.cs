using System;

namespace WebBaseFrame.Models
{
    public partial class PermissionMapRepository
    {
        /// <summary>
        /// 添加菜单权限
        /// </summary>
        /// <param name="menuID"></param>
        public static void AddMenuPermissionMap(int menuID)
        {
            try
            {
                var mt = new PermissionMapRepository().Search().Where(b => b.MID == menuID && b.Name == "菜单").First();
                if (mt == null || mt.ID == 0)
                {
                    var pml = new PermissionMapRepository();
                    var pmt = new PermissionMap()
                    {
                        SortID = 0,
                        MID = menuID,
                        Name = "菜单",
                        Description = "菜单",
                        IsBasic = 0,
                        CreateUserID = 2,
                        LastUpdateUserID = 2,
                        CreateDate = DateTime.Now,
                        LastUpdateDate = DateTime.Now,
                        IsDeleted = false
                    };
                    pml.Insert(pmt);
                }

            }
            catch { }
        }

    }
}
