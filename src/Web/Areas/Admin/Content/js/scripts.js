/// <reference path="ckplayer6.3/ckplayer/ckplayer.js" />
/// <reference path="jquery.min.js" />
/// <reference path="artDialog/jquery.artDialog.js" />
/// <reference path="artDialog/plugins/iframeTools.js" />
/// <reference path="formvalidator/formvalidator.js" />
/// <reference path="picklist/picklist.min.js" />


$(function () {
    loading();
    colorbox_init();
    date_init();
    tab_init();
    fieldset_init();
    setval_init();
    sheepIt_init();
    formvalidator_init();
    select2_init();
    tooltip_init();
    popover_init();
    uniform_init();
    picklist_init();
    //spinner_init();
    ajaxsubmit_init();
    go_init();
    sort_init();
    pagesize_init();
    wizard_nav_init();
    dt_header_btn_group_init();
    pagination_init();
    $(".alert .close").click(function () {
        $(this).parent().fadeOut();
    });
});
//页面加载
function loading() {
    try {
        art.dialog({ title: "数据加载...", lock: true, icon: "load", cancel: false, dbClose: false, content: "正在努力加载数据，请稍候..." })
    } catch (e) { }
}
//页面完成加载
$(window).bind("load", function () {
    closeDialog();
});

//关闭art.dialog
function closeDialog() {
    try {
        var list = art.dialog.list;
        for (var i in list) {
            list[i].close();
        };
    }
    catch (e) { }
}

/********** JS 应用样式 *************/
//分页
function pagination_init() {
    $(".dt_footer .pagination a").each(function () {
        var href = $(this).attr("href");
        $(this).attr("href", "javascript:void(0);").attr("data-href", href);
    });

    $(".dt_footer .pagination a").click(function () {
        var href = $(this).attr("data-href");
        if (href != undefined && href != "") {
            var form = $(".dt_header form");
            var div = form.parents(".dt-pages:first").find(".dataTables_wrapper");
            var box = art.dialog({ title: "数据加载...", lock: true, icon: "load", cancel: false, dbClose: false, content: "正在努力加载数据，请稍候..." });
            $.post(href, $(".dt_header form").serialize(), function (data) {
                div.html(data);
                sort_init();
                pagination_init();
                box.close();
            });
        }
        return false;
    });
}

//表头部按钮组功能
function dt_header_btn_group_init() {
    //导出
    $(".dt_header .icol-doc-excel-csv").parents("a.btn").not(".skip").click(function () {
        var table = $(this).parents(".dt-pages:first").find(".dataTables_wrapper:first table");
        var oldtable_html = table.html();
        table.find(".checkbox-column,.btns-column").remove();
        var newtable = $(this).parents(".dt-pages:first").find(".dataTables_wrapper:first table");
        try {
            $.post("/admin/ajax/_csv", { tableHtml: "<table>" + newtable.html() + "</table>" }, function (data) {
                location.href = "/upload/csvTemp/" + data;
            });
        } catch (e) { }
        datatable_load(location.href);
    });
    //打印
    $(".dt_header .icol-printer").parents("a.btn").click(function () {
        window.print();
    });

    /*搜索*/
    $(".dt_header form button[type='submit']").click(function () {
        var url = $(".dt_header form").attr("action");
        if (url == "") url = location.href;
        datatable_load(url);
        return false;
    });
}

//按月份搜索
function wizard_nav_init() {
    $(".wizard-nav[data-key] li").click(function () {
        $(".wizard-nav[data-key] li").removeClass("current");
        $(this).addClass("current");
        var key = $(this).parents(".wizard-nav[data-key]").attr("data-key");
        var value = $(this).attr("data-value");
        var form = $(".dt_header form");
        if (form.find("input[name='" + key + "']").length == 0) {
            form.append('<input type="hidden" name="' + key + '" value="" />');
            form = $(".dt_header form");
        }
        form.find("input[name='" + key + "']").val(value);
        datatable_load(location.href);
    });
}

//列表重新加载
function datatable_load(url) {
    var form = $(".dt_header form");
    var order = form.find("input[name='order']").val();
    var by = form.find("input[name='by']").val();
    var div = form.parents(".dt-pages:first").find(".dataTables_wrapper");
    var box = art.dialog({ title: "数据加载...", lock: true, icon: "load", cancel: false, dbClose: false, content: "正在努力加载数据，请稍候...", time: 3 });
    if (url.indexOf("?") > -1) {
        url += "&rnd=" + (new Date()).getMilliseconds();
    }
    else {
        url += "?rnd=" + (new Date()).getMilliseconds();
    }
    $.post(url, form.serialize(), function (data) {
        div.html(data);
        if (order != undefined) {
            var th = $("th[data-sort='" + order + "']");
            th.removeClass("sorting");
            th.addClass("sorting_" + by);
            th.css("background-color", "#EAEAEA");
        }
        sort_init();
        pagination_init();
        box.close();
    });
}
/*ajax加载数据*/
function loadAjax(url, serialize, div) {
    var box = art.dialog({ title: "数据加载...", lock: true, icon: "load", dbClose: false, cancel: false, content: "正在努力加载数据，请稍候..." });
    $.get(url, serialize, function (data) {
        div.html(data);
        box.close();
    });
}

//每页显示个数
function pagesize_init() {
    $(".dt-pages select.input-mini").change(function () {
        datatable_load(location.href);
    });
}


