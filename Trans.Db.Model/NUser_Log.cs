using System;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Web.Script.Serialization;

namespace Trans.Db.Model
{
	/// <summary>
	/// 数据实体类:NUser_Log
	/// </summary>
	[Serializable]
	public class NUser_Log : ORM.Base.ModelBase
	{
		#region 表名
		private static string _tablename = "NUser_Log";
		/// <summary>
		/// 表名
		/// </summary>
		[JsonIgnore]
		[ScriptIgnore]
		public string TableName { get { return _tablename; } }
		#endregion

		#region 实体类型
		private static string _typeName = "Trans.Db.Model.NUser_Log";
		#endregion

		#region 列名列表
		private static List<string> _cols = new List<string>() { "LOGID", "USERID", "ACTION", "CONTENT", "ISDEL", "CREATETIME" };
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
		public Int64 LogId
		{
			get{ return this._LogId;}
			set{ this._LogId = value; ModifiedColumns.Add("[LogId]"); }
		}
		private Int64 _LogId = 0L;
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
		public string Action
		{
			get{ return this._Action;}
			set{ this._Action = value; ModifiedColumns.Add("[Action]"); }
		}
		private string _Action = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string Content
		{
			get{ return this._Content;}
			set{ this._Content = value; ModifiedColumns.Add("[Content]"); }
		}
		private string _Content = "";
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
		public NUser_Log()
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
		public NUser_Log(DataRow dr) : this()
		{
			if (dr != null && dr.Table.Columns.Contains("LogId") && !(dr["LogId"] is DBNull))
			{
				this._LogId = Convert.ToInt64(dr["LogId"]);
			}
			if (dr != null && dr.Table.Columns.Contains("UserId") && !(dr["UserId"] is DBNull))
			{
				this._UserId = Convert.ToInt32(dr["UserId"]);
			}
			if (dr != null && dr.Table.Columns.Contains("Action") && !(dr["Action"] is DBNull))
			{
				this._Action = Convert.ToString(dr["Action"]);
			}
			if (dr != null && dr.Table.Columns.Contains("Content") && !(dr["Content"] is DBNull))
			{
				this._Content = Convert.ToString(dr["Content"]);
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
