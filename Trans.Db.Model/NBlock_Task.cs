using System;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Web.Script.Serialization;

namespace Trans.Db.Model
{
	/// <summary>
	/// 数据实体类:NBlock_Task
	/// </summary>
	[Serializable]
	public class NBlock_Task : ORM.Base.ModelBase
	{
		#region 表名
		private static string _tablename = "NBlock_Task";
		/// <summary>
		/// 表名
		/// </summary>
		[JsonIgnore]
		[ScriptIgnore]
		public string TableName { get { return _tablename; } }
		#endregion

		#region 实体类型
		private static string _typeName = "Trans.Db.Model.NBlock_Task";
		#endregion

		#region 列名列表
		private static List<string> _cols = new List<string>() { "ID", "BLOCKCODE", "FILEPATH", "STATUS", "UPLOADLOG", "COVERSTATUS", "ERRORSERVER", "ISDEL", "CREATETIME" };
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
		public string FilePath
		{
			get{ return this._FilePath;}
			set{ this._FilePath = value; ModifiedColumns.Add("[FilePath]"); }
		}
		private string _FilePath = "";
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
		public string ErrorServer
		{
			get{ return this._ErrorServer;}
			set{ this._ErrorServer = value; ModifiedColumns.Add("[ErrorServer]"); }
		}
		private string _ErrorServer = "";
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
		public NBlock_Task()
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
		public NBlock_Task(DataRow dr) : this()
		{
			if (dr != null && dr.Table.Columns.Contains("ID") && !(dr["ID"] is DBNull))
			{
				this._ID = Convert.ToInt64(dr["ID"]);
			}
			if (dr != null && dr.Table.Columns.Contains("BlockCode") && !(dr["BlockCode"] is DBNull))
			{
				this._BlockCode = Convert.ToString(dr["BlockCode"]);
			}
			if (dr != null && dr.Table.Columns.Contains("FilePath") && !(dr["FilePath"] is DBNull))
			{
				this._FilePath = Convert.ToString(dr["FilePath"]);
			}
			if (dr != null && dr.Table.Columns.Contains("Status") && !(dr["Status"] is DBNull))
			{
				this._Status = Convert.ToInt32(dr["Status"]);
			}
			if (dr != null && dr.Table.Columns.Contains("UploadLog") && !(dr["UploadLog"] is DBNull))
			{
				this._UploadLog = Convert.ToString(dr["UploadLog"]);
			}
			if (dr != null && dr.Table.Columns.Contains("CoverStatus") && !(dr["CoverStatus"] is DBNull))
			{
				this._CoverStatus = Convert.ToString(dr["CoverStatus"]);
			}
			if (dr != null && dr.Table.Columns.Contains("ErrorServer") && !(dr["ErrorServer"] is DBNull))
			{
				this._ErrorServer = Convert.ToString(dr["ErrorServer"]);
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