//列表字段排序
function sort_init() {
    $("table .sorting,table .sorting_asc,table .sorting_desc").click(function () {
        var div = $(this).parents(".dataTables_wrapper:first");
        var table = $(this).parents("table:first");
        var form = $(this).parents(".dt-pages:first").find("form");
        var order = "", by = "";
        if ($(this).attr("data-sort") != undefined) {
            order = $(this).attr("data-sort");
            if ($(this).hasClass("sorting") || $(this).hasClass("sorting_desc"))
                by = "asc";
            else if ($(this).hasClass("sorting_asc"))
                by = "desc";
            var href = location.href;
            if (href.indexOf("?") == -1)
                href += "?order=" + order + "&by=" + by;
            else
                href += "&order=" + order + "&by=" + by;
            //保存信息
            if (form.find("input[name='order']").length == 0)
                form.append('<input type="hidden" name="order" value="" />');
            if (form.find("input[name='by']").length == 0)
                form.append('<input type="hidden" name="by" value="" />');

            form = $(this).parents(".dt-pages:first").find("form");
            var input_order = form.find("input[name='order']").val(order);
            var input_by = form.find("input[name='by']").val(by);

            datatable_load(location.href);
        }

    });
}

//获取地址栏参数
function urlParam() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    try {
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs.split("=")[0]] = unescape(strs.split("=")[1]);
            }
        }
    }
    catch (e) { }
    return theRequest;
}

//返回网页顶部、底部
function go_init() {
    $(".go").hide();
    $(".go .top").click(//定义返回顶部点击向上滚动的动画
        function () {
            $('html,body').animate({ scrollTop: 0 }, 200);
        });
    $(".go .bottom").click(//定义返回顶部点击向上滚动的动画
    function () {
        $('html,body').animate({ scrollTop: document.body.clientHeight }, 200);

    });

    if (document.body.clientHeight > window.innerHeight)
        $(".go").show();

}

//获取当前相对路径的方法
function getUrlRelativePath(varabsoluteUrl) {
    if (varabsoluteUrl.indexOf("://") > -1) {
        var arrUrl = varabsoluteUrl.split("//");
        var relUrl = arrUrl[1].substring(arrUrl[1].indexOf("/"));//stop省略，截取从start开始到结尾的所有字符
        return relUrl;
    }
    return varabsoluteUrl;
}


function colorbox_init() {
    $(".colorbox").each(function () {
        var _width = window.innerWidth;
        var _height = window.innerHeight;
        var title = $(this).attr("title");
        var obj = $(this);
        obj.attr("data-href", obj.attr("href"));
        obj.attr("href", "javascript:void(0);");
        obj.click(function () {
            art.dialog.open(obj.attr("data-href"), { title: title, lock: true, dbClose: false, width: _width * 0.8, height: _height * 0.8 });
        });
    });
}

function setval_init() {
    /************************ Js 赋值*******************/
    //select 赋值
    $("select").each(function () {
        if ($(this).attr("val") != undefined) {
            $(this).val($(this).attr("val"));
            $(this).change();
        }
    });
    //radio 赋值
    $(":radio").each(function () {
        if ($(this).attr("val") != undefined) {
            var val = $(this).attr("val");
            var name = $(this).attr("name");
            $("input[name='" + name + "']").each(function () {
                if ($(this).val() == val) {
                    $(this).attr("checked", true);
                    $(this).change();
                    return;
                }
            });
        }
    });
    //checkbox 赋值
    $(":checkbox").each(function () {
        if ($(this).attr("val") != undefined) {
            var val = "," + $(this).attr("val") + ",";
            var name = $(this).attr("name");
            $("input[name='" + name + "']").each(function () {
                if (val.indexOf("," + $(this).val() + ",") != -1) {
                    $(this).attr("checked", true);
                    $(this).change();
                }
            });
        }
    });
    //搜索赋值
    try {
        var cur_url = location.href;
        var arr1 = cur_url.split('?');
        if (arr1.length > 1) {
            var arr2 = arr1[1].split("&");
            for (i = 0; i < arr2.length; i++) {
                var arr3 = arr2[i].split('=');
                $("#" + arr3[0]).val(decodeURI(arr3[1]));
            }
        }
    }
    catch (e) { }
    /************************ Js 赋值完毕*******************/
}

function fieldset_init() {
    $("fieldset.toggle legend").each(function () {
        $(this).css("cursor", "pointer");
        if ($(this).html().indexOf("icon-chevron-") == -1)
            $(this).html('<i class="icon-chevron-up"></i>' + $(this).html());
        $(this).attr("onclick", "fieldset_toggle(this)");
    });
}

function fieldset_toggle(obj) {
    var fieldset = $(obj).parent();
    $("#div_" + fieldset.attr("id")).toggle();
    var icon = $(obj).find("i");
    if (icon.attr("class").indexOf("icon-chevron-up") != -1) {
        icon.removeClass("icon-chevron-up");
        icon.addClass("icon-chevron-down");
    } else {
        icon.removeClass("icon-chevron-down");
        icon.addClass("icon-chevron-up");
    }
}

function tab_init() {
    var panes = $(".nav-tabs").parents().find(".tab-pane");
    panes.hide();
    $(".nav-tabs li").eq(0).addClass("active");
    panes.eq(0).show();
    $(".nav-tabs li").each(function () {
        $(this).click(function () {
            panes.hide();
            $(".nav-tabs li").removeClass("active");
            $(this).addClass("active");
            var index = $(this).index();
            panes.eq(index).show();
        });
    });
}
function wizard_init() {
    $(".wizard").each(function () {
        $(this).find(".wizard-nav li").click(function () {
            var i = $(this).index() + 1;
            //全部重置
            $(".wizard .wizard-nav li").removeClass("current");
            $(".wizard-step").hide();
            //当前步骤
            var obj = $(".wizard .wizard-nav li").eq(i - 1);
            var targetid = $(obj).attr("data-target");
            $(obj).addClass("current");
            $("#" + targetid).fadeIn(500);
        });
        $(this).find(".wizard-step").hide();
        if ($(this).attr("data-step") == undefined || $(this).attr("data-step") == null || $(this).attr("data-step") == "")
            $(this).find(".wizard-nav li:first").trigger("click");
        else
            $(this).find(".wizard-nav li").eq(parseInt($(this).attr("data-step")) - 1).trigger("click");
    });
}

