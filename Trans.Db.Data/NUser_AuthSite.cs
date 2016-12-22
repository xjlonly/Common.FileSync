using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlTypes;
using System.Xml;
using Trans.Db.Model;
using Trans.Db.DBUtility;

namespace Trans.Db.Data
{
	/// <summary>
	/// 数据库操作:NUser_AuthSite
	/// </summary>
	public class NUser_AuthSite
	{
		/// <summary>
		/// 默认查询是否用只读串
		/// </summary>
		public static bool ReadOnlyDataSource = true;
		#region 查询
		#region 查询：返回数据表
		/// <summary>
		/// 查询：返回数据表
		/// </summary>
		/// <param name="sqlWhere">查询条件，可带参数，如username='123' and usertype=@usertype</param>
		/// <param name="sqlSort">排序，如 date desc,id asc</param>
		/// <param name="sqlCols">需要查询的字段列表,如:ID,Name,CreateTime,默认*或空标识全部字段</param>
		/// <param name="ParamsList">参数值列表，需与sqlWhere中顺序对应，相同参数只提供一次，如 {"123",1}</param>
		/// <param name="ReadOnlyDataSource">是否使用只读数据源</param>
		/// <returns></returns>
		public static DataTable GetTable(string sqlWhere = "", string sqlSort = "", string sqlCols = "*", object[] ParamsList = null, bool ReadOnlyDataSource = true)
		{
			DataTable table = new DataTable();
			string txtCols = sqlCols;
			if (string.IsNullOrEmpty(sqlCols))
			{
				txtCols = "*";
			}
			StringBuilder sql = new StringBuilder("SELECT " + sqlCols + " FROM [NUser_AuthSite](NOLOCK)");
			if (!string.IsNullOrEmpty(sqlWhere))
			{
				sql.Append(" WHERE ").Append(sqlWhere);
			}
			if (!string.IsNullOrEmpty(sqlSort))
			{
				sql.Append(" ORDER BY ").Append(sqlSort);
			}
			SqlCommand cmd = BuildCommand(sql.ToString(), ParamsList);
			table = new Trans.Db.DBUtility.DBHelper(ReadOnlyDataSource).ExecDataTable(cmd);
			if (table == null)
			{
				table = new DataTable();
			}
			table.TableName = "table";
			return table;
		}
		#endregion

		#region 查询：返回对象列表
		/// <summary>
		/// 查询：返回对象列表
		/// </summary>
		/// <param name="sqlWhere">查询条件，如username='123' and usertype=@usertype或distinct userid,username</param>
		/// <param name="sqlSort">排序，如 date desc,id asc</param>
		/// <param name="sqlCols">需要查询的字段列表,如:ID,Name,CreateTime,默认*或空标识全部字段</param>
		/// <param name="ParamsList">参数值列表，需与sqlWhere中顺序对应，相同参数只提供一次，如 {"123",1}</param>
		/// <param name="ReadOnlyDataSource">是否使用只读数据源</param>
		/// <returns></returns>
		public static List<T> GetList<T>(string sqlWhere = "", string sqlSort = "", string sqlCols = "*", object[] ParamsList = null, bool ReadOnlyDataSource = true)
		{
			DataTable table = GetTable(sqlWhere, sqlSort, sqlCols, ParamsList, ReadOnlyDataSource);
			List<T> rtnObjList = new List<T>();
			if (table != null && table.Rows.Count > 0)
			{
				foreach (DataRow dr in table.Rows)
				{
					rtnObjList.Add((T)System.Activator.CreateInstance(typeof(T), dr));
				}
			}
			return rtnObjList;
		}
		#endregion

		#region 查询：返回对象列表
		/// <summary>
		/// 查询：返回对象列表
		/// </summary>
		/// <param name="sqlWhere">查询条件，如username='123' and usertype=@usertype</param>
		/// <param name="sqlSort">排序，如 date desc,id asc</param>
		/// <param name="ParamsList">参数值列表，需与sqlWhere中顺序对应，相同参数只提供一次，如 {"123",1}</param>
		/// <param name="ReadOnlyDataSource">是否使用只读数据源</param>
		/// <returns></returns>
		public static List<Trans.Db.Model.NUser_AuthSite> GetList(string sqlWhere = "", string sqlSort = "", object[] ParamsList = null, bool ReadOnlyDataSource = true)
		{
			DataTable table = GetTable(sqlWhere, sqlSort, "*", ParamsList, ReadOnlyDataSource);
			List<Trans.Db.Model.NUser_AuthSite> rtnObjList = new List<Trans.Db.Model.NUser_AuthSite>();
			if (table != null && table.Rows.Count > 0)
			{
				foreach (DataRow dr in table.Rows)
				{
					rtnObjList.Add(new Trans.Db.Model.NUser_AuthSite(dr));
				}
			}
			return rtnObjList;
		}
		#endregion

