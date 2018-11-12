//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WebBaseFrame.Models;
using WEF;

namespace Libraries
{
    /// <summary>
    /// 权限文件操作类
    /// </summary>
    public class PermissionHelper
    {
        private List<Menu> _Menus;
        private List<PermissionMap> _PermissionMaps;
        private List<PermissionData> _PermissionDatas;
        private string password = "web";

        //
        private string baseFilePath = System.Web.HttpContext.Current.Server.MapPath("/Content/Permission/");
        private string menuFilePath;
        private string permissionMapFilePath;
        private string permissionDataFilePath;

        //
        private static Site site = SiteHelper.Default;

        public List<Menu> Menus
        {
            get { return _Menus; }
        }
        public List<PermissionMap> PermissionMaps
        {
            get { return _PermissionMaps; }
        }
        public List<PermissionData> PermissionDatas
        {
            get { return _PermissionDatas; }
        }



        /// <summary>
        /// 构造函数
        /// </summary>
        public PermissionHelper()
        {
            Read();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            menuFilePath = baseFilePath + @"Menus\";
            permissionMapFilePath = baseFilePath + @"PermissionMaps\";
            permissionDataFilePath = baseFilePath + @"PermissionDatas\";
            if (site.IsFromFile == true)
            {
                Common.DirHelper.CheckFolder(baseFilePath);
                Common.DirHelper.CheckFolder(menuFilePath);
                Common.DirHelper.CheckFolder(permissionMapFilePath);
                Common.DirHelper.CheckFolder(permissionDataFilePath);
            }
        }
        /// <summary>
        /// 更新文件（非阻塞方式）
        /// </summary>
        /// <returns></returns>
        public bool Write()
        {
            bool result = false;
            if (site.IsFromFile == true)
            {
                var application = System.Web.HttpContext.Current.Application;
                application.Lock();
                Init();
                try
                {
                    Thread mt = new Thread(new ThreadStart(() =>
                    {
                        var mlts = new MenuRepository().Search().Where(b => b.IsDeleted == false).ToList();
                        if (mlts != null)
                        {
                            _Menus = mlts;
                            string menusStr = Common.JsonHelper.JsonSerializer<List<Menu>>(mlts);
                            menusStr = Common.EncryptionHelper.AESHelper.Encrypt(menusStr, password);
                            string menuFile = menuFilePath + "Menu.html";
                            Common.FileHelper.DeleteFile(menuFile);
                            Common.FileHelper.WriteFile(menuFile, menusStr);
                        }
                    }));
                    Thread pmt = new Thread(new ThreadStart(() =>
                    {
                        var pmlts = new PermissionMapRepository().Search().Where(b => b.IsDeleted == false).ToList();
                        if (pmlts != null)
                        {
                            _PermissionMaps = pmlts;
                            string permissionmapsStr = Common.JsonHelper.JsonSerializer<List<PermissionMap>>(pmlts);
                            permissionmapsStr = Common.EncryptionHelper.AESHelper.Encrypt(permissionmapsStr, password);
                            string permissionmapFile = permissionMapFilePath + "PermissionMap.html";
                            Common.FileHelper.DeleteFile(permissionmapFile);
                            Common.FileHelper.WriteFile(permissionmapFile, permissionmapsStr);
                        }
                    }));
                    Thread pdt = new Thread(new ThreadStart(() =>
                    {
                        var pdlts = new PermissionDataRepository().Search().Where(b => b.IsDeleted == false).ToList();
                        if (pdlts != null)
                        {
                            _PermissionDatas = pdlts;
                            string permissiondatasStr = Common.JsonHelper.JsonSerializer<List<PermissionData>>(pdlts);
                            permissiondatasStr = Common.EncryptionHelper.AESHelper.Encrypt(permissiondatasStr, password);
                            string permissiondataFile = permissionDataFilePath + "PermissionData.html";
                            Common.FileHelper.DeleteFile(permissiondataFile);
                            Common.FileHelper.WriteFile(permissiondataFile, permissiondatasStr);
                        }
                    }));
                    mt.Start();
                    pmt.Start();
                    pdt.Start();
                    while (true)
                    {
                        if (mt.ThreadState == ThreadState.Stopped && pmt.ThreadState == ThreadState.Stopped && pdt.ThreadState == ThreadState.Stopped)
                        {
                            break;
                        }
                        System.Threading.Thread.Sleep(100);
                    }
                    result = true;
                }
                catch { }
                application.UnLock();
            }
            return result;
        }
        /// <summary>
        /// 读取权限内容
        /// </summary>
        private void Read()
        {
            Init();
            _Menus = new List<Menu>();
            _PermissionMaps = new List<PermissionMap>();
            _PermissionDatas = new List<PermissionData>();
            string baseUrl = Common.UrlHelper.HostUrl + "/Content/Permission/";
            Thread mt = new Thread(new ThreadStart(() =>
            {
                bool hasFile = false;
                if (site.IsFromFile == true)
                {
                    try
                    {
                        string menuFile = baseUrl + "Menus/" + "Menu.html";
                        string menusStr = Common.HttpHelper.GET(menuFile).ToString();
                        menusStr = Common.EncryptionHelper.AESHelper.Decrypt(menusStr, password);
                        _Menus = Common.JsonHelper.JsonDeserialize<List<Menu>>(menusStr);
                        hasFile = true;
                    }
                    catch
                    {
                        try
                        {
                            Write();
                        }
                        catch { }
                    }
                }
                if (!hasFile)
                {
                    try
                    {
                        _Menus = new MenuRepository().Search().Where(b => b.IsDeleted == false).ToList();
                    }
                    catch { }
                }
            }));
            Thread pmt = new Thread(new ThreadStart(() =>
            {
                bool hasFile = false;
                if (site.IsFromFile == true)
                {
                    try
                    {
                        string permissionmapFile = baseUrl + "PermissionMaps/" + "PermissionMap.html";
                        string permissionmapStr = Common.HttpHelper.GET(permissionmapFile).ToString();
                        permissionmapStr = Common.EncryptionHelper.AESHelper.Decrypt(permissionmapStr, password);
                        _PermissionMaps = Common.JsonHelper.JsonDeserialize<List<PermissionMap>>(permissionmapStr);
                        hasFile = true;
                    }
                    catch
                    {
                        try
                        {
                            Write();
                        }
                        catch { }
                    }
                }
                if (!hasFile)
                {
                    try
                    {
                        _PermissionMaps = new PermissionMapRepository().Search().Where(b => b.IsDeleted == false).ToList();
                    }
                    catch { }
                }
            }));
            Thread pdt = new Thread(new ThreadStart(() =>
            {
                bool hasFile = false;
                if (site.IsFromFile == true)
                {
                    try
                    {
                        string permissionDataFile = baseUrl + "PermissionDatas/" + "PermissionData.html";
                        string permissionDatasStr = Common.HttpHelper.GET(permissionDataFile).ToString();
                        permissionDatasStr = Common.EncryptionHelper.AESHelper.Decrypt(permissionDatasStr, password);
                        _PermissionDatas = Common.JsonHelper.JsonDeserialize<List<PermissionData>>(permissionDatasStr);
                        hasFile = true;
                    }
                    catch
                    {
                        try
                        {
                            Write();
                        }
                        catch { }
                    }
                }
                if (!hasFile)
                {
                    try
                    {
                        _PermissionDatas = new PermissionDataRepository().Search().Where(b => b.IsDeleted == false).ToList();
                    }
                    catch { }
                }
            }));
            mt.Start();
            pmt.Start();
            pdt.Start();
            while (true)
            {
                if (mt.ThreadState == ThreadState.Stopped && pmt.ThreadState == ThreadState.Stopped && pdt.ThreadState == ThreadState.Stopped)
                {
                    break;
                }
                System.Threading.Thread.Sleep(100);
            }
        }