function select2_init() {
    if ($.fn.select2) {
        $("select").not(".select2_skip").select2({});
    }
}
function tooltip_init() {
    if ($('[rel="tooltip"],.tip').length > 0) {
        if ($.fn.tooltip)
            $(this).tooltip();
    }
}
function popover_init() {
    if ($('[rel="popover"]').length > 0) {
        if ($.fn.popover)
            $(this).popover();
    }
}
function uniform_init() {
    if ($.fn.uniform) {
        if ($.uniform.restore)//清理格式
            $.uniform.restore(":radio, :checkbox");
        $(':radio, :checkbox').not(".skip,.ibutton").uniform(); //重新应用格式
    }
}
function spinner_init() {
    if ($.fn.spinner) {
        $('input.int').each(function () {
            if ($(this).val() == "")
                $(this).val("1");
            $(this).spinner({});
        });
        $('input.decimal').each(function () {
            if ($(this).val() == "")
                $(this).val("1.00");
            $(this).spinner({
                step: 0.01,
                numberFormat: "n"
            });
        });
    }
}
//select多选 picklist
function picklist_init() {
    if ($.fn.picklist) {
        $('select.picklist').not(".picklist_skip").each(function () {
            $(this).picklist({
                addAllLabel: '<i class="icon-right"></i><i class="icon-right"></i>',
                addLabel: '<i class="icon-right"></i>',
                removeAllLabel: '<i class="icon-left"></i><i class="icon-left"></i>',
                removeLabel: '<i class="icon-left"></i>'
            });
        });
    }
}
/*sheepIt 动态复制表单*/
function sheepIt_init() {
    $('.sheepIt').not(".sheepIt_skip").each(function () {
        $(this).sheepIt({
            separator: '',
            iniFormsCount: 0,
            minFormsCount: 0,
            maxFormsCount: 999
        });
    });
}

//表单验证  validator_skip为跳过验证
function formvalidator_init() {
    $("form.validator").each(function (i) {
        var form_id = $(this).attr("id");
        $.formValidator.initConfig({
            validatorgroup: form_id,
            formid: form_id,
            autotip: true,
            onerror: function (msg, obj) {
                //var tip = "";
                //if ($(obj).attr("tip") != undefined)
                //    tip = $(obj).attr("tip");
                //ArtAlertB("<b>" + tip + "</b> " + msg);
            }
        });

        $(this).find(":text,:password,:radio,:checkbox,textarea,select").not(".validator_skip").each(function (eq) {
            var id = $(this).attr("id");
            if (id != "" && id != undefined && id != null) {
                var required = false;
                var minlen = 1;
                var maxlen = 9999999999999999999;
                var regex = "";
                var ajax_url = "/admin/ajax/_validator";
                var name = $(this).attr("name");
                var type = $(this).attr("type");
                var val = $(this).val();
                var _formvalidator = null;
                //验证必填
                var input = $(this);
                if ($(this).attr("required") != undefined && $(this).is(":visible")) {
                    if (type == "radio" || type == "checkbox")
                        _formvalidator = $(":" + type + "[name=" + name + "]").formValidator({ validatorgroup: form_id });
                    else
                        _formvalidator = $("#" + id).formValidator({ validatorgroup: form_id });
                } else {
                    if (type == "radio" || type == "checkbox")
                        _formvalidator = $(":" + type + "[name=" + name + "]").formValidator({ empty: true, validatorgroup: form_id });
                    else
                        _formvalidator = $("#" + id).formValidator({ empty: true, validatorgroup: form_id });
                }

                //验证长度
                if ($(this).attr("min") != undefined && $(this).attr("min") != "") {
                    minlen = parseInt($(this).attr("min"));
                }
                if ($(this).attr("max") != undefined && $(this).attr("max") != "") {
                    maxlen = parseInt($(this).attr("max"));
                }
                _formvalidator.inputValidator({ min: minlen, max: maxlen });

                //验证格式
                if ($(this).attr("regex") != undefined && $(this).attr("regex") != "") {
                    regex = $(this).attr("regex");
                    _formvalidator.regexValidator({ regexp: regex, datatype: "enum", onerror: "格式不正确" });
                }

                //ajax验证
                if ($(this).attr("url") != undefined && $(this).attr("url") != "") {
                    ajax_url = $(this).attr("url");
                    var sdata = $(this).val();
                    _formvalidator.ajaxValidator({
                        type: "get",
                        url: ajax_url,
                        data: sdata,
                        datatype: "html",
                        async: 'false',
                        success: function (getdata) {
                            if (getdata == "1") {
                                return true;
                            } else {
                                return false;
                            }
                        },
                        onerror: "数据已存在",
                        onwait: "请稍候..."
                    }).defaultPassed();
                }
                //判定是否通过
                if ($(this).val() != "" && $(this).is(":text"))
                    _formvalidator.defaultPassed();
            }
        });
    });
}
//ajax 提交表单
function ajaxsubmit_init() {
    $("[type='submit'],.submit").not(".skip").click(function () {
        var form = $(this).parents("form:first");
        var data_action = $(this).attr("data-action");
        if (data_action != undefined && data_action != null)
            form.attr("action", data_action);
        return dosubmit(form, 2);
    });
}

/********** JS 应用样式END *************/
//表单提交
function dosubmit(form, showtime) {
    formvalidator_init();
    //验证form表单
    if (form.length == 0) {
        //alert("未能找到表单<form>元素");
        return false;
    }
    //Fckeditor特殊处理,使用脚本赋值，否则无法通过ajax提交后台
    try {
        $(".Fckeditor,.FckeditorBasic").each(function () {
            var id = $(this).attr("id");
            var oEditor = CKEDITOR.instances[id];
            var data = oEditor.getData();
            $(this).val(data);
        });
    } catch (e) { }
    var url = form.attr("action");
    if (url == undefined || url == "" || url == null)
        url = location.href;

    var param = form.serialize();
    if (form.attr("class") != undefined && form.attr("class").indexOf("validator") > -1) {
        if ($.formValidator.pageIsValid(form.attr("id")))
            _post(url, showtime, param, null, form.find("[type='submit'],.submit").attr("data-msg"));
    } else {
        _post(url, showtime, param, null, form.find("[type='submit'],.submit").attr("data-msg"));
    }
    return false;
}

