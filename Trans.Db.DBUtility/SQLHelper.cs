using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Configuration;

namespace Trans.Db.DBUtility
{
    /// <summary>
    /// MS-SQL数据库访问类
    /// </summary>
    internal class SQLHelper
    {
        private SqlConnection SQLConn = null;
        private bool IsReadOnly = false;
        private string _ConnectionMark = "TRANS";

        private DateTime begintime = new DateTime();
        private DateTime endtime = new DateTime();
        /// <summary>
        /// 构造函数
        /// </summary>
        public SQLHelper(bool isReadOnly)
        {
            this.IsReadOnly = isReadOnly;
            if (SQLConn == null)
            {
                SQLConn = new SqlConnection(MyConnStr);
            }
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        private string MyConnStr
        {
            get
            {
                string ConfigStr = string.Empty;
                if (!IsReadOnly)
                {
                    ConfigStr = ConfigurationManager.ConnectionStrings[_ConnectionMark + "_WRITE"].ConnectionString;
                }
                else
                {
                    ConfigStr = ConfigurationManager.ConnectionStrings[_ConnectionMark + "_READ"].ConnectionString;
                }
                return ConfigStr;
            }
        }
        /// <summary>
        /// 参数缓存哈希表
        /// </summary>
        private Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        #region 执行SQL语句
        /// <summary>
        /// 执行SQL语句,返回数据库中影响的行数
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns></returns>
        public int ExecNonquery(string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                CommonPreCmd(cmdText, cmd, null, CommandType.Text, commandParameters);
                begintime = DateTime.Now;
                int val = cmd.ExecuteNonQuery();
                endtime = DateTime.Now;
                cmd.Parameters.Clear();
                this.Close();
                return val;
            }
            catch
            {
                this.Close();
                return -1;
            }
        }

        /// <summary>
        /// 执行SQL语句,返回数据库中影响的行数
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <returns></returns>
        public int ExecNonquery(string cmdText)
        {
            return ExecNonquery(cmdText, null);
        }

        /// <summary>
        /// 执行SQL语句,返回数据中第一行第一列的值
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns></returns>
        public object ExecScalar(string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                CommonPreCmd(cmdText, cmd, null, CommandType.Text, commandParameters);
                begintime = DateTime.Now;
                object val = cmd.ExecuteScalar();
                endtime = DateTime.Now;
                cmd.Parameters.Clear();
                this.Close();
                return val;
            }
            catch
            {
                this.Close();
                return -1;
            }
        }

        /// <summary>
        /// 执行SQL语句,返回数据中第一行第一列的值
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <returns></returns>
        public object ExecScalar(string cmdText)
        {
            return ExecScalar(cmdText, null);
        }