        #region 验证地址权限
        /// <summary>
        /// 验证当前地址权限
        /// </summary>
        /// <param name="privilegeValue">/admin/article/index</param>
        /// <returns></returns>
        public static bool IsPast(string title, string url)
        {
            if (site.IsValidePermission == true)
            {
                //
                //开发者开发时使用此代码,后继发布后因性能优化需要屏蔽下行代码
                //
                AutoGenerateMenuAndMap(title, url);
                //
                PermissionHelper permission = new PermissionHelper();
                try
                {
                    if (CurrentMember.ID > 0 && CurrentMember.RoleID == 1) return true;
                    //菜单查询
                    int menuID = 0;
                    try
                    {
                        var menuUrl = url.Substring(0, url.LastIndexOf("/") + 1) + "Index";
                        if (permission.Menus != null)
                        {
                            foreach (var item in permission.Menus)
                            {
                                if (!string.IsNullOrEmpty(item.Url) && item.Url.ToUpper() == menuUrl.ToUpper())
                                {
                                    menuID = item.ID;
                                }
                            }
                        }
                    }
                    catch
                    {
                        menuID = 0;
                    }
                    if (menuID == 0) return true;
                    //查询权限配置表，若有明确记录不允许访问，则返回false
                    var pmt = new PermissionMap();
                    try
                    {
                        pmt = permission.PermissionMaps.Where(b => b.MID == menuID && b.Name == title).First();

                    }
                    catch { return true; }
                    var plts = permission.PermissionDatas.Where(b => b.RID == CurrentMember.RoleID && b.PID == pmt.ID);
                    if (plts != null)
                    {
                        foreach (var item in plts)
                        {
                            if (item.HasPermission == false) return false;
                        }
                    }
                }
                catch { }
            }
            return true;
        }