function _post(url, showtime, param, fun, showtype) {
    if (showtype == "fixed") {
        var fixed = $("#fixed-alert .alert");
        fixed.fadeIn();
        fixed.find("i").attr("class", "icon-loading");
        fixed.find("span").html("正在提交数据请稍候...");
        $.post(url, param, function (data) {
            if (fun) {
                fun.apply(this, [data]);
            } else {
                var arr = data.split('|');
                var icon = _icon(parseInt(arr[0]));
                fixed.removeClass("alert-success").removeClass("alert-error").removeClass("alert-info");
                fixed.find("i").removeClass("icol-accept").removeClass("icol-delete").removeClass("icol-exclamation-octagon-fram");
                switch (icon) {
                    case "succeed":
                        fixed.addClass("alert-success");
                        fixed.find("i").attr("class", "icol-accept");
                        break;
                    case "error":
                        fixed.addClass("alert-error");
                        fixed.find("i").attr("class", "icol-delete");
                        break;
                    default:
                        fixed.find("i").attr("class", "icol-exclamation-octagon-fram");
                        break;
                }
                if (icon == "error")
                    showtime = 3;
                if (arr.length > 3 && arr[3] != "") {
                    art.dialog({ title: "温馨提醒", lock: false, icon: icon, dbClose: false, ok: true, content: arr[1] }).time(showtime);
                }
                else if (arr.length > 2 && arr[2] != "") {
                    fixed.find("span").html("页面正在跳转，请勿刷新网页...");
                    setTimeout(function () { location.href = arr[2]; }, 1000);
                } else {
                    fixed.find("span").html(arr[1]);
                    setTimeout(function () { fixed.fadeOut(); }, showtime * 200);
                }
            }
        });
    }
    else {
        var box = art.dialog({ title: "数据正在提交，请勿刷新网页...", lock: true, icon: "load", dbClose: false, cancel: false, content: "正在提交数据请稍候..." });
        $.post(url, param, function (data) {
            box.close();
            if (fun) {
                fun.apply(this, [data]);
            } else {
                var arr = data.split('|');
                var icon = _icon(parseInt(arr[0]));
                if (icon == "error")
                    showtime = 3;
                if (arr.length > 3 && arr[3] != "") {
                    art.dialog({ title: "温馨提醒", lock: true, icon: icon, dbClose: false, ok: true, content: arr[1] }).time(showtime);
                }
                else if (arr.length > 2 && arr[2] != "") {
                    art.dialog({ title: "页面正在跳转，请勿刷新网页...", lock: true, icon: icon, dbClose: false, cancel: false, content: arr[1] + " 页面正在跳转..." }).time(showtime);
                    setTimeout(function () { location.href = arr[2]; }, 1000);
                } else {
                    art.dialog({ title: "温馨提醒", lock: true, icon: icon, dbClose: false, ok: true, content: arr[1] }).time(showtime);
                }
            }
        });
    }
}

function _icon(i) {
    var icon = "load";
    switch (i) {
        case -1:
            icon = "error";
            break;
        case 0:
            icon = "warning";
            break;
        case 1:
            icon = "succeed";
            break;
        case 2:
            icon = "question";
            break;
        case 3:
            icon = "face-smile";
            break;
        case 4:
            icon = "face-sad";
            break;
    }
    return icon;
}

//提示
function ArtAlert(message) {
    art.dialog.alert(message);
}
//提示
function ArtAlertB(message) {
    art.dialog({ lock: false, icon: _icon(0), content: message, time: 2, dbClose: false, ok: true, cancel: false });
}
//提示
function ArtAlertC(i, message, url, showtime, showcancel) {
    art.dialog({
        lock: true, icon: _icon(i), content: message, time: showtime, dbClose: false,
        ok: function () {
            if (url != "" && url != "#") {
                location.href = url;
            }
        },
        cancel: showcancel
    });

    if (url != "" && url != "#") {
        setTimeout(function () { location.href = url; }, showtime * 1000);
    }
}
//确认框
function ArtConfirm(message, fun) {
    art.dialog.confirm(message, function () {
        if (fun) { fun.apply(this, null); }
    }, function () {

    });
}
//输入框
function ArtPrompt(message, fun) {
    art.dialog.prompt(message, function (data) {
        if (fun)
            if (fun) { fun.apply(this, [data]); }
    }, '');
}
//打开弹出框
function ArtOpen(url, title, width, height) {
    art.dialog.open(url, {
        title: title, width: width, height: height, lock: true, dbClose: false,
        ok: function () {
            var iframe = this.iframe.contentWindow;
            if (!iframe.document.body) {
                alert('iframe还没加载完毕呢')
                return false;
            };
            var btn = iframe.document.getElementById('dosubmit');
            btn.click();
            return false;
        }, cancel: true
    });
}
//弹出提示
function ArtTip(title) {
    art.dialog.tips(title);
}

/*确定是否跳转*/
function confirmurl(url, message) {
    if (confirm(message)) redirect(url);
}
/*页面直接调整*/
function redirect(url) {
    //if(url.indexOf('://') == -1 && url.substr(0, 1) != '/' && url.substr(0, 1) != '?') url = $('base').attr('href')+url;
    location.href = url;
}

/*关闭弹出层*/
function closeAllDialog() {
    art.dialog.close()
    location.reload();
}
function close_dialog(dialogid) {
    window.top.right.location.reload();
    window.top.art.dialog({ id: dialogid }).close();
}

