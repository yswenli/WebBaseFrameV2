
using Newtonsoft.Json;

namespace WeiXinAPIs.Models
{
    public class MenuList
    {
        public MenuList()
        {
            Menu = new Menu();
        }

        [JsonProperty("menu")]
        public Menu Menu { get; set; }
    }

    public class Menu
    {
        /// <summary>
        /// 一级菜单数组，个数应为1~3个
        /// </summary>
        [JsonProperty("button")]
        public MenuButton[] Buttons { get; set; }
    }

    public class MenuButton
    {
        /// <summary>
        /// 菜单的响应动作类型，目前有click、view两种类型
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// 菜单标题，不超过16个字节，子菜单不超过40个字节
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 菜单KEY值，用于消息接口推送，不超过128字节
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }
        /// <summary>
        /// 网页链接，用户点击菜单可打开链接，不超过256字节
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
        /// <summary>
        /// 二级菜单数组，个数应为1~5个
        /// </summary>
        [JsonProperty("sub_button")]
        public MenuButton[] SubButton { get; set; }
    }

    public class MenuButtonType
    {
        /// <summary>
        /// 点击推事件	用户点击click类型按钮后，微信服务器会通过消息接口推送消息类型为event的结构给开发者（参考消息接口指南），并且带上按钮中开发者填写的key值，开发者可以通过自定义的key值与用户进行交互；
        /// </summary>
        public static string Click = "click";

        /// <summary>
        /// 跳转URL	用户点击view类型按钮后，微信客户端将会打开开发者在按钮中填写的网页URL，可与网页授权获取用户基本信息接口结合，获得用户基本信息。
        /// </summary>
        public static string View = "view";

        /// <summary>
        /// 扫码推事件	用户点击按钮后，微信客户端将调起扫一扫工具，完成扫码操作后显示扫描结果（如果是URL，将进入URL），且会将扫码的结果传给开发者，开发者可以下发消息。
        /// </summary>
        public static string ScancodePush = "scancode_push";

        /// <summary>
        /// 扫码推事件且弹出“消息接收中”提示框	用户点击按钮后，微信客户端将调起扫一扫工具，完成扫码操作后，将扫码的结果传给开发者，同时收起扫一扫工具，然后弹出“消息接收中”提示框，随后可能会收到开发者下发的消息。
        /// </summary>
        public static string ScancodeWaitmsg = "scancode_waitmsg";

        /// <summary>
        /// 弹出系统拍照发图	用户点击按钮后，微信客户端将调起系统相机，完成拍照操作后，会将拍摄的相片发送给开发者，并推送事件给开发者，同时收起系统相机，随后可能会收到开发者下发的消息。
        /// </summary>
        public static string PicSysphoto = "pic_sysphoto";

        /// <summary>
        /// 弹出拍照或者相册发图	用户点击按钮后，微信客户端将弹出选择器供用户选择“拍照”或者“从手机相册选择”。用户选择后即走其他两种流程。
        /// </summary>
        public static string PicPhotoOrAlbum = "pic_photo_or_album";

        /// <summary>
        /// 弹出微信相册发图器	用户点击按钮后，微信客户端将调起微信相册，完成选择操作后，将选择的相片发送给开发者的服务器，并推送事件给开发者，同时收起相册，随后可能会收到开发者下发的消息。
        /// </summary>
        public static string PicWeixin = "pic_weixin";

        /// <summary>
        /// 弹出地理位置选择器	用户点击按钮后，微信客户端将调起地理位置选择工具，完成选择操作后，将选择的地理位置发送给开发者的服务器，同时收起位置选择工具，随后可能会收到开发者下发的消息。
        /// </summary>
        public static string LocationSelect = "location_select";
    }
}