		#region 分页查询
		/// <summary>
		/// 分页查询
		/// </summary>
		/// <param name="sqlWhere">查询条件，如username='123' and password='123'</param>
		/// <param name="sqlSort">排序条件,如username desc</param>
		/// <param name="sqlCols">数据库字段名组,用逗号分割,例如:username,userpwd,userid</param>
		/// <param name="pageIndex">页码</param>
		/// <param name="pageSize">页大小</param>
		/// <param name="recordCount">记录行数</param>
		/// <param name="ParamsList">参数值列表，需与sqlWhere中顺序对应，相同参数只提供一次，如 {"123",1}</param>
		/// <param name="ReadOnlyDataSource">是否使用只读数据源</param>
		/// <returns></returns>
		public static List<Trans.Db.Model.NUser_AuthSite> GetList(string sqlWhere, string sqlSort, string sqlCols, int pageIndex, int pageSize, out int recordCount, object[] ParamsList = null, bool ReadOnlyDataSource = true)
		{
			long LRecordCount = 0;
			DataTable table = GetTable(sqlWhere, sqlSort, sqlCols, pageIndex, pageSize, out LRecordCount, ParamsList, ReadOnlyDataSource);
			recordCount = Convert.ToInt32(LRecordCount);
			List<Trans.Db.Model.NUser_AuthSite> rtnObjList = new List<Trans.Db.Model.NUser_AuthSite>();
			if (table != null && table.Rows.Count > 0)
			{
				foreach (DataRow dr in table.Rows)
				{
					rtnObjList.Add(new Trans.Db.Model.NUser_AuthSite(dr));
				}
			}
			return rtnObjList;
		}
		#endregion

		#region 分页查询
		/// <summary>
		/// 分页查询
		/// </summary>
		/// <param name="sqlWhere">查询条件，如username='123' and password='123'</param>
		/// <param name="sqlSort">排序条件,如username desc</param>
		/// <param name="sqlCols">数据库字段名组,用逗号分割,例如:username,userpwd,userid</param>
		/// <param name="pageIndex">页码</param>
		/// <param name="pageSize">页大小</param>
		/// <param name="recordCount">记录行数</param>
		/// <param name="ParamsList">参数值列表，需与sqlWhere中顺序对应，相同参数只提供一次，如 {"123",1}</param>
		/// <param name="ReadOnlyDataSource">是否使用只读数据源</param>
		/// <returns></returns>
		public static List<Trans.Db.Model.NUser_AuthSite> GetList(string sqlWhere, string sqlSort, string sqlCols, int pageIndex, int pageSize, out long recordCount, object[] ParamsList = null, bool ReadOnlyDataSource = true)
		{
			long LRecordCount = 0;
			DataTable table = GetTable(sqlWhere, sqlSort, sqlCols, pageIndex, pageSize, out LRecordCount, ParamsList, ReadOnlyDataSource);
			recordCount = Convert.ToInt64(LRecordCount);
			List<Trans.Db.Model.NUser_AuthSite> rtnObjList = new List<Trans.Db.Model.NUser_AuthSite>();
			if (table != null && table.Rows.Count > 0)
			{
				foreach (DataRow dr in table.Rows)
				{
					rtnObjList.Add(new Trans.Db.Model.NUser_AuthSite(dr));
				}
			}
			return rtnObjList;
		}
		#endregion

		#region 根据主键:ID 查询
		/// <summary>
		/// 根据主键:ID 查询
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="ReadOnlyDataSource">是否使用只读数据源</param>
		/// <returns></returns>
		public static DataTable GetTableByID(Int32 _ID, bool ReadOnlyDataSource = true)
		{
			DataTable table = new DataTable();
			string sql = "SELECT * FROM NUser_AuthSite(NOLOCK) WHERE [ID]=@ID";
			SqlCommand cmd = new SqlCommand();
			cmd.CommandText = sql;
			cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = _ID;
			table = new Trans.Db.DBUtility.DBHelper(ReadOnlyDataSource).ExecDataTable(cmd);
			if (table == null)
			{
				table = new DataTable();
			}
			table.TableName = "table";
			return table;
		}
		#endregion

		#region 根据主键:ID 查询，返回对象列表
		/// <summary>
		/// 根据主键:ID 查询，返回对象列表
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="ReadOnlyDataSource">是否使用只读数据源</param>
		/// <returns></returns>
		public static List<Trans.Db.Model.NUser_AuthSite> GetListByID(Int32 _ID, bool ReadOnlyDataSource = true)
		{
			DataTable table = GetTableByID(_ID, ReadOnlyDataSource);
			List<Trans.Db.Model.NUser_AuthSite> objList = new List<Trans.Db.Model.NUser_AuthSite>();
			if (table != null && table.Rows.Count > 0)
			{
				foreach (DataRow dr in table.Rows)
				{
					objList.Add(new Trans.Db.Model.NUser_AuthSite(dr));
				}
			}
			return objList;
		}
		#endregion