/**
* 全选checkbox,注意：标识checkbox id固定为为check_box
* @param string name 列表check名称,如 uid[]
*/
function selectall(obj, name) {
    if ($(obj).attr("checked") == true || $(obj).attr("checked") == "checked") {
        $("input[name='" + name + "']").each(function () {
            this.checked = true;
            if ($(this).parent().is("span"))
                $(this).parent().addClass("uniform-checked");
        });
    } else {
        $("input[name='" + name + "']").each(function () {
            this.checked = false;
            if ($(this).parent().is("span"))
                $(this).parent().removeClass("uniform-checked");
        });
    }
}
//获取复选框的值
function checkboxval(name) {
    var id = tag = '';
    $("input[name='" + name + "']").each(function () {
        if ($(this).attr('checked') == true || $(this).attr("checked") == "checked") {
            id += tag + $(this).val();
            tag = ',';
        }
    });
    return id;
}


/*弹出网页窗口*/
function openwinx(url, name, w, h) {
    if (!w) w = screen.width;
    if (!h) h = screen.height - 60;
    window.open(url, name, "top=100,left=400,width=" + w + ",height=" + h + ",toolbar=no,menubar=no,scrollbars=yes,resizable=yes,location=no,status=no");
}
//弹出对话框
function openDialog(title, link, width, height, close_type) {
    if (!width) width = 700;
    if (!height) height = 500;
    art.dialog.open(link, {
        title: title, width: width, height: height, lock: true, dbClose: false,
        okVal: '确定',
        ok: function () {
            if (close_type == 1) {
                var iframe = this.iframe.contentWindow;
                if (iframe.document.body) {
                    iframe.document.getElementById('dosubmit').click();
                }
            }
            else {
                this.time(3);
            }
            return false;
        },
        cancelVal: '关闭',
        cancel: function () {
            $(document).find("#searchbutton").click();
        }
    });
}
function openDialogWidthOutCancel(title, link, width, height, close_type) {
    if (!width) width = 700;
    if (!height) height = 500;
    art.dialog.open(link, {
        title: title, width: width, height: height, lock: true, dbClose: false,
        ok: function () {
            if (close_type == 1) {
                var iframe = this.iframe.contentWindow;
                if (iframe.document.body) {
                    iframe.document.getElementById('dosubmit').click();
                }
            }
            else {
                this.time(3);
            }
            return false;
        },
        cancel: false
    });
}
function openDialogWidthOutOK(title, link, width, height) {
    if (!width) width = 700;
    if (!height) height = 500;
    art.dialog.open(link, {
        title: title, width: width, height: height, lock: true, dbClose: false,
        ok: false,
        cancel: true
    });
}

//列表Delete方法 url:POST地址，obj:删除链接 type:为one只删除一条记录为all删除所有选中
function Delete(url, obj, type) {
    if (type == "one") {
        art.dialog.confirm('你确认删除操作？', function () {
            $.post(url, null, function (data) {
                if (data == "1") {
                    art.dialog.tips("删除成功");
                    $(obj).parent().parent().fadeOut();
                }
                else
                    art.dialog.tips(data)
            });
        }, function () {
            $(':checkbox').removeAttr("checked").parent('span.uniform-checked').removeClass('uniform-checked');
        }).lock();
    }
    else {
        var val = "";
        $("input[name='ID']").each(function () {
            if ($(this).is(":checked")) {
                val += $(this).val() + ",";
            }
        });
        if (val != "") {
            art.dialog.confirm('你确认删除操作？', function () {
                $.post(url + "/0", { IDs: val }, function (data) {
                    if (data == "1") {
                        art.dialog.tips("删除成功");
                        location.reload();
                    }
                    else
                        art.dialog.tips(data)
                });
            }, function () {
                $(':checkbox').removeAttr("checked").parent('span.uniform-checked').removeClass('uniform-checked');
            }).lock();
        }
    }
    return;
}

//审核
function Check(id) {
    var dialog = art.dialog({
        title: '审核',
        content: '审核备注：<br/><textarea id="RejectRemark" style="width:300px;height:100px;"></textarea>',
        dbClose: false,
        button: [{
            name: '<i class="icol-accept"></i> 通过',
            callback: function () {
                $.post("/admin/TReservation/VerifyTrue/" + id, { RejectRemark: $("#RejectRemark").val() }, function (data) {
                    if (data == "1") {
                        ArtTip('<i class="icol-accept"></i> 操作成功！');
                        dialog.close();
                        datatable_load(location.href);
                    }
                    else
                        ArtTip('<i class="icol-cancel"></i> ' + data);
                });
                return false;
            }
        },
        {
            name: '<i class="icol-cancel"></i> 驳回',
            callback: function () {
                $.post("/admin/TReservation/VerifyFalse/" + id, { RejectRemark: $("#RejectRemark").val() }, function (data) {
                    if (data == "1") {
                        ArtTip('<i class="icol-accept"></i> 操作成功！');
                        dialog.close();
                        datatable_load(location.href);
                    }
                    else
                        ArtTip('<i class="icol-cancel"></i> ' + data);
                });
                return false;
            }
        }
        ]
    });
}
//关闭窗体
function windowclose() {
    var browserName = navigator.appName;
    if (browserName == "Netscape") {
        window.open('', '_self', '');
        window.close();
    }
    else {
        if (browserName == "Microsoft Internet Explorer") {
            window.opener = "whocares";
            window.opener = null;
            window.open('', '_top');
            window.close();
        }
    }
}
//去掉所有的html标记 
function delHtmlTag(str) {
    return str.replace(/<[^>]+>/g, "");
}

