using Common;
using MvcPager;
using System;
using System.Linq;
using System.Web.Mvc;
using WEF;
using WEF.Common;
using WEF.MvcPager;

namespace Web.Areas.Manager.Controllers
{
    public class DataBaseController : BaseController
    {
        public DataBaseController()
        {
            //if (!SqliteDBHelper.IsMSSQL)
            //{
            //    System.Web.HttpContext.Current.Response.Redirect("/Manager/SqliteDataBase/Tables");
            //}
        }


        DBContext db = new DBContext();


        /// <summary>
        /// 数据库结构列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Index(string tableName, int? pageIndex, int? pageSize)
        {
            string sql = "SELECT 数据表=CASE WHEN C.column_id=1 THEN O.name ELSE N'' END, 表描述=ISNULL(CASE WHEN C.column_id=1 THEN PTB.[value] END,N''), 列名=C.name, 主键=ISNULL(IDX.PrimaryKey,N''), 标识=CASE WHEN C.is_identity=1 THEN N'√'ELSE N'' END, 类型=T.name, 长度=C.max_length, 可空=CASE WHEN C.is_nullable=1 THEN N'√'ELSE N'' END, 默认值=ISNULL(D.definition,N''), 列描述=ISNULL(PFD.[value],N''), 创建日期=O.Create_Date, 更改日期=O.Modify_date FROM sys.columns C INNER JOIN sys.objects O ON C.[object_id]=O.[object_id] AND O.type='U' AND O.is_ms_shipped=0 INNER JOIN sys.types T ON C.user_type_id=T.user_type_id LEFT JOIN sys.default_constraints D ON C.[object_id]=D.parent_object_id AND C.column_id=D.parent_column_id AND C.default_object_id=D.[object_id] LEFT JOIN sys.extended_properties PFD ON PFD.class=1  AND C.[object_id]=PFD.major_id  AND C.column_id=PFD.minor_id LEFT JOIN sys.extended_properties PTB ON PTB.class=1  AND PTB.minor_id=0  AND C.[object_id]=PTB.major_id LEFT JOIN  (SELECT IDXC.[object_id], IDXC.column_id, Sort=CASE INDEXKEY_PROPERTY(IDXC.[object_id],IDXC.index_id,IDXC.index_column_id,'IsDescending') WHEN 1 THEN 'DESC' WHEN 0 THEN 'ASC' ELSE '' END, PrimaryKey=CASE WHEN IDX.is_primary_key=1 THEN N'√'ELSE N'' END, IndexName=IDX.Name FROM sys.indexes IDX INNER JOIN sys.index_columns IDXC ON IDX.[object_id]=IDXC.[object_id] AND IDX.index_id=IDXC.index_id LEFT JOIN sys.key_constraints KC ON IDX.[object_id]=KC.[parent_object_id] AND IDX.index_id=KC.unique_index_id INNER JOIN (SELECT [object_id], Column_id, index_id=MIN(index_id) FROM sys.index_columns GROUP BY [object_id], Column_id ) IDXCUQ ON IDXC.[object_id]=IDXCUQ.[object_id] AND IDXC.Column_id=IDXCUQ.Column_id AND IDXC.index_id=IDXCUQ.index_id ) IDX ON C.[object_id]=IDX.[object_id] AND C.column_id=IDX.column_id ";
            if (!string.IsNullOrEmpty(tableName))
            {
                tableName = tableName.Replace("'", "").Replace("[", "").Replace("]", "").Replace("%", "");
                sql += " WHERE o.Name like '%" + tableName + "%'";
            }
            sql += " ORDER BY O.name,C.column_id";
            var dt = db.ExecuteDataTable(sql);
            var sql2 = "SELECT top 0 数据表=CASE WHEN C.column_id=1 THEN O.name ELSE N'' END, 表描述=ISNULL(CASE WHEN C.column_id=1 THEN PTB.[value] END,N''), 列名=C.name, 主键=ISNULL(IDX.PrimaryKey,N''), 标识=CASE WHEN C.is_identity=1 THEN N'√'ELSE N'' END, 类型=T.name, 长度=C.max_length, 可空=CASE WHEN C.is_nullable=1 THEN N'√'ELSE N'' END, 默认值=ISNULL(D.definition,N''), 列描述=ISNULL(PFD.[value],N''), 创建日期=O.Create_Date, 更改日期=O.Modify_date FROM sys.columns C INNER JOIN sys.objects O ON C.[object_id]=O.[object_id] AND O.type='U' AND O.is_ms_shipped=0 INNER JOIN sys.types T ON C.user_type_id=T.user_type_id LEFT JOIN sys.default_constraints D ON C.[object_id]=D.parent_object_id AND C.column_id=D.parent_column_id AND C.default_object_id=D.[object_id] LEFT JOIN sys.extended_properties PFD ON PFD.class=1  AND C.[object_id]=PFD.major_id  AND C.column_id=PFD.minor_id LEFT JOIN sys.extended_properties PTB ON PTB.class=1  AND PTB.minor_id=0  AND C.[object_id]=PTB.major_id LEFT JOIN  (SELECT IDXC.[object_id], IDXC.column_id, Sort=CASE INDEXKEY_PROPERTY(IDXC.[object_id],IDXC.index_id,IDXC.index_column_id,'IsDescending') WHEN 1 THEN 'DESC' WHEN 0 THEN 'ASC' ELSE '' END, PrimaryKey=CASE WHEN IDX.is_primary_key=1 THEN N'√'ELSE N'' END, IndexName=IDX.Name FROM sys.indexes IDX INNER JOIN sys.index_columns IDXC ON IDX.[object_id]=IDXC.[object_id] AND IDX.index_id=IDXC.index_id LEFT JOIN sys.key_constraints KC ON IDX.[object_id]=KC.[parent_object_id] AND IDX.index_id=KC.unique_index_id INNER JOIN (SELECT [object_id], Column_id, index_id=MIN(index_id) FROM sys.index_columns GROUP BY [object_id], Column_id ) IDXCUQ ON IDXC.[object_id]=IDXCUQ.[object_id] AND IDXC.Column_id=IDXCUQ.Column_id AND IDXC.index_id=IDXCUQ.index_id ) IDX ON C.[object_id]=IDX.[object_id] AND C.column_id=IDX.column_id ";
            if (!string.IsNullOrEmpty(tableName))
            {
                tableName = tableName.Replace("'", "").Replace("[", "").Replace("]", "").Replace("%", "");
                sql2 += " WHERE o.Name like '%" + tableName + "%'";
            }
            sql2 += " ORDER BY O.name,C.column_id";
            var columnDt = db.ExecuteDataTable(sql2);
            ViewData["column"] = columnDt;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Index", dt.ToPagedList(pageIndex ?? 1, pageSize ?? 20));
            }
            return View(dt.ToPagedList(pageIndex ?? 1, pageSize ?? 20));
        }

