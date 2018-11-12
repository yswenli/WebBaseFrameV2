using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Mvc;
using WEF;

//

namespace Libraries
{
    /// <summary>
    /// 权限按钮 html扩展
    /// </summary>
    public static class HtmlHelperExtention
    {
        #region 自定义验证按钮
        /// <summary>
        /// 创建新记录按钮
        /// </summary>
        /// <param name="html"></param>
        /// <param name="title"></param>
        /// <param name="menuID">菜单选项ID</param>
        /// <param name="permissionName"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString CreateButton(this HtmlHelper html, string title, string url, object htmlAttributes)
        {
            string attr = "";
            if (htmlAttributes != null)
            {
                var type = htmlAttributes.GetType();
                var pis = type.GetProperties();
                if (pis != null)
                {
                    foreach (var item in pis)
                    {
                        attr += item.Name + "=\"" + item.GetValue(htmlAttributes, null) + "\"  ";
                    }
                }
            }
            string value = "<div class=\"btn-toolbar pull-left\"><a style=\"display:none;\" title='" + title + "' data-permissionUrl=\"" + url + "\" class=\"btn btn-primary permission\" " + attr + ">" + title + "</a></div>";
            return new MvcHtmlString(value);
        }
        /// <summary>
        /// 创建子项新记录按钮
        /// </summary>
        /// <param name="html"></param>
        /// <param name="title"></param>
        /// <param name="htmlAttributes"></param>
        public static MvcHtmlString CreateSonButton(this HtmlHelper html, string title, string url, object htmlAttributes)
        {
            string attr = "";
            if (htmlAttributes != null)
            {
                var type = htmlAttributes.GetType();
                var pis = type.GetProperties();
                if (pis != null)
                {
                    foreach (var item in pis)
                    {
                        attr += item.Name + "=\"" + item.GetValue(htmlAttributes, null) + "\"  ";
                    }
                }
            }
            string value = "<a style=\"display:none;\" class='tip permission' title='" + title + "' data-permissionUrl=\"" + url + "\" " + attr + "><i class='icol-chart-organisation'></i>" + title + "</a>";
            return new MvcHtmlString(value);
        }
        /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="html"></param>
        /// <param name="title"></param>
        /// <param name="htmlAttributes"></param>
        public static MvcHtmlString EditButton(this HtmlHelper html, string title, string url, object htmlAttributes)
        {
            string attr = "";
            if (htmlAttributes != null)
            {
                var type = htmlAttributes.GetType();
                var pis = type.GetProperties();
                if (pis != null)
                {
                    foreach (var item in pis)
                    {
                        attr += item.Name + "=\"" + item.GetValue(htmlAttributes, null) + "\"  ";
                    }
                }
            }
            string value = "<a style=\"display:none;\" class=\"tip permission\" title=\"" + title + "\" data-permissionUrl=\"" + url + "\" " + attr + "><i class=\"icon-pencil\"></i>" + title + "</a>";
            return new MvcHtmlString(value);
        }
        /// <summary>
        /// 查看按钮
        /// </summary>
        /// <param name="html"></param>
        /// <param name="title"></param>
        /// <param name="htmlAttributes"></param>
        public static MvcHtmlString ViewButton(this HtmlHelper html, string title, string url, object htmlAttributes)
        {
            string attr = "";
            if (htmlAttributes != null)
            {
                var type = htmlAttributes.GetType();
                var pis = type.GetProperties();
                if (pis != null)
                {
                    foreach (var item in pis)
                    {
                        attr += item.Name + "=\"" + item.GetValue(htmlAttributes, null) + "\"  ";
                    }
                }
            }
            string value = " <a style=\"display:none;\" class=\"tip permission\" title=\"" + title + "\" data-permissionUrl=\"" + url + "\"" + attr + "><i class=\"icol-magnifier\"></i>" + title + "</a>";
            return new MvcHtmlString(value);
        }
        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="html"></param>
        /// <param name="title"></param>
        /// <param name="htmlAttributes"></param>
        public static MvcHtmlString DeleteButton(this HtmlHelper html, string title, string url, object htmlAttributes)
        {
            string attr = "";
            if (htmlAttributes != null)
            {
                var type = htmlAttributes.GetType();
                var pis = type.GetProperties();
                if (pis != null)
                {
                    foreach (var item in pis)
                    {
                        attr += item.Name + "=\"" + item.GetValue(htmlAttributes, null) + "\"  ";
                    }
                }
            }
            string value = " <a style=\"display:none;\" class=\"tip permission\" title=\"" + title + "\" data-permissionUrl=\"" + url + "\"" + attr + "><i class=\"icon-trash \"></i>" + title + "</a>";
            return new MvcHtmlString(value);
        }
        /// <summary>
        /// 全部删除按钮
        /// </summary>
        /// <param name="html"></param>
        /// <param name="title"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString DeleteAllButton(this HtmlHelper html, string title, string url, object htmlAttributes)
        {
            string attr = "";
            if (htmlAttributes != null)
            {
                var type = htmlAttributes.GetType();
                var pis = type.GetProperties();
                if (pis != null)
                {
                    foreach (var item in pis)
                    {
                        attr += item.Name + "=\"" + item.GetValue(htmlAttributes, null) + "\"  ";
                    }
                }
            }
            string value = "<a style=\"display:none;\" class=\"btn permission\" " + attr + " title=\"" + title + "\" data-permissionUrl=\"" + url + "\"><span><i class=\"icol-bin-closed\"></i>" + title + "</span></a>";
            return new MvcHtmlString(value);
        }
        /// <summary>
        /// 自定交列表图标linkButton
        /// </summary>
        /// <param name="html"></param>
        /// <param name="title"></param>
        /// <param name="valideUrl"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static MvcHtmlString CustomIconButton(this HtmlHelper html, string title, string valideUrl, string linkurl, string icon)
        {
            string value = " <a style=\"display:none;\" class=\"tip permission\" title=\"" + title + "\" data-permissionUrl=\"" + valideUrl + "\" href=\"" + linkurl + "\"><i class=\"" + icon + "\"></i>" + title + "</a>";
            return new MvcHtmlString(value);
        }

