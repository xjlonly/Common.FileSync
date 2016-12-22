using System;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Web.Script.Serialization;

namespace Trans.Db.Model
{
	/// <summary>
	/// 数据实体类:NUser_AuthDetail
	/// </summary>
	[Serializable]
	public class NUser_AuthDetail : ORM.Base.ModelBase
	{
		#region 表名
		private static string _tablename = "NUser_AuthDetail";
		/// <summary>
		/// 表名
		/// </summary>
		[JsonIgnore]
		[ScriptIgnore]
		public string TableName { get { return _tablename; } }
		#endregion

		#region 实体类型
		private static string _typeName = "Trans.Db.Model.NUser_AuthDetail";
		#endregion

		#region 列名列表
		private static List<string> _cols = new List<string>() { "ID", "USERID", "DETAILNAME", "ALLOWLIST", "ALLOWSYNC", "ALLOWROLL", "SITENAME", "ISDIR", "ISDEL", "CREATETIME" };
		/// <summary>
		/// 列名列表
		/// </summary>
		[JsonIgnore]
		[ScriptIgnore]
		public List<string> ColList { get { return _cols; } }
		#endregion

		#region 字段、属性
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public Int32 ID
		{
			get{ return this._ID;}
			set{ this._ID = value; ModifiedColumns.Add("[ID]"); }
		}
		private Int32 _ID = 0;
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public Int32 UserID
		{
			get{ return this._UserID;}
			set{ this._UserID = value; ModifiedColumns.Add("[UserID]"); }
		}
		private Int32 _UserID = 0;
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string DetailName
		{
			get{ return this._DetailName;}
			set{ this._DetailName = value; ModifiedColumns.Add("[DetailName]"); }
		}
		private string _DetailName = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public Int32 AllowList
		{
			get{ return this._AllowList;}
			set{ this._AllowList = value; ModifiedColumns.Add("[AllowList]"); }
		}
		private Int32 _AllowList = 0;
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public Int32 AllowSync
		{
			get{ return this._AllowSync;}
			set{ this._AllowSync = value; ModifiedColumns.Add("[AllowSync]"); }
		}
		private Int32 _AllowSync = 0;
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public Int32 AllowRoll
		{
			get{ return this._AllowRoll;}
			set{ this._AllowRoll = value; ModifiedColumns.Add("[AllowRoll]"); }
		}
		private Int32 _AllowRoll = 0;
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string SiteName
		{
			get{ return this._SiteName;}
			set{ this._SiteName = value; ModifiedColumns.Add("[SiteName]"); }
		}
		private string _SiteName = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public Int32 IsDir
		{
			get{ return this._IsDir;}
			set{ this._IsDir = value; ModifiedColumns.Add("[IsDir]"); }
		}
		private Int32 _IsDir = 0;
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		[JsonIgnore]
		[ScriptIgnore]
		public Int32 IsDel
		{
			get{ return this._IsDel;}
			set{ this._IsDel = value; ModifiedColumns.Add("[IsDel]"); }
		}
		private Int32 _IsDel = 0;
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public DateTime CreateTime
		{
			get{ return this._CreateTime;}
			set{ this._CreateTime = value; ModifiedColumns.Add("[CreateTime]"); }
		}
		private DateTime _CreateTime = DateTime.Now;
		#endregion

		#region 构造函数
		/// <summary>
		/// 构造函数
		/// </summary>
		public NUser_AuthDetail()
		{
			_ConnectionMark = "TRANS";
			_tabinfo = TableInfo;
			base.Init();
		}
		[ScriptIgnore]
		public static ORM.Base.TabInfo TableInfo = new ORM.Base.TabInfo()
		{
			TableName = _tablename,
			ColList = _cols,
			TypeName = _typeName,
			AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().Location
		};
		/// <summary>
		/// 构造函数,初始化一行数据
		/// </summary>
		public NUser_AuthDetail(DataRow dr) : this()
		{
			if (dr != null && dr.Table.Columns.Contains("ID") && !(dr["ID"] is DBNull))
			{
				this._ID = Convert.ToInt32(dr["ID"]);
			}
			if (dr != null && dr.Table.Columns.Contains("UserID") && !(dr["UserID"] is DBNull))
			{
				this._UserID = Convert.ToInt32(dr["UserID"]);
			}
			if (dr != null && dr.Table.Columns.Contains("DetailName") && !(dr["DetailName"] is DBNull))
			{
				this._DetailName = Convert.ToString(dr["DetailName"]);
			}
			if (dr != null && dr.Table.Columns.Contains("AllowList") && !(dr["AllowList"] is DBNull))
			{
				this._AllowList = Convert.ToInt32(dr["AllowList"]);
			}
			if (dr != null && dr.Table.Columns.Contains("AllowSync") && !(dr["AllowSync"] is DBNull))
			{
				this._AllowSync = Convert.ToInt32(dr["AllowSync"]);
			}
			if (dr != null && dr.Table.Columns.Contains("AllowRoll") && !(dr["AllowRoll"] is DBNull))
			{
				this._AllowRoll = Convert.ToInt32(dr["AllowRoll"]);
			}
			if (dr != null && dr.Table.Columns.Contains("SiteName") && !(dr["SiteName"] is DBNull))
			{
				this._SiteName = Convert.ToString(dr["SiteName"]);
			}
			if (dr != null && dr.Table.Columns.Contains("IsDir") && !(dr["IsDir"] is DBNull))
			{
				this._IsDir = Convert.ToInt32(dr["IsDir"]);
			}
			if (dr != null && dr.Table.Columns.Contains("IsDel") && !(dr["IsDel"] is DBNull))
			{
				this._IsDel = Convert.ToInt32(dr["IsDel"]);
			}
			if (dr != null && dr.Table.Columns.Contains("CreateTime") && !(dr["CreateTime"] is DBNull))
			{
				this._CreateTime = Convert.ToDateTime(dr["CreateTime"]);
			}
		}
		#endregion
		#region 被修改字段序列
		[ScriptIgnore]
		public HashSet<string> ModifiedColumns = new HashSet<string>();
		#endregion

	}
}