		#region 查询某字段数据第一行第一列数据
		/// <summary>
		/// 条件查询,查询某字段数据第一行第一列数据
		/// </summary>
		/// <param name="sqlCol">数据库字段名,也可以为COUNT(),TOP 1 列名</param>
		/// <param name="sqlWhere">查询条件，如username='123' and password='123' and usertype=@usertype</param>
		/// <param name="sqlSort">排序，如 date desc,id asc</param>
		/// <param name="ParamsList">参数值列表，需与sqlWhere中顺序对应，相同参数只提供一次，如 {"123",1}</param>
		/// <param name="ReadOnlyDataSource">是否使用只读数据源</param>
		/// <returns></returns>
		public static object GetSingle(string sqlCol, string sqlWhere = "", string sqlSort = "", object[] ParamsList = null, bool ReadOnlyDataSource = true)
		{
			StringBuilder sql = new StringBuilder("SELECT TOP 1 " + sqlCol + " FROM NUser_AuthSite(NOLOCK)");
			if (!string.IsNullOrEmpty(sqlWhere))
			{
				sql.Append(" WHERE ").Append(sqlWhere);
			}
			if (!string.IsNullOrEmpty(sqlSort))
			{
				sql.Append(" ORDER BY ").Append(sqlSort);
			}
			SqlCommand cmd = BuildCommand(sql.ToString(), ParamsList);
			return new Trans.Db.DBUtility.DBHelper(ReadOnlyDataSource).ExecScalar(cmd);
		}
		#endregion

		#region 条件查询,查询记录总数
		/// <summary>
		/// 查询记录总数
		/// </summary>
		/// <param name="sqlWhere">查询条件，如username='123' and password='123',并可带排序条件</param>
		/// <param name="ParamsList">参数值列表，需与sqlWhere中顺序对应，相同参数只提供一次，如 {"123",1}</param>
		/// <param name="ReadOnlyDataSource">是否使用只读数据源</param>
		/// <returns></returns>
		public static long CountOutLong(string sqlWhere, object[] ParamsList = null, bool ReadOnlyDataSource = true)
		{
			StringBuilder sql = new StringBuilder("SELECT COUNT(1) FROM NUser_AuthSite(NOLOCK) WHERE 1=1");
			if (!string.IsNullOrEmpty(sqlWhere))
			{
				sql.Append(" AND ").Append(sqlWhere);
			}
			SqlCommand cmd = BuildCommand(sql.ToString(), ParamsList);
			object CountObj = new Trans.Db.DBUtility.DBHelper(ReadOnlyDataSource).ExecScalar(cmd);
			long RtnCount = 0;
			if (CountObj != null && CountObj != DBNull.Value && !string.IsNullOrEmpty(CountObj.ToString()))
			{
				long.TryParse(CountObj.ToString(), out RtnCount);
			}
			return RtnCount;
		}
		#endregion

		#region 条件查询,查询记录总数
		/// <summary>
		/// 条件查询,查询记录总数
		/// </summary>
		/// <param name="sqlWhere">查询条件，如username='123' and password='123',并可带排序条件</param>
		/// <param name="ParamsList">参数值列表，需与sqlWhere中顺序对应，相同参数只提供一次，如 {"123",1}</param>
		/// <param name="ReadOnlyDataSource">是否使用只读数据源</param>
		/// <returns></returns>
		public static int Count(string sqlWhere, object[] ParamsList = null, bool ReadOnlyDataSource = true)
		{
			long rtnInt= CountOutLong(sqlWhere, ParamsList, ReadOnlyDataSource);
			return Convert.ToInt32(rtnInt);
		}
		#endregion

		#region 分页查询
		/// <summary>
		/// 分页查询
		/// </summary>
		/// <param name="sqlWhere">查询条件，如username='123' and password='123'</param>
		/// <param name="sqlSort">排序条件,如username desc</param>
		/// <param name="sqlCols">数据库字段名组,用逗号分割,例如:username,userpwd,userid</param>
		/// <param name="pageIndex">页码</param>
		/// <param name="pageSize">页大小</param>
		/// <param name="recordCount">记录行数</param>
		/// <param name="ParamsList">参数值列表，需与sqlWhere中顺序对应，相同参数只提供一次，如 {"123",1}</param>
		/// <param name="ReadOnlyDataSource">是否使用只读数据源</param>
		/// <returns></returns>
		public static DataTable GetTable(string sqlWhere, string sqlSort, string sqlCols, int pageIndex, int pageSize, out int recordCount, object[] ParamsList = null, bool ReadOnlyDataSource = true)
		{
			long LRecordCount = 0;
			DataTable table = GetTable(sqlWhere, sqlSort, sqlCols, pageIndex, pageSize, out LRecordCount, ParamsList, ReadOnlyDataSource);
			recordCount = Convert.ToInt32(LRecordCount);
			return table;
		}
		#endregion

