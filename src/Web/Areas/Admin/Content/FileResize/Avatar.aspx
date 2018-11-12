<%@ Import Namespace="Libraries" %>

<%@ Page Language="C#" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>修改头像</title>
</head>
<body>
    <script type="text/javascript">
        function UploadComplete(Alumbid) {
            alert("图像上传成功!");
            location.reload();
        }
    </script>
    <div style="margin: 10px;">
        <img src="<%=MemberHelper.Avatar(Libraries.CurrentMember.ID) %>" title="当前正在使用的头像" style="border: 1px dashed #9D8EBA; padding: 5px; margin: 5px;" />
    </div>
    <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" id="FileUploadApp" width="520"
        height="440" codebase="http://fpdownload.macromedia.com/get/flashplayer/current/swflash.cab">
        <param name="movie" value="/areas/admin/content/FileResize/FileResize.swf" />
        <param name="quality" value="high" />
        <param name="bgcolor" value="#808080" />
        <param name="allowScriptAccess" value="sameDomain" />
        <param name="FlashVars" value="usercategory=">
        <embed src="/areas/admin/content/FileResize/FileResize.swf" quality="high" bgcolor="#808080"
            width="520" height="440" name="FileUploadApp" align="middle" play="true" loop="false"
            quality="high" allowscriptaccess="sameDomain" type="application/x-shockwave-flash"
            flashvars="usercategory=" pluginspage="http://www.adobe.com/go/getflashplayer">
		</embed>
    </object>
</body>
</html>
