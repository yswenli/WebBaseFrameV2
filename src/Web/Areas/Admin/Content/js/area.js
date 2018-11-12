//查询所有省份,市医院联动(注意：多级联动时，必须给下拉控件增加两个属性
//val 属性 表示编辑时当前下拉框选中的值
//ChildNode 属性 表示联动是的下一个控件的ID，且ChildNode属性的值必须在页面唯一

$(function () {
    //绑定所有省份
    $(".Province").each(function (j, obj) {
        //属性val表示编辑时当前选中的值
        var CurrentSelectedValue = $(this).attr("val"); //当前选中值

        var ChildNode = $(obj).attr("ChildNode");

        //绑定省份值改变事件
        $(obj).bind("change", function (event) {

            $("#" + ChildNode).bind("change", function (event) {

                GetHospital($(this).val(), $(this).attr("ChildNode"));
            });
            GetCtiy($(checkbox).val(), ChildNode);
        })

        $.get('/Area/FindProvince/?CurrentSelectedValue=' + CurrentSelectedValue, function (date) {
            $(checkbox).html(date); //绑定省
            $(checkbox).trigger("change"); //立即触发城市值改变事件

        });
    });
    //绑定城市值改变事件
    //    $(".City").each(function (j, checkbox) {
    //        //绑定省份值改变事件
    //        $(checkbox).bind("change", function (event) {
    //            alert("进来");
    //            GetHospital($(checkbox).val(), $(checkbox).attr("ChildNode"));
    //        });
    //    });

    //绑定联系地址身份
    $(".ProvinceAddress").each(function (j, checkbox) {
        //属性CurrentSelectedValue表示编辑时当前选中的值
        var CurrentSelectedValue = $(this).attr("CurrentSelectedValue"); //当前选中值

        //绑定省份值改变事件
        $(checkbox).bind("change", function (event) {
            GetCtiyAddress($(checkbox).val(), $(checkbox).attr("ChildNode"));
        });

        $.get('/Area/FindProvinceAddress/?CurrentSelectedValue=' + CurrentSelectedValue, function (date) {
            $(checkbox).html(date); //绑定省
            $(checkbox).trigger("change"); //立即触发城市值改变事件
        });

    });

});
//省份值改变，绑定城市下拉框的值
function GetCtiy(ProvinceID, CityControlID) {
    //alert("ProvinceID"+ProvinceID);
    var CurrentSelectedValue = $("#" + CityControlID).attr("CurrentSelectedValue"); //当前选中值
    // alert("CurrentSelectedValue"+CurrentSelectedValue);
    $.get('/Area/FindCity/?ProvinceID=' + ProvinceID + "&CurrentSelectedValue=" + CurrentSelectedValue, function (date) {
        $("#" + CityControlID).html(date); //绑定市
        $("#" + CityControlID).trigger("change"); //立即触发城市值改变事件
    });
}

//城市值改变，绑定医院下拉框的值
function GetHospital(CityID, HospitalControlID) {
    var CurrentSelectedValue = $("#" + HospitalControlID).attr("CurrentSelectedValue"); //当前选中值
    $.get('/Area/FindHospital/?CityID=' + CityID + "&CurrentSelectedValue=" + CurrentSelectedValue, function (date) {
        $("#" + HospitalControlID).html(date); //绑定医院
        // $("#" + HospitalControlID).trigger("change"); //立即触发城市值改变事件
    });
}

//省联系地址改变，绑定城市地址下拉框的值
function GetCtiyAddress(ProvinceID, CityControlID) {
    var CurrentSelectedValue = $("#" + CityControlID).attr("CurrentSelectedValue"); //当前选中值
    $.get('/Area/FindCityAddress/?ProvinceID=' + ProvinceID + "&CurrentSelectedValue=" + CurrentSelectedValue, function (date) {

        $("#" + CityControlID).html(date); //绑定市
    });
}