		#region 分页查询
		/// <summary>
		/// 分页查询
		/// </summary>
		/// <param name="sqlWhere">查询条件，如username='123' and password='123'</param>
		/// <param name="sqlSort">排序条件,如username desc</param>
		/// <param name="sqlCols">数据库字段名组,用逗号分割,例如:username,userpwd,userid</param>
		/// <param name="pageIndex">页码</param>
		/// <param name="pageSize">页大小</param>
		/// <param name="recordCount">记录行数</param>
		/// <param name="ParamsList">参数值列表，需与sqlWhere中顺序对应，相同参数只提供一次，如 {"123",1}</param>
		/// <param name="ReadOnlyDataSource">是否使用只读数据源</param>
		/// <returns></returns>
		public static DataTable GetTable(string sqlWhere, string sqlSort, string sqlCols, int pageIndex, int pageSize, out long recordCount, object[] ParamsList = null, bool ReadOnlyDataSource = true)
		{
			int SI = pageIndex * pageSize - pageSize + 1;
			int EI = pageIndex * pageSize;
			DataSet ds = new DataSet();
			if (string.IsNullOrEmpty(sqlCols) || sqlCols == "*")
			{
				sqlCols = "ID,UserID,SiteName,AllowList,AllowSync,AllowRoll,IsDel,CreateTime";
			}
			StringBuilder sql = new StringBuilder("WITH PST(RN," + sqlCols + ") AS");
			sql.Append("(");
			sql.Append("SELECT ROW_NUMBER() OVER(ORDER BY ").Append(sqlSort).Append(") RN,").Append(sqlCols).Append(" ");
			sql.Append("FROM NUser_AuthSite(NOLOCK)");
			if (!string.IsNullOrEmpty(sqlWhere))
			{
				sql.Append(" WHERE ").Append(sqlWhere);
			}
			sql.Append(")");
			sql.Append("SELECT RN,").Append(sqlCols).Append(" FROM PST WHERE RN BETWEEN @SI AND @EI");
			object[] NewParamsList;
			if (ParamsList != null)
			{
				NewParamsList = new object[ParamsList.Length + 2];
				for (int i = 0;i < ParamsList.Length; i++)
				{
					NewParamsList[i] = ParamsList[i];
				}
				NewParamsList[ParamsList.Length] = SI;
				NewParamsList[ParamsList.Length + 1] = EI;
			}
			else
			{
				NewParamsList = new object[2];
				NewParamsList[0] = SI;
				NewParamsList[1] = EI;
			}
			if (!string.IsNullOrEmpty(sqlWhere))
			{
				sql.Append(";SELECT COUNT(1) FROM NUser_AuthSite(NOLOCK) WHERE ").Append(sqlWhere);
			}
			else
			{
				sql.Append(";SELECT COUNT(1) FROM NUser_AuthSite(NOLOCK)");
			}
			SqlCommand cmd = BuildCommand(sql.ToString(), NewParamsList);
			ds = new Trans.Db.DBUtility.DBHelper(ReadOnlyDataSource).ExecDataSet(cmd);
			if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count >0)
			{
				recordCount = Convert.ToInt64(ds.Tables[1].Rows[0][0]);
			}
			else
			{
				recordCount = 0;
			}
			if (ds != null && ds.Tables.Count >1)
			{
				return ds.Tables[0];
			}
			else
			{
				return new DataTable();
			}
		}
		#endregion

		#region 查询并构建对象,返回第一个对象
		/// <summary>
		/// 查询并构建对象,返回第一个对象
		/// </summary>
		/// <param name="sqlWhere">查询条件</param>
		/// <param name="sqlSort">排序条件</param>
		/// <param name="ParamsList">参数值列表，需与sqlWhere中顺序对应，相同参数只提供一次，如 {"123",1}</param>
		/// <param name="ReadOnlyDataSource">是否使用只读数据源</param>
		/// <returns></returns>
		public static Trans.Db.Model.NUser_AuthSite Get(string sqlWhere, string sqlSort = "", object[] ParamsList = null, bool ReadOnlyDataSource = true)
		{
			StringBuilder sql = new StringBuilder("SELECT TOP 1 * FROM NUser_AuthSite(NOLOCK)");
			if (!string.IsNullOrEmpty(sqlWhere))
			{
				sql.Append(" WHERE ").Append(sqlWhere);
			}
			if (!string.IsNullOrEmpty(sqlSort))
			{
				sql.Append(" ORDER BY ").Append(sqlSort);
			}
			SqlCommand cmd = BuildCommand(sql.ToString(), ParamsList);
			DataTable DT = new Trans.Db.DBUtility.DBHelper(ReadOnlyDataSource).ExecDataTable(cmd);
			if(DT != null && DT.Rows.Count > 0)
			{
				return new Trans.Db.Model.NUser_AuthSite(DT.Rows[0]);
			}
			else
			{
				return new Trans.Db.Model.NUser_AuthSite();
			}
		}
		#endregion
		#endregion

		#region 删除
		/// <summary>
		/// 删除数据，数据删除，Delete
		/// </summary>
		/// <param name="sqlWhere">查询条件，如username='123' and password='123'</param>
		/// <returns></returns>
		public static int DBDelete(string sqlWhere, object[] ParamsList = null, bool IsRowLock = true)
		{
			string deleteSql = string.Empty;
			if (IsRowLock)
			{
				deleteSql = "DELETE FROM NUser_AuthSite WITH(ROWLOCK) WHERE ";
			}
			else
			{
				deleteSql = "DELETE FROM NUser_AuthSite WHERE ";
			}
			StringBuilder sql = new StringBuilder(deleteSql);
			sql.Append(sqlWhere);
			SqlCommand cmd = BuildCommand(sql.ToString(), ParamsList);
			return new Trans.Db.DBUtility.DBHelper(false).ExecNonQuery(cmd);
		}

