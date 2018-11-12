function flashupload(uploadid, name, textareaid, funcName, args, module, catid) {
  
    var args = args ? '&args=' + args : '';
    var setting = '&module=' + module + '&catid=' + catid;
  
    art.dialog.open('/admin/attachment/swfupload?1=1' + args + setting, {
        id: uploadid,
        width: 500,
        height:400,
        lock: true,
        title: name,
        ok: function () {
            var iframe = this.iframe.contentWindow;
            if (!iframe.document.body) {
                alert('iframe还没加载完毕呢')
            
                return false;
            };
            if (funcName) {
               
                return funcName.apply(this, [uploadid, textareaid]);
            } else {
               
                return submit_ckeditor(iframe, uploadid, textareaid);
            }
        },
        cancel: true
    });
}

function submit_ckeditor(iframe, uploadid, textareaid) {
    var d = iframe;
    var in_content = d.$("#att-status").html().replace(/(^\s*)|(\s*$)/g, "");
    var del_content = d.$("#att-status-del").html();
    insert2editor(textareaid, in_content, del_content);
}

function submit_imags(uploadid, returnid) {
    var d = this.iframe.contentWindow;
    var in_content = d.$("#att-status").html().replace(/(^\s*)|(\s*$)/g, "").substring(1);
    var in_content = in_content.split('|');
    if (IsImg(in_content[0])) {
        $('#' + returnid).attr("value", in_content[0]);
        $('#thumb_preview').attr("src", in_content[0]);
    }
    else {
        art.dialog.alert('选择的类型必须为图片类型');
        return false;
    }
}

function submit_files(uploadid, returnid) {
    var d = this.iframe.contentWindow;
    var in_content = d.$("#att-status").html().replace(/(^\s*)|(\s*$)/g, "").substring(1);
    var in_content = in_content.split('|');
    var new_filepath = in_content[0]; //.replace(uploadurl, '/');
    $('#' + returnid).attr("value", new_filepath);
}

//验证是否为flv或m4v类型
function submit_flvOrf4v(uploadid, returnid) {
    var d = this.iframe.contentWindow;
    var in_content = d.$("#att-status").html().replace(/(^\s*)|(\s*$)/g, "").substring(1);
    var in_content = in_content.split('|');
    if (IsFlvOrF4v(in_content[0]))
        $('#' + returnid).attr("value", in_content[0]);
    else {
        art.dialog.alert('选择的类型必须为flv|f4v类型');
        return false;
    }
}

//验证是否为办公文档
function submit_OfficeFiles(uploadid, returnid) {
    var d = this.iframe.contentWindow;
    var in_content = d.$("#att-status").html().replace(/(^\s*)|(\s*$)/g, "").substring(1);
    var in_content = in_content.split('|');
    if (IsOfficeFiles(in_content[0]))
        $('#' + returnid).attr("value", in_content[0]);
    else {
        art.dialog.alert('选择的类型必须为doc|ppt类型');
        return false;
    }
}

function insert2editor(id, in_content, del_content) {
    if (in_content == '') { return false; }
    var data = in_content.substring(1).split('|');
    var img = '';
    for (var n in data) {
        var attname = data[n];
        //        $.post("/admin/attachment/GetAttachmentName?n=" + (new Date().toString()), { val: data[n], index: n }, function (result) {
        //            var results = result.split(',');
        //            n = results[0];
        //            if (results[1] != "") {
        //                attname = results[1];
        //            }
        if (IsImg(data[n])) {
            img = img + '<img src="' + data[n] + '" /><br />';
        } else if (IsSwf(data[n])) {
            img = img + '<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0"><param name="quality" value="high" /><param name="movie" value="' + data[n] + '" /><embed pluginspage="http://www.macromedia.com/go/getflashplayer" quality="high" src="' + data[n] + '" type="application/x-shockwave-flash" width="460"></embed></object><br/>';
        } else {
            img = img + '<a href="' + data[n] + '" target="_blank" />' + attname + '</a><br />';
        }
        CKEDITOR.instances[id].insertHtml(img);
        var newcontent = CKEDITOR.instances[id].getData();
        if (newcontent.indexOf(img) <= -1) {
            newcontent = newcontent + "</ br>" + img;
            CKEDITOR.instances[id].setData(newcontent);
        }
        //        });
    }

}

function IsImg(url) {
    var sTemp;
    var b = false;
    var opt = "jpg|gif|png|bmp|jpeg";
    var s = opt.toUpperCase().split("|");
    for (var i = 0; i < s.length; i++) {
        sTemp = url.substr(url.length - s[i].length - 1);
        sTemp = sTemp.toUpperCase();
        s[i] = "." + s[i];
        if (s[i] == sTemp) {
            b = true;
            break;
        }
    }
    return b;
}

function IsSwf(url) {
    var sTemp;
    var b = false;
    var opt = "swf";
    var s = opt.toUpperCase().split("|");
    for (var i = 0; i < s.length; i++) {
        sTemp = url.substr(url.length - s[i].length - 1);
        sTemp = sTemp.toUpperCase();
        s[i] = "." + s[i];
        if (s[i] == sTemp) {
            b = true;
            break;
        }
    }
    return b;

}

function IsFlvOrF4v(url) {
    var sTemp;
    var b = false;
    var opt = "flv|f4v";
    var s = opt.toUpperCase().split("|");
    for (var i = 0; i < s.length; i++) {
        sTemp = url.substr(url.length - s[i].length - 1);
        sTemp = sTemp.toUpperCase();
        s[i] = "." + s[i];
        if (s[i] == sTemp) {
            b = true;
            break;
        }
    }
    return b;
}

function IsOfficeFiles(url) {
    var sTemp;
    var b = false;
    var opt = "doc|docx|ppt|pptx|pdf|xls|xlsx";
    var s = opt.toUpperCase().split("|");
    for (var i = 0; i < s.length; i++) {
        sTemp = url.substr(url.length - s[i].length - 1);
        sTemp = sTemp.toUpperCase();
        s[i] = "." + s[i];
        if (s[i] == sTemp) {
            b = true;
            break;
        }
    }
    return b;
}