        /// <summary>
        /// 自定义按钮
        /// </summary>
        /// <param name="html"></param>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString CustomButton(this HtmlHelper html, string title, string url, object htmlAttributes)
        {
            string attr = "";
            if (htmlAttributes != null)
            {
                var type = htmlAttributes.GetType();
                var pis = type.GetProperties();
                if (pis != null)
                {
                    foreach (var item in pis)
                    {
                        attr += item.Name + "=\"" + item.GetValue(htmlAttributes, null) + "\"  ";
                    }
                }
            }
            string value = "<a style=\"display:none;\" title='" + title + "' data-permissionUrl=\"" + url + "\" class=\"btn btn-primary permission\" " + attr + ">" + title + "</a>";
            return new MvcHtmlString(value);
        }

        /// <summary>
        /// 权限验证脚本
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString PermissionValideScript(this HtmlHelper html)
        {
            var script = @"<script type=""text/javascript"">
                            $(function () {
                                var titleArray = new Array();
                                var urlArray = new Array();
                                $("".permission"").each(function () {
                                    var alink = $(this);
                                    var notExsits = true;
                                    if (titleArray != undefined) {
                                        for (var i = 0; i < titleArray.length; i++) {
                                            if (titleArray[i] == alink.attr(""title"")) {
                                                notExsits = false;
                                            }
                                        }
                                    }
                                    if (notExsits) {
                                        titleArray.push(alink.attr(""title""));
                                        urlArray.push(alink.attr(""data-permissionUrl""));
                                    }
                                });
                                if (titleArray != undefined && titleArray.length > 0) {
                                    var titleStr = titleArray.join("","");
                                    var urlStr = urlArray.join("","");
                                    $.post(""/admin/ajax/HasPermission?rnd=""+((new Date()).getMilliseconds()), { ""titleStr"": titleStr, ""urlStr"": urlStr }, function (data) {
                                        var dataArry = data.split("","");
                                        for (var i = 0; i < dataArry.length; i++) {
                                            if (dataArry[i] == ""1"") {
                                                $("".permission[title='"" + titleArray[i] + ""']"").show();
                                            }
                                            else {
                                                $("".permission[title='"" + titleArray[i] + ""']"").remove();
                                            }
                                        }
                                    });
                                }
                            });
                        </script>";
            return new MvcHtmlString(script);
        }
        #endregion

