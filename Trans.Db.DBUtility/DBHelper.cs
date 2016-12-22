using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace Trans.Db.DBUtility
{
    /// <summary>
    /// 数据通用访问类
    /// </summary>
    public class DBHelper
    {
        private bool isReadonly = false;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public DBHelper()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="IsReadOnly">是否连接只读数据库</param>
        public DBHelper(bool IsReadOnly)
        {
            this.isReadonly = IsReadOnly;
        }
        #endregion

        #region 执行SQL语句,返回第一行第一列数据
        /// <summary>
        /// 执行SQL语句,返回第一行第一列数据
        /// </summary>
        /// <param name="SQLText">SQL语句</param>
        /// <returns></returns>
        public object ExecTextScalar(string SQLText)
        {
            return new SQLHelper(this.isReadonly).ExecScalar(SQLText);
        }
        /// <summary>
        /// 执行SQL语句,返回第一行第一列数据
        /// </summary>
        /// <param name="SQLText">SQL语句</param>
        /// <param name="cmdParams">参数数组</param>
        /// <returns></returns>
        public object ExecTextScalar(string SQLText, params object[] cmdParams)
        {
            return new SQLHelper(this.isReadonly).ExecScalar(SQLText, (SqlParameter[])cmdParams);
        }
        #endregion

        #region 执行存储过程,返回第一行第一列数据
        /// <summary>
        /// 执行存储过程,返回第一行第一列数据
        /// </summary>
        /// <param name="ProcName">存储过程</param>
        /// <returns></returns>
        public object ExecProcScalar(string ProcName)
        {
            return new SQLHelper(this.isReadonly).ExecScalar_Proc(ProcName);
        }
        /// <summary>
        /// 执行存储过程,返回第一行第一列数据
        /// </summary>
        /// <param name="ProcName">存储过程</param>
        /// <param name="cmdParams">参数数组</param>
        /// <returns></returns>
        public object ExecProcScalar(string ProcName, params object[] cmdParams)
        {
            return new SQLHelper(this.isReadonly).ExecScalar_Proc(ProcName, (SqlParameter[])cmdParams);
        }
        #endregion

        #region 执行SQL语句,返回受影响行数
        /// <summary>
        /// 执行SQL语句,返回受影响行数
        /// </summary>
        /// <param name="SQLText">SQL语句</param>
        /// <returns></returns>
        public int ExecTextNonQuery(string SQLText)
        {
            return new SQLHelper(this.isReadonly).ExecNonquery(SQLText);
        }
        /// <summary>
        /// 执行SQL语句,返回受影响行数
        /// </summary>
        /// <param name="SQLText">SQL语句</param>
        /// <param name="cmdParams">参数数组</param>
        /// <returns></returns>
        public int ExecTextNonQuery(string SQLText, params object[] cmdParams)
        {
            return new SQLHelper(this.isReadonly).ExecNonquery(SQLText, (SqlParameter[])cmdParams);
        }
        #endregion

        #region 执行存储过程,返回受影响行数
        /// <summary>
        /// 执行存储过程,返回受影响行数
        /// </summary>
        /// <param name="ProcName">存储过程</param>
        /// <returns></returns>
        public int ExecProcNonQuery(string ProcName)
        {
            return new SQLHelper(this.isReadonly).ExecNonquery_Proc(ProcName);
        }
        /// <summary>
        /// 执行存储过程,返回受影响行数
        /// </summary>
        /// <param name="ProcName">存储过程</param>
        /// <param name="cmdParams">参数数组</param>
        /// <returns></returns>
        public int ExecProcNonQuery(string ProcName, params object[] cmdParams)
        {
            return new SQLHelper(this.isReadonly).ExecNonquery_Proc(ProcName, (SqlParameter[])cmdParams);
        }
        #endregion

        #region 执行SQL语句,返回DataSet
        /// <summary>
        /// 执行SQL语句,返回DataSet
        /// </summary>
        /// <param name="SQLText">SQL语句</param>
        /// <returns></returns>
        public DataSet ExecTextDataSet(string SQLText)
        {
            return new SQLHelper(this.isReadonly).ExecDataSet(SQLText);
        }
        /// <summary>
        /// 执行SQL语句,返回DataSet
        /// </summary>
        /// <param name="SQLText">SQL语句</param>
        /// <param name="cmdParams">参数数组</param>
        /// <returns></returns>
        public DataSet ExecTextDataSet(string SQLText, params object[] cmdParams)
        {
            return new SQLHelper(this.isReadonly).ExecDataSet(SQLText, (SqlParameter[])cmdParams);
        }
        #endregion

        #region 执行存储过程,返回DataSet
        /// <summary>
        /// 执行存储过程,返回DataSet
        /// </summary>
        /// <param name="ProcName">存储过程</param>
        /// <returns></returns>
        public DataSet ExecProcDataSet(string ProcName)
        {
            return new SQLHelper(this.isReadonly).ExecDataSet_Proc(ProcName);
        }
        /// <summary>
        /// 执行存储过程,返回DataSet
        /// </summary>
        /// <param name="ProcName">存储过程</param>
        /// <param name="cmdParams">参数数组</param>
        /// <returns></returns>
        public DataSet ExecProcDataSet(string ProcName, params object[] cmdParams)
        {
            return new SQLHelper(this.isReadonly).ExecDataSet_Proc(ProcName, (SqlParameter[])cmdParams);
        }
        #endregion

        #region 执行SQL语句,返回DataTable
        /// <summary>
        /// 执行SQL语句,返回DataTable
        /// </summary>
        /// <param name="SQLText">SQL语句</param>
        /// <returns></returns>
        public DataTable ExecTextDataTable(string SQLText)
        {
            return new SQLHelper(this.isReadonly).ExecDataTable(SQLText);
        }
        /// <summary>
        /// 执行SQL语句,返回DataTable
        /// </summary>
        /// <param name="SQLText">SQL语句</param>
        /// <param name="cmdParams">参数数组</param>
        /// <returns></returns>
        public DataTable ExecTextDataTable(string SQLText, params object[] cmdParams)
        {
            return new SQLHelper(this.isReadonly).ExecDataTable(SQLText, (SqlParameter[])cmdParams);
        }
        #endregion

        #region 执行存储过程,返回DataTable
        /// <summary>
        /// 执行存储过程,返回DataTable
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <returns></returns>
        public DataTable ExecProcDataTable(string ProcName)
        {
            return new SQLHelper(this.isReadonly).ExecDataTable_Proc(ProcName);
        }
        /// <summary>
        /// 执行存储过程,返回DataTable
        /// </summary>
        /// <param name="ProcName">存储过程</param>
        /// <param name="cmdParams">参数数组</param>
        /// <returns></returns>
        public DataTable ExecProcDataTable(string ProcName, params object[] cmdParams)
        {
            return new SQLHelper(this.isReadonly).ExecDataTable_Proc(ProcName, (SqlParameter[])cmdParams);
        }
        #endregion

        #region 执行SQL命令,返回第一行第一列数据
        /// <summary>
        /// 执行SQL命令,返回第一行第一列数据
        /// </summary>
        /// <param name="cmd">SQL命令</param>
        /// <returns></returns>
        public object ExecScalar(DbCommand cmd)
        {
            return new SQLHelper(this.isReadonly).ExecScalar((SqlCommand)cmd);
        }
        public object ExecScalar(string cmdText)
        {
            return new SQLHelper(this.isReadonly).ExecScalar(cmdText);
        }
        #endregion

        #region 执行SQL命令,返回受影响行数
        /// <summary>
        /// 执行SQL命令,返回受影响行数
        /// </summary>
        /// <param name="cmd">SQL命令</param>
        /// <returns></returns>
        public int ExecNonQuery(DbCommand cmd)
        {
            return new SQLHelper(this.isReadonly).ExecNonquery((SqlCommand)cmd);
        }
        #endregion

        #region 执行SQL命令,返回DataTable
        /// <summary>
        /// 执行SQL命令,返回DataTable
        /// </summary>
        /// <param name="cmd">SQL命令</param>
        /// <returns></returns>
        public DataTable ExecDataTable(DbCommand cmd)
        {
            return new SQLHelper(this.isReadonly).ExecDataTable((SqlCommand)cmd);
        }
        #endregion

        #region 执行SQL命令,返回DataSet
        /// <summary>
        /// 执行SQL命令,返回DataSet
        /// </summary>
        /// <param name="cmd">SQL命令</param>
        /// <returns></returns>
        public DataSet ExecDataSet(DbCommand cmd)
        {
            return new SQLHelper(this.isReadonly).ExecDataSet((SqlCommand)cmd);
        }
        #endregion

        #region 执行SQL命令,返回DataSet
        /// <summary>
        /// 执行SQL命令,返回SqlDataReader
        /// </summary>
        /// <param name="cmd">SQL命令</param>
        /// <returns></returns>
        public SqlDataReader ExecDataReader(string cmdText)
        {
            return new SQLHelper(this.isReadonly).ExecuteReader(cmdText);
        }
        #endregion

        #region 事务执行多个SQL语句
        /// <summary>
        /// 事务提交多个数据库操作,返回每一次执行影响的行数列表
        /// </summary>
        /// <param name="cmdTextList">SQL语句组</param>
        /// <returns></returns>
        public List<int> ExecTransaction(List<string> cmdTextList)
        {
            List<int> ExecQueryList = new List<int>();
            ExecQueryList = new SQLHelper(this.isReadonly).ExecTransaction(cmdTextList);
            return ExecQueryList;
        }
        /// <summary>
        /// 事务提交多个数据库操作,返回每一次执行影响的行数列表
        /// </summary>
        /// <param name="cmdTextList">SQL语句组</param>
        /// <returns></returns>
        public List<int> ExecTransaction(List<string> cmdTextList, List<List<SqlParameter>> cmdParametersList)
        {
            return new SQLHelper(this.isReadonly).ExecTransaction(cmdTextList, cmdParametersList);
        }
        #endregion
    }
}