using System;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Web.Script.Serialization;

namespace Trans.Db.Model
{
	/// <summary>
	/// 数据实体类:NBlock_Error
	/// </summary>
	[Serializable]
	public class NBlock_Error : ORM.Base.ModelBase
	{
		#region 表名
		private static string _tablename = "NBlock_Error";
		/// <summary>
		/// 表名
		/// </summary>
		[JsonIgnore]
		[ScriptIgnore]
		public string TableName { get { return _tablename; } }
		#endregion

		#region 实体类型
		private static string _typeName = "Trans.Db.Model.NBlock_Error";
		#endregion

		#region 列名列表
		private static List<string> _cols = new List<string>() { "ID", "BLOCKCODE", "COMMAND", "SENDTIME", "ACTION", "ISDEL", "CREATETIME" };
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
		public Int64 ID
		{
			get{ return this._ID;}
			set{ this._ID = value; ModifiedColumns.Add("[ID]"); }
		}
		private Int64 _ID = 0L;
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string BlockCode
		{
			get{ return this._BlockCode;}
			set{ this._BlockCode = value; ModifiedColumns.Add("[BlockCode]"); }
		}
		private string _BlockCode = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string Command
		{
			get{ return this._Command;}
			set{ this._Command = value; ModifiedColumns.Add("[Command]"); }
		}
		private string _Command = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public DateTime SendTime
		{
			get{ return this._SendTime;}
			set{ this._SendTime = value; ModifiedColumns.Add("[SendTime]"); }
		}
		private DateTime _SendTime = DateTime.Now;
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
		public NBlock_Error()
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
		public NBlock_Error(DataRow dr) : this()
		{
			if (dr != null && dr.Table.Columns.Contains("ID") && !(dr["ID"] is DBNull))
			{
				this._ID = Convert.ToInt64(dr["ID"]);
			}
			if (dr != null && dr.Table.Columns.Contains("BlockCode") && !(dr["BlockCode"] is DBNull))
			{
				this._BlockCode = Convert.ToString(dr["BlockCode"]);
			}
			if (dr != null && dr.Table.Columns.Contains("Command") && !(dr["Command"] is DBNull))
			{
				this._Command = Convert.ToString(dr["Command"]);
			}
			if (dr != null && dr.Table.Columns.Contains("SendTime") && !(dr["SendTime"] is DBNull))
			{
				this._SendTime = Convert.ToDateTime(dr["SendTime"]);
			}
			if (dr != null && dr.Table.Columns.Contains("Action") && !(dr["Action"] is DBNull))
			{
				this._Action = Convert.ToString(dr["Action"]);
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