        /// <summary>
        /// 添加表
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateTable()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTable(string tableName, bool basicFiled, string[] 列名, string[] 主键, string[] 标识, string[] 类型, int[] 长度, string[] 可空, string[] 默认值, string[] 列描述, FormCollection collection)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    tableName = tableName.Replace("[", "").Replace("]", "").Replace("'", "").Replace("\"", "");
                    #region 生成SQL
                    string sql = "CREATE TABLE [" + tableName + "](";
                    //主键
                    string primaryKey = "";
                    //描述SQL
                    string desSql = "";
                    //默认值SQL
                    string defSql = "";
                    #region 自动生成基本字段
                    if (basicFiled == true)
                    {
                        sql += @"[ID] [int] IDENTITY(1,1) NOT NULL,
	                            [Sort] [int] NULL,
	                            [CreateUserID] [int] NULL,
	                            [LastUpdateUserID] [int] NULL,
	                            [CreateDate] [datetime] NULL,
	                            [LastUpdateDate] [datetime] NULL,
	                            [IsDeleted] [bit] NOT NULL,";
                        primaryKey = "ID";
                    }
                    #endregion
                    #region 前端传入集合处理
                    if (列名 != null && 列名.Length > 0)
                    {
                        for (int i = 0; i < 列名.Length; i++)
                        {
                            //主键
                            if (列名[i] != "" && 主键[i] == "是" && 标识[i] == "是" && 类型[i] == "int")
                            {
                                sql += "[" + 列名[i].Replace("[", "").Replace("]", "") + "] [int] IDENTITY(1,1) NOT NULL,";
                                primaryKey = 列名[i].Replace("[", "").Replace("]", "");
                            }
                            //直对字符型的设置长度
                            if (列名[i] != "" && 类型[i].IndexOf("char") > -1)
                            {
                                if (可空[i] == "是")
                                {
                                    sql += "[" + 列名[i].Replace("[", "").Replace("]", "") + "] [" + 类型[i] + "](" + 长度[i] + ") NULL,";
                                }
                                else
                                {
                                    sql += "[" + 列名[i].Replace("[", "").Replace("]", "") + "] [" + 类型[i] + "](" + 长度[i] + ") NOT NULL,";
                                }
                            }

                            if (列名[i] != "" && 类型[i].IndexOf("char") == -1 && (主键[i] != "是" || 标识[i] != "是"))
                            {
                                if (可空[i] == "是")
                                {
                                    sql += "[" + 列名[i].Replace("[", "").Replace("]", "") + "] [" + 类型[i] + "] NULL,";
                                }
                                else
                                {
                                    sql += "[" + 列名[i].Replace("[", "").Replace("]", "") + "] [" + 类型[i] + "] NOT NULL,";
                                }
                            }
                            if (列名[i] != "" && !string.IsNullOrEmpty(列描述[i]))
                            {
                                desSql += " EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'" + 列描述[i].Replace("'", "") + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + tableName.Replace(",", "") + "', @level2type=N'COLUMN',@level2name=N'" + 列名[i].Replace("'", "") + "' ";
                            }
                            if (列名[i] != "" && !string.IsNullOrEmpty(默认值[i]))
                            {
                                if (类型[i].IndexOf("char") == -1)
                                {
                                    defSql += " ALTER TABLE [" + tableName.Replace("[", "").Replace("]", "").Replace("'", "") + "] ADD  CONSTRAINT [DF_" + tableName.Replace("[", "").Replace("]", "").Replace("'", "") + "_" + 列名[i].Replace("[", "").Replace("]", "").Replace("'", "") + "]  DEFAULT ((" + 默认值[i] + ")) FOR [" + 列名[i].Replace("[", "").Replace("]", "").Replace("'", "") + "] ";
                                }
                                else
                                {
                                    defSql += " ALTER TABLE [" + tableName.Replace("[", "").Replace("]", "").Replace("'", "") + "] ADD  CONSTRAINT [DF_" + tableName.Replace("[", "").Replace("]", "").Replace("'", "") + "_" + 列名[i].Replace("[", "").Replace("]", "").Replace("'", "") + "]  DEFAULT (N'" + 默认值[i] + "') FOR [" + 列名[i].Replace("[", "").Replace("]", "").Replace("'", "") + "] ";
                                }
                            }
                        }
                    }
                    #endregion
                    sql += " CONSTRAINT [PK_" + tableName.Replace("[", "").Replace("]", "").Replace("'", "") + "] PRIMARY KEY CLUSTERED([" + primaryKey + "] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY] ";
                    if (!string.IsNullOrEmpty(desSql))
                    {
                        sql += " " + desSql;
                    }
                    if (!string.IsNullOrEmpty(defSql))
                    {
                        sql += " " + defSql;
                    }
                    #endregion
                    db.ExecuteNonQuery(sql);
                    result = true;
                }
                else throw new Exception("数据表名不能为空");
                return result ? Content(BaseController.ContentIcon.Succeed + "|操作成功|/Manager/DataBase/Index") : Content(BaseController.ContentIcon.Error + "|操作失败");
            }
            catch (Exception ex)
            {
                return Content(BaseController.ContentIcon.Error + "|" + ex.Message);
            }
        }
        /// <summary>
        /// 编辑表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public ActionResult EditTable(string tableName)
        {
            string sql = "SELECT 列名=C.name, 主键=(Case When IsNull(IDX.PrimaryKey,N'否')='否' Then N'否' Else N'是' End), 标识=CASE WHEN C.is_identity=1 THEN N'是'ELSE N'否' END, 类型=T.name, 长度=C.max_length, 可空=CASE WHEN C.is_nullable=1 THEN N'是'ELSE N'否' END, 默认值=ISNULL(D.definition,N''), 列描述=ISNULL(PFD.[value],N'') FROM sys.columns C INNER JOIN sys.objects O ON C.[object_id]=O.[object_id] AND O.type='U' AND O.is_ms_shipped=0 INNER JOIN sys.types T ON C.user_type_id=T.user_type_id LEFT JOIN sys.default_constraints D ON C.[object_id]=D.parent_object_id AND C.column_id=D.parent_column_id AND C.default_object_id=D.[object_id] LEFT JOIN sys.extended_properties PFD ON PFD.class=1  AND C.[object_id]=PFD.major_id  AND C.column_id=PFD.minor_id LEFT JOIN sys.extended_properties PTB ON PTB.class=1  AND PTB.minor_id=0  AND C.[object_id]=PTB.major_id LEFT JOIN  (SELECT IDXC.[object_id], IDXC.column_id, Sort=CASE INDEXKEY_PROPERTY(IDXC.[object_id],IDXC.index_id,IDXC.index_column_id,'IsDescending') WHEN 1 THEN 'DESC' WHEN 0 THEN 'ASC' ELSE '' END, PrimaryKey=CASE WHEN IDX.is_primary_key=1 THEN N'是'ELSE N'否' END, IndexName=IDX.Name FROM sys.indexes IDX INNER JOIN sys.index_columns IDXC ON IDX.[object_id]=IDXC.[object_id] AND IDX.index_id=IDXC.index_id LEFT JOIN sys.key_constraints KC ON IDX.[object_id]=KC.[parent_object_id] AND IDX.index_id=KC.unique_index_id INNER JOIN (SELECT [object_id], Column_id, index_id=MIN(index_id) FROM sys.index_columns GROUP BY [object_id], Column_id ) IDXCUQ ON IDXC.[object_id]=IDXCUQ.[object_id] AND IDXC.Column_id=IDXCUQ.Column_id AND IDXC.index_id=IDXCUQ.index_id ) IDX ON C.[object_id]=IDX.[object_id] AND C.column_id=IDX.column_id WHERE O.name='" + tableName + "' ORDER BY O.name,C.column_id";
            var dt = db.ExecuteDataTable(sql);
            var columnDt = db.ExecuteDataTable("SELECT top 0 列名=C.name, 主键=(Case When IsNull(IDX.PrimaryKey,N'否')='否' Then N'否' Else N'是' End), 标识=CASE WHEN C.is_identity=1 THEN N'是'ELSE N'否' END, 类型=T.name, 长度=C.max_length, 可空=CASE WHEN C.is_nullable=1 THEN N'是'ELSE N'否' END, 默认值=ISNULL(D.definition,N''), 列描述=ISNULL(PFD.[value],N'') FROM sys.columns C INNER JOIN sys.objects O ON C.[object_id]=O.[object_id] AND O.type='U' AND O.is_ms_shipped=0 INNER JOIN sys.types T ON C.user_type_id=T.user_type_id LEFT JOIN sys.default_constraints D ON C.[object_id]=D.parent_object_id AND C.column_id=D.parent_column_id AND C.default_object_id=D.[object_id] LEFT JOIN sys.extended_properties PFD ON PFD.class=1  AND C.[object_id]=PFD.major_id  AND C.column_id=PFD.minor_id LEFT JOIN sys.extended_properties PTB ON PTB.class=1  AND PTB.minor_id=0  AND C.[object_id]=PTB.major_id LEFT JOIN  (SELECT IDXC.[object_id], IDXC.column_id, Sort=CASE INDEXKEY_PROPERTY(IDXC.[object_id],IDXC.index_id,IDXC.index_column_id,'IsDescending') WHEN 1 THEN 'DESC' WHEN 0 THEN 'ASC' ELSE '' END, PrimaryKey=CASE WHEN IDX.is_primary_key=1 THEN N'是'ELSE N'否' END, IndexName=IDX.Name FROM sys.indexes IDX INNER JOIN sys.index_columns IDXC ON IDX.[object_id]=IDXC.[object_id] AND IDX.index_id=IDXC.index_id LEFT JOIN sys.key_constraints KC ON IDX.[object_id]=KC.[parent_object_id] AND IDX.index_id=KC.unique_index_id INNER JOIN (SELECT [object_id], Column_id, index_id=MIN(index_id) FROM sys.index_columns GROUP BY [object_id], Column_id ) IDXCUQ ON IDXC.[object_id]=IDXCUQ.[object_id] AND IDXC.Column_id=IDXCUQ.Column_id AND IDXC.index_id=IDXCUQ.index_id ) IDX ON C.[object_id]=IDX.[object_id] AND C.column_id=IDX.column_id WHERE O.name='" + tableName + "' ORDER BY O.name,C.column_id");
            ViewData["column"] = columnDt;
            return View(dt.ToPagedList(1, int.MaxValue));
        }
        [HttpPost]
        public ActionResult EditTable(string tableName, string oldTableName, string[] 列名, string[] 主键, string[] 标识, string[] 类型, int[] 长度, string[] 可空, string[] 默认值, string[] 列描述, FormCollection collection)
        {
            bool result = false;
            string errorMessage = "";
            try
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    tableName = tableName.Replace("[", "").Replace("]", "").Replace("'", "").Replace("\"", "");
                    //修改表名
                    string renameTableSql = "exec sp_rename '" + oldTableName + "','" + tableName + "','object'";
                    db.ExecuteNonQuery(renameTableSql);
                    //旧表全列
                    string allColumnSql = "SELECT a.name FROM dbo.syscolumns AS a LEFT OUTER JOIN dbo.systypes AS b ON a.xtype = b.xusertype INNER JOIN dbo.sysobjects AS d ON a.id = d.id AND d.xtype = 'U' AND d.status >= 0 where d.name='" + tableName + "'";
                    var allColumnDT = db.ExecuteDataTable(allColumnSql);
                    for (int i = 0; i < 列名.Length; i++)
                    {
                        try
                        {
                            if (!Enum.GetNames(typeof(BaseFieldType)).Contains(列名[i]))
                            {
                                //更改表列
                                if (allColumnDT.Rows.Count > i)
                                {
                                    try
                                    {
                                        string renameSql = "exec sp_rename '" + tableName + "." + allColumnDT.Rows[i][0] + "', '" + 列名[i] + "', 'column'";
                                        db.ExecuteNonQuery(renameSql);
                                        try
                                        {
                                            string sql = "alter table " + tableName + " ALTER COLUMN " + 列名[i] + " " + 类型[i];
                                            if (类型[i].IndexOf("char") > -1)
                                            {
                                                sql += "(" + 长度[i] + ")";
                                            }
                                            else if (类型[i].IndexOf("decimal") > -1)
                                            {
                                                sql += "(18," + 长度[i] + ")";
                                            }
                                            if (可空[i] == "是")
                                            {
                                                sql += " null";
                                            }
                                            else
                                            {
                                                sql += " not null";
                                            }
                                            db.ExecuteNonQuery(sql);
                                        }
                                        catch { }

                                        //
                                        string desSql = "";
                                        if (!string.IsNullOrEmpty(列描述[i]))
                                        {
                                            desSql += " EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'" + 列描述[i].Replace("'", "") + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + tableName.Replace(",", "") + "', @level2type=N'COLUMN',@level2name=N'" + 列名[i].Replace("'", "") + "' ";
                                            db.ExecuteNonQuery(desSql);
                                        }
                                        string defSql = "";
                                        if (!string.IsNullOrEmpty(默认值[i]))
                                        {
                                            if (类型[i].IndexOf("char") == -1)
                                            {
                                                defSql += " ALTER TABLE [" + tableName.Replace("[", "").Replace("]", "").Replace("'", "") + "] ADD  CONSTRAINT [DF_" + tableName.Replace("[", "").Replace("]", "").Replace("'", "") + "_" + 列名[i].Replace("[", "").Replace("]", "").Replace("'", "") + "]  DEFAULT ((" + 默认值[i] + ")) FOR [" + 列名[i].Replace("[", "").Replace("]", "").Replace("'", "") + "] ";
                                            }
                                            else
                                            {
                                                defSql += " ALTER TABLE [" + tableName.Replace("[", "").Replace("]", "").Replace("'", "") + "] ADD  CONSTRAINT [DF_" + tableName.Replace("[", "").Replace("]", "").Replace("'", "") + "_" + 列名[i].Replace("[", "").Replace("]", "").Replace("'", "") + "]  DEFAULT (N'" + 默认值[i] + "') FOR [" + 列名[i].Replace("[", "").Replace("]", "").Replace("'", "") + "] ";
                                            }
                                            db.ExecuteNonQuery(defSql);
                                        }
                                    }
                                    catch { }
                                }
                                else //新加列
                                {
                                    try
                                    {
                                        string sql = "alter table " + tableName + " ADD " + 列名[i] + " " + 类型[i];
                                        if (类型[i].IndexOf("char") > -1)
                                        {
                                            sql += "(" + 长度[i] + ")";
                                        }
                                        else if (类型[i].IndexOf("decimal") > -1)
                                        {
                                            sql += "(18," + 长度[i] + ")";
                                        }
                                        if (可空[i] == "是")
                                        {
                                            sql += " null";
                                        }
                                        else
                                        {
                                            sql += " not null";
                                        }
                                        db.ExecuteNonQuery(sql);
                                        //
                                        string desSql = "";
                                        if (!string.IsNullOrEmpty(列描述[i]))
                                        {
                                            desSql += " EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'" + 列描述[i].Replace("'", "") + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + tableName.Replace(",", "") + "', @level2type=N'COLUMN',@level2name=N'" + 列名[i].Replace("'", "") + "' ";
                                            db.ExecuteNonQuery(desSql);
                                        }
                                        string defSql = "";
                                        if (!string.IsNullOrEmpty(默认值[i]))
                                        {
                                            if (类型[i].IndexOf("char") == -1)
                                            {
                                                defSql += " ALTER TABLE [" + tableName.Replace("[", "").Replace("]", "").Replace("'", "") + "] ADD  CONSTRAINT [DF_" + tableName.Replace("[", "").Replace("]", "").Replace("'", "") + "_" + 列名[i].Replace("[", "").Replace("]", "").Replace("'", "") + "]  DEFAULT ((" + 默认值[i] + ")) FOR [" + 列名[i].Replace("[", "").Replace("]", "").Replace("'", "") + "] ";
                                            }
                                            else
                                            {
                                                defSql += " ALTER TABLE [" + tableName.Replace("[", "").Replace("]", "").Replace("'", "") + "] ADD  CONSTRAINT [DF_" + tableName.Replace("[", "").Replace("]", "").Replace("'", "") + "_" + 列名[i].Replace("[", "").Replace("]", "").Replace("'", "") + "]  DEFAULT (N'" + 默认值[i] + "') FOR [" + 列名[i].Replace("[", "").Replace("]", "").Replace("'", "") + "] ";
                                            }
                                            db.ExecuteNonQuery(defSql);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        errorMessage += ex.Message + "\r\n";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errorMessage += ex.Message + "\r\n";
                        }
                    }
                    result = true;
                }
                else throw new Exception("数据表名不能为空");
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = "，但是在执行时出现如下错误：" + errorMessage;
                }
                return result ? Content(BaseController.ContentIcon.Succeed + "|操作成功" + errorMessage + "|/Manager/DataBase/Index") : Content(BaseController.ContentIcon.Error + "|操作失败");
            }
            catch (Exception ex)
            {
                return Content(BaseController.ContentIcon.Error + "|" + ex.Message);
            }
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public ActionResult RemoveTable(string tableName)
        {
            string result = "";
            string sql = "drop table [" + tableName + "]";
            db.ExecuteNonQuery(sql);
            result = "ok";
            return Content(result);
        }
        /// <summary>
        /// 添加表中的列所需Html
        /// </summary>
        /// <returns></returns>
        public ActionResult GetColumnHtml()
        {
            string columnHtml = "<div class=\"control-group\">            <div style=\"clear: both; margin-bottom: 5px;\">                    <label class=\"control-label\">                        <input class=\"btn btn-primary\" value=\"删除列\" onclick=\"if (confirm('确定要删除当前列吗？')) { $(this).parents('.control-group').remove(); }\" type=\"button\">                        列名</label><div class=\"controls\">                            <input name=\"列名\" value=\"\" required=\"\" type=\"text\">                        </div>            </div>            <div style=\"clear: both; margin-bottom: 5px;\">                    <label class=\"control-label\">主键</label><div class=\"controls\">                                                <select tabindex=\"-1\" name=\"主键\" required=\"\">                            <option value=\"否\">否</option>                            <option value=\"是\">是</option>                        </select>                    </div>            </div>            <div style=\"clear: both; margin-bottom: 5px;\">                    <label class=\"control-label\">标识</label><div class=\"controls\">                                                <select tabindex=\"-1\" name=\"标识\" required=\"\">                            <option value=\"否\">否</option>                            <option value=\"是\">是</option>                        </select>                    </div>            </div>            <div style=\"clear: both; margin-bottom: 5px;\">                    <label class=\"control-label\">类型</label><div class=\"controls\">                                                <select tabindex=\"-1\" name=\"类型\">                                <option value=\"char\">char</option>                                <option value=\"nchar\">nchar</option>                                <option value=\"varchar\">varchar</option>                                <option value=\"nvarchar\">nvarchar</option>                                <option value=\"text\">text</option>                                <option value=\"ntext\">ntext</option>                                <option value=\"bit\">bit</option>                                <option value=\"tinyint\">tinyint</option>                                <option value=\"int\">int</option>                                <option value=\"bigint\">bigint</option>                                <option value=\"decimal\">decimal</option>                                <option value=\"float\">float</option>                                <option value=\"money\">money</option>                                <option value=\"datetime\">datetime</option>                                <option value=\"binary\">binary</option>                                <option value=\"varbinary\">varbinary</option>                                <option value=\"image\">image</option>                        </select>                    </div>            </div>            <div style=\"clear: both; margin-bottom: 5px;\">                    <label class=\"control-label\">长度</label><div class=\"controls\">                        <input name=\"长度\" value=\"50\" required=\"\" type=\"text\">                    </div>            </div>            <div style=\"clear: both; margin-bottom: 5px;\">                    <label class=\"control-label\">可空</label><div class=\"controls\">                                                <select tabindex=\"-1\" name=\"可空\" required=\"\">                            <option value=\"是\">是</option>                            <option value=\"否\">否</option>                        </select>                    </div>            </div>            <div style=\"clear: both; margin-bottom: 5px;\">                    <label class=\"control-label\">默认值</label><div class=\"controls\">                        <input name=\"默认值\" value=\"\" required=\"\" type=\"text\">                    </div>            </div>            <div style=\"clear: both; margin-bottom: 5px;\">                    <label class=\"control-label\">列描述</label><div class=\"controls\">                        <input name=\"列描述\" value=\"\" required=\"\" type=\"text\">                    </div>            </div>    <div>";
            return Content(columnHtml);
        }
        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public ActionResult RemoveColumn(string tableName, string columnName)
        {
            string result = "";
            string sql = "alter table [" + tableName + "] drop column [" + columnName + "]";
            db.ExecuteNonQuery(sql);
            result = "ok";
            return Content(result);
        }

        /// <summary>
        /// 数据表数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult List(string tableName, int? pageIndex, int? pageSize)
        {
            tableName = tableName.Replace("[", "").Replace("]", "").Replace("\"", "").Replace("'", "").Replace("\r", "").Replace("\n", "").Replace("(", "").Replace(")", "");
            string sql = "SELECT * FROM [" + tableName + "]";
            var dt = db.ExecuteDataTable(sql);
            var sql2 = "SELECT top 0 * FROM [" + tableName + "]";
            var columnDt = db.ExecuteDataTable(sql2);
            ViewData["column"] = columnDt;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_List", dt.ToPagedList(pageIndex ?? 1, pageSize ?? 20));
            }
            return View(dt.ToPagedList(pageIndex ?? 1, pageSize ?? 20));
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateRecord(string tableName)
        {
            ViewData["tableName"] = tableName;
            string sql = "SELECT TOP 0 * FROM [" + tableName + "]";
            var dt = db.ExecuteDataTable(sql);
            ViewData["column"] = dt;
            return View(dt.ToPagedList(1, int.MaxValue));
        }
        [HttpPost]
        public ActionResult CreateRecord(string tableName, FormCollection collection)
        {
            bool result = false;
            try
            {
                if (db.UpdateModel(0, tableName, collection, DBContext.EnumOperation.Insert) > -1)
                {
                    result = true;
                }
                return result ? Content(BaseController.ContentIcon.Succeed + "|操作成功|/Manager/DataBase/List?tableName=" + Url.Encode(tableName)) : Content(BaseController.ContentIcon.Error + "|操作失败");
            }
            catch (Exception ex)
            {
                return Content(BaseController.ContentIcon.Error + "|" + ex.Message);
            }
        }
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public ActionResult EditRecord(string tableName, int id)
        {
            ViewData["tableName"] = tableName;
            string sql = "SELECT * FROM [" + tableName + "] WHERE [ID]=" + id;
            var dt = db.ExecuteDataTable(sql);
            ViewData["column"] = dt;
            return View(dt.ToPagedList(1, int.MaxValue));
        }
        [HttpPost]
        public ActionResult EditRecord(string tableName, int id, FormCollection collection)
        {
            bool result = false;
            try
            {
                if (db.UpdateModel(id, tableName, collection, DBContext.EnumOperation.Update) > -1)
                {
                    result = true;
                }
                return result ? Content(BaseController.ContentIcon.Succeed + "|操作成功|/Manager/DataBase/List?tableName=" + Url.Encode(tableName)) : Content(BaseController.ContentIcon.Error + "|操作失败");
            }
            catch (Exception ex)
            {
                return Content(BaseController.ContentIcon.Error + "|" + ex.Message);
            }
        }
        /// <summary>
        /// 删除表记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RemoveRecord(string tableName, int id)
        {
            string result = "";
            db.ExecuteNonQuery("DELETE FROM [" + tableName + "] WHERE ID=" + id);
            result = "ok";
            return Content(result);
        }
        /// <summary>
        /// 清空表数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public ActionResult TruncateTable(string tableName)
        {
            string result = "";
            db.ExecuteNonQuery("TRUNCATE TABLE [" + tableName + "]");
            result = "ok";
            return Content(result);

        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <returns></returns>
        public ActionResult ExcuteSQL()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExcuteSQL(string sqlStr)
        {
            string result = "";
            if (string.IsNullOrEmpty(sqlStr))
            {
                result = "SQL语句不能为空!";
            }
            else
            {
                try
                {
                    db.ExecuteNonQuery(sqlStr);
                    result = "执行成功";
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }
            return Content(result);
        }
        /// <summary>
        /// 导出SQL数据
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public ActionResult ExportSQL(string sqlStr)
        {
            try
            {
                var dt = db.ExecuteDataTable(sqlStr);
                Common.ExcelHelper.GetExcel(dt, DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.Write("导出失败：" + ex.Message);
                Response.Write("<script>alert(\"导出失败：" + ex.Message + "\"); history.back();</script>");
                Response.Flush();
                Response.End();
            }
            return Content(string.Empty);
        }
    }
}
