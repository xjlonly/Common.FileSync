using System;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Web.Script.Serialization;

namespace Trans.Db.Model
{
	/// <summary>
	/// 数据实体类:NUser_Info
	/// </summary>
	[Serializable]
	public class NUser_Info : ORM.Base.ModelBase
	{
		#region 表名
		private static string _tablename = "NUser_Info";
		/// <summary>
		/// 表名
		/// </summary>
		[JsonIgnore]
		[ScriptIgnore]
		public string TableName { get { return _tablename; } }
		#endregion

		#region 实体类型
		private static string _typeName = "Trans.Db.Model.NUser_Info";
		#endregion

		#region 列名列表
		private static List<string> _cols = new List<string>() { "USERID", "USERNAME", "PASSWORD", "REFUSERID", "STATUS", "ISDEL", "CREATETIME" };
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
		public Int32 UserId
		{
			get{ return this._UserId;}
			set{ this._UserId = value; ModifiedColumns.Add("[UserId]"); }
		}
		private Int32 _UserId = 0;
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string UserName
		{
			get{ return this._UserName;}
			set{ this._UserName = value; ModifiedColumns.Add("[UserName]"); }
		}
		private string _UserName = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string Password
		{
			get{ return this._Password;}
			set{ this._Password = value; ModifiedColumns.Add("[Password]"); }
		}
		private string _Password = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public Int32 RefUserId
		{
			get{ return this._RefUserId;}
			set{ this._RefUserId = value; ModifiedColumns.Add("[RefUserId]"); }
		}
		private Int32 _RefUserId = 0;
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public Int32 Status
		{
			get{ return this._Status;}
			set{ this._Status = value; ModifiedColumns.Add("[Status]"); }
		}
		private Int32 _Status = 0;
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
		public NUser_Info()
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
		public NUser_Info(DataRow dr) : this()
		{
			if (dr != null && dr.Table.Columns.Contains("UserId") && !(dr["UserId"] is DBNull))
			{
				this._UserId = Convert.ToInt32(dr["UserId"]);
			}
			if (dr != null && dr.Table.Columns.Contains("UserName") && !(dr["UserName"] is DBNull))
			{
				this._UserName = Convert.ToString(dr["UserName"]);
			}
			if (dr != null && dr.Table.Columns.Contains("Password") && !(dr["Password"] is DBNull))
			{
				this._Password = Convert.ToString(dr["Password"]);
			}
			if (dr != null && dr.Table.Columns.Contains("RefUserId") && !(dr["RefUserId"] is DBNull))
			{
				this._RefUserId = Convert.ToInt32(dr["RefUserId"]);
			}
			if (dr != null && dr.Table.Columns.Contains("Status") && !(dr["Status"] is DBNull))
			{
				this._Status = Convert.ToInt32(dr["Status"]);
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
