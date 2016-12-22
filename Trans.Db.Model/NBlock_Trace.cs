using System;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Web.Script.Serialization;

namespace Trans.Db.Model
{
	/// <summary>
	/// 数据实体类:NBlock_Trace
	/// </summary>
	[Serializable]
	public class NBlock_Trace : ORM.Base.ModelBase
	{
		#region 表名
		private static string _tablename = "NBlock_Trace";
		/// <summary>
		/// 表名
		/// </summary>
		[JsonIgnore]
		[ScriptIgnore]
		public string TableName { get { return _tablename; } }
		#endregion

		#region 实体类型
		private static string _typeName = "Trans.Db.Model.NBlock_Trace";
		#endregion

		#region 列名列表
		private static List<string> _cols = new List<string>() { "ID", "TITLE", "SITE", "DESCRIPTION", "BLOCKCODE", "YEARVAL", "MONTHVAL", "DAYVAL", "ISDEL", "CREATETIME" };
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
		public string Title
		{
			get{ return this._Title;}
			set{ this._Title = value; ModifiedColumns.Add("[Title]"); }
		}
		private string _Title = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string Site
		{
			get{ return this._Site;}
			set{ this._Site = value; ModifiedColumns.Add("[Site]"); }
		}
		private string _Site = "";
		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string Description
		{
			get{ return this._Description;}
			set{ this._Description = value; ModifiedColumns.Add("[Description]"); }
		}
		private string _Description = "";
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
		public NBlock_Trace()
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
		public NBlock_Trace(DataRow dr) : this()
		{
			if (dr != null && dr.Table.Columns.Contains("ID") && !(dr["ID"] is DBNull))
			{
				this._ID = Convert.ToInt64(dr["ID"]);
			}
			if (dr != null && dr.Table.Columns.Contains("Title") && !(dr["Title"] is DBNull))
			{
				this._Title = Convert.ToString(dr["Title"]);
			}
			if (dr != null && dr.Table.Columns.Contains("Site") && !(dr["Site"] is DBNull))
			{
				this._Site = Convert.ToString(dr["Site"]);
			}
			if (dr != null && dr.Table.Columns.Contains("Description") && !(dr["Description"] is DBNull))
			{
				this._Description = Convert.ToString(dr["Description"]);
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
