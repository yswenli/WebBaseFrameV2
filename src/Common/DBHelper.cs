using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.Collections;
using System.Data;
using System.Web.Mvc;
using System.Configuration;
using System.Data.OracleClient;
using System.Data.SqlClient;

namespace Common
{
    #region Oracle

    /// <summary>
    /// OracleDBHelper
    /// </summary>
    public abstract class OracleDBHelper
    {

        public static readonly string DBNAME = "PAYDEV";

        ///<summary>
        /// 数据库连接字符串
        ///</summary>
        //public static string ConnectionString = EncryptionHelper.AESHelper.Decrypt(ConfigurationManager.AppSettings["Oracle"].ToString(), "wenguoli");
        public static string ConnectionString = ConfigurationManager.AppSettings["Oracle"].ToString();

        ///<summary>
        /// 
        ///</summary>
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        #region ExecuteNonQuery

        ///<summary>
        /// Executes the non query.
        ///</summary>
        ///<param name="cmdType">Type of the CMD.</param>
        ///<param name="cmdText">The CMD text.</param>
        ///<param name="commandParameters">The command parameters.</param>
        ///<returns></returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            int result = 0;
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                    result = cmd.ExecuteNonQuery();
                    try
                    {
                        cmd.Parameters.Clear();
                    }
                    catch { }
                }
            }
            return result;
        }

        ///<summary>
        /// Executes the non query.
        ///</summary>
        ///<param name="cmdText">The CMD text.</param>
        ///<param name="commandParameters">The command parameters.</param>
        ///<returns></returns>
        public static int ExecuteNonQuery(string cmdText, params OracleParameter[] commandParameters)
        {
            return ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
        }

        ///<summary>
        /// Executes the non query.
        ///</summary>
        ///<param name="trans">The trans.</param>
        ///<param name="cmdType">Type of the CMD.</param>
        ///<param name="cmdText">The CMD text.</param>
        ///<param name="commandParameters">The command parameters.</param>
        ///<returns></returns>
        public static int ExecuteNonQuery(OracleTransaction trans, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            int result = 0;
            using (OracleCommand cmd = new OracleCommand())
            {
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
                result = cmd.ExecuteNonQuery();
                try
                {
                    cmd.Parameters.Clear();
                }
                catch { }
            }
            return result;
        }

        ///<summary>
        /// Executes the non query.
        ///</summary>
        ///<param name="connection">The connection.</param>
        ///<param name="cmdType">Type of the CMD.</param>
        ///<param name="cmdText">The CMD text.</param>
        ///<param name="commandParameters">The command parameters.</param>
        ///<returns></returns>
        public static int ExecuteNonQuery(OracleConnection connection, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            int result = 0;
            using (OracleCommand cmd = new OracleCommand())
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                result = cmd.ExecuteNonQuery();
                try
                {
                    cmd.Parameters.Clear();
                }
                catch { }
            }
            return result;
        }
        #endregion

        #region ExecuteReader

        ///<summary>
        /// Executes the reader.
        ///</summary>
        ///<param name="cmdType">Type of the CMD.</param>
        ///<param name="cmdText">The CMD text.</param>
        ///<param name="commandParameters">The command parameters.</param>
        ///<returns></returns>
        public static OracleDataReader ExecuteReader(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            return OracleDBHelper.ExecuteReader(new OracleConnection(ConnectionString), cmdType, cmdText, commandParameters);
        }

        ///<summary>
        /// Executes the reader.
        ///</summary>
        ///<param name="conn">数据库连接</param>
        ///<param name="cmdType">命令类型</param>
        ///<param name="cmdText">命令内容</param>
        ///<param name="commandParameters">命令参数</param>
        ///<returns>reader</returns>
        public static OracleDataReader ExecuteReader(OracleConnection conn, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            try
            {
                OracleCommand cmd = new OracleCommand();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception exc)
            {
                conn.Close();
                throw exc;
            }
        }

        ///<summary>
        /// 执行数据读取器(当DataReader关闭时，相关的数据库连接不关闭)
        ///</summary>
        ///<param name="conn">数据库连接</param>
        ///<param name="cmdType">命令类型</param>
        ///<param name="cmdText">命令内容</param>
        ///<param name="commandParameters">命令参数</param>
        ///<returns>reader</returns>
        public static OracleDataReader ExecuteReaderWithoutClosingConnection(OracleConnection conn, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                OracleDataReader rdr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception exc)
            {
                conn.Close();
                throw exc;
            }
        }

        ///<summary>
        /// Executes the reader.
        ///</summary>
        ///<param name="cmdText">The CMD text.</param>
        ///<param name="commandParameters">The command parameters.</param>
        ///<returns></returns>
        public static OracleDataReader ExecuteReader(string cmdText, params OracleParameter[] commandParameters)
        {
            return ExecuteReader(CommandType.Text, cmdText, commandParameters);
        }
        #endregion

        ///<summary>
        /// Executes the data set.
        ///</summary>
        ///<param name="cmdText">The CMD text.</param>
        ///<param name="commandParameters">The command parameters.</param>
        ///<returns></returns>
        public static DataSet ExecuteDataSet(string cmdText, params OracleParameter[] commandParameters)
        {
            return ExecuteDataSet(CommandType.Text, cmdText, commandParameters);
        }

        ///<summary>
        /// Executes the data set.
        ///</summary>
        ///<param name="cmdType">Type of the CMD.</param>
        ///<param name="cmdText">The CMD text.</param>
        ///<param name="commandParameters">The command parameters.</param>
        ///<returns></returns>
        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    OracleDataAdapter oda = new OracleDataAdapter(cmd);
                    oda.Fill(ds);
                    try
                    {
                        cmd.Parameters.Clear();
                    }
                    catch { }
                }
            }
            return ds;
        }

        #region ExecuteScalar

        ///<summary>
        /// Executes the scalar.
        ///</summary>
        ///<param name="cmdType">Type of the CMD.</param>
        ///<param name="cmdText">The CMD text.</param>
        ///<param name="commandParameters">The command parameters.</param>
        ///<returns></returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            object val = null;
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    val = cmd.ExecuteScalar();
                    try
                    {
                        cmd.Parameters.Clear();
                    }
                    catch { }

                }
            }
            return val;
        }

        ///<summary>
        /// Executes the scalar.
        ///</summary>
        ///<param name="cmdText">The CMD text.</param>
        ///<param name="commandParameters">The command parameters.</param>
        ///<returns></returns>
        public static object ExecuteScalar(string cmdText, params OracleParameter[] commandParameters)
        {
            return ExecuteScalar(CommandType.Text, cmdText, commandParameters);
        }

        ///<summary>
        /// Executes the scalar.
        ///</summary>
        ///<param name="transaction">The transaction.</param>
        ///<param name="commandType">Type of the command.</param>
        ///<param name="commandText">The command text.</param>
        ///<param name="commandParameters">The command parameters.</param>
        ///<returns></returns>
        public static object ExecuteScalar(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            object retval = null;
            using (OracleCommand cmd = new OracleCommand())
            {
                PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
                retval = cmd.ExecuteScalar();
                try
                {
                    cmd.Parameters.Clear();
                }
                catch { }
            }
            return retval;
        }

        ///<summary>
        /// Executes the scalar.
        ///</summary>
        ///<param name="connectionString">The connection string.</param>
        ///<param name="cmdType">Type of the CMD.</param>
        ///<param name="cmdText">The CMD text.</param>
        ///<param name="commandParameters">The command parameters.</param>
        ///<returns></returns>
        public static object ExecuteScalar(OracleConnection connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            object val = null;
            using (OracleCommand cmd = new OracleCommand())
            {
                PrepareCommand(cmd, connectionString, null, cmdType, cmdText, commandParameters);
                val = cmd.ExecuteScalar();
                try
                {
                    cmd.Parameters.Clear();
                }
                catch { }
            }
            return val;
        }

        #endregion

        ///<summary>
        /// Caches the parameters.
        ///</summary>
        ///<param name="cacheKey">The cache key.</param>
        ///<param name="commandParameters">The command parameters.</param>
        public static void CacheParameters(string cacheKey, params OracleParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        ///<summary>
        /// Gets the cached parameters.
        ///</summary>
        ///<param name="cacheKey">The cache key.</param>
        ///<returns></returns>
        public static OracleParameter[] GetCachedParameters(string cacheKey)
        {
            OracleParameter[] cachedParms = (OracleParameter[])parmCache[cacheKey];
            if (cachedParms == null) return null;
            // If the parameters are in the cache
            OracleParameter[] clonedParms = new OracleParameter[cachedParms.Length];
            // return a copy of the parameters
            for (int i = 0, j = cachedParms.Length; i < j; i++) clonedParms[i] = (OracleParameter)((ICloneable)cachedParms[i]).Clone();
            return clonedParms;
        }

        ///<summary>
        /// Prepares the command.
        ///</summary>
        ///<param name="cmd">The CMD.</param>
        ///<param name="conn">The conn.</param>
        ///<param name="trans">The trans.</param>
        ///<param name="cmdType">Type of the CMD.</param>
        ///<param name="cmdText">The CMD text.</param>
        ///<param name="commandParameters">The command parameters.</param>
        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter[] commandParameters)
        {

            //Open the connection if required
            if (conn.State != ConnectionState.Open)
                conn.Open();

            //Set up the command
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            //Bind it to the transaction if it exists
            if (trans != null)
                cmd.Transaction = trans;
            // Bind the parameters passed in
            if (commandParameters != null)
            {
                foreach (OracleParameter parm in commandParameters) cmd.Parameters.Add(parm);
            }
        }

        ///<summary>
        /// Oras the bit.
        ///</summary>
        ///<param name="value">if set to <c>true</c> [value].</param>
        ///<returns></returns>
        public static string OraBit(bool value)
        {
            if (value)
                return "Y";
            else
                return "N";
        }

        ///<summary>
        /// Oras the bool.
        ///</summary>
        ///<param name="value">The value.</param>
        ///<returns></returns>
        public static bool OraBool(string value)
        {
            if (value.Equals("Y"))
                return true;
            else
                return false;
        }

        ///<summary>
        /// Safes the value.
        ///</summary>
        ///<param name="obj">The obj.</param>
        ///<returns></returns>
        public static object SafeValue(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            else
                return obj;
        }

        #region IDataBase 成员

        ///<summary>
        /// 获得数据库连接
        ///</summary>
        ///<returns>数据库连接</returns>
        public static System.Data.Common.DbConnection GetConnection()
        {
            return new OracleConnection(ConnectionString);
        }

        #endregion

    }

    #endregion

    #region SQLite

    /// <summary>
    /// 数据访问基础类(基于SQLite)
    /// 可以用户可以修改满足自己项目的需要。
    /// </summary>
    public sealed class SqliteDBHelper
    {
        /// <summary>
        /// 是否是MSSQLServer
        /// </summary>
        public static bool IsMSSQL
        {
            get
            {
                var result = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsMSSQL"]);

                if (!result)
                {
                    connectionString = System.Configuration.ConfigurationManager.AppSettings["Sqlite"].ToString();
                }
                return result;
            }
        }

        /// <summary>
        /// 数据库连接字符串。默认读取web.config中的dbconnectionString
        /// </summary>
        private static string connectionString = string.Empty;


        /// <summary>
        /// 数据库连接字符串。默认读取web.config中的dbconnectionString
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = System.Configuration.ConfigurationManager.AppSettings["Sqlite"].ToString();
                }

                return connectionString;
            }
            set { connectionString = value; }
        }

        #region 枚举
        /// <summary>
        /// 列举操作方法
        /// </summary>
        public enum SqliteEnumOperation
        {
            INSERT,
            UPDATE
        }
        /// <summary>
        /// Sqlite数据库字段类型
        /// </summary>
        public enum SqliteDBDataType
        {
            INTEGER,
            NVARCHAR,
            NTEXT,
            DATETIME,
            BIT,
            BLOB,
            FLOAT,
            DECIMAL,
            MONEY
        }
        #endregion

        #region 自定列描述信息类
        /// <summary>
        /// 列描述信息
        /// </summary>
        public sealed class ColumnInfo
        {
            /// <summary>
            /// 序号
            /// </summary>
            public int Ordinal { get; set; }
            /// <summary>
            /// 列名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 是否可空
            /// </summary>
            public bool AllowDBNull { get; set; }
            /// <summary>
            /// 最大长度
            /// </summary>
            public int MaxLength { get; set; }
            /// <summary>
            /// 数据类型
            /// </summary>
            public string DataTypeName { get; set; }
            /// <summary>
            /// 是否自增长
            /// </summary>
            public bool AutoIncrement { get; set; }
            /// <summary>
            /// 是否主键
            /// </summary>
            public bool IsPrimaryKey { get; set; }
            /// <summary>
            /// 是否唯一
            /// </summary>
            public bool Unique { get; set; }
            /// <summary>
            /// 是否只读
            /// </summary>
            public bool IsReadOnly { get; set; }

        }
        #endregion

        #region  执行SQL
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parmas">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string sql, params SQLiteParameter[] parmas)
        {
            return SqliteDBHelper.ExecuteSql(connectionString, sql, parmas);
        }
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="sql"></param>
        /// <param name="parmas"></param>
        /// <returns></returns>
        public static int ExecuteSql(string connectionStr, string sql, params SQLiteParameter[] parmas)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionStr))
            {
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                if (parmas != null)
                {
                    cmd.Parameters.AddRange(parmas);
                }
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SQLite.SQLiteException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int InsertImage(string sql, byte[] fs)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                SQLiteParameter myParameter = new SQLiteParameter("@fs", DbType.Binary);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SQLite.SQLiteException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sql">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string sql, params SQLiteParameter[] parmas)
        {
            return SqliteDBHelper.GetSingle(connectionString, sql, parmas);
        }
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="sql"></param>
        /// <param name="parmas"></param>
        /// <returns></returns>
        public static object GetSingle(string connectionStr, string sql, params SQLiteParameter[] parmas)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        if (parmas != null)
                        {
                            cmd.Parameters.AddRange(parmas);
                        }
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SQLite.SQLiteException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>SQLiteDataReader</returns>
        public static SQLiteDataReader ExecuteReader(string sql)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            try
            {
                connection.Open();
                SQLiteDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.SQLite.SQLiteException e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string sql, params SQLiteParameter[] parmas)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                PrepareCommand(cmd, connection, null, sql, parmas);
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SQLite.SQLiteException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqls"></param>
        public static void ExecuteSqlTran(List<string> sqls)
        {
            SqliteDBHelper.ExecuteSqlTran(sqls.ToArray());
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqls">多条SQL语句</param>    
        public static void ExecuteSqlTran(string[] sqls)
        {
            if (sqls != null)
            {
                Dictionary<string, SQLiteParameter[]> plts = new Dictionary<string, SQLiteParameter[]>();
                for (int i = 0; i < sqls.Length; i++)
                {
                    plts.Add(sqls[i], null);
                }
                SqliteDBHelper.ExecuteSqlTran(plts);
            }
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SQLiteParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable parmas)
        {
            if (parmas != null && parmas.Count > 0)
            {
                Dictionary<string, SQLiteParameter[]> plts = new Dictionary<string, SQLiteParameter[]>();
                foreach (DictionaryEntry item in parmas)
                {
                    string sql = item.Key.ToString();
                    SQLiteParameter[] cmdParms = (SQLiteParameter[])item.Value;
                    plts.Add(sql, cmdParms);
                }
                SqliteDBHelper.ExecuteSqlTran(plts);
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="parmas"></param>
        public static void ExecuteSqlTran(Dictionary<string, SQLiteParameter[]> parmas)
        {
            SqliteDBHelper.ExecuteSqlTran(connectionString, parmas);
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="parmas"></param>
        public static void ExecuteSqlTran(string connectionStr, Dictionary<string, SQLiteParameter[]> parmas)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionStr))
            {
                conn.Open();
                using (SQLiteTransaction trans = conn.BeginTransaction())
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    try
                    {
                        foreach (var item in parmas)
                        {
                            string cmdText = item.Key.ToString();
                            SQLiteParameter[] cmdParms = item.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            trans.Commit();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 拼结cmd对象
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="sql"></param>
        /// <param name="parmas"></param>
        private static void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, string sql, SQLiteParameter[] parmas)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = sql;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;
            if (parmas != null)
            {
                cmd.Parameters.AddRange(parmas);
            }
        }

        #endregion

        #region 数据库操作
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="dbfileName"></param>
        /// <returns></returns>
        public static SQLiteConnection CreateDB(string dbfileName)
        {
            return SqliteDBHelper.CreateDB(dbfileName, string.Empty);
        }
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="dbfileName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static SQLiteConnection CreateDB(string dbfileName, string password)
        {
            SQLiteConnection.CreateFile(dbfileName);
            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection();
            System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
            connstr.DataSource = dbfileName;
            if (!string.IsNullOrEmpty(password))
            {
                connstr.Password = password;
            }
            conn.ConnectionString = connstr.ToString();
            return conn;
        }
        /// <summary>
        /// 更改数据库密码
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newPassword"></param>
        /// <param name="oldPassword"></param>
        /// <returns></returns>
        public static SQLiteConnection ChangePassword(string dbfileName, string newPassword, string oldPassword = null)
        {
            System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
            string connectionStr = "";
            connstr.DataSource = dbfileName;
            if (!string.IsNullOrEmpty(oldPassword))
            {
                connstr.Password = oldPassword;
            }
            using (var conn = new SQLiteConnection())
            {
                conn.ConnectionString = connstr.ToString();
                conn.Open();
                conn.ChangePassword(newPassword);
                connectionStr = conn.ConnectionString;
            }
            return new SQLiteConnection(connectionStr);
        }



        /// <summary>
        /// 收缩数据库
        /// </summary>
        /// <returns></returns>
        public static bool ShrinkDatabase()
        {
            return SqliteDBHelper.ShrinkDatabase(connectionString);
        }
        /// <summary>
        /// 收缩数据库
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <returns></returns>
        public static bool ShrinkDatabase(string connectionStr)
        {
            bool result = false;
            SqliteDBHelper.ExecuteSql("VACUUM");
            return result;
        }

        #endregion

        #region 表结构操作
        /// <summary>
        /// 获取全部表信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTables()
        {
            return SqliteDBHelper.GetTables(connectionString);
        }
        /// <summary>
        /// 获取全部表信息
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static DataTable GetTables(SQLiteConnectionStringBuilder connStr)
        {
            return SqliteDBHelper.GetTables(connStr.ToString());
        }
        /// <summary>
        /// 获取全部表信息
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <returns></returns>
        public static DataTable GetTables(string connectionStr)
        {
            DataTable schemaTable = new DataTable("TABLES");
            using (SQLiteConnection conn = new SQLiteConnection(connectionStr))
            {
                conn.Open();
                schemaTable = conn.GetSchema("TABLES");
            }
            return schemaTable;
        }

        /// <summary>
        /// 获取全部列信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMap(string tableName)
        {
            return SqliteDBHelper.GetMap(connectionString, tableName);
        }
        /// <summary>
        /// 获取全部列信息
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <returns></returns>
        public static DataTable GetMap(SQLiteConnectionStringBuilder connectionStr, string tableName)
        {
            return SqliteDBHelper.GetMap(connectionStr.ToString(), tableName);
        }
        /// <summary>
        /// 获取全部列信息
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <returns></returns>
        public static DataTable GetMap(string connectionStr, string tableName)
        {
            DataTable schemaTable = new DataTable("Colums");
            using (SQLiteConnection conn = new SQLiteConnection(connectionStr))
            {
                conn.Open();
                if (SqliteDBHelper.ExistsTable(tableName))
                {
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        tableName = tableName.Replace("'", "").Replace("%", "");
                        schemaTable = SqliteDBHelper.GetSchemaByReader(conn, tableName);
                    }
                }
            }
            return schemaTable;
        }

        /// <summary>
        /// 获取列信息（封装到自定义类）
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static ColumnInfo GetColumnInfo(string tableName, string columnName)
        {
            return SqliteDBHelper.GetColumnInfo(connectionString, tableName, columnName);
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static ColumnInfo GetColumnInfo(string connectionStr, string tableName, string columnName)
        {
            return SqliteDBHelper.GetColumnInfos(connectionStr, tableName).Where(b => b.Name == columnName).FirstOrDefault();
        }

        /// <summary>
        /// 获取表中全部列信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<ColumnInfo> GetColumnInfos(string tableName)
        {
            return SqliteDBHelper.GetColumnInfos(connectionString, tableName);
        }
        /// <summary>
        /// 获取表中全部列信息
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<ColumnInfo> GetColumnInfos(string connectionStr, string tableName)
        {
            var clts = new List<ColumnInfo>();
            DataTable schemaTable = SqliteDBHelper.GetMap(connectionStr, tableName);
            if (schemaTable != null && schemaTable.Rows.Count > 0)
            {
                foreach (DataRow dr in schemaTable.Rows)
                {
                    ColumnInfo info = new ColumnInfo();
                    info.Name = dr["ColumnName"].ToString().Trim();
                    info.Ordinal = Convert.ToInt32(dr["ColumnOrdinal"].ToString());
                    info.AllowDBNull = (bool)dr["AllowDBNull"];
                    info.MaxLength = Convert.ToInt32(dr["ColumnSize"].ToString());
                    info.DataTypeName = dr["DataTypeName"].ToString().Trim();
                    info.AutoIncrement = (bool)dr["IsAutoIncrement"];
                    info.IsPrimaryKey = (bool)dr["IsKey"];
                    info.Unique = (bool)dr["IsUnique"];
                    info.IsReadOnly = (bool)dr["IsReadOnly"];
                    clts.Add(info);
                }
            }
            return clts;
        }


        /// <summary>
        /// 获取列描述
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static DataTable GetSchemaByReader(string tableName)
        {
            return SqliteDBHelper.GetSchemaByReader(tableName, connectionString);
        }
        /// <summary>
        /// 获取列描述
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static DataTable GetSchemaByReader(string connectionStr, string tableName)
        {
            DataTable dt = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(connectionStr))
            {
                conn.Open();
                dt = SqliteDBHelper.GetSchemaByReader(conn, tableName);
            }
            return dt;
        }
        /// <summary>
        /// 获取列描述
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <returns></returns> 
        private static DataTable GetSchemaByReader(SQLiteConnection connection, string tableName)
        {
            DataTable schemaTable = null;
            IDbCommand cmd = new SQLiteCommand();
            cmd.CommandText = string.Format("SELECT * FROM [{0}]", tableName);
            cmd.Connection = connection;
            using (IDataReader reader = cmd.ExecuteReader(CommandBehavior.KeyInfo | CommandBehavior.SchemaOnly))
            {
                schemaTable = reader.GetSchemaTable();
            }
            return schemaTable;
        }

        /// <summary>
        /// 获取主键信息
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetPKColumn(string tableName)
        {
            return SqliteDBHelper.GetPKColumn(tableName, connectionString);
        }
        /// <summary>
        /// 获取主键信息
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetPKColumn(string connectionStr, string tableName)
        {
            DataTable dt = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(connectionStr))
            {
                conn.Open();
                dt = SqliteDBHelper.GetPKColumn(conn, tableName);
            }
            return dt;
        }
        /// <summary>
        /// 获取主键信息
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetPKColumn(SQLiteConnection connection, string tableName)
        {
            DataTable schemaTable = null;
            IDbCommand cmd = new SQLiteCommand();
            cmd.CommandText = string.Format("SELECT * FROM [{0}] limit 0,1", tableName);
            cmd.Connection = connection;
            using (IDataReader reader = cmd.ExecuteReader(CommandBehavior.KeyInfo))
            {
                schemaTable = reader.GetSchemaTable();
            }
            return schemaTable;
        }

        /// <summary>
        /// 更改表名
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="oldTableName"></param>
        /// <returns></returns>
        public static bool ReNameTable(string tableName, string oldTableName)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(oldTableName))
                {
                    tableName = tableName.Replace("'", "").Replace("%", "");
                    oldTableName = oldTableName.Replace("'", "").Replace("%", "");
                    if (SqliteDBHelper.ExistsTable(oldTableName) && oldTableName != tableName)
                    {
                        string sql = "Alter Table MAIN.[" + oldTableName + "] Rename To [" + tableName + "];";
                        if (SqliteDBHelper.ExecuteSql(sql) > 0)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 添加或者编辑表
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static bool CreateTable(string tableName, List<ColumnInfo> columns)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(tableName) && columns != null && columns.Count > 0)
                {
                    tableName = tableName.Replace("'", "").Replace("%", "");
                    string sql = "";

                    #region 增加表
                    if (!SqliteDBHelper.ExistsTable(tableName))
                    {
                        sql = "Create TABLE MAIN.[" + tableName + "](";
                        columns = columns.OrderBy(b => b.Ordinal).ToList();
                        foreach (var item in columns)
                        {
                            if (item.IsPrimaryKey)
                            {
                                sql += " [" + item.Name + "] integer PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL,";
                            }
                            else
                            {
                                string allowNull = ",";
                                if (!item.AllowDBNull)
                                {
                                    allowNull = " NOT NULL, ";
                                }
                                if (item.DataTypeName.ToLower().IndexOf("char") > -1)
                                {
                                    sql += "[" + item.Name + "] nvarchar(" + item.MaxLength + ")" + allowNull;
                                }
                                else if (item.DataTypeName.ToLower().IndexOf("decimal") > -1)
                                {
                                    sql += "[" + item.Name + "] decimal(18,2)" + allowNull;
                                }
                                else
                                {
                                    sql += "[" + item.Name + "] " + item.DataTypeName + allowNull;
                                }
                            }
                        }
                        sql = sql.Substring(0, sql.LastIndexOf(","));
                        sql += ");";
                    }
                    #endregion

                    #region 编辑表
                    else
                    {
                        sql = "Begin Transaction;";
                        var rndStr = new Random(DateTime.Now.Millisecond).Next(999999, 9999999).ToString();
                        var tempTableName = "Temp_" + rndStr;
                        sql += "Create  TABLE MAIN.[" + tempTableName + "](";
                        string fileds = "";
                        var oldColumns = SqliteDBHelper.GetColumnInfos(tableName);
                        columns = columns.OrderBy(b => b.Ordinal).ToList();
                        foreach (var item in columns)
                        {
                            if (item.IsPrimaryKey)
                            {
                                sql += " [" + item.Name + "] integer PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL,";
                            }
                            else
                            {
                                string allowNull = ",";
                                if (!item.AllowDBNull)
                                {
                                    allowNull = " NOT NULL, ";
                                }
                                if (item.DataTypeName.ToLower().IndexOf("char") > -1)
                                {
                                    sql += "[" + item.Name + "] nvarchar(" + item.MaxLength + ")" + allowNull;
                                }
                                else if (item.DataTypeName.ToLower().IndexOf("decimal") > -1)
                                {
                                    sql += "[" + item.Name + "] decimal(18,2)" + allowNull;
                                }
                                else
                                {
                                    sql += "[" + item.Name + "] " + item.DataTypeName + allowNull;
                                }
                            }
                            if (oldColumns.Where(b => b.Name == item.Name).Count() > 0)
                            {
                                fileds += "[" + item.Name + "],";
                            }
                        }
                        sql = sql.Substring(0, sql.LastIndexOf(","));
                        sql += ");";
                        fileds = fileds.Substring(0, fileds.LastIndexOf(","));
                        sql += "Insert Into MAIN.[" + tempTableName + "](" + fileds + ") Select " + fileds + "From MAIN.[" + tableName + "];Drop Table MAIN.[" + tableName + "];Alter Table MAIN.[" + tempTableName + "] Rename To [" + tableName + "];Commit Transaction;";
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(sql))
                    {
                        if (SqliteDBHelper.ExecuteSql(sql) > 0)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 更改列属性
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static bool AlterColumns(string tableName, List<ColumnInfo> columns)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(tableName) && columns != null && columns.Count > 0)
                {
                    tableName = tableName.Replace("'", "").Replace("%", "");
                    string sql = "";

                    #region 更改列
                    if (SqliteDBHelper.ExistsTable(tableName))
                    {
                        sql = "Begin Transaction;";
                        var rndStr = new Random(DateTime.Now.Millisecond).Next(999999, 9999999).ToString();
                        var tempTableName = "Temp_" + rndStr;
                        sql += "Create  TABLE MAIN.[" + tempTableName + "](";
                        string fileds = "";
                        string fileds2 = "";
                        var oldColumns = SqliteDBHelper.GetColumnInfos(tableName);
                        if (oldColumns != null && oldColumns.Count > 0)
                        {
                            oldColumns = oldColumns.OrderBy(b => b.Ordinal).ToList();
                            foreach (var item in oldColumns)
                            {
                                fileds2 += "[" + item.Name + "],";
                            }
                        }
                        columns = columns.OrderBy(b => b.Ordinal).ToList();
                        foreach (var item in columns)
                        {
                            if (item.IsPrimaryKey)
                            {
                                sql += " [" + item.Name + "] integer PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL,";
                            }
                            else
                            {
                                string allowNull = ",";
                                if (!item.AllowDBNull)
                                {
                                    allowNull = " NOT NULL, ";
                                }
                                if (item.DataTypeName.ToLower().IndexOf("char") > -1)
                                {
                                    sql += "[" + item.Name + "] nvarchar(" + item.MaxLength + ")" + allowNull;
                                }
                                else if (item.DataTypeName.ToLower().IndexOf("decimal") > -1)
                                {
                                    sql += "[" + item.Name + "] decimal(18,2)" + allowNull;
                                }
                                else
                                {
                                    sql += "[" + item.Name + "] " + item.DataTypeName + allowNull;
                                }
                            }
                            fileds += "[" + item.Name + "],";
                        }
                        sql = sql.Substring(0, sql.LastIndexOf(","));
                        sql += ");";
                        fileds = fileds.Substring(0, fileds.LastIndexOf(","));
                        fileds2 = fileds2.Substring(0, fileds2.LastIndexOf(","));
                        sql += "Insert Into MAIN.[" + tempTableName + "](" + fileds + ") Select " + fileds2 + "From MAIN.[" + tableName + "];Drop Table MAIN.[" + tableName + "];Alter Table MAIN.[" + tempTableName + "] Rename To [" + tableName + "];Commit Transaction;";
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(sql))
                    {
                        if (SqliteDBHelper.ExecuteSql(sql) > 0)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool DropTable(string tableName)
        {
            var tlts = new List<string>();
            tlts.Add(tableName);
            return SqliteDBHelper.DropTables(tlts);
        }

        /// <summary>
        /// 删除多张表
        /// </summary>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public static bool DropTables(List<string> tableNames)
        {
            bool result = false;
            if (tableNames != null)
            {
                List<string> sqls = new List<string>();
                foreach (var item in tableNames)
                {
                    var tableName = item.Replace("'", "").Replace("%", "").Replace("[", "").Replace("]", "");
                    sqls.Add(" DROP TABLE [" + tableName + "];");
                }
                SqliteDBHelper.ExecuteSqlTran(sqls);
            }
            return result;
        }


        #endregion

        #region 数据操作
        /// <summary>
        /// 根据ID获取记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetRecord(string tableName, int id)
        {
            DictionaryEntry de = new DictionaryEntry();
            de.Key = "ID";
            de.Value = id;
            return SqliteDBHelper.GetRecords(tableName, de);
        }
        /// <summary>
        /// 按条件搜索记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sqlParmas"></param>
        /// <returns></returns>
        public static DataTable GetRecords(string tableName, params DictionaryEntry[] sqlParmas)
        {
            return SqliteDBHelper.GetRecords(tableName, 1, int.MaxValue, sqlParmas);
        }
        /// <summary>
        /// 按条件搜索记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sqlParmas"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static DataTable GetRecords(string tableName, int pageIndex, int pageSize, params DictionaryEntry[] sqlParmas)
        {
            DataTable dt = new DataTable(tableName);
            try
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    tableName = tableName.Replace("'", "").Replace("%", "");

                    string sql = "SELECT * FROM [" + tableName + "] ";
                    string where = " WHERE 1=1 ";

                    List<SQLiteParameter> parmas = new List<SQLiteParameter>();

                    var columns = SqliteDBHelper.GetColumnInfos(tableName);

                    if (sqlParmas != null && sqlParmas.Length > 0)
                    {
                        foreach (var item in sqlParmas)
                        {
                            var ct = columns.Where(b => b.Name.ToUpper() == item.Key.ToString().ToUpper()).FirstOrDefault();
                            if (ct != null && !string.IsNullOrEmpty(ct.Name))
                            {
                                where += " AND [" + item.Key + "]=@" + item.Key;
                                parmas.Add(new SQLiteParameter("@" + item.Key, item.Value));
                            }
                        }
                    }
                    sql += where + " Limit " + pageSize + " Offset " + ((pageIndex - 1) * pageSize);
                    dt = SqliteDBHelper.Query(sql, parmas.ToArray()).Tables[0];
                }
            }
            catch { }
            return dt;
        }

        /// <summary>
        /// MVC前端传值添加或更新数据
        /// </summary>
        /// <param name="id">更新时的主键值</param>
        /// <param name="tableName">表名</param>
        /// <param name="collection">form表单</param>
        /// <param name="operation">更新类型</param>
        /// <returns>更新的记录数</returns>
        public static int UpdateRecord(int? id, string tableName,
            FormCollection collection,
            SqliteEnumOperation operation)
        {
            var dics = new Dictionary<string, object>();
            collection.CopyTo(dics);
            return SqliteDBHelper.UpdateRecord(id ?? 0, tableName, dics, operation);
        }
        /// <summary>
        /// 将模型添加或更新到数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="tableName"></param>
        /// <param name="t"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static int UpdateRecord<T>(T t, SqliteEnumOperation operation) where T : new()
        {
            int id = 0;
            Type type = typeof(T);
            var dics = new Dictionary<string, object>();
            var plts = t.GetType().GetProperties();
            if (plts != null)
            {
                foreach (var item in plts)
                {
                    if (item.Name.ToLower() == "id")
                    {
                        id = Convert.ToInt32(item.GetValue(t, null));
                    }
                    dics.Add(item.Name, item.GetValue(t, null));
                }
            }
            return SqliteDBHelper.UpdateRecord(id, type.Name, dics, operation);
        }
        /// <summary>
        /// 添加或更新数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tableName"></param>
        /// <param name="datas"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static int UpdateRecord(int id, string tableName, Dictionary<string, object> datas, SqliteEnumOperation operation)
        {
            int error = 0;
            try
            {
                var clts = SqliteDBHelper.GetColumnInfos(tableName.ToString());
                List<SQLiteParameter> parmas = new List<SQLiteParameter>();
                string sqlStr = string.Empty;
                if (operation == SqliteEnumOperation.INSERT)
                {
                    sqlStr = operation.ToString() + " INTO [" + tableName + "] ";
                    string conStrColumn = "";
                    string conStrValue = "";
                    foreach (var item in clts)
                    {
                        string columnName = item.Name;
                        if (datas[columnName] != null && !item.IsPrimaryKey)
                        {
                            string itemValue = datas[columnName].ToString().Replace("'", "").Trim();
                            if (item.DataTypeName.ToUpper() == "DATETIME")
                            {
                                conStrColumn += " [" + columnName + "] ,";
                                conStrValue += " @" + columnName + " ,";
                                if (string.IsNullOrEmpty(itemValue))
                                {
                                    parmas.Add(new SQLiteParameter("@" + columnName, DBNull.Value));
                                }
                                else
                                {
                                    parmas.Add(new SQLiteParameter("@" + columnName, (object)Convert.ToDateTime(itemValue).ToString("yyyy-MM-dd hh:mm:ss.f", System.Globalization.CultureInfo.InvariantCulture)));
                                }
                            }
                            else if (item.DataTypeName.ToUpper() == "BIT")
                            {
                                conStrColumn += " [" + columnName + "] ,";
                                conStrValue += " @" + columnName + " ,";
                                if (string.IsNullOrEmpty(itemValue))
                                {
                                    parmas.Add(new SQLiteParameter("@" + columnName, DBNull.Value));
                                }
                                else
                                {
                                    if (itemValue.ToUpper().Trim() == "True")
                                    {
                                        itemValue = "1";
                                    }
                                    else
                                    {
                                        itemValue = "0";
                                    }
                                    parmas.Add(new SQLiteParameter("@" + columnName, (object)itemValue));
                                }

                            }
                            else
                            {
                                conStrColumn += " [" + columnName + "] ,";
                                conStrValue += " @" + columnName + " ,";
                                if (string.IsNullOrEmpty(itemValue))
                                {
                                    parmas.Add(new SQLiteParameter("@" + columnName, DBNull.Value));
                                }
                                else
                                {
                                    parmas.Add(new SQLiteParameter("@" + columnName, (object)itemValue));
                                }

                            }
                        }
                    }
                    conStrColumn = conStrColumn.Substring(0, conStrColumn.LastIndexOf(","));
                    conStrValue = conStrValue.Substring(0, conStrValue.LastIndexOf(","));
                    sqlStr = sqlStr + " (" + conStrColumn + ") VALUES (" + conStrValue + ")";
                }
                else if (operation == SqliteEnumOperation.UPDATE)
                {
                    sqlStr = operation.ToString() + " [" + tableName + "] ";
                    if (id > 0)
                    {
                        string conStrColumn = "";
                        string ConStr = " SET ";
                        string PKColumnName = "";
                        foreach (var item in clts)
                        {
                            string columnName = item.Name;
                            if (datas[columnName] != null && !item.IsPrimaryKey)
                            {
                                string itemValue = datas[columnName].ToString().Replace("'", "").Trim();

                                if (item.DataTypeName.ToUpper() == "DATETIME")
                                {
                                    conStrColumn += " [" + columnName + "]" + " =@" + columnName + " ,";
                                    if (!string.IsNullOrEmpty(itemValue))
                                    {
                                        parmas.Add(new SQLiteParameter("@" + columnName, (object)Convert.ToDateTime(itemValue).ToString("yyyy-MM-dd hh:mm:ss.f", System.Globalization.CultureInfo.InvariantCulture)));
                                    }
                                    else
                                    {
                                        parmas.Add(new SQLiteParameter("@" + columnName, DBNull.Value));
                                    }
                                }
                                else if (item.DataTypeName.ToUpper() == "BIT")
                                {
                                    if (!string.IsNullOrEmpty(itemValue) && itemValue.ToUpper().Trim() != "FALSE")
                                    {
                                        itemValue = "1";
                                    }
                                    else
                                    {
                                        itemValue = "0";
                                    }
                                    conStrColumn += " [" + columnName + "]" + " =@" + columnName + " ,";
                                    parmas.Add(new SQLiteParameter("@" + columnName, (object)itemValue));
                                }
                                else
                                {
                                    conStrColumn += " [" + columnName + "]" + " =@" + columnName + " ,";
                                    if (!string.IsNullOrEmpty(itemValue))
                                    {
                                        parmas.Add(new SQLiteParameter("@" + columnName, (object)itemValue));
                                    }
                                    else
                                    {
                                        parmas.Add(new SQLiteParameter("@" + columnName, DBNull.Value));
                                    }
                                }
                            }
                            else
                            {
                                PKColumnName = columnName;
                            }
                        }
                        ConStr += conStrColumn;
                        ConStr = ConStr.Substring(0, ConStr.Length - 1);
                        sqlStr += ConStr + " WHERE [" + PKColumnName + "]=" + id;
                    }
                    else
                    {
                        error = -1;
                    }
                }
                if (error == -1)
                    return error;
                else
                    return SqliteDBHelper.ExecuteSql(sqlStr, parmas.ToArray());
            }
            catch { }
            return error;

        }

        /// <summary>
        /// 删除某条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteRecord(string tableName, int id)
        {
            var dlts = new Dictionary<string, object>();
            dlts.Add("ID", id);
            return SqliteDBHelper.DeleteRecords(tableName, dlts);
        }
        /// <summary>
        /// 删除全部记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool DeleteRecords(string tableName)
        {
            return SqliteDBHelper.DeleteRecords(tableName, new Dictionary<string, object>());
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="wheres"></param>
        /// <returns></returns>
        public static bool DeleteRecords(string tableName, Dictionary<string, object> wheres)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    tableName = tableName.Replace("'", "").Replace("%", "");

                    string sql = "DELETE FROM [" + tableName + "] ";
                    string where = " WHERE 1=1 ";
                    List<SQLiteParameter> parmas = new List<SQLiteParameter>();

                    var columns = SqliteDBHelper.GetColumnInfos(tableName);

                    if (wheres != null && wheres.Count > 0)
                    {
                        foreach (var item in wheres)
                        {
                            var ct = columns.Where(b => b.Name.ToUpper() == item.Key.ToUpper()).FirstOrDefault();
                            if (ct != null && !string.IsNullOrEmpty(ct.Name))
                            {
                                where += " AND [" + item.Key + "]=@" + item.Key;
                                parmas.Add(new SQLiteParameter("@" + item.Key, item.Value));
                            }
                        }
                    }
                    sql += where;
                    if (SqliteDBHelper.ExecuteSql(sql, parmas.ToArray()) > -1)
                    {
                        result = true;
                    }
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 清空表数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool Trancate(string tableName)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    tableName = tableName.Replace("'", "").Replace("%", "");
                    var al = new List<string>();
                    al.Add("DELETE FROM '" + tableName + "';");
                    al.Add("DELETE FROM sqlite_sequence WHERE name = '" + tableName + "';");
                    SqliteDBHelper.ExecuteSqlTran(al);
                    result = true;
                }
            }
            catch { }
            return result;
        }
        #endregion

        #region 公用方法
        /// <summary>
        /// 返回最大ID
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static int GetMaxID(string fieldName, string tableName)
        {
            int result = 0;
            try
            {
                if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(fieldName))
                {
                    tableName = tableName.Replace("'", "").Replace("%", "");
                    fieldName = fieldName.Replace("'", "").Replace("%", "");

                    string strsql = "SELECT MAX(" + fieldName + ")+1 FROM " + tableName;
                    object obj = SqliteDBHelper.GetSingle(strsql);
                    if (obj == null)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = int.Parse(obj.ToString());
                    }
                }
            }
            catch { }
            return result;
        }

        public static bool Exists(string strSql)
        {
            object obj = SqliteDBHelper.GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool Exists(string strSql, params SQLiteParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检测Sqlite库中指定表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool ExistsTable(string tableName)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    tableName = tableName.Replace("'", "").Replace("%", "");
                    string sql = "SELECT  COUNT('') FROM [sqlite_master] WHERE [type] = 'table' AND [name] = '" + tableName + "';";
                    int count = Convert.ToInt32(SqliteDBHelper.GetSingle(sql));
                    if (count > 0)
                    {
                        result = true;
                    }
                }
            }
            catch { }
            return result;
        }

        #endregion


    }

    #endregion

    #region SQLServer
    /// <summary>
    /// 数据访问抽象基础类
    /// 也可以去掉static,修改为普通类，通过实例化的构造函数来更换不同的ConnectionString
    /// </summary>
    public abstract class SQLServerHelper
    {
        public static string connectionString = System.Configuration.ConfigurationSettings.AppSettings["SqlServer"];
        protected static SqlConnection Connection = new SqlConnection(connectionString);
        protected static System.Data.IDbTransaction tran;

        public SQLServerHelper()
        {
            DataTable dt = new DataTable();
        }

        #region 公用方法

        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        cmd.CommandTimeout = 600;
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        public static int ExecuteSqlByTime(string SQLString, int Times)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static void ExecuteSqlTran(ArrayList SQLStringList)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                SqlTransaction tx = connection.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, string content)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@content", SqlDbType.NText);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                }
            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static object ExecuteSqlGet(string SQLString, string content)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@content", SqlDbType.NText);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                }
            }
        }
        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@fs", SqlDbType.Image);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        cmd.CommandTimeout = 180;
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回SqlDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string strSQL)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                try
                {
                    connection.Open();
                    SqlDataReader myReader = cmd.ExecuteReader();
                    return myReader;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    cmd.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                    ds.AcceptChanges();
                    command.Dispose();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
        public static DataSet Query(string SQLString, int Times)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = Times;
                    command.Fill(ds, "ds");
                    ds.AcceptChanges();
                    command.Dispose();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }



        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        connection.Open();
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                        connection.Close();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction trans = connection.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                            PrepareCommand(cmd, connection, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            cmd.Dispose();
                            trans.Commit();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            connection.Close();
                            return null;
                        }
                        else
                        {
                            connection.Close();
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回SqlDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        ds.AcceptChanges();
                        da.Dispose();
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return ds;
                }
            }
        }


        private static void PrepareCommand(SqlCommand cmd, SqlConnection connection, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            cmd.Connection = connection;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion

        #region 存储过程操作

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataReader returnReader;
                connection.Open();
                SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
                command.CommandType = CommandType.StoredProcedure;
                returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return returnReader;
            }
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                dataSet.AcceptChanges();
                sqlDA.Dispose();
                return dataSet;
            }
        }
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.SelectCommand.CommandTimeout = Times;
                sqlDA.Fill(dataSet, tableName);
                return dataSet;
            }
        }


        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }

        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int result;
                connection.Open();
                SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                command.Dispose();
                return result;
            }
        }

        /// <summary>
        /// 创建 SqlCommand 对象实例(用来返回一个整数值)	
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand 对象实例</returns>
        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }
        #endregion

        #region 事务处理

        public static System.Data.IDbTransaction BeginTransaction()
        {
            Connection.Open();
            tran = Connection.BeginTransaction();
            return tran;
        }

        public static void Commit()
        {
            if (tran != null)
                tran.Commit();
            Connection.Close();
        }

        public static void Rollback()
        {
            if (tran != null)
                tran.Rollback();
            Connection.Close();
        }

        #endregion

        #region 大批理数据插入
        public static bool BulkCopy(string talbeName, DataTable dt)
        {
            bool result = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = talbeName;
                    bulkCopy.BatchSize = dt.Rows.Count;
                    try
                    {
                        connection.Open();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                        }
                        bulkCopy.WriteToServer(dt);
                        result = true;
                    }
                    catch { }
                }
            }
            return result;
        }
        #endregion

    }
    #endregion

}
