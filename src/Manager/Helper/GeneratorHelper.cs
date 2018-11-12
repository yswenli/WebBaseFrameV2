using Common;
using System;
//
using System.Data;
using System.Linq;
using System.Text;
using WEF;

namespace Web.Areas.Manager.Helper
{
    /// <summary>
    /// 生成辅助类
    /// </summary>
    public class GeneratorHelper
    {
        //
        private string constring = "";
        /// <summary>
        /// 是否覆盖
        /// </summary>
        private bool isCovert = false;
        //
        string filePath = PathHelper.BasePath;

        /// <summary>
        /// 生成可空字段时替换
        /// </summary>
        /// <param name="collumn"></param>
        /// <returns></returns>
        private string RelaceType(DataColumn collumn)
        {
            string result = "";
            if (collumn.DataType.Name == "String")
            {
                result = "String";
            }
            else
            {
                if (collumn.AllowDBNull)
                {
                    result = collumn.DataType.Name + "?";
                }
                else
                {
                    result = collumn.DataType.Name;
                }
            }
            return result;
        }


        #region 根据模块生成内容方法集合
        //创建Entity
        void CreateEntity(string table)
        {
            DirHelper.CheckFolder(PathHelper.Entitypath);
            DataTable dt = new DBContext(constring).GetMap(table);
            string savePath = PathHelper.Entitypath + table + ".cs";

            string templatePath = PathHelper.SelectedTempPath + "Entity.txt";
            string content = FileHelper.ReadFile(templatePath);

            content = content.Replace("[ClassName]", table);
            StringBuilder sb = new StringBuilder();
            sb.Append("        /// <summary>\r\n        /// 属性更改通知\r\n        /// </summary>\r\n");
            sb.Append("        private List<string> _ChangedList = new List<string>();\r\n");
            sb.Append("        /// <summary>\r\n        /// 属性更改通知\r\n        /// </summary>\r\n");
            sb.Append("        [ColumnAttribute(\"ChangedList\", true, false, true)]\r\n");
            sb.Append("        public List<string> ChangedList{get{return _ChangedList;}}\r\n");
            sb.Append("        /// <summary>\r\n        /// 客户端通知事件\r\n        /// </summary>\r\n");
            sb.Append("        public event PropertyChangedEventHandler PropertyChanged;\r\n");
            sb.Append("        protected virtual void OnPropertyChanged(string propName)\r\n        {\r\n");
            sb.Append("                if (_ChangedList == null || !_ChangedList.Contains(propName))\r\n                {\r\n");
            sb.Append("                        _ChangedList.Add(propName);\r\n");
            sb.Append("\r\n                }\r\n                 if(PropertyChanged != null)\r\n                {\r\n");
            sb.Append("                        PropertyChanged(this, new PropertyChangedEventArgs(propName));\r\n");
            sb.Append("\r\n                }\r\n        }\r\n");
            var d_dt = DBColumnDescriptionHelper.GetDescriptions(constring, table);
            foreach (DataColumn item in dt.Columns)
            {
                string MS_Description = DBColumnDescriptionHelper.GetDescription(d_dt, item.ColumnName);
                sb.Append("        /// <summary>\r\n        ///" + MS_Description + "\r\n        /// </summary>\r\n");
                sb.Append("        private " + RelaceType(item) + " _" + item.ColumnName + ";\r\n");
                sb.Append("        /// <summary>\r\n        ///" + MS_Description + "\r\n        /// </summary>\r\n");
                sb.Append("        [ColumnAttribute(\"" + item.ColumnName + "\", false, " + (item.AutoIncrement == true ? "true" : "false") + ", " + (item.AllowDBNull == true ? "true" : "false") + ")]\r\n");
                sb.Append("        public " + RelaceType(item) + " " + item.ColumnName + " { get { return _" + item.ColumnName + ";} set{");
                sb.Append("_" + item.ColumnName + " = value;OnPropertyChanged(\"" + item.ColumnName + "\");");
                sb.Append("} } \r\n\r\n\r\n");
            }
            content = content.Replace("[FieldName]", sb.ToString());
            if (isCovert)
            {
                FileHelper.WriteFile(savePath, content);
            }
        }
        //创建Service
        void CreateService(string table)
        {
            DirHelper.CheckFolder(filePath + "..\\Models\\Service");
            string savePath = filePath + "..\\Models\\Service\\" + table + "Service.cs";

            string templatePath = PathHelper.SelectedTempPath + "Service.txt";
            if (!System.IO.File.Exists(savePath))
            {
                SaveFile(table, templatePath, savePath);
            }
        }
        //创建Logic
        void CreateLogic(string table)
        {

            DirHelper.CheckFolder(filePath + "..\\Models\\Logic");
            string savePath = filePath + "..\\Models\\Logic\\" + table + "Logic.cs";

            string templatePath = PathHelper.SelectedTempPath + "Logic.txt";
            if (!System.IO.File.Exists(savePath))
            {
                SaveFile(table, templatePath, savePath);
            }
        }
        //创建Controller
        void CreateController(string table)
        {
            string savePath = PathHelper.Controllerpath + table + "Controller.cs";
            string templatePath = PathHelper.SelectedTempPath + "Controller.txt";
            if (isCovert)
            {
                SaveFile(table, templatePath, savePath);
            }
        }
        //创建APIController
        void CreateAPI(string table)
        {
            //string savePath = filePath + "Web\\Areas\\API\\Controllers\\" + table + "Controller.cs";

            //string templatePath = filePath + "AutoGenerator\\Template\\API.txt";

            //SaveFile(table, templatePath, savePath);
        }
        //创建列表页面
        void CreateIndex(string table)
        {
            string savePath = PathHelper.Viewpath + table + "\\Index.cshtml";
            string templatePath = PathHelper.SelectedTempPath + "Index.txt";
            if (isCovert)
            {
                SaveFile(table, templatePath, savePath);
            }
        }
        //创建列表控件
        void Create_Index(string table)
        {
            DataTable dt = new DBContext(constring).GetMap(table);
            string savePath = PathHelper.Viewpath + table + "\\_Index.cshtml";
            string templatePath = PathHelper.SelectedTempPath + "_Index.txt";
            string content = FileHelper.ReadFile(templatePath);
            StringBuilder fieldName = new StringBuilder();
            StringBuilder itemName = new StringBuilder();
            //
            var d_dt = DBColumnDescriptionHelper.GetDescriptions(constring, table);
            foreach (DataColumn item in dt.Columns)
            {
                string MS_Description = DBColumnDescriptionHelper.GetDescription(d_dt, item.ColumnName);
                if (!Enum.GetNames(typeof(Common.EnumHelper.BaseField)).Contains(item.ColumnName) || item.ColumnName == "ID")
                {

                    itemName.Append("               <th class=\"sorting\" data-sort=\"" + item.ColumnName + "\">" + MS_Description + "</th>\r\n");


                    if (item.DataType == typeof(DateTime) || item.DataType == typeof(Boolean))
                    {
                        fieldName.Append("               <td>@item." + item.ColumnName + ".Format()</td>\r\n");
                    }
                    else
                    {
                        fieldName.Append("               <td>@item." + item.ColumnName + "</td>\r\n");
                    }
                }
            }
            content = content.Replace("[ClassName]", table).Replace("[ModelName]", table).Replace("[FieldName]", fieldName.ToString()).Replace("[ItemName]", itemName.ToString());
            if (isCovert)
            {
                FileHelper.WriteFile(savePath, content);
            }
        }
        //创建表单控件
        void Create_Form(string table)
        {
            DataTable dt = new DBContext(constring).GetMap(table);
            string savePath = PathHelper.Viewpath + table + "\\_Form.cshtml";
            string templatePath = PathHelper.SelectedTempPath + "_Form.txt";
            string content = FileHelper.ReadFile(templatePath);
            StringBuilder field = new StringBuilder();
            //
            var d_dt = DBColumnDescriptionHelper.GetDescriptions(constring, table);
            foreach (DataColumn item in dt.Columns)
            {
                string MS_Description = DBColumnDescriptionHelper.GetDescription(d_dt, item.ColumnName);
                if (!Enum.GetNames(typeof(Common.EnumHelper.BaseField)).Contains(item.ColumnName) || item.Unique)
                {
                    if (item.Unique == true)
                    {
                        field.Append("<div class=\"control-group\"><label class=\"control-label\">" + MS_Description +
                                "</label><div class=\"controls\"><input id=\"" + item.ColumnName + "\" name =\"" + item.ColumnName +
                                "\" type=\"text\" value =\"@Model." + item.ColumnName + "\" required readonly=\"readonly\" /></div></div>\r\n");
                    }
                    else
                    {
                        var typeAndLengthTB = new DBContext().GetTypeAndLength(table, item.ColumnName);
                        string inputFlag = "1";
                        if (item.AllowDBNull)
                        {
                            inputFlag = "0";
                        }
                        if (typeAndLengthTB.Rows[0][1].ToString().IndexOf("char") > -1)
                        {
                            if (Convert.ToInt32(typeAndLengthTB.Rows[0][2]) >= 200 && Convert.ToInt32(typeAndLengthTB.Rows[0][2]) < 1000)
                            {
                                inputFlag = "1" + inputFlag;
                            }
                            else if (Convert.ToInt32(typeAndLengthTB.Rows[0][2]) >= 1000)
                            {
                                inputFlag = "2" + inputFlag;
                            }
                        }
                        if (typeAndLengthTB.Rows[0][1].ToString().IndexOf("text") > -1)
                        {
                            inputFlag = "2" + inputFlag;
                        }
                        if (typeAndLengthTB.Rows[0][1].ToString().IndexOf("date") > -1)
                        {
                            inputFlag = "3" + inputFlag;
                        }
                        if (typeAndLengthTB.Rows[0][1].ToString().IndexOf("bit") > -1)
                        {
                            inputFlag = "4" + inputFlag;
                        }
                        if (typeAndLengthTB.Rows[0][1].ToString().IndexOf("int") > -1 || typeAndLengthTB.Rows[0][1].ToString().IndexOf("float") > -1 || typeAndLengthTB.Rows[0][1].ToString().IndexOf("decimal") > -1)
                        {
                            inputFlag = "5" + inputFlag;
                        }
                        switch (inputFlag)
                        {
                            case "0":
                                field.Append("<div class=\"control-group\"><label class=\"control-label\">" + MS_Description +
                                "</label><div class=\"controls\"><input id=\"" + item.ColumnName + "\" name =\"" + item.ColumnName +
                                "\" type=\"text\" value =\"@Model." + item.ColumnName + "\" /></div></div>\r\n");
                                break;
                            case "10":
                                field.Append("<div class=\"control-group\"><label class=\"control-label\">" + MS_Description +
                                "</label><div class=\"controls\"><textarea id=\"" + item.ColumnName + "\" name =\"" + item.ColumnName +
                                "\">@Model." + item.ColumnName + "</textarea></div></div>\r\n");
                                break;
                            case "11":
                                field.Append("<div class=\"control-group\"><label class=\"control-label\">" + MS_Description +
                                "</label><div class=\"controls\"><textarea id=\"" + item.ColumnName + "\" name =\"" + item.ColumnName +
                                "\" required>@Model." + item.ColumnName + "</textarea></div></div>\r\n");
                                break;
                            case "20":
                                field.Append("<div class=\"control-group\"><label class=\"control-label\">" + MS_Description +
                                "</label><div class=\"controls\"><input id=\"" + item.ColumnName + "\" name =\"" + item.ColumnName +
                                "\" type=\"text\" value =\"@Model." + item.ColumnName + "\" class=\"FckeditorBasic\" /></div></div>\r\n");
                                break;
                            case "21":
                                field.Append("<div class=\"control-group\"><label class=\"control-label\">" + MS_Description +
                                "</label><div class=\"controls\"><input id=\"" + item.ColumnName + "\" name =\"" + item.ColumnName +
                                "\" type=\"text\" value =\"@Model." + item.ColumnName + "\" class=\"FckeditorBasic\" required /></div></div>\r\n");
                                break;
                            case "30":
                                field.Append("<div class=\"control-group\"><label class=\"control-label\">" + MS_Description +
                                "</label><div class=\"controls\"><input id=\"" + item.ColumnName + "\" name =\"" + item.ColumnName +
                                "\" type=\"text\" class=\"date\" value =\"@Model." + item.ColumnName + ".Format()\" /></div></div>\r\n");
                                break;
                            case "31":
                                field.Append("<div class=\"control-group\"><label class=\"control-label\">" + MS_Description +
                                "</label><div class=\"controls\"><input id=\"" + item.ColumnName + "\" name =\"" + item.ColumnName +
                                "\" type=\"text\" class=\"date\" value =\"@Model." + item.ColumnName + ".Format()\" required /></div></div>\r\n");
                                break;
                            case "40":
                                field.Append("<div class=\"control-group\"><label class=\"control-label\">" + MS_Description +
                                "</label><div class=\"controls\"><select id=\"" + item.ColumnName + "\" name =\"" + item.ColumnName +
                                "\" val=\"@Model." + item.ColumnName + "\"><option value=\"\">请选择..</option><option value=\"True\">是</option>" +
                                "<option value=\"False\">否</option></select></div></div>\r\n");
                                break;
                            case "41":
                                field.Append("<div class=\"control-group\"><label class=\"control-label\">" + MS_Description +
                                "</label><div class=\"controls\"><select id=\"" + item.ColumnName + "\" name =\"" + item.ColumnName +
                                "\" val=\"@Model." + item.ColumnName + "\" required><option value=\"\">请选择..</option><option value=\"True\">是</option>" +
                                "<option value=\"False\">否</option></select></div></div>\r\n");
                                break;
                            case "50":
                                field.Append("<div class=\"control-group\"><label class=\"control-label\">" + MS_Description +
                                "</label><div class=\"controls\"><input id=\"" + item.ColumnName + "\" name =\"" + item.ColumnName +
                                "\" type=\"text\" value =\"@Model." + item.ColumnName + "\" style=\"width:30px;\"/></div></div>\r\n");
                                break;
                            case "51":
                                field.Append("<div class=\"control-group\"><label class=\"control-label\">" + MS_Description +
                                "</label><div class=\"controls\"><input id=\"" + item.ColumnName + "\" name =\"" + item.ColumnName +
                                "\" type=\"text\" value =\"@Model." + item.ColumnName + "\" style=\"width:30px;\" required/></div></div>\r\n");
                                break;
                            case "1":
                            default:
                                field.Append("<div class=\"control-group\"><label class=\"control-label\">" + MS_Description +
                                "</label><div class=\"controls\"><input id=\"" + item.ColumnName + "\" name =\"" + item.ColumnName +
                                "\" type=\"text\" value =\"@Model." + item.ColumnName + "\" required /></div></div>\r\n");
                                break;
                        }
                    }
                }
            }
            content = content.Replace("[ClassName]", table).Replace("[ModelName]", table).Replace("[Field]", field.ToString());
            if (isCovert)
            {
                FileHelper.WriteFile(savePath, content);
            }
        }
        //创建新增页面
        void CreateCreate(string table)
        {
            string savePath = PathHelper.Viewpath + table + "\\Create.cshtml";
            string templatePath = PathHelper.SelectedTempPath + "Create.txt";
            if (isCovert)
            {
                SaveFile(table, templatePath, savePath);
            }
        }
        //创建修改页面
        void CreateEdit(string table)
        {
            string savePath = PathHelper.Viewpath + table + "\\Edit.cshtml";
            string templatePath = PathHelper.SelectedTempPath + "Edit.txt";
            if (isCovert)
            {
                SaveFile(table, templatePath, savePath);
            }
        }
        //创建详细控件
        void Create_Detail(string table)
        {
            DataTable dt = new DBContext(constring).GetMap(table);
            string savePath = PathHelper.Viewpath + table + "\\_Detail.cshtml";
            string templatePath = PathHelper.SelectedTempPath + "_Detail.txt";
            string content = FileHelper.ReadFile(templatePath);
            StringBuilder field = new StringBuilder();
            //
            var d_dt = DBColumnDescriptionHelper.GetDescriptions(constring, table);
            foreach (DataColumn item in dt.Columns)
            {
                string MS_Description = DBColumnDescriptionHelper.GetDescription(d_dt, item.ColumnName);
                if (!Enum.GetNames(typeof(Common.EnumHelper.BaseField)).Contains(item.ColumnName))
                    field.Append("<div class=\"control-group\"><label class=\"control-label\">" + MS_Description +
                        "</label><div class=\"controls padt5\">@Model." + item.ColumnName + "</div></div>\r\n");
            }

            content = content.Replace("[ClassName]", table).Replace("[ModelName]", table).Replace("[Field]", field.ToString());
            if (isCovert)
            {
                FileHelper.WriteFile(savePath, content);
            }
        }
        //创建详细页面
        void CreateDetail(string table)
        {
            string savePath = PathHelper.Viewpath + table + "\\Detail.cshtml";
            string templatePath = PathHelper.SelectedTempPath + "Detail.txt";
            if (isCovert)
            {
                SaveFile(table, templatePath, savePath);
            }
        }
        //创建生成文件并保存
        void SaveFile(string table, string templatePath, string savePath)
        {
            string content = FileHelper.ReadFile(templatePath);
            content = content.Replace("[ClassName]", table).Replace("[ModelName]", table);
            FileHelper.WriteFile(savePath, content);
        }
        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="iscovert"></param>
        public GeneratorHelper(string cnn, bool iscovert)
        {
            constring = cnn;
            isCovert = iscovert;
        }