		/// <summary>
		/// 删除数据，逻辑删除，IsDel=1
		/// </summary>
		/// <param name="sqlWhere">查询条件，如username='123' and password='123'</param>
		/// <returns></returns>
		public static int Delete(string sqlWhere, object[] ParamsList = null)
		{
			StringBuilder sql = new StringBuilder("UPDATE NUser_AuthSite SET IsDel=1 WHERE ");
			sql.Append(sqlWhere);
			SqlCommand cmd = BuildCommand(sql.ToString(), ParamsList);
			return new Trans.Db.DBUtility.DBHelper(false).ExecNonQuery(cmd);
		}
		#endregion

		#region 更新
		/// <summary>
		/// 更新数据
		/// </summary>
		/// <param name="sqlWhere">查询条件，如username='123' and password='123'</param>
		/// <param name="sqlSet">数据设置，如username='123',password='123'</param>
		/// <returns></returns>
		public static int Update(string sqlWhere, string sqlSet, object[] ParamsList = null)
		{
			StringBuilder sql = new StringBuilder("UPDATE NUser_AuthSite SET ");
			sql.Append(sqlSet).Append(" WHERE ").Append(sqlWhere);
			SqlCommand cmd = BuildCommand(sqlWhere + " " + sqlSet, ParamsList);
			cmd.CommandText = sql.ToString();
			return new Trans.Db.DBUtility.DBHelper(false).ExecNonQuery(cmd);
		}

		/// <summary>
		/// 更新数据，更新指定XML列
		/// </summary>
		/// <param name="sqlWhere">条件</param>
		/// <param name="whereParamsList">条件参数</param>
		/// <param name="xmlCol">XML类型列</param>
		/// <param name="XmlContent">XML内容</param>
		/// <returns></returns>
		public static int UpdateXml(string sqlWhere, object[] whereParamsList, string xmlCol, string XmlContent)
		{
			StringBuilder sql = new StringBuilder("UPDATE Product_Info SET {0} ");
			sql.Append(" WHERE ").Append(sqlWhere);
			SqlCommand cmd = BuildCommand(sql.ToString(), whereParamsList);
			if (xmlCol.Contains("["))
			{
				cmd.CommandText = string.Format(sql.ToString(), xmlCol + "=@" + xmlCol);
			}
			else
			{
				cmd.CommandText = string.Format(sql.ToString(), "[" + xmlCol + "]=@" + xmlCol);
			}
			cmd.Parameters.Add("@" + xmlCol, SqlDbType.Xml).Value = new SqlXml(new XmlTextReader(XmlContent, XmlNodeType.Document, null));
			return new Trans.Db.DBUtility.DBHelper(false).ExecNonQuery(cmd);
		}
		/// <summary>
		/// 更新数据
		/// 返回：操作影响记录数，-1表示没有需要更新的列
		/// </summary>
		/// <param name="NUser_AuthSite_obj"></param>
		/// <returns></returns>
		public static int Update(Trans.Db.Model.NUser_AuthSite NUser_AuthSite_obj)
		{
			if(NUser_AuthSite_obj.ModifiedColumns.Count <= 0)
			{
				return -1;
			}
			StringBuilder sqlSet = new StringBuilder();
			StringBuilder sqlWhere = new StringBuilder();
			string sql = "UPDATE NUser_AuthSite SET {0} WHERE {1}";
			SqlCommand cmd = new SqlCommand();
			cmd.Parameters.AddWithValue("@ID", NUser_AuthSite_obj.ID);
			cmd.Parameters["@ID"].SqlDbType = System.Data.SqlDbType.Int;
			if (sqlWhere.Length > 0)
			{
				sqlWhere.Append(" AND ");
			}
			sqlWhere.Append("[ID]=@ID");
			if(NUser_AuthSite_obj.ModifiedColumns.Contains("[UserID]"))
			{
				if (sqlSet.Length > 0)
				{
					sqlSet.Append(",");
				}
				sqlSet.Append("[UserID]=@UserID");
				cmd.Parameters.AddWithValue("@UserID", NUser_AuthSite_obj.UserID);
				cmd.Parameters["@UserID"].SqlDbType = System.Data.SqlDbType.Int;
			}
			if(NUser_AuthSite_obj.ModifiedColumns.Contains("[SiteName]"))
			{
				if (string.IsNullOrEmpty(NUser_AuthSite_obj.SiteName))
				{
					NUser_AuthSite_obj.SiteName = "";
				}
				if (sqlSet.Length > 0)
				{
					sqlSet.Append(",");
				}
				sqlSet.Append("[SiteName]=@SiteName");
				cmd.Parameters.AddWithValue("@SiteName", NUser_AuthSite_obj.SiteName);
				cmd.Parameters["@SiteName"].SqlDbType = System.Data.SqlDbType.NVarChar;
			}
			if(NUser_AuthSite_obj.ModifiedColumns.Contains("[AllowList]"))
			{
				if (sqlSet.Length > 0)
				{
					sqlSet.Append(",");
				}
				sqlSet.Append("[AllowList]=@AllowList");
				cmd.Parameters.AddWithValue("@AllowList", NUser_AuthSite_obj.AllowList);
				cmd.Parameters["@AllowList"].SqlDbType = System.Data.SqlDbType.Int;
			}
			if(NUser_AuthSite_obj.ModifiedColumns.Contains("[AllowSync]"))
			{
				if (sqlSet.Length > 0)
				{
					sqlSet.Append(",");
				}
				sqlSet.Append("[AllowSync]=@AllowSync");
				cmd.Parameters.AddWithValue("@AllowSync", NUser_AuthSite_obj.AllowSync);
				cmd.Parameters["@AllowSync"].SqlDbType = System.Data.SqlDbType.Int;
			}
			if(NUser_AuthSite_obj.ModifiedColumns.Contains("[AllowRoll]"))
			{
				if (sqlSet.Length > 0)
				{
					sqlSet.Append(",");
				}
				sqlSet.Append("[AllowRoll]=@AllowRoll");
				cmd.Parameters.AddWithValue("@AllowRoll", NUser_AuthSite_obj.AllowRoll);
				cmd.Parameters["@AllowRoll"].SqlDbType = System.Data.SqlDbType.Int;
			}
			if(NUser_AuthSite_obj.ModifiedColumns.Contains("[IsDel]"))
			{
				if (sqlSet.Length > 0)
				{
					sqlSet.Append(",");
				}
				sqlSet.Append("[IsDel]=@IsDel");
				cmd.Parameters.AddWithValue("@IsDel", NUser_AuthSite_obj.IsDel);
				cmd.Parameters["@IsDel"].SqlDbType = System.Data.SqlDbType.Int;
			}
			if (NUser_AuthSite_obj.CreateTime != null && NUser_AuthSite_obj.CreateTime > DateTime.MinValue && NUser_AuthSite_obj.ModifiedColumns.Contains("[CreateTime]"))
			{
				if (sqlSet.Length > 0)
				{
					sqlSet.Append(",");
				}
				sqlSet.Append("[CreateTime]=@CreateTime");
				cmd.Parameters.AddWithValue("@CreateTime", NUser_AuthSite_obj.CreateTime);
				cmd.Parameters["@CreateTime"].SqlDbType = System.Data.SqlDbType.DateTime2;
			}
			sql = string.Format(sql, sqlSet.ToString(), sqlWhere.ToString());
			cmd.CommandText = sql;
			try
			{
				return new Trans.Db.DBUtility.DBHelper(false).ExecNonQuery(cmd);
			}
			catch { return -1; }
		}

