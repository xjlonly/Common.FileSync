using System;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Web.Script.Serialization;

namespace Trans.Db.Model
{
	/// <summary>
	/// 数据实体类:NBlock_Info
	/// </summary>
	[Serializable]
	public class NBlock_Info : ORM.Base.ModelBase
	{
		#region 表名
		private static string _tablename = "NBlock_Info";
		/// <summary>
		/// 表名
		/// </summary>
		[JsonIgnore]
		[ScriptIgnore]
		public string TableName { get { return _tablename; } }
		#endregion

		#region 实体类型
		private static string _typeName = "Trans.Db.Model.NBlock_Info";
		#endregion

		#region 列名列表
		private static List<string> _cols = new List<string>() { "ID", "BLOCKCODE", "STATUS", "STARTTIME", "UPLOADSUCCESS", "BACKUPSUCCESS", "COVERSUCCESS", "CANCELTIME", "ERRORFINISHTIME", "COVERSTATUS", "UPLOADLOG", "ACTIONMARK", "ISDEL", "CREATETIME" };
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
		public DateTime StartTime
		{
			get{ return this._StartTime;}
			set{ this._StartTime = value; ModifiedColumns.Add("[StartTime]"); }
		}
		private DateTime _StartTime = DateTime.Now;
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public DateTime UploadSuccess
		{
			get{ return this._UploadSuccess;}
			set{ this._UploadSuccess = value; ModifiedColumns.Add("[UploadSuccess]"); }
		}
		private DateTime _UploadSuccess = DateTime.Parse("1900-01-01");
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public DateTime BackupSuccess
		{
			get{ return this._BackupSuccess;}
			set{ this._BackupSuccess = value; ModifiedColumns.Add("[BackupSuccess]"); }
		}
		private DateTime _BackupSuccess = DateTime.Parse("1900-01-01");
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public DateTime CoverSuccess
		{
			get{ return this._CoverSuccess;}
			set{ this._CoverSuccess = value; ModifiedColumns.Add("[CoverSuccess]"); }
		}
		private DateTime _CoverSuccess = DateTime.Parse("1900-01-01");
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public DateTime CancelTime
		{
			get{ return this._CancelTime;}
			set{ this._CancelTime = value; ModifiedColumns.Add("[CancelTime]"); }
		}
		private DateTime _CancelTime = DateTime.Parse("1900-01-01");
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public DateTime ErrorFinishTime
		{
			get{ return this._ErrorFinishTime;}
			set{ this._ErrorFinishTime = value; ModifiedColumns.Add("[ErrorFinishTime]"); }
		}
		private DateTime _ErrorFinishTime = DateTime.Parse("1900-01-01");
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string CoverStatus
		{
			get{ return this._CoverStatus;}
			set{ this._CoverStatus = value; ModifiedColumns.Add("[CoverStatus]"); }
		}
		private string _CoverStatus = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string UploadLog
		{
			get{ return this._UploadLog;}
			set{ this._UploadLog = value; ModifiedColumns.Add("[UploadLog]"); }
		}
		private string _UploadLog = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string ActionMark
		{
			get{ return this._ActionMark;}
			set{ this._ActionMark = value; ModifiedColumns.Add("[ActionMark]"); }
		}
		private string _ActionMark = "";
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
		public NBlock_Info()
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
		public NBlock_Info(DataRow dr) : this()
		{
			if (dr != null && dr.Table.Columns.Contains("ID") && !(dr["ID"] is DBNull))
			{
				this._ID = Convert.ToInt32(dr["ID"]);
			}
			if (dr != null && dr.Table.Columns.Contains("BlockCode") && !(dr["BlockCode"] is DBNull))
			{
				this._BlockCode = Convert.ToString(dr["BlockCode"]);
			}
			if (dr != null && dr.Table.Columns.Contains("Status") && !(dr["Status"] is DBNull))
			{
				this._Status = Convert.ToInt32(dr["Status"]);
			}
			if (dr != null && dr.Table.Columns.Contains("StartTime") && !(dr["StartTime"] is DBNull))
			{
				this._StartTime = Convert.ToDateTime(dr["StartTime"]);
			}
			if (dr != null && dr.Table.Columns.Contains("UploadSuccess") && !(dr["UploadSuccess"] is DBNull))
			{
				this._UploadSuccess = Convert.ToDateTime(dr["UploadSuccess"]);
			}
			if (dr != null && dr.Table.Columns.Contains("BackupSuccess") && !(dr["BackupSuccess"] is DBNull))
			{
				this._BackupSuccess = Convert.ToDateTime(dr["BackupSuccess"]);
			}
			if (dr != null && dr.Table.Columns.Contains("CoverSuccess") && !(dr["CoverSuccess"] is DBNull))
			{
				this._CoverSuccess = Convert.ToDateTime(dr["CoverSuccess"]);
			}
			if (dr != null && dr.Table.Columns.Contains("CancelTime") && !(dr["CancelTime"] is DBNull))
			{
				this._CancelTime = Convert.ToDateTime(dr["CancelTime"]);
			}
			if (dr != null && dr.Table.Columns.Contains("ErrorFinishTime") && !(dr["ErrorFinishTime"] is DBNull))
			{
				this._ErrorFinishTime = Convert.ToDateTime(dr["ErrorFinishTime"]);
			}
			if (dr != null && dr.Table.Columns.Contains("CoverStatus") && !(dr["CoverStatus"] is DBNull))
			{
				this._CoverStatus = Convert.ToString(dr["CoverStatus"]);
			}
			if (dr != null && dr.Table.Columns.Contains("UploadLog") && !(dr["UploadLog"] is DBNull))
			{
				this._UploadLog = Convert.ToString(dr["UploadLog"]);
			}
			if (dr != null && dr.Table.Columns.Contains("ActionMark") && !(dr["ActionMark"] is DBNull))
			{
				this._ActionMark = Convert.ToString(dr["ActionMark"]);
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