        /// <summary>
        /// 自动生成菜单及权限结构
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private static void AutoGenerateMenuAndMap(string title, string url)
        {
            DBContext db = new DBContext();
            //分析当前地址
            url = url.Substring(0, url.LastIndexOf("/") + 1) + "Index";
            int menuID = 0;
            //寻找菜单
            try
            {
                menuID = db.FromSql("SELECT * FROM [Menu] AS m WHERE [Url] LIKE '" + url.ToLower() + "'").ToFirst<Menu>().ID;
                if (menuID < 1) throw new Exception("菜单中不存在则新建");
            }
            catch
            {
                try
                {
                    //若当前数据库中不存在该菜单，则自动创建一个菜单
                    var mt = new Menu()
                    {
                        Name = title,
                        ParentID = 0,
                        Icon = "icos-list-images",
                        Url = url,
                        Sort = 0,
                        Level = 1,
                        IsDeleted = false,
                        LastUpdateDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateUserID = CurrentMember.ID,
                        LastUpdateUserID = CurrentMember.ID
                    };
                    MenuRepository ml = new MenuRepository();
                    menuID = ml.Insert(mt);
                }
                catch { }
            }
            //权限结构
            var pmt = new PermissionMap();
            bool hasMap = false;
            try
            {
                pmt = db.FromSql("SELECT * FROM [PermissionMap] AS pm WHERE [MID]=" + menuID + " AND [Name]='" + title + "'").ToFirst<PermissionMap>();
                if (pmt != null && pmt.ID > 0) hasMap = true;
            }
            catch { }
            if (hasMap == false)
            {
                //自动产生权限结构
                pmt = new PermissionMap()
                {
                    SortID = GetSort(title),
                    MID = menuID,
                    Name = title,
                    Description = title,
                    IsBasic = 0,
                    CreateUserID = CurrentMember.ID,
                    CreateDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    LastUpdateUserID = CurrentMember.ID,
                    IsDeleted = false
                };
                var pml = new PermissionMapRepository();
                pml.Insert(pmt);
            }
        }

        /// <summary>
        /// 默认生成菜单的排序
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        private static int GetSort(string title)
        {
            int sort = 0;
            switch (title)
            {
                case "菜单":
                    sort = 0;
                    break;
                case "创建新记录":
                    sort = 1;
                    break;
                case "编辑":
                    sort = 2;
                    break;
                case "删除":
                    sort = 3;
                    break;
                case "添加子栏目":
                    sort = 4;
                    break;
                case "查看":
                    sort = 5;
                    break;
                default:
                    sort = 6;
                    break;
            }
            return sort;
        }
        #endregion
    }
}