		/// <summary>
		/// 更新数据，行级数据锁定
		/// 返回：操作影响记录数，-1表示没有需要更新的列
		/// </summary>
		/// <param name="NUser_AuthSite_obj"></param>
		/// <param name="IsRowLock">是否锁行</param>
		/// <returns></returns>
		public static int Update(Trans.Db.Model.NUser_AuthSite NUser_AuthSite_obj, bool IsRowLock)
		{
			if(NUser_AuthSite_obj.ModifiedColumns.Count <= 0)
			{
				return -1;
			}
			StringBuilder sqlSet = new StringBuilder();
			StringBuilder sqlWhere = new StringBuilder();
			string sql = string.Empty;
			if (IsRowLock)
			{
				sql = "UPDATE NUser_AuthSite WITH(ROWLOCK) SET {0} WHERE {1}";
			}
			else
			{
				sql = "UPDATE NUser_AuthSite SET {0} WHERE {1}";
			}
			SqlCommand cmd = new SqlCommand();
			cmd.Parameters.AddWithValue("@ID", NUser_AuthSite_obj.ID);
			cmd.Parameters["@ID"].SqlDbType = System.Data.SqlDbType.Int;
			if (sqlWhere.Length > 0)
			{
				sqlWhere.Append(" AND ");
			}
			sqlWhere.Append("[ID]=@ID");
			if(NUser_AuthSite_obj.ModifiedColumns.Contains("[UserID]"))
			{
				if (sqlSet.Length > 0)
				{
					sqlSet.Append(",");
				}
				sqlSet.Append("[UserID]=@UserID");
				cmd.Parameters.AddWithValue("@UserID", NUser_AuthSite_obj.UserID);
				cmd.Parameters["@UserID"].SqlDbType = System.Data.SqlDbType.Int;
			}
			if(NUser_AuthSite_obj.ModifiedColumns.Contains("[SiteName]"))
			{
				if (string.IsNullOrEmpty(NUser_AuthSite_obj.SiteName))
				{
					NUser_AuthSite_obj.SiteName = "";
				}
				if (sqlSet.Length > 0)
				{
					sqlSet.Append(",");
				}
				sqlSet.Append("[SiteName]=@SiteName");
				cmd.Parameters.AddWithValue("@SiteName", NUser_AuthSite_obj.SiteName);
				cmd.Parameters["@SiteName"].SqlDbType = System.Data.SqlDbType.NVarChar;
			}
			if(NUser_AuthSite_obj.ModifiedColumns.Contains("[AllowList]"))
			{
				if (sqlSet.Length > 0)
				{
					sqlSet.Append(",");
				}
				sqlSet.Append("[AllowList]=@AllowList");
				cmd.Parameters.AddWithValue("@AllowList", NUser_AuthSite_obj.AllowList);
				cmd.Parameters["@AllowList"].SqlDbType = System.Data.SqlDbType.Int;
			}
			if(NUser_AuthSite_obj.ModifiedColumns.Contains("[AllowSync]"))
			{
				if (sqlSet.Length > 0)
				{
					sqlSet.Append(",");
				}
				sqlSet.Append("[AllowSync]=@AllowSync");
				cmd.Parameters.AddWithValue("@AllowSync", NUser_AuthSite_obj.AllowSync);
				cmd.Parameters["@AllowSync"].SqlDbType = System.Data.SqlDbType.Int;
			}
			if(NUser_AuthSite_obj.ModifiedColumns.Contains("[AllowRoll]"))
			{
				if (sqlSet.Length > 0)
				{
					sqlSet.Append(",");
				}
				sqlSet.Append("[AllowRoll]=@AllowRoll");
				cmd.Parameters.AddWithValue("@AllowRoll", NUser_AuthSite_obj.AllowRoll);
				cmd.Parameters["@AllowRoll"].SqlDbType = System.Data.SqlDbType.Int;
			}
			if(NUser_AuthSite_obj.ModifiedColumns.Contains("[IsDel]"))
			{
				if (sqlSet.Length > 0)
				{
					sqlSet.Append(",");
				}
				sqlSet.Append("[IsDel]=@IsDel");
				cmd.Parameters.AddWithValue("@IsDel", NUser_AuthSite_obj.IsDel);
				cmd.Parameters["@IsDel"].SqlDbType = System.Data.SqlDbType.Int;
			}
			if (NUser_AuthSite_obj.CreateTime != null && NUser_AuthSite_obj.CreateTime > DateTime.MinValue && NUser_AuthSite_obj.ModifiedColumns.Contains("[CreateTime]"))
			{
				if (sqlSet.Length > 0)
				{
					sqlSet.Append(",");
				}
				sqlSet.Append("[CreateTime]=@CreateTime");
				cmd.Parameters.AddWithValue("@CreateTime", NUser_AuthSite_obj.CreateTime);
				cmd.Parameters["@CreateTime"].SqlDbType = System.Data.SqlDbType.DateTime2;
			}
			sql = string.Format(sql, sqlSet.ToString(), sqlWhere.ToString());
			cmd.CommandText = sql;
			try
			{
				return new Trans.Db.DBUtility.DBHelper(false).ExecNonQuery(cmd);
			}
			catch { return -1; }
		}
		#endregion