        /// <summary>
        /// 从模板生成文件
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="template">生成项</param>
        public void CreateFileFromTemplate(string table, string template)
        {
            switch (template)
            {
                case "Entity":
                    CreateEntity(table);
                    ProjectFileHelper.AddEntities(table);
                    break;
                case "Logic":
                    CreateLogic(table);
                    ProjectFileHelper.AddLogic(table);
                    break;
                case "Service":
                    CreateService(table);
                    ProjectFileHelper.AddService(table);
                    break;
                case "Controller":
                    CreateController(table);
                    ProjectFileHelper.AddControllers(table);
                    break;
                case "Index":
                    CreateIndex(table);
                    Create_Index(table);
                    ProjectFileHelper.AddViews(table, "Index.cshtml");
                    ProjectFileHelper.AddViews(table, "_Index.cshtml");
                    break;
                case "Create":
                    CreateCreate(table);
                    ProjectFileHelper.AddViews(table, "Create.cshtml");
                    break;
                case "Edit":
                    CreateEdit(table);
                    ProjectFileHelper.AddViews(table, "Edit.cshtml");
                    break;
                case "_Form":
                    Create_Form(table);
                    ProjectFileHelper.AddViews(table, "_Form.cshtml");
                    break;
                case "Detail":
                    Create_Detail(table);
                    CreateDetail(table);
                    ProjectFileHelper.AddViews(table, "Detail.cshtml");
                    ProjectFileHelper.AddViews(table, "_Detail.cshtml");
                    break;
                case "API":
                    CreateAPI(table);
                    break;
            }
        }
    }
}