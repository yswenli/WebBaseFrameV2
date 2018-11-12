using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
//
using WEF.MvcPager;

namespace Web.Areas.Manager.Controllers
{
    public class SqliteDataBaseController : BaseController
    {

        public SqliteDataBaseController()
        {
            if (SqliteDBHelper.IsMSSQL)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Manager/DataBase/Index");
            }
        }

        //
        // GET: /DataBase/

        #region 表结构操作
        /// <summary>
        /// 获取全部表
        /// </summary>
        /// <returns></returns>
        public ActionResult Tables()
        {
            return PartialView(SqliteDBHelper.GetTables());
        }
        /// <summary>
        /// 创建表
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateTable()
        {
            return View();
        }
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="Ordinal"></param>
        /// <param name="Name"></param>
        /// <param name="IsPrimaryKey"></param>
        /// <param name="DataTypeName"></param>
        /// <param name="MaxLength"></param>
        /// <param name="AllowDBNull"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateTable(string tableName,
            bool basicFiled,
            int[] Ordinal,
            string[] Name,
            bool[] IsPrimaryKey,
            string[] DataTypeName,
            int[] MaxLength,
            bool[] AllowDBNull,
            FormCollection collection)
        {
            if (!string.IsNullOrEmpty(tableName) && Name != null)
            {
                var tlts = new List<SqliteDBHelper.ColumnInfo>();

                if (basicFiled)
                {
                    tlts.AddRange(new List<SqliteDBHelper.ColumnInfo>()
                    {
                        new SqliteDBHelper.ColumnInfo(){Ordinal=0,Name="ID",IsPrimaryKey=true,AutoIncrement=true,DataTypeName=SqliteDBHelper.SqliteDBDataType.INTEGER.ToString(),AllowDBNull=false},
                        new SqliteDBHelper.ColumnInfo(){Ordinal=0,Name="CreateUserID",IsPrimaryKey=false,DataTypeName=SqliteDBHelper.SqliteDBDataType.INTEGER.ToString(),AllowDBNull=false},
                        new SqliteDBHelper.ColumnInfo(){Ordinal=0,Name="LastUpdateUserID",IsPrimaryKey=false,DataTypeName=SqliteDBHelper.SqliteDBDataType.INTEGER.ToString(),AllowDBNull=false},
                        new SqliteDBHelper.ColumnInfo(){Ordinal=0,Name="CreateDate",IsPrimaryKey=false,DataTypeName=SqliteDBHelper.SqliteDBDataType.DATETIME.ToString(),AllowDBNull=false},
                        new SqliteDBHelper.ColumnInfo(){Ordinal=0,Name="LastUpdateDate",IsPrimaryKey=false,DataTypeName=SqliteDBHelper.SqliteDBDataType.DATETIME.ToString(),AllowDBNull=false},
                        new SqliteDBHelper.ColumnInfo(){Ordinal=0,Name="IsDeleted",IsPrimaryKey=false,DataTypeName=SqliteDBHelper.SqliteDBDataType.BIT.ToString(),AllowDBNull=false}
                    });
                }

                for (int i = 0; i < Name.Length; i++)
                {
                    SqliteDBHelper.ColumnInfo ct = new SqliteDBHelper.ColumnInfo();
                    ct.Ordinal = Ordinal[i];
                    ct.Name = Name[i];
                    ct.IsPrimaryKey = IsPrimaryKey[i];
                    ct.DataTypeName = DataTypeName[i];
                    ct.MaxLength = MaxLength[i];
                    ct.AllowDBNull = AllowDBNull[i];
                    tlts.Add(ct);
                }
                if (!SqliteDBHelper.ExistsTable(tableName))
                {
                    SqliteDBHelper.CreateTable(tableName, tlts);
                }
            }
            return Redirect("/Manager/SqliteDataBase/Tables");
        }

        /// <summary>
        /// 编辑表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditTable(string id)
        {
            ViewBag.TableName = id;
            return View(SqliteDBHelper.GetColumnInfos(id));
        }
        /// <summary>
        /// 编辑表
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="oldTableName"></param>
        /// <param name="Ordinal"></param>
        /// <param name="Name"></param>
        /// <param name="IsPrimaryKey"></param>
        /// <param name="DataTypeName"></param>
        /// <param name="MaxLength"></param>
        /// <param name="AllowDBNull"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditTable(string tableName, string oldTableName,
            int[] Ordinal,
            string[] Name,
            bool[] IsPrimaryKey,
            string[] DataTypeName,
            int[] MaxLength,
            bool[] AllowDBNull,
            FormCollection collection)
        {
            ViewBag.TableName = tableName;
            if (!string.IsNullOrEmpty(tableName) && Name != null)
            {
                var tlts = new List<SqliteDBHelper.ColumnInfo>();

                for (int i = 0; i < Name.Length; i++)
                {
                    SqliteDBHelper.ColumnInfo ct = new SqliteDBHelper.ColumnInfo();
                    ct.Ordinal = Ordinal[i];
                    ct.Name = Name[i];
                    ct.IsPrimaryKey = IsPrimaryKey[i];
                    ct.DataTypeName = DataTypeName[i];
                    ct.MaxLength = MaxLength[i];
                    ct.AllowDBNull = AllowDBNull[i];
                    tlts.Add(ct);
                }

                if (SqliteDBHelper.ExistsTable(oldTableName))
                {
                    if (tableName != oldTableName && !SqliteDBHelper.ExistsTable(tableName))
                    {
                        if (SqliteDBHelper.ReNameTable(tableName, oldTableName))
                        {
                            SqliteDBHelper.CreateTable(tableName, tlts);
                        }
                    }
                    else
                    {
                        SqliteDBHelper.CreateTable(tableName, tlts);
                    }
                }
            }
            return Redirect("/Manager/SqliteDataBase/Tables");
        }

        /// <summary>
        /// 编辑列
        /// </summary>
        /// <param name="id"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public ActionResult EditColumn(string id, string columnName)
        {
            SqliteDBHelper.ColumnInfo ct = new SqliteDBHelper.ColumnInfo();
            ViewBag.TableName = id;
            ViewBag.ColumnName = columnName;
            try
            {
                var clts = SqliteDBHelper.GetColumnInfos(id);
                ct = clts.Where(b => b.Name == columnName).First();
            }
            catch { }
            return View(ct);
        }

        /// <summary>
        /// 编辑列
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditColumn(string tableName, SqliteDBHelper.ColumnInfo ct)
        {
            try
            {
                var columns = new List<SqliteDBHelper.ColumnInfo>();
                var clts = SqliteDBHelper.GetColumnInfos(tableName);
                if (clts != null && clts.Count > 0)
                {
                    foreach (var item in clts)
                    {
                        if (item.Ordinal == ct.Ordinal)
                        {
                            columns.Add(ct);
                        }
                        else
                        {
                            columns.Add(item);
                        }
                    }
                }
                columns = columns.OrderBy(b => b.Ordinal).ToList();
                SqliteDBHelper.AlterColumns(tableName, columns);
            }
            catch { }
            return Redirect("/Manager/SqliteDataBase/EditTable/" + tableName);
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DropTable(string id)
        {
            SqliteDBHelper.DropTable(id);
            SqliteDBHelper.ShrinkDatabase();
            return Redirect("/Manager/SqliteDataBase/Tables");
        }
        #endregion

        #region 表数据操作
        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Records(string id, int? pageIndex, int? pageSize)
        {
            ViewBag.TableName = id;
            if (!string.IsNullOrEmpty(id))
            {
                id = id.Replace("'", "").Replace("%", "");
            }

            ViewData["column"] = SqliteDBHelper.GetMap(id);

            return View(SqliteDBHelper.GetRecords(id).ToPagedList(pageIndex ?? 1, pageSize ?? 12));
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CreateRecord(string id)
        {
            ViewBag.TableName = id;
            if (!string.IsNullOrEmpty(id))
            {
                id = id.Replace("'", "").Replace("%", "");
            }
            return View(SqliteDBHelper.GetRecord(id, 0));
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreateRecord(string tableName, FormCollection collection)
        {
            SqliteDBHelper.UpdateRecord(null, tableName, collection, SqliteDBHelper.SqliteEnumOperation.INSERT);

            return Redirect("/Manager/SqliteDataBase/Records/" + tableName);
        }

        /// <summary>
        /// 编辑记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult EditRecord(string id, int key)
        {
            ViewBag.TableName = id;
            if (!string.IsNullOrEmpty(id))
            {
                id = id.Replace("'", "").Replace("%", "");
            }
            return View(SqliteDBHelper.GetRecords(id, new DictionaryEntry() { Key = "ID", Value = key }));
        }

        /// <summary>
        /// 编辑记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tableName"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditRecord(int id, string tableName, FormCollection collection)
        {
            SqliteDBHelper.UpdateRecord(id, tableName, collection, SqliteDBHelper.SqliteEnumOperation.UPDATE);
            return Redirect("/Manager/SqliteDataBase/Records/" + tableName);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult DeleteRecord(string id, int key)
        {
            SqliteDBHelper.DeleteRecord(id, key);
            return Redirect("/Manager/SqliteDataBase/Records/" + id);
        }
        /// <summary>
        /// 清空表数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Truncate(string id)
        {
            SqliteDBHelper.Trancate(id);
            SqliteDBHelper.ShrinkDatabase();
            return Redirect("/Manager/SqliteDataBase/Records/" + id);
        }
        #endregion

        #region 其他执行SQL

        /// <summary>
        /// 其他执行SQL
        /// </summary>
        /// <returns></returns>
        public ActionResult ExecuteSQL()
        {
            ViewBag.Access = "False";
            ViewBag.Message = "";
            return View();
        }

        /// <summary>
        /// 其他执行SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="IsExport"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ExecuteSQL(string sql, bool? IsExport)
        {
            try
            {
                if (IsExport.HasValue && IsExport.Value)
                {
                    ExcelHelper excelHelper = new ExcelHelper();
                    excelHelper.AddSheet("Sheet1", SqliteDBHelper.Query(sql).Tables[0]);
                    excelHelper.Export();
                    return Content(string.Empty);
                }
                else
                {
                    SqliteDBHelper.ExecuteSql(sql);
                }
                ViewBag.Message = "SQL执行成功";
                ViewBag.Access = "True";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "执行错误" + ex.Message;
            }
            return View();
        }
        #endregion
    }
}