//附件上传
$(function () {
    try {
        //上传图片 class="image"
        KindEditor.ready(function (K) {
            $(".image").each(function () {
                var thisinput = $(this);
                var id = thisinput.attr("id");
                thisinput.parent().append("<span class='ke-button-common'><input id='btn_img_" + id + "' type='button' class='ke-button-common ke-button' value='上传图片'/></span>");
                var img_editor = K.editor({
                    allowFileManager: true,
                    themeType: 'default',
                    uploadJson: '/upload_json.ashx',
                    fileManagerJson: '/file_manager_json.ashx'
                });
                $("#btn_img_" + id).click(function () {
                    img_editor.loadPlugin('image', function () {
                        img_editor.plugin.imageDialog({
                            showRemote: false,
                            //showLocal: false,
                            imageUrl: K('#' + id).val(),
                            clickFn: function (url, title, width, height, border, align) {
                                K('#' + id).val(url);
                                img_editor.hideDialog();
                            }
                        });
                    });
                });
            });
        });
        //上传文件 class="file"
        KindEditor.ready(function (K) {
            $(".file").not(".skip").each(function () {
                var thisinput = $(this);
                var id = thisinput.attr("id");
                thisinput.parent().append("<input id='btn_file_" + id + "' type='button' class='button' value='上传附件'/>");
                var uploadbutton = K.uploadbutton({
                    button: K('#btn_file_' + id)[0],
                    fieldName: 'imgFile',
                    url: '/upload_json.ashx?dir=file',
                    afterUpload: function (data) {
                        if (data.error == 0) {
                            thisinput.val(K.formatUrl(data.url, 'absolute'));
                        } else {
                            alert(data.message);
                        }
                        art.dialog({ id: "fileDialog" }).close();
                    },
                    afterError: function (str) {
                        alert('自定义错误信息: ' + str);
                    }
                });
                uploadbutton.fileBox.change(function (e) {
                    uploadbutton.submit();
                    art.dialog({ id: "fileDialog", title: "文件上传提示", lock: true, icon: "load", dbClose: false, cancel: false, content: "正在上传文件，请稍候..." });
                });
            });
        });

        //上传文件 class="ppt"
        KindEditor.ready(function (K) {
            $(".ppt").not(".skip").each(function () {
                var thisinput = $(this);
                var id = thisinput.attr("id");
                thisinput.parent().append("<input id='btn_file_" + id + "' type='button' class='button' value='上传附件'/>");
                var uploadbutton = K.uploadbutton({
                    button: K('#btn_file_' + id)[0],
                    fieldName: 'imgFile',
                    url: '/upload_json.ashx?dir=ppt',
                    afterUpload: function (data) {
                        if (data.error == 0) {
                            thisinput.val(K.formatUrl(data.url, 'absolute'));
                        } else {
                            alert(data.message);
                        }
                        art.dialog({ id: "fileDialog" }).close();
                    },
                    afterError: function (str) {
                        alert('自定义错误信息: ' + str);
                    }
                });
                uploadbutton.fileBox.change(function (e) {
                    uploadbutton.submit();
                    art.dialog({ id: "fileDialog", title: "文件上传提示", lock: true, icon: "load", dbClose: false, cancel: false, content: "正在上传文件，请稍候..." });
                });
            });
        });

        //上传媒体文件 class="media"
        KindEditor.ready(function (K) {
            $(".media").each(function () {
                var thisinput = $(this);
                var id = thisinput.attr("id");
                thisinput.parent().append("<input id='btn_media_" + id + "' type='button' class='button' value='上传附件'/>");
                var uploadbutton = K.uploadbutton({
                    button: K('#btn_media_' + id)[0],
                    fieldName: 'imgFile',
                    url: '/upload_json.ashx?dir=media',
                    afterUpload: function (data) {
                        if (data.error == 0) {
                            thisinput.val(K.formatUrl(data.url, 'absolute'));
                        } else {
                            alert(data.message);
                        }
                        art.dialog({ id: "fileDialog" }).close();
                    },
                    afterError: function (str) {
                        alert('自定义错误信息: ' + str);
                    }
                });
                uploadbutton.fileBox.change(function (e) {
                    uploadbutton.submit();
                    art.dialog({ id: "fileDialog", title: "文件上传提示", lock: true, icon: "load", dbClose: false, cancel: false, content: "正在上传文件，请稍候..." });
                });
            });
        });

        //批量上传图片 class="images"
        KindEditor.ready(function (K) {
            $(".images").not(".skip").each(function () {
                var thisinput = $(this);
                var id = thisinput.attr("id");
                thisinput.parent().append("<span class='ke-button-common'><input id='btn_imgs_" + id + "' type='button' class='ke-button-common ke-button' value='批量上传'/></span>");
                var img_editor = K.editor({
                    allowFileManager: true,
                    themeType: 'default',
                    uploadJson: '/upload_json.ashx',
                    fileManagerJson: '/file_manager_json.ashx'
                });
                K('#btn_imgs_' + id).click(function () {
                    img_editor.loadPlugin('multiimage', function () {
                        img_editor.plugin.multiImageDialog({
                            clickFn: function (urlList) {
                                var srcs = "";
                                K.each(urlList, function (i, data) {
                                    srcs += getUrlRelativePath(data.url) + ",";
                                });
                                if (thisinput.val() != undefined && thisinput.val() != "") {
                                    srcs += thisinput.val() + ",";
                                }
                                thisinput.val(srcs.substr(0, srcs.length - 1));
                                img_editor.hideDialog();
                            }
                        });
                    });
                });
            });
        });
    }
    catch (e) { }
});