        #region 自定义扩展
        /// <summary>
        /// 自定义RadioButton
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name">RadioButton的name</param>
        /// <param name="values">RadioButton集合值</param>
        /// <param name="labels">RadioButton集合显示内容</param>
        /// <param name="choosen">选中的值</param>
        /// <returns></returns>
        public static MvcHtmlString RadioButton(this HtmlHelper html, string name, string[] values, string[] labels, string choosen)
        {
            string str = "";
            if (values != null && values.Length > 0)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (choosen != null && (choosen.ToString() == values[i]))
                    {
                        str += "<input class=\"radio_style\" name=\"" + name + "\" value=\"" + values[i] + "\" type=\"radio\" checked /> " + labels[i];
                    }
                    else
                    {
                        str += "<input class=\"radio_style\" name=\"" + name + "\" value=\"" + values[i] + "\" type=\"radio\" /> " + labels[i];
                    }
                }
            }
            return new MvcHtmlString(str);
        }
        /// <summary>
        /// 自定义checkbox
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id"></param>
        /// <param name="tableName"></param>
        /// <param name="whereSql"></param>
        /// <param name="keyName"></param>
        /// <param name="valName"></param>
        /// <param name="selectKey"></param>
        /// <returns></returns>
        public static MvcHtmlString Checkbox(this HtmlHelper htmlHelper, string id, string tableName, string whereSql, string keyName, string valName, string selectKey)
        {
            string sql = "select [" + keyName + "],[" + valName + "] from [" + tableName + "]";
            whereSql = string.IsNullOrEmpty(whereSql) ? "" : (" where " + whereSql);
            DataTable dt = new DBContext().ExecuteDataSet(sql).Tables[0];
            StringBuilder sb = new StringBuilder();
            foreach (DataRow item in dt.Rows)
            {
                sb.Append("<label><input type='checkbox' name='" + id + "' value='" + item[keyName] + "' val='" + selectKey + "'/>" + valName + "</label>");
            }
            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// 自定义下拉列表
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id"></param>
        /// <param name="sql"></param>
        /// <param name="selectKey"></param>
        /// <param name="htmlAttr"></param>
        /// <returns></returns>
        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string id, string sql, string keyName, string valName, object selectKey, string htmlAttr)
        {
            DataTable dt = new DBContext().ExecuteDataSet(sql).Tables[0];
            StringBuilder sb = new StringBuilder();
            sb.Append("<select id='" + id + "' name='" + id + "' val='" + selectKey + "' " + htmlAttr + ">");
            sb.Append("<option value=''>请选择...</option>");
            foreach (DataRow item in dt.Rows)
            {
                sb.Append("<option value='" + item[keyName] + "'>" + item[valName] + "</option>");
            }
            sb.Append("</select>");
            return new MvcHtmlString(sb.ToString());
        }
        /// <summary>
        /// 自定义下拉列表
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id">控件ID</param>
        /// <param name="tableName">查询表名</param>
        /// <param name="whereSql">条件</param>
        /// <param name="keyName">Key的列名</param>
        /// <param name="valName">Value的列名</param>
        /// <param name="selectKey">赋值</param>
        /// <param name="htmlAttr">其他属性</param>
        /// <returns></returns>
        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string id, string tableName, string whereSql, string keyName, string valName, object selectKey, string htmlAttr)
        {
            string sql = "select [" + keyName + "],[" + valName + "] from [" + tableName + "]";
            whereSql = string.IsNullOrEmpty(whereSql) ? "" : (" where " + whereSql);
            DataTable dt = new DBContext().ExecuteDataSet(sql + whereSql).Tables[0];
            StringBuilder sb = new StringBuilder();
            sb.Append("<select id='" + id + "' name='" + id + "' val='" + selectKey + "' " + htmlAttr + ">");
            sb.Append("<option value=''>请选择...</option>");
            foreach (DataRow item in dt.Rows)
            {
                sb.Append("<option value='" + item[keyName] + "'>" + item[valName] + "</option>");
            }
            sb.Append("</select>");
            return new MvcHtmlString(sb.ToString());
        }
        /// <summary>
        /// 自定义下拉列表
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id"></param>
        /// <param name="tableName"></param>
        /// <param name="whereSql"></param>
        /// <param name="orderby"></param>
        /// <param name="keyName"></param>
        /// <param name="valName"></param>
        /// <param name="selectKey"></param>
        /// <param name="htmlAttr"></param>
        /// <returns></returns>
        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string id, string tableName, string whereSql, string orderby, string keyName, string valName, object selectKey, string htmlAttr)
        {
            string sql = "select [" + keyName + "],[" + valName + "] from [" + tableName + "]";
            whereSql = string.IsNullOrEmpty(whereSql) ? "" : (" where " + whereSql);
            orderby = string.IsNullOrEmpty(orderby) ? "" : (" order by " + orderby);

            DataTable dt = new DBContext().ExecuteDataSet(sql + whereSql + orderby).Tables[0];
            StringBuilder sb = new StringBuilder();
            sb.Append("<select id='" + id + "' name='" + id + "' val='" + selectKey + "' " + htmlAttr + ">");
            sb.Append("<option value=''>请选择...</option>");
            foreach (DataRow item in dt.Rows)
            {
                sb.Append("<option value='" + item[keyName] + "'>" + item[valName] + "</option>");
            }
            sb.Append("</select>");
            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// 自定义扩展
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListFor(this HtmlHelper helper, string id, string name, string value, Dictionary<string, string> options)
        {
            string html = "<select id=\"" + id + "\" name=\"" + name + "\"  class=\"form-control\">";
            if (options != null)
            {
                foreach (var item in options)
                {
                    if (item.Value == value || (!string.IsNullOrEmpty(item.Value) && !string.IsNullOrEmpty(value) && (item.Value.ToUpper() == value.ToUpper())))
                    {
                        html += "<option value=\"" + item.Value + "\" selected>" + item.Key + "</option>";
                    }
                    else
                    {
                        html += "<option value=\"" + item.Value + "\">" + item.Key + "</option>";
                    }
                }
            }
            html += "</select>";
            return new MvcHtmlString(html); ;
        }
        #endregion
    }

}