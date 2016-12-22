using System;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Web.Script.Serialization;

namespace Trans.Db.Model
{
	/// <summary>
	/// 数据实体类:NRoll_Action
	/// </summary>
	[Serializable]
	public class NRoll_Action : ORM.Base.ModelBase
	{
		#region 表名
		private static string _tablename = "NRoll_Action";
		/// <summary>
		/// 表名
		/// </summary>
		[JsonIgnore]
		[ScriptIgnore]
		public string TableName { get { return _tablename; } }
		#endregion

		#region 实体类型
		private static string _typeName = "Trans.Db.Model.NRoll_Action";
		#endregion

		#region 列名列表
		private static List<string> _cols = new List<string>() { "ID", "ROLLCODE", "SERVERIP", "SERVERPORT", "SERVERBAKURI", "SERVERAIMURI", "SITENAME", "OP", "ISUSED", "AUTOROLLBACK", "BLOCKCODE", "YEARVAL", "MONTHVAL", "DAYVAL", "ISDEL", "CREATETIME" };
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
		public string RollCode
		{
			get{ return this._RollCode;}
			set{ this._RollCode = value; ModifiedColumns.Add("[RollCode]"); }
		}
		private string _RollCode = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string ServerIP
		{
			get{ return this._ServerIP;}
			set{ this._ServerIP = value; ModifiedColumns.Add("[ServerIP]"); }
		}
		private string _ServerIP = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public Int32 ServerPort
		{
			get{ return this._ServerPort;}
			set{ this._ServerPort = value; ModifiedColumns.Add("[ServerPort]"); }
		}
		private Int32 _ServerPort = 0;
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string ServerBakUri
		{
			get{ return this._ServerBakUri;}
			set{ this._ServerBakUri = value; ModifiedColumns.Add("[ServerBakUri]"); }
		}
		private string _ServerBakUri = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string ServerAimUri
		{
			get{ return this._ServerAimUri;}
			set{ this._ServerAimUri = value; ModifiedColumns.Add("[ServerAimUri]"); }
		}
		private string _ServerAimUri = "";
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
		public string Op
		{
			get{ return this._Op;}
			set{ this._Op = value; ModifiedColumns.Add("[Op]"); }
		}
		private string _Op = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public Int32 IsUsed
		{
			get{ return this._IsUsed;}
			set{ this._IsUsed = value; ModifiedColumns.Add("[IsUsed]"); }
		}
		private Int32 _IsUsed = 0;
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public Int32 AutoRollBack
		{
			get{ return this._AutoRollBack;}
			set{ this._AutoRollBack = value; ModifiedColumns.Add("[AutoRollBack]"); }
		}
		private Int32 _AutoRollBack = 0;
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
		public string YearVal
		{
			get{ return this._YearVal;}
			set{ this._YearVal = value; ModifiedColumns.Add("[YearVal]"); }
		}
		private string _YearVal = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string MonthVal
		{
			get{ return this._MonthVal;}
			set{ this._MonthVal = value; ModifiedColumns.Add("[MonthVal]"); }
		}
		private string _MonthVal = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string DayVal
		{
			get{ return this._DayVal;}
			set{ this._DayVal = value; ModifiedColumns.Add("[DayVal]"); }
		}
		private string _DayVal = "";
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
		public NRoll_Action()
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
		public NRoll_Action(DataRow dr) : this()
		{
			if (dr != null && dr.Table.Columns.Contains("ID") && !(dr["ID"] is DBNull))
			{
				this._ID = Convert.ToInt64(dr["ID"]);
			}
			if (dr != null && dr.Table.Columns.Contains("RollCode") && !(dr["RollCode"] is DBNull))
			{
				this._RollCode = Convert.ToString(dr["RollCode"]);
			}
			if (dr != null && dr.Table.Columns.Contains("ServerIP") && !(dr["ServerIP"] is DBNull))
			{
				this._ServerIP = Convert.ToString(dr["ServerIP"]);
			}
			if (dr != null && dr.Table.Columns.Contains("ServerPort") && !(dr["ServerPort"] is DBNull))
			{
				this._ServerPort = Convert.ToInt32(dr["ServerPort"]);
			}
			if (dr != null && dr.Table.Columns.Contains("ServerBakUri") && !(dr["ServerBakUri"] is DBNull))
			{
				this._ServerBakUri = Convert.ToString(dr["ServerBakUri"]);
			}
			if (dr != null && dr.Table.Columns.Contains("ServerAimUri") && !(dr["ServerAimUri"] is DBNull))
			{
				this._ServerAimUri = Convert.ToString(dr["ServerAimUri"]);
			}
			if (dr != null && dr.Table.Columns.Contains("SiteName") && !(dr["SiteName"] is DBNull))
			{
				this._SiteName = Convert.ToString(dr["SiteName"]);
			}
			if (dr != null && dr.Table.Columns.Contains("Op") && !(dr["Op"] is DBNull))
			{
				this._Op = Convert.ToString(dr["Op"]);
			}
			if (dr != null && dr.Table.Columns.Contains("IsUsed") && !(dr["IsUsed"] is DBNull))
			{
				this._IsUsed = Convert.ToInt32(dr["IsUsed"]);
			}
			if (dr != null && dr.Table.Columns.Contains("AutoRollBack") && !(dr["AutoRollBack"] is DBNull))
			{
				this._AutoRollBack = Convert.ToInt32(dr["AutoRollBack"]);
			}
			if (dr != null && dr.Table.Columns.Contains("BlockCode") && !(dr["BlockCode"] is DBNull))
			{
				this._BlockCode = Convert.ToString(dr["BlockCode"]);
			}
			if (dr != null && dr.Table.Columns.Contains("YearVal") && !(dr["YearVal"] is DBNull))
			{
				this._YearVal = Convert.ToString(dr["YearVal"]);
			}
			if (dr != null && dr.Table.Columns.Contains("MonthVal") && !(dr["MonthVal"] is DBNull))
			{
				this._MonthVal = Convert.ToString(dr["MonthVal"]);
			}
			if (dr != null && dr.Table.Columns.Contains("DayVal") && !(dr["DayVal"] is DBNull))
			{
				this._DayVal = Convert.ToString(dr["DayVal"]);
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