//文本编辑器
$(function () {
    try {
        //完整模式 .Fckeditor
        KindEditor.ready(function (K) {
            $(".Fckeditor").each(function () {
                var id = $(this).attr("id");
                var editor1 = K.create('#' + id, {
                    themeType: 'default',
                    cssPath: '/Areas/Admin/Content/js/kindeditor-4.1.9/plugins/code/prettify.css',
                    uploadJson: '/upload_json.ashx',
                    fileManagerJson: '/file_manager_json.ashx',
                    allowFileManager: true,
                    width: "99%",
                    height: "350px",
                    filterMode: false,
                    //afterCreate: function () {
                    //    var self = this;
                    //    K.ctrl(document, 13, function () {
                    //        self.sync();
                    //        if (editor1.isEmpty()) {
                    //            alert("请在编辑器中输入内容!");
                    //            return;
                    //        }
                    //        K('form[name=example]')[0].submit();
                    //    });
                    //    K.ctrl(self.edit.doc, 13, function () {
                    //        self.sync();
                    //        if (editor1.isEmpty()) {
                    //            alert("请在编辑器中输入内容!");
                    //            return;
                    //        }
                    //        K('form[name=example]')[0].submit();
                    //    });
                    //},
                    afterChange: function () {
                        $("#" + id).val(this.html());
                        //$("#Summary").val(delHtmlTag(this.text()));
                    }
                });
            });
        });
        //基本模式 .FckeditorBasic
        KindEditor.ready(function (K) {
            K.each({
                'plug-align': {
                    name: '对齐方式',
                    method: {
                        'justifyleft': '左对齐',
                        'justifycenter': '居中对齐',
                        'justifyright': '右对齐'
                    }
                },
                'plug-order': {
                    name: '编号',
                    method: {
                        'insertorderedlist': '数字编号',
                        'insertunorderedlist': '项目编号'
                    }
                },
                'plug-indent': {
                    name: '缩进',
                    method: {
                        'indent': '向右缩进',
                        'outdent': '向左缩进'
                    }
                }
            }, function (pluginName, pluginData) {
                var lang = {};
                lang[pluginName] = pluginData.name;
                KindEditor.lang(lang);
                KindEditor.plugin(pluginName, function (K) {
                    var self = this;
                    self.clickToolbar(pluginName, function () {
                        var menu = self.createMenu({
                            name: pluginName,
                            width: pluginData.width || 100
                        });
                        K.each(pluginData.method, function (i, v) {
                            menu.addItem({
                                title: v,
                                checked: false,
                                iconClass: pluginName + '-' + i,
                                click: function () {
                                    self.exec(i).hideMenu();
                                }
                            });
                        })
                    });
                });
            });
            $(".FckeditorBasic").each(function () {
                var id = $(this).attr("id");
                K.create('#' + id, {
                    //themeType: 'qq',
                    themeType: 'default',
                    resizeType: 0,
                    width: "450px",
                    height: "125px",
                    filterMode: false,
                    items: [
                        'source', 'bold', 'italic', 'underline', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', 'plug-align', 'plug-order'
                    ],
                    afterChange: function () {
                        $("#" + id).val(this.html());
                        //$("#Summary").val(delHtmlTag(this.text()));
                    }
                });
            });
        });
    }
    catch (e) { }
});

///CKPlayer播放器
///<script src="~/Areas/Admin/Content/js/ckplayer6.3/ckplayer/ckplayer.js"></script>
///<div id="myvideo" class="video" data-img="http://localhost:51482/Content/images/video.png" data-video="http://localhost:51482/Content/images/video.mp4" data-width="600" data-height="400"></div>
$(function () {
    $(".video").not(".skip").each(function () {
        var id = $(this).attr("id");
        var imgurl = $(this).attr("data-img");
        var videourl = $(this).attr("data-video");
        var width = $(this).attr("data-width");
        var height = $(this).attr("data-height");
        if (imgurl == undefined || imgurl == "") {
            imgurl = "http://www.ckplayer.com/images/loadimg2.jpg";
        }
        if (videourl == undefined || videourl == "") {
            videourl = "http://movie.ks.js.cn/flv/other/1_0.mp4";
        }
        if (width == undefined || width == "") {
            width = "600";
        }
        if (height == undefined || height == "") {
            height = "400";
        }
        var flashvars = {
            f: videourl,
            c: 0,
            b: 1,
            i: imgurl
        };
        var params = { bgcolor: '#FFF', allowFullScreen: true, allowScriptAccess: 'always' };
        CKobject.embedSWF('/Areas/Admin/Content/js/ckplayer6.3/ckplayer/ckplayer.swf', id, 'ckplayer_a1', width, height, flashvars, params);
        var video = [videourl + '->video/mp4', 'http://www.ckplayer.com/webm/0.webm->video/webm', 'http://www.ckplayer.com/webm/0.ogv->video/ogg'];
        var support = ['iPad', 'iPhone', 'ios', 'android+false', 'msie10+false', 'chrome'];
        CKobject.embedHTML5(id, 'ckplayer_a1', width, height, video, flashvars, support);

    });
})

//自动完成
//<input class="autocomplete" data-talbe="Member" data-field="UserName">
$(function () {
    $(".autocomplete").keyup(function () {
        var cur = $(this);
        var wordStr = cur.val();
        var tableStr = (cur.attr("data-talbe") == undefined ? "Saler" : cur.attr("data-talbe"));
        var fieldStr = (cur.attr("data-field") == undefined ? "UserName" : cur.attr("data-field"));
        if ($("#autocomplete_div").html() == undefined) {
            $("body").append("<div id='autocomplete_div' style='position:absolute;margin:0px;padding:3px;border:1px solid #ccc;display:block;width:150px;min-height:50px;height:auto;overflow:hidden;background:#fafafa;'></div>")
        }
        $("#autocomplete_div").css("left", cur.offset().left).css("top", cur.offset().top + 25);
        $("#autocomplete_div").mouseleave(function () {
            $("#autocomplete_div").slideUp(300);
        });
        $("#autocomplete_div").html("");
        $.post("/admin/Ajax/autoComplete?date=" + (new Date().getMilliseconds()), { words: wordStr, table: tableStr, field: fieldStr }, function (data) {
            if (data != "") {
                $("#autocomplete_div").html(data).slideDown(300);
                $("#autocomplete_div div").hover(function () { $(this).css({ "background": "#0563C1", "color": "#fff" }); }, function () { $(this).css({ "background": "#aaa", "color": "#000" }); });
            }
            else {
                $("#autocomplete_div").slideUp(300);
            }
            $("#autocomplete_div div").click(function () {
                cur.val($(this).html());
                $("#autocomplete_div").slideUp(300);
            });
        });
    });
    $(".autocomplete").click(function () {
        $(this).keyup();
    });
});

