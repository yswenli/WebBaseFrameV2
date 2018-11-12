using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //jquery
            bundles.Add(new StyleBundle("~/areas/admin/content/js/jquery-ui").Include(
                "~/areas/admin/content/js/jquery-ui/jquery-ui.css"
                ));
            bundles.Add(new StyleBundle("~/areas/admin/content/js/jquery.timepicker.addon").Include(
                "~/areas/admin/content/js/jquery.timepicker.addon/jquery-ui-timepicker-addon.css"
                ));
            bundles.Add(new StyleBundle("~/areas/admin/content/js/ibutton").Include(
                "~/areas/admin/content/js/ibutton/jquery.ibutton.css"
                ));
            bundles.Add(new ScriptBundle("~/bundles/jQuery").Include(
                "~/areas/admin/content/js/jquery.min.js"
                ));
            bundles.Add(new ScriptBundle("~/areas/admin/content/js").Include(
                "~/areas/admin/content/js/jquery.sGallery.js",
                "~/areas/admin/content/js/jquery.placeholder.min.js",
                "~/areas/admin/Content/js/sidebar.js",
                "~/areas/admin/content/js/scripts.js"
                ));
            bundles.Add(new ScriptBundle("~/areas/admin/content/js/jquery-ui").Include(
                "~/areas/admin/content/js/jquery-ui/jquery-ui.js"
                ));
            bundles.Add(new ScriptBundle("~/areas/admin/content/js/jquery.timepicker.addon").Include(
                "~/areas/admin/content/js/jquery.timepicker.addon/jquery-ui-timepicker-addon.js"
                ));
            bundles.Add(new ScriptBundle("~/areas/admin/content/js/jquery.timepicker.addon/i18n").Include(
                "~/areas/admin/content/js/jquery.timepicker.addon/i18n/jquery-ui-timepicker-zh-CN.js"
                ));
            bundles.Add(new ScriptBundle("~/areas/admin/content/js/sheepit").Include(
                "~/areas/admin/content/js/sheepit/jquery.sheepit.js"
                ));
            bundles.Add(new ScriptBundle("~/areas/admin/content/js/ibutton").Include(
                "~/areas/admin/content/js/ibutton/jquery.ibutton.min.js"
                ));
            //自定义添加Bootstrap            
            bundles.Add(new StyleBundle("~/areas/admin/content/js/bootstrap/css").Include(
                "~/areas/admin/content/js/bootstrap/css/bootstrap.css",
                "~/areas/admin/content/js/bootstrap/css/bootstrap-responsive.css"
                ));
            bundles.Add(new ScriptBundle("~/Areas/Admin/Content/js/bootstrap/js").Include(
                "~/Areas/Admin/Content/js/bootstrap/js/bootstrap.min.js"
                ));
            //
            bundles.Add(new StyleBundle("~/areas/admin/content").Include(
                "~/areas/admin/content/login.min.css",
                "~/areas/admin/content/print.css",
                "~/areas/admin/content/css.css"
                ));
            //uniform
            bundles.Add(new StyleBundle("~/areas/admin/content/js/uniform/css").Include(
                "~/areas/admin/content/js/uniform/css/uniform.default.css"
                ));
            bundles.Add(new ScriptBundle("~/areas/admin/content/js/uniform").Include(
                "~/areas/admin/content/js/uniform/jquery.uniform.min.js"
                ));
            //formvalidator
            bundles.Add(new StyleBundle("~/areas/admin/content/js/formvalidator").Include(
                "~/areas/admin/content/js/formvalidator/formvalidator.css"
                ));
            bundles.Add(new ScriptBundle("~/areas/admin/content/js/formvalidator").Include(
                "~/areas/admin/content/js/formvalidator/formvalidator.js",
                "~/areas/admin/content/js/formvalidator/formvalidatorregex.js"
                ));
            //artDialog4
            bundles.Add(new StyleBundle("~/areas/admin/content/js/artDialog/skins").Include(
                "~/areas/admin/content/js/artDialog/skins/blue.css"
                ));
            bundles.Add(new ScriptBundle("~/Areas/Admin/Content/js/artDialog").Include(
                "~/Areas/Admin/Content/js/artDialog/artDialog.source.js",
                "~/Areas/Admin/Content/js/artDialog/plugins/iframeTools.source.js"
                ));
            //ckeditor
            bundles.Add(new ScriptBundle("~/areas/admin/content/js/ckeditor").Include(
                "~/areas/admin/content/js/ckeditor/ckeditor_basic.js"
                ));
            //swfupload
            bundles.Add(new ScriptBundle("~/areas/admin/content/js/swfupload").Include(
                "~/areas/admin/content/js/swfupload/swf2ckeditor.js"
                ));
            //wizard
            bundles.Add(new StyleBundle("~/areas/admin/content/js/wizard").Include(
                "~/areas/admin/content/js/wizard/wizard.css"
                ));
            //picklist
            bundles.Add(new StyleBundle("~/areas/admin/content/js/picklist").Include(
                "~/areas/admin/content/js/picklist/picklist.css"
                ));
            bundles.Add(new ScriptBundle("~/areas/admin/content/js/picklist").Include(
               "~/areas/admin/content/js/picklist/picklist.min.js"
               ));
            //select2
            bundles.Add(new StyleBundle("~/areas/admin/content/js/select2").Include(
                "~/areas/admin/content/js/select2/select2.css"
                ));
            bundles.Add(new ScriptBundle("~/areas/admin/content/js/select2").Include(
               "~/areas/admin/content/js/select2/select2.min.js"
               ));
            //icon
            bundles.Add(new StyleBundle("~/areas/admin/content/icons").Include(
               "~/areas/admin/content/icons/icons.css"
               ));
            //kindeditor-4.1.9
            bundles.Add(new StyleBundle("~/Areas/Admin/Content/js/kindeditor-4.1.9/plugins/code").Include(
                "~/Areas/Admin/Content/js/kindeditor-4.1.9/plugins/code/prettify.css"
                ));
            bundles.Add(new ScriptBundle("~/Areas/Admin/Content/js/kindeditor-4.1.9").Include(
                "~/Areas/Admin/Content/js/kindeditor-4.1.9/kindeditor.js"
                ));
            bundles.Add(new ScriptBundle("~/Areas/Admin/Content/js/kindeditor-4.1.9/lang").Include(
                "~/Areas/Admin/Content/js/kindeditor-4.1.9/lang/zh_CN.js"
                ));
            bundles.Add(new ScriptBundle("~/Areas/Admin/Content/js/kindeditor-4.1.9/plugins/code").Include(
                "~/Areas/Admin/Content/js/kindeditor-4.1.9/plugins/code/prettify.js"
                ));
        }
    }
}