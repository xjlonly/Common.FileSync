using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace Common.LogUtility
{
    public class DBLog
    {
        /// <summary>
        /// 批次编码
        /// </summary>
        public string BlockStr = string.Empty;

        public string VMARKTEXT = "";
        DateTime VDT = DateTime.Now;

        #region 记录回滚系统所需操作
        /// <summary>
        /// 记录回滚系统所需操作
        /// </summary>
        /// <param name="RBMark"></param>
        /// <param name="RBServerIP"></param>
        /// <param name="RBServerPort"></param>
        /// <param name="RBServerBakUri"></param>
        /// <param name="RBServerAimUri"></param>
        /// <param name="RBSiteName"></param>
        /// <param name="RBOp"></param>
        /// <param name="Block"></param>
        /// <returns></returns>
        public bool RollBakLog(string RBMark, string RBServerIP, int RBServerPort, string RBServerBakUri, string RBServerAimUri, string RBSiteName, string RBOp, string Block)
        {
            //return Trans.DataLayer.Data.DBOper.RollBak_Op.insert(new Trans.DataLayer.Model.RollBak_Op()
            //{
            //    RBMark = RBMark,
            //    RBServerIP = RBServerIP,
            //    RBServerPort = RBServerPort,
            //    RBServerBakUri = RBServerBakUri,
            //    RBServerAimUri = RBServerAimUri,
            //    RBSiteName = RBSiteName,
            //    RBOp = RBOp,
            //    AutoRollBack = 0,
            //    IsUsed = 0,
            //    Block = Block,
            //    Year = VDT.Year.ToString(),
            //    Month = VDT.Month.ToString().PadLeft(2,'0'),
            //    Day = VDT.Day.ToString().PadLeft(2, '0')
            //});
            return Trans.Db.Data.NRoll_Action.insert(new Trans.Db.Model.NRoll_Action()
            {
                RollCode = RBMark,
                ServerIP = RBServerIP,
                ServerPort = RBServerPort,
                ServerBakUri = RBServerBakUri,
                ServerAimUri = RBServerAimUri,
                SiteName = RBSiteName,
                Op = RBOp,
                AutoRollBack = 0,
                IsUsed = 0,
                BlockCode = Block,
                YearVal = VDT.Year.ToString(),
                MonthVal = VDT.Month.ToString().PadLeft(2, '0'),
                DayVal = VDT.Day.ToString().PadLeft(2, '0')
            });
        }
        #endregion

        #region 记录自动回滚操作
        /// <summary>
        /// 记录自动回滚操作
        /// </summary>
        /// <param name="RBMark"></param>
        /// <param name="RBServerIP"></param>
        /// <param name="RBServerPort"></param>
        /// <param name="RBServerBakUri"></param>
        /// <param name="RBServerAimUri"></param>
        /// <param name="RBSiteName"></param>
        /// <param name="RBOp"></param>
        /// <param name="Block"></param>
        public void RollBakAutoSuccess(string RBMark, string RBServerIP, int RBServerPort, string RBServerBakUri, string RBServerAimUri, string RBSiteName, string RBOp, string Block)
        {
            //Trans.DataLayer.Data.DBOper.RollBak_Op.Update("RBMark=@RBMark AND RBServerIP=@RBServerIP AND RBServerPort=@RBServerPort AND RBServerBakUri=@RBServerBakUri AND RBServerAimUri=@RBServerAimUri AND RBSiteName=@RBSiteName AND RBOp=@RBOp AND Block=@Block",
            //    "AutoRollBack=1", new object[] { RBMark, RBServerIP, RBServerPort, RBServerBakUri, RBServerAimUri, RBSiteName, RBOp, Block });
            Trans.Db.Data.NRoll_Action.Update("RollCode=@RollCode AND ServerIP=@ServerIP AND ServerPort=@ServerPort AND ServerBakUri=@ServerBakUri AND ServerAimUri=@ServerAimUri AND SiteName=@SiteName AND Op=@Op AND BlockCode=@BlockCode",
                "AutoRollBack=1", new object[] { RBMark, RBServerIP, RBServerPort, RBServerBakUri, RBServerAimUri, RBSiteName, RBOp, Block });
        }
        #endregion

        #region 获取有备份同步批次日期列表
        /// <summary>
        /// 获取有备份同步批次日期列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetBakDateList()
        {
            //return Trans.DataLayer.Data.DBOper.RollBak_Op.GetTable("(AutoRollBack IS NULL OR AutoRollBack<>1)", "", "distinct Year+Month+Day", null, true);
            return Trans.Db.Data.NRoll_Action.GetTable("(AutoRollBack IS NULL OR AutoRollBack<>1)", "", "distinct YearVal+MonthVal+DayVal", null, true);
        }
        /// <summary>
        /// 获取有备份同步批次日期列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetBakDates(string SiteName)
        {
            //return Trans.DataLayer.Data.DBOper.RollBak_Op.GetTable("(AutoRollBack IS NULL OR AutoRollBack<>1)", "", "distinct Year+Month+Day", null, true);
            DataTable dt = Trans.Db.Data.NRoll_Action.GetTable("(AutoRollBack IS NULL OR AutoRollBack<>1) and SiteName=@SiteName ", "", "distinct YearVal+MonthVal+DayVal", new object[] { SiteName }, true);
            if(dt!=null&&dt.Rows.Count>0)
            {
                List<string> DateStrs = new List<string>();
                foreach(DataRow dr in dt.Rows)
                {
                    if (dr[0] != DBNull.Value)
                    {
                        DateStrs.Add(dr[0].ToString());
                    }
                }
                return DateStrs;
            }
            return new List<string>();
        }
        #endregion

        #region 获取备份回滚批次列表
        /// <summary>
        /// 获取备份回滚批次列表
        /// </summary>
        /// <param name="BakDateStr"></param>
        /// <returns></returns>
        public DataTable GetBakRollMark(string BakDateStr)
        {
            return Trans.Db.Data.NRoll_Action.GetTable("RollCode like '%'+@BakDate+'%' AND (AutoRollBack IS NULL OR AutoRollBack<>1)", "", "distinct RollCode,SiteName,IsUsed", new object[] { BakDateStr }, true);
        }
        /// <summary>
        /// 获取备份回滚批次列表
        /// </summary>
        /// <param name="BakDateStr"></param>
        /// <returns></returns>
        public List<Trans.Db.Model.NRoll_Action> GetRollbackMarks(string BakDateStr,string SelBakSite)
        {
            return Trans.Db.Data.NRoll_Action.GetList<Trans.Db.Model.NRoll_Action>("RollCode like '%'+@BakDate+'%' AND (AutoRollBack IS NULL OR AutoRollBack<>1) And SiteName=@SiteName ", "", "distinct RollCode,SiteName,IsUsed", new object[] { BakDateStr, SelBakSite }, true);
        }
        #endregion

        #region 获取可回滚操作列表
        /// <summary>
        /// 获取回滚操作列表
        /// </summary>
        /// <param name="RollBackMark"></param>
        /// <returns></returns>
        public DataTable GetBakUps(string RollCode)
        {
            return Trans.Db.Data.NRoll_Action.GetTable("IsDel=0 AND RollCode=@RollCode AND (AutoRollBack IS NULL OR AutoRollBack<>1)", "ID ASC", "*", new object[] { RollCode }, true);
        }
        public List<Trans.Db.Model.NRoll_Action> GetRollActions(string RollCode)
        {
            return Trans.Db.Data.NRoll_Action.GetList("IsDel=0 AND RollCode=@RollCode AND (AutoRollBack IS NULL OR AutoRollBack<>1)", "ID ASC", new object[] { RollCode }, true);
        }
        public bool GetRollSiteInfo(string RollCode, out string SiteName, out int IsUsed, out string BlockCode)
        {
            Trans.Db.Model.NRoll_Action action = Trans.Db.Data.NRoll_Action.Get("IsDel=0 AND RollCode=@RollCode AND (AutoRollBack IS NULL OR AutoRollBack<>1)", "ID ASC", new object[] { RollCode }, true);
            if (action != null && action.ID > 0)
            {
                SiteName = action.SiteName;
                IsUsed = action.IsUsed;
                BlockCode = action.BlockCode;
                return true;
            }
            else
            {
                SiteName = "";
                IsUsed = 0;
                BlockCode = "";
                return false;
            }
        }
        #endregion

        #region 标记回滚批次已被执行，用于全批次回滚
        /// <summary>
        /// 标记回滚批次已被执行，用于全批次回滚
        /// </summary>
        /// <param name="RollBackMark"></param>
        public void UpdateBakUsedMark(string RollBackMark)
        {
            Trans.Db.Data.NRoll_Action.Update("RollCode=@RollCode", "IsUsed=1", new object[] { RollBackMark });
        }
        #endregion

        #region 通用日志
        /// <summary>
        /// 通用日志
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Site"></param>
        /// <param name="Description"></param>
        public void Trace(string Title, string Site, string Description)
        {
            Trans.Db.Data.NBlock_Trace.insert(new Trans.Db.Model.NBlock_Trace()
            {
                BlockCode = this.BlockStr,
                DayVal = VDT.Day.ToString().PadLeft(2, '0'),
                MonthVal = VDT.Month.ToString().PadLeft(2, '0'),
                YearVal = VDT.Year.ToString(),
                Title = Title,
                Site = Site,
                Description = Description
            });
        }
        #endregion
        #region 人员操作通用日志
        /// <summary>
        /// 人员操作通用日志
        /// </summary>
        /// <param name="ActionName">动作名称</param>
        /// <param name="SiteName">操作站点</param>
        /// <param name="UserId">用户ID</param>
        /// <param name="BlockCode">操作批次号</param>
        public void UserOperLog(string ActionName, string Content, int UserId)
        {
            Trans.Db.Data.NUser_Log.insert(new Trans.Db.Model.NUser_Log()
            {
                UserId = UserId,
                Action = ActionName,
                Content = Content,
                CreateTime= DateTime.Now,
                IsDel=0
                
            });
        }
        #endregion

        #region 记录错误日志
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="CmdName"></param>
        /// <param name="CmdExcContent"></param>
        public void Error(string CmdName, string CmdExcContent)
        {
            Trans.Db.Data.NBlock_Error.insert(new Trans.Db.Model.NBlock_Error()
            {
                BlockCode = this.BlockStr,
                Command = CmdName,
                Action = CmdExcContent
            });
        }
        #endregion

        #region 记录与构建批次，防止重复
        /// <summary>
        /// 记录与构建批次，防止重复
        /// </summary>
        /// <param name="SiteName"></param>
        public void UpdateBlock(string SiteName)
        {
            VDT = DateTime.Now;
            VMARKTEXT = VDT.ToString("yyyyMMdd") + "/" + VDT.ToString("HH:mm:ss").Replace(":", "_");
            string BlockVMark = VDT.ToString("yyyy:MM:dd HH:mm:ss").Replace(':', '_').Replace(' ', '_');
            string str = SiteName.Replace('.', '_').ToUpper() + "_" + BlockVMark;
            if (Trans.Db.Data.NBlock_Info.Count("BlockCode=@BlockCode", new object[] { str }, true) > 0)
            {
                Thread.Sleep(1000);
                UpdateBlock(SiteName);
            }
            else
            {
                Trans.Db.Data.NBlock_Info.insert(new Trans.Db.Model.NBlock_Info()
                {
                    BlockCode = str,
                    ActionMark = VMARKTEXT
                });
                this.BlockStr = str;
            }
        }
        #endregion

        #region 获取批次跟踪日志
        /// <summary>
        /// 获取批次跟踪日志
        /// </summary>
        /// <param name="BlockCode"></param>
        /// <returns></returns>
        public DataTable GetTraces(string BlockCode,string SiteName)
        {
            return Trans.Db.Data.NBlock_Trace.GetTable("BlockCode=@Block and Site=@SiteName ", "ID ASC", "*", new object[] { BlockCode, SiteName }, true);
        }
        #endregion

        #region 获取日志年份
        /// <summary>
        /// 获取日志年份
        /// </summary>
        /// <returns></returns>
        public List<string> GetBlockYears(string SiteName )
        {
            List<string> Years = new List<string>();
            DataTable DT = Trans.Db.Data.NBlock_Trace.GetTable("IsDel=0 and Site=@SiteName ", "", "DISTINCT YearVal", new object[] { SiteName }, true);
            foreach (DataRow dr in DT.Rows)
            {
                if (dr != null && dr["YearVal"] != null && !string.IsNullOrEmpty(dr["YearVal"].ToString()) && !Years.Contains(dr["YearVal"].ToString()))
                {
                    Years.Add(dr["YearVal"].ToString());
                }
            }
            return Years;
        }
        #endregion

        #region 获取日志月份
        /// <summary>
        /// 获取日志月份
        /// </summary>
        /// <param name="Year"></param>
        /// <returns></returns>
        public DataTable GetBlockMonthList(string Year, string SiteName)
        {
            return Trans.Db.Data.NBlock_Trace.GetTable("YearVal=@Year and Site=@SiteName   ", "", "DISTINCT MonthVal", new object[] { Year ,SiteName}, true);
        }
        #endregion

        #region 获取日志日期
        /// <summary>
        /// 获取日志日期
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public DataTable GetBlockDayList(string Year, string Month, string SiteName)
        {
            return Trans.Db.Data.NBlock_Trace.GetTable("YearVal=@Year AND MonthVal=@Month and Site=@SiteName  ", "", "DISTINCT DayVal", new object[] { Year, Month ,SiteName}, true);
        }
        #endregion

        #region 根据日期获取批次
        /// <summary>
        /// 根据日期获取批次
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <param name="Day"></param>
        /// <returns></returns>
        public DataTable GetMarkBlocks(string Year, string Month, string Day, string SiteName)
        {
            return Trans.Db.Data.NBlock_Trace.GetTable("YearVal=@Year AND MonthVal=@Month AND DayVal=@Day  and Site=@SiteName  ", "", "DISTINCT BlockCode", new object[] { Year, Month, Day, SiteName }, true);
        }
        #endregion
    }
}