		#region 插入
		/// <summary>
		/// 插入数据
		/// </summary>
		/// <returns></returns>
		public static bool insert( Trans.Db.Model.NUser_AuthSite NUser_AuthSite_obj)
		{
			SqlCommand cmd = new SqlCommand();
			StringBuilder cols = new StringBuilder();
			StringBuilder parameters = new StringBuilder();
			string sql = "INSERT INTO NUser_AuthSite({0}) values({1})";
			if (cols.Length > 0)
			{
				cols.Append(",");
				parameters.Append(",");
			}
			cols.Append("[UserID]");
			parameters.Append("@UserID");
			cmd.Parameters.AddWithValue("@UserID", NUser_AuthSite_obj.UserID);
			if (cols.Length > 0)
			{
				cols.Append(",");
				parameters.Append(",");
			}
			cols.Append("[SiteName]");
			parameters.Append("@SiteName");
			if (!string.IsNullOrEmpty(NUser_AuthSite_obj.SiteName))
			{
				cmd.Parameters.AddWithValue("@SiteName", NUser_AuthSite_obj.SiteName);
			}
			else
			{
				cmd.Parameters.AddWithValue("@SiteName", "");
			}
			if (cols.Length > 0)
			{
				cols.Append(",");
				parameters.Append(",");
			}
			cols.Append("[AllowList]");
			parameters.Append("@AllowList");
			cmd.Parameters.AddWithValue("@AllowList", NUser_AuthSite_obj.AllowList);
			if (cols.Length > 0)
			{
				cols.Append(",");
				parameters.Append(",");
			}
			cols.Append("[AllowSync]");
			parameters.Append("@AllowSync");
			cmd.Parameters.AddWithValue("@AllowSync", NUser_AuthSite_obj.AllowSync);
			if (cols.Length > 0)
			{
				cols.Append(",");
				parameters.Append(",");
			}
			cols.Append("[AllowRoll]");
			parameters.Append("@AllowRoll");
			cmd.Parameters.AddWithValue("@AllowRoll", NUser_AuthSite_obj.AllowRoll);
			if (cols.Length > 0)
			{
				cols.Append(",");
				parameters.Append(",");
			}
			cols.Append("[IsDel]");
			parameters.Append("@IsDel");
			cmd.Parameters.AddWithValue("@IsDel", NUser_AuthSite_obj.IsDel);
			if (cols.Length > 0)
			{
				cols.Append(",");
				parameters.Append(",");
			}
			cols.Append("[CreateTime]");
			parameters.Append("@CreateTime");
			if(NUser_AuthSite_obj.CreateTime == null || NUser_AuthSite_obj.CreateTime == DateTime.MinValue)
			{
				cmd.Parameters.AddWithValue("@CreateTime", DateTime.Now);
			}
			else
			{
				cmd.Parameters.AddWithValue("@CreateTime", NUser_AuthSite_obj.CreateTime);
			}
			sql = string.Format(sql, cols.ToString(), parameters.ToString());
			cmd.CommandText = sql;
			bool b = true;
			try
			{
				int QueryCount = new Trans.Db.DBUtility.DBHelper(false).ExecNonQuery(cmd);
				if (QueryCount < 1)
				{
					b = false;
				}
			}
			catch { b = false; }
			return b;
		}

