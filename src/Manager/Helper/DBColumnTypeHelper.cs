using System.Data;
using WEF;

namespace Web.Areas.Manager.Helper
{
    /// <summary>
    /// 列描述辅助类
    /// </summary>
    public class DBColumnDescriptionHelper
    {
        /// <summary>
        /// 获取某库中表全部列描述
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetDescriptions(string connStr, string tableName)
        {
            DataTable dt = new DataTable();
            try
            {
                if (Common.SqliteDBHelper.IsMSSQL)
                {
                    dt = new DBContext(connStr).ExecuteDataTable("SELECT ID = newid(),库名= 'WEPM_OA',表名= convert(varchar(50), d.name ),字段名= convert(varchar(100), a.name),字段说明=convert(varchar(50),isnull(g.[value],'')) FROM dbo.syscolumns a left join dbo.systypes b on a.xusertype=b.xusertype inner join dbo.sysobjects d on a.id=d.id and d.xtype='U' and d.name<>'dtproperties' left join dbo.syscomments e on a.cdefault=e.id left join sys.extended_properties g on a.id=g.major_id and a.colid=g.minor_id left join sys.extended_properties f on d.id=f.major_id and f.minor_id=0 where d.name ='" + tableName + "'");
                }
            }
            catch { };
            return dt;
        }
        /// <summary>
        /// 获取某列的描述
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string GetDescription(DataTable dataTable, string columnName)
        {
            string result = columnName;
            try
            {
                foreach (DataRow item in dataTable.Rows)
                {
                    if (item[3].ToString().ToLower() == columnName.ToLower())
                    {
                        if (item[4] != null && !string.IsNullOrEmpty(item[4].ToString()))
                        {
                            result = item[4].ToString();
                        }
                    }
                }


            }
            catch { }
            return result;
        }

        /// <summary>
        /// 添加列描述
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="tableName"></param>
        /// <param name="columName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static bool AddDescription(string connStr, string tableName, string columName, string description)
        {
            bool result = false;
            try
            {
                new DBContext(connStr).ExecuteDataTable("EXEC [sys].[sp_addextendedproperty] @name=N'MS_Description', @value=N'" + description + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + tableName + "', @level2type=N'COLUMN',@level2name=N'" + columName + "'");
                result = true;
            }
            catch { }
            return result;
        }


        /// <summary>
        /// 添加列描述
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="tableName"></param>
        /// <param name="columName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static bool UpdateDescription(string connStr, string tableName, string columName, string description)
        {
            bool result = false;
            try
            {
                new DBContext(connStr).ExecuteDataTable("EXEC [sys].[sp_updateextendedproperty] @name=N'MS_Description', @value=N'" + description + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + tableName + "', @level2type=N'COLUMN',@level2name=N'" + columName + "'");
                result = true;
            }
            catch { }
            return result;
        }
    }
}