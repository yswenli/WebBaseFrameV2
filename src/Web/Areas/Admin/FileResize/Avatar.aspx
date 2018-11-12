<%@ Page Language="C#" ValidateRequest="false" %>
<% System.Web.HttpContext.Current.Session.Remove("ArticImg"); %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>上传缩略图</title>
</head>
<body>
    <script type="text/javascript" src="/Areas/Admin/Content/js/jquery.min.js"></script>
    <script type="text/javascript" src="/Areas/Admin/Content/js/artDialog/artDialog.source.js?skin=default"></script>
    <script type="text/javascript" src="/Areas/Admin/Content/js/artDialog/plugins/iframeTools.source.js"></script>
    <script type="text/javascript">
        $(function () {
            art.dialog.data('bValue', 'open');
        });
        function UploadComplete() {
            $.post("/admin/article/GetImg", null, function (data) {
                art.dialog.data('bValue', data);
                    art.dialog.close()
                });
            }
    </script>
    <div>
        <div style="text-align: center; margin: 10px;">
            <%--  <img src="@Model.HeadImage" title="当前正在使用的头像" style="border:1px dashed #9D8EBA;padding:5px;margin:5px;" />--%>
        </div>
        <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" id="FileUploadApp" width="520"
            height="440" codebase="http://fpdownload.macromedia.com/get/flashplayer/current/swflash.cab">
            <param name="movie" value="/areas/admin/content/js/swfupload/FileResize.swf" />
            <param name="quality" value="high" />
            <param name="bgcolor" value="#869ca7" />
            <param name="allowScriptAccess" value="sameDomain" />
            <param name="FlashVars" value="usercategory=">
            <embed src="/areas/admin/content/js/swfupload/FileResize.swf" quality="high" bgcolor="#869ca7"
                width="520" height="440" name="FileUploadApp" align="middle" play="true" loop="false"
                quality="high" allowscriptaccess="sameDomain" type="application/x-shockwave-flash"
                flashvars="usercategory=" style="margin: 0px 0px 0px 30px;" pluginspage="http://www.adobe.com/go/getflashplayer">
		    </embed>
        </object>
        <iframe id="refreshFrame" src="Avatar.aspx" style="display: none;"></iframe>
    </div>
</body>
</html>