		/// <summary>
		/// 插入数据,返回自增列ID
		/// </summary>
		/// <returns></returns>
		public static bool Add( Trans.Db.Model.NUser_AuthSite NUser_AuthSite_obj, out Int32 ID)
		{
			ID = 0;
			SqlCommand cmd = new SqlCommand();
			StringBuilder cols = new StringBuilder();
			StringBuilder parameters = new StringBuilder();
			string sql = "INSERT INTO NUser_AuthSite({0}) values({1});SELECT @@IDENTITY;";
			if (cols.Length > 0)
			{
				cols.Append(",");
				parameters.Append(",");
			}
			cols.Append("[UserID]");
			parameters.Append("@UserID");
			cmd.Parameters.AddWithValue("@UserID", NUser_AuthSite_obj.UserID);
			if (cols.Length > 0)
			{
				cols.Append(",");
				parameters.Append(",");
			}
			cols.Append("[SiteName]");
			parameters.Append("@SiteName");
			if (!string.IsNullOrEmpty(NUser_AuthSite_obj.SiteName))
			{
				cmd.Parameters.AddWithValue("@SiteName", NUser_AuthSite_obj.SiteName);
			}
			else
			{
				cmd.Parameters.AddWithValue("@SiteName", "");
			}
			if (cols.Length > 0)
			{
				cols.Append(",");
				parameters.Append(",");
			}
			cols.Append("[AllowList]");
			parameters.Append("@AllowList");
			cmd.Parameters.AddWithValue("@AllowList", NUser_AuthSite_obj.AllowList);
			if (cols.Length > 0)
			{
				cols.Append(",");
				parameters.Append(",");
			}
			cols.Append("[AllowSync]");
			parameters.Append("@AllowSync");
			cmd.Parameters.AddWithValue("@AllowSync", NUser_AuthSite_obj.AllowSync);
			if (cols.Length > 0)
			{
				cols.Append(",");
				parameters.Append(",");
			}
			cols.Append("[AllowRoll]");
			parameters.Append("@AllowRoll");
			cmd.Parameters.AddWithValue("@AllowRoll", NUser_AuthSite_obj.AllowRoll);
			if (cols.Length > 0)
			{
				cols.Append(",");
				parameters.Append(",");
			}
			cols.Append("[IsDel]");
			parameters.Append("@IsDel");
			cmd.Parameters.AddWithValue("@IsDel", NUser_AuthSite_obj.IsDel);
			if (cols.Length > 0)
			{
				cols.Append(",");
				parameters.Append(",");
			}
			cols.Append("[CreateTime]");
			parameters.Append("@CreateTime");
			if(NUser_AuthSite_obj.CreateTime == null || NUser_AuthSite_obj.CreateTime == DateTime.MinValue)
			{
				cmd.Parameters.AddWithValue("@CreateTime", DateTime.Now);
			}
			else
			{
				cmd.Parameters.AddWithValue("@CreateTime", NUser_AuthSite_obj.CreateTime);
			}
			sql = string.Format(sql, cols.ToString(), parameters.ToString());
			cmd.CommandText = sql;
			bool b = true;
			try
			{
				object idobj = new Trans.Db.DBUtility.DBHelper(false).ExecScalar(cmd);
				ID = Convert.ToInt32(idobj);
				if (ID == 0)
				{
					b = false;
				}
			}
			catch { b = false; }
			return b;
		}
		#endregion

		#region 查询执行器构造
		/// <summary>
		/// 查询执行器构造
		/// </summary>
		/// <param name="sql">完整SQL语句</param>
		/// <param name="ParamsList">可选参数列表</param>
		/// <returns></returns>
		private static SqlCommand BuildCommand(string sql, object[] ParamsList = null)
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandText = sql;
			if (!string.IsNullOrEmpty(sql))
			{
				List<string> ParameterList = new List<string>();
				Regex reg = new Regex("(@[0-9a-zA-Z_]{1,30})", RegexOptions.IgnoreCase);
				MatchCollection mc = reg.Matches(sql);
				if (mc != null && mc.Count > 0)
				{
					foreach (Match m in mc)
					{
						if (!ParameterList.Contains(m.Groups[1].Value))
						{
							ParameterList.Add(m.Groups[1].Value);
						}
					}
				}
				if (ParameterList.Count > 0)
				{
					int i = 0;
					foreach (string ParameterName in ParameterList)
					{
						cmd.Parameters.AddWithValue(ParameterName, ParamsList[i]);
						i++;
					}
				}
			}
			return cmd;
		}
		#endregion

	}
}
