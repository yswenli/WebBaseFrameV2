// 设置对话框全局默认配置
(function () {
    var d = art.dialog.defaults;
    // 按需加载要用到的皮肤，数组第一个为默认皮肤
    // 如果只使用默认皮肤，可以不填写skin
    d.skin = ['black', 'default', 'chrome', 'facebook', 'aero'];
    alert(d.skin);
    // 支持拖动
    d.drag = true;
    // 超过此面积大小的对话框使用替身拖动
    d.showTemp = 100000;
})();