//省市联动
//<select id="province" name="province" data-mycity="city" val="1"></select>
$(function () {
    $("[data-mycity]").each(function () {
        var mine = $(this);
        $.post("/admin/ajax/GetProvinces", null, function (data) {
            mine.html(data);
            mine.change(function () {
                $.post("/admin/ajax/GetCitys", { "province": mine.val() }, function (sdata) {
                    $("[name='" + mine.attr("data-mycity") + "']").html(sdata).change();
                    $("[name='" + mine.attr("data-mycity") + "']").each(function () {
                        $(this).val($(this).attr("val")).change();
                    });
                });
            });
            mine.val(mine.attr("val")).change();
        });
    });
});

//隐藏显示 详见/Home/demo
//<input data-target="DrugContent" data-binddata="是" value="是"/>
//<div data-display="DrugContent"></div>
$(function () {
    $("[data-target]").each(function () {
        var mine = $(this);
        var target = $("[data-display='" + mine.attr("data-target") + "']");
        var isPass = false;
        if (mine.is("select") && mine.val() != undefined && mine.val() != "") {
            if (mine.attr("data-binddata").indexOf(mine.val()) > -1) {
                target.show();
                isPass = true;
            }
        }
        else if (mine.is("option")) {
            var mineParent = mine.parent();
            mineParent.val(mineParent.attr("val")).change();
            if (mine.val() == mineParent.val()) {
                target.show();
                isPass = true;
            }
            mineParent.unbind("change", optionEvent);
            mineParent.bind("change", optionEvent);
        }
        else if (mine.is("input")) {
            if (mine.is(":checked") && mine.val() == mine.attr("data-binddata")) {
                target.show();
                isPass = true;
            }
        }
        if (!isPass) {
            target.hide();
        }
    });
    $("[data-target]").change(function () {
        var mine = $(this);
        var target = $("[data-display='" + mine.attr("data-target") + "']");
        var isPass = false;
        if (mine.is("select") && mine.val() != undefined && mine.val() != "") {
            if (mine.attr("data-binddata").indexOf(mine.val()) > -1) {
                target.show();
                isPass = true;
            }
        }
        else if (mine.is("input")) {
            if (mine.is(":checked") && mine.val() == mine.attr("data-binddata")) {
                target.show();
                isPass = true;
            }
        }
        if (!isPass) {
            target.hide();
            if (target.is("[type='radio'],[type='checkbox']")) {
                target.removeAttr("checked").change();
            }
            else if (target.is("select")) {
                target.val("").change();
            }
            else if (target.is("[type='text'],[type='hidden']")) {
                target.val("").change();
            }
            else {
                var items1 = target.find("input[type='radio']");
                items1.removeAttr("checked").change();
                var items2 = target.find("input[type='checkbox']");
                items2.removeAttr("checked").change();
                var items3 = target.find("select");
                items3.val("").change();
                var items4 = target.find("input[type='text']");
                items4.val("").change();
                var items5 = target.find("textarea");
                items5.val("").change();
                var items6 = target.find("select");
                items6.val("").change();
            }
        }
    });
});
//option对应select的绑定事件处理函数
function optionEvent() {
    var parent = $(this);
    var isPass = false;
    parent.children("option").each(function () {
        var sTarget = $("[data-display='" + $(this).attr("data-target") + "']");
        if ($(this).val() == parent.val()) {
            sTarget.show();
            isPass = true;
        }
        else {
            sTarget.hide();
            isPass = false;
        }
        if (!isPass) {
            if (sTarget.is("[type='radio'],[type='checkbox']")) {
                sTarget.removeAttr("checked").change();
            }
            else if (sTarget.is("select")) {
                sTarget.val("").change();
            }
            else if (sTarget.is("[type='text'],[type='hidden']")) {
                sTarget.val("").change();
            }
            else {
                var items1 = sTarget.find("input[type='radio']");
                items1.removeAttr("checked").change();
                var items2 = sTarget.find("input[type='checkbox']");
                items2.removeAttr("checked").change();
                var items3 = sTarget.find("select");
                items3.val("").change();
                var items4 = sTarget.find("input[type='text']");
                items4.val("").change();
                var items5 = sTarget.find("textarea");
                items5.val("").change();
                var items6 = sTarget.find("select");
                items6.val("").change();
            }
        }
    });
}


//日期时间
function date_init() {
    if ($.fn.datetimepicker) {
        $(".datetime").not(".skip").datetimepicker({
            showMonthAfterYear: true, // 月在年之后显示        
            changeMonth: true,   // 允许选择月份        
            changeYear: true,   // 允许选择年份        
            dateFormat: 'yy-mm-dd',  // 设置日期格式   
            timeFormat: 'HH:mm:ss',
            closeText: '关闭',   // 只有showButtonPanel: true才会显示出来           duration: 'fast',
            showAnim: 'fadeIn',
            showOtherMonths: true,
            minDate: '-50y',
            maxDate: '+1y'
        });
    }
    if ($.fn.timepicker)
        $(".time").not(".skip").timepicker({
            timeFormat: 'HH:mm:ss'
        });

    if ($.fn.datepicker) {
        $(".date").not(".skip").datepicker({
            showMonthAfterYear: true, // 月在年之后显示        
            changeMonth: true,   // 允许选择月份        
            changeYear: true,   // 允许选择年份        
            dateFormat: 'yy-mm-dd',  // 设置日期格式        
            closeText: '关闭',   // 只有showButtonPanel: true才会显示出来           duration: 'fast',
            showAnim: 'fadeIn',
            showOtherMonths: true,
            minDate: '-50y',
            maxDate: '+1y'
        });
    }
}