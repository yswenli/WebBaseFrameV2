
using Newtonsoft.Json;
using System.Net;
using System.Text;
using WeiXinAPIs.Models;
//
using WeiXinAPIs.Libs;

namespace WeiXinAPIs
{
    public class MenuAPI : BaseAPI
    {
        /// <summary>
        /// 这是一个菜单封装实例
        /// </summary>
        /// <returns></returns>
        public MenuList MenuJsonBuilder()
        {
            MenuList menuList = new MenuList();

            menuList.Menu.Buttons = new MenuButton[3];
            var megagame = new MenuButton
            {
                Type = MenuButtonType.Click,
                Name = "大赛资讯",
                Key = "EVENT_ACTIVE_MEGAGAME"
            };
            megagame.SubButton = new MenuButton[3]
                {
                    new MenuButton{Type = MenuButtonType.View,Name = "活动规则",Url = "http://njc.web1.com/rule/index?OpenID=_OpenID_"},
                    new MenuButton {Type = MenuButtonType.View, Name = "大赛视频", Url = "http://mp.weixin.qq.com/s?__biz=MzA4MzUzNDQyMQ==&mid=200455372&idx=1&sn=58276aee59980585e62cf2d68dd90704#rd"},
                    new MenuButton{Type = MenuButtonType.View,Name = "最新动态",Url = "http://njc.web1.com/news/index?OpenID=_OpenID_"}
                };
            menuList.Menu.Buttons[0] = megagame;

            var aboutme = new MenuButton
            {
                Type = MenuButtonType.Click,
                Name = "我",
                Key = "EVENT_ACTIVE_ABOUTME"
            };
            aboutme.SubButton = new MenuButton[4]
                {
                    new MenuButton{Type = MenuButtonType.Click,Name = "认证",Key = "Certification" },
                    new MenuButton{Type = MenuButtonType.Click,Name = "刮刮乐",Key = "Gua"},
                    new MenuButton{Type = MenuButtonType.Click,Name = "上传作品",Key = "UploadImages"},
                    new MenuButton{Type = MenuButtonType.Click,Name = "我的作品",Key = "MyItem"}
                };
            menuList.Menu.Buttons[1] = aboutme;

            var appreciation = new MenuButton
            {
                Type = MenuButtonType.Click,
                Name = "作品互动",
                Key = "EVENT_ACTIVE_APPRECIATION"
            };
            appreciation.SubButton = new MenuButton[2]
                {
                    new MenuButton{Type = MenuButtonType.Click,Name = "全部作品",Key = "Item"},
                    new MenuButton{Type = MenuButtonType.Click,Name = "人气作品",Key = "Excellent"},
                };
            menuList.Menu.Buttons[2] = appreciation;

            return menuList;
        }

        public QyJsonResult CreateMenu(int? agentid, MenuList menuList)
        {
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/menu/create?access_token={0}&agentid={1}", AccessToken.access_token, agentid), menuList);
        }

        public QyJsonResult DeleteMenu(int? agentid)
        {
            return this.Get<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/menu/delete?access_token={0}&agentid={1}", AccessToken.access_token, agentid));
        }

        public MenuList GetMenuList(int? agentid)
        {
            return this.Get<MenuList>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/menu/get?access_token={0}&agentid={1}", AccessToken.access_token, agentid));
        }
    }
}