        /// <summary>
        /// 执行SQL语句,返回一个ExecuteReader
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns></returns>
        public SqlDataReader ExecReader(string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                CommonPreCmd(cmdText, cmd, null, CommandType.Text, commandParameters);
                begintime = DateTime.Now;
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                endtime = DateTime.Now;

                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                this.Close();
                return null;
            }
        }

        /// <summary>
        /// 执行SQL语句,返回一个ExecuteReader
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string cmdText)
        {
            return ExecReader(cmdText, null);
        }

        /// <summary>
        /// 执行SQL语句,返回DataSet
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns></returns>
        public DataSet ExecDataSet(string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                CommonPreCmd(cmdText, cmd, null, CommandType.Text, commandParameters);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                begintime = DateTime.Now;
                da.Fill(ds);
                cmd.Parameters.Clear();
                this.Close();
                return ds;
            }
            catch
            {
                this.Close();
                return null;
            }
        }

        /// <summary>
        /// 执行SQL语句,返回DataSet
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <returns></returns>
        public DataSet ExecDataSet(string cmdText)
        {
            return ExecDataSet(cmdText, null);
        }

        /// <summary>
        /// 执行SQL语句,返回DataTable
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns></returns>
        public DataTable ExecDataTable(string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                CommonPreCmd(cmdText, cmd, null, CommandType.Text, commandParameters);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                begintime = DateTime.Now;
                da.Fill(dt);
                cmd.Parameters.Clear();
                this.Close();
                return dt;
            }
            catch
            {
                this.Close();
                return null;
            }
        }

        /// <summary>
        /// 执行SQL语句,返回DataSet
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <returns></returns>
        public DataTable ExecDataTable(string cmdText)
        {
            return ExecDataTable(cmdText, null);
        }

        /// <summary>
        /// 执行SQL语句,返回ExecuteXmlReader
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns></returns>
        public XmlReader ExecXmlReader(string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                CommonPreCmd(cmdText, cmd, null, CommandType.Text, commandParameters);

                begintime = DateTime.Now;

                XmlReader xr = cmd.ExecuteXmlReader();

                xr.MoveToElement();
                xr.Close();

                return xr;
            }
            catch
            {
                this.Close();
                return null;
            }
        }

        /// <summary>
        /// 执行SQL语句,返回ExecuteXmlReader
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <returns></returns>
        public XmlReader ExecXmlReader(string cmdText)
        {
            return ExecXmlReader(cmdText, null);
        }
        #endregion

        #region 执行存储过程
        /// <summary>
        /// 执行存储过程,返回数据库中影响的行数
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns></returns>
        public int ExecNonquery_Proc(string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                CommonPreCmd(cmdText, cmd, null, CommandType.StoredProcedure, commandParameters);

                begintime = DateTime.Now;

                int val = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                this.Close();
                return val;
            }
            catch
            {
                this.Close();
                return -1;
            }
        }

        /// <summary>
        /// 执行存储过程,返回数据库中影响的行数
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <returns></returns>
        public int ExecNonquery_Proc(string cmdText)
        {
            return ExecNonquery_Proc(cmdText, null);
        }

        /// <summary>
        /// 执行存储过程,返回数据中第一行第一列的值
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns></returns>
        public object ExecScalar_Proc(string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                CommonPreCmd(cmdText, cmd, null, CommandType.StoredProcedure, commandParameters);

                begintime = DateTime.Now;

                object val = cmd.ExecuteScalar();

                cmd.Parameters.Clear();
                this.Close();
                return val;
            }
            catch
            {
                this.Close();
                return -1;
            }
        }

        /// <summary>
        /// 执行存储过程,返回数据中第一行第一列的值
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <returns></returns>
        public object ExecScalar_Proc(string cmdText)
        {
            return ExecScalar_Proc(cmdText, null);
        }

        /// <summary>
        /// 执行存储过程,返回一个ExecuteReader
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns></returns>
        public SqlDataReader ExecReader_Proc(string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                CommonPreCmd(cmdText, cmd, null, CommandType.StoredProcedure, commandParameters);

                begintime = DateTime.Now;

                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                this.Close();
                return null;
            }
        }

        /// <summary>
        /// 执行存储过程,返回一个ExecuteReader
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <returns></returns>
        public SqlDataReader ExecReader_Proc(string cmdText)
        {
            return ExecReader_Proc(cmdText, null);
        }

        /// <summary>
        /// 执行存储过程,返回DataSet
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns></returns>
        public DataSet ExecDataSet_Proc(string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                CommonPreCmd(cmdText, cmd, null, CommandType.StoredProcedure, commandParameters);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                begintime = DateTime.Now;

                da.Fill(ds);

                cmd.Parameters.Clear();
                this.Close();
                return ds;
            }
            catch
            {
                this.Close();
                return null;
            }
        }

        /// <summary>
        /// 执行存储过程,返回DataSet
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <returns></returns>
        public DataSet ExecDataSet_Proc(string cmdText)
        {
            return ExecDataSet_Proc(cmdText, null);
        }

        /// <summary>
        /// 执行存储过程,返回DataTable
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns></returns>
        public DataTable ExecDataTable_Proc(string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                CommonPreCmd(cmdText, cmd, null, CommandType.StoredProcedure, commandParameters);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                begintime = DateTime.Now;

                da.Fill(dt);

                cmd.Parameters.Clear();
                this.Close();
                return dt;
            }
            catch
            {
                this.Close();
                return null;
            }
        }

        /// <summary>
        /// 执行存储过程,返回DataSet
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <returns></returns>
        public DataTable ExecDataTable_Proc(string cmdText)
        {
            return ExecDataTable_Proc(cmdText, null);
        }

        /// <summary>
        /// 执行存储过程,返回ExecuteXmlReader
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns></returns>
        public XmlReader ExecXmlReader_Proc(string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                CommonPreCmd(cmdText, cmd, null, CommandType.StoredProcedure, commandParameters);

                begintime = DateTime.Now;

                XmlReader xr = cmd.ExecuteXmlReader();

                xr.MoveToElement();
                xr.Close();

                return xr;
            }
            catch
            {
                this.Close();
                return null;
            }
        }

        /// <summary>
        /// 执行存储过程,返回ExecuteXmlReader
        /// </summary>
        /// <param name="cmdText">存储过程</param>
        /// <returns></returns>
        public XmlReader ExecXmlReader_Proc(string cmdText)
        {
            return ExecXmlReader_Proc(cmdText, null);
        }
        #endregion

        #region 执行SQL命令
        public int ExecNonquery(SqlCommand cmd)
        {
            try
            {
                cmd.Connection = this.SQLConn;
                this.Open();

                begintime = DateTime.Now;

                int val = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                this.Close();
                return val;
            }
            catch(Exception e)
            {

                return -1;
            }
        }
        public object ExecScalar(SqlCommand cmd)
        {
            try
            {
                cmd.Connection = this.SQLConn;
                this.Open();

                begintime = DateTime.Now;

                object obj = cmd.ExecuteScalar();

                this.Close();
                return obj;
            }
            catch
            {
                this.Close();
                return null;
            }
        }
        public DataTable ExecDataTable(SqlCommand cmd)
        {
            try
            {
                cmd.Connection = this.SQLConn;
                this.Open();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                begintime = DateTime.Now;

                adp.Fill(dt);

                this.Close();
                return dt;
            }
            catch
            {
                this.Close();
                return null;
            }
        }
        public DataSet ExecDataSet(SqlCommand cmd)
        {
            try
            {
                cmd.Connection = this.SQLConn;
                this.Open();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                begintime = DateTime.Now;

                adp.Fill(ds);

                this.Close();
                return ds;
            }
            catch
            {
                this.Close();
                return null;
            }
        }
        #endregion

        #region 外部公共方法
        /// <summary>
        /// 打开连接
        /// </summary>
        public void Open()
        {
            if (this.SQLConn == null)
            {
                this.SQLConn = new SqlConnection(this.MyConnStr);
            }
            if (this.SQLConn != null && this.SQLConn.State == ConnectionState.Closed)
            {
                this.SQLConn.Open();
            }
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (this.SQLConn != null && this.SQLConn.State == ConnectionState.Open)
            {
                this.SQLConn.Close();
                this.SQLConn.Dispose();
            }
        }
        #endregion

        #region 内部公共方法
        /// <summary>
        /// 把需要传递的参数放入缓存中
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="commandParameters"></param>
        void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// 操作时把放入缓存中的参数取出来
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// 对命令属性进行初始化
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// 对命令属性进行初始化（重载，无parms参数）
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;
        }
        /// <summary>
        /// 初始化各项数据
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="cmd"></param>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="commandParameters"></param>
        void CommonPreCmd(string cmdText, SqlCommand cmd, SqlTransaction trans, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            if (commandParameters != null)
            {
                PrepareCommand(cmd, SQLConn, trans, cmdType, cmdText, commandParameters);
            }
            else
            {
                PrepareCommand(cmd, SQLConn, trans, cmdType, cmdText);
            }
        }
        #endregion

        #region 事务处理
        /// <summary>
        /// 事务提交多个数据库操作,返回每一次执行影响的行数列表
        /// </summary>
        /// <param name="cmdTextList">SQL语句组</param>
        /// <returns></returns>
        public List<int> ExecTransaction(List<string> cmdTextList)
        {
            List<int> ExecQueryList = new List<int>();
            string timePoint = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string transName = "TRANS" + timePoint;
            this.Open();
            SqlCommand cmd = this.SQLConn.CreateCommand();
            SqlTransaction trans = this.SQLConn.BeginTransaction(transName);
            cmd.Connection = this.SQLConn;
            cmd.Transaction = trans;
            try
            {
                trans.Save(transName);
                for (int i = 0; i < cmdTextList.Count; i++)
                {
                    cmd.CommandText = cmdTextList[i];
                    ExecQueryList.Add(cmd.ExecuteNonQuery());
                }
                trans.Commit();
            }
            catch
            {
                trans.Rollback(transName);
            }
            this.Close();
            return ExecQueryList;
        }

        /// <summary>
        /// 事务提交多个数据库操作,返回每一次执行影响的行数列表
        /// </summary>
        /// <param name="cmdTextList">SQL语句组</param>
        /// <param name="cmdParametersList">SQL参数列表集合</param>
        /// <returns></returns>
        public List<int> ExecTransaction(List<string> cmdTextList, List<List<SqlParameter>> cmdParametersList)
        {
            if (cmdTextList.Count != cmdParametersList.Count)
            {
                throw new Exception("SQL语句数与参数列表数不相等");
            }

            List<int> ExecQueryList = new List<int>();
            string timePoint = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string transName = "TRANS" + timePoint;
            this.Open();
            SqlCommand cmd = this.SQLConn.CreateCommand();
            SqlTransaction trans = this.SQLConn.BeginTransaction(transName);
            cmd.Connection = this.SQLConn;
            cmd.Transaction = trans;
            try
            {
                trans.Save(transName);
                for (int i = 0; i < cmdTextList.Count; i++)
                {
                    cmd.CommandText = cmdTextList[i];
                    cmd.Parameters.AddRange(cmdParametersList[i].ToArray());
                    ExecQueryList.Add(cmd.ExecuteNonQuery());
                    cmd.Parameters.Clear();
                }
                trans.Commit();
            }
            catch
            {
                trans.Rollback(transName);
            }
            this.Close();
            return ExecQueryList;
        }
        #endregion
    }
}