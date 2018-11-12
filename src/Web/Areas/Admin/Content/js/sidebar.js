$(function () {
    //初始设置
    $("#sidemenu dd").hide();

    $("#sidemenu dd:first").show();
    $("#sidemenu dt:first").addClass("hover");
    $("#sidemenu i:first").removeClass("icon-chevron-down");
    $("#sidemenu i:first").addClass("icon-chevron-up");

    $("#sidemenu dl dt").each(function (index) {
        $(this).click(function () {
            if ($(this).next().css("display") == "none") {
                $("#sidemenu dl dt").removeClass("hover");
                $("#sidemenu dl dd").slideUp();
                $(".arrow-icon").removeClass("icon-chevron-up").addClass("icon-chevron-down");
                $(this).addClass("hover");
                $(this).next().slideDown();
                $(this).find(".arrow-icon").removeClass("icon-chevron-down").addClass("icon-chevron-up");
            } else {

            }
        });

    });
    $("#sidemenu dl li a").each(function (index) {
        $(this).click(function () {
            $("#sidemenu dl li a").removeClass("hover");
            $(this).addClass("hover");
        });
    });

});