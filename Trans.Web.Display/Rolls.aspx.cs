using System;
using Common.LogUtility;
using Common.FileUtility;
using System.Collections.Generic;
using Common.ConfigUtility;

namespace Trans.Web.Display
{
    public partial class Roll : BasePage
    {
        protected string BakDateLab = string.Empty;
        protected string SelBakDate = string.Empty;
        protected string SelBakSite = string.Empty;
        protected string BakMarkLab = string.Empty;
        protected string RollBackMark = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.GetParameters();
            this.Data_Bind();
        }
        private void Data_Bind()
        {
            List<string> BackupSiteStrs = Common.FileUtility.PopedomCmd.AllowRollSite(OpUser.UserId);

            List<string> WebList = Common.ConfigUtility.Config.GetSiteList();//XML配置的
            this.BakDateLab+= "<select id='BakSiteList' name='BakSiteList' onchange=\"javascript:SelBakSite()\"><option value='-1'>---请选择备份站点---</option>";
            if (BackupSiteStrs!=null&&BackupSiteStrs.Count>0)
            {
                foreach (var item in BackupSiteStrs)
                {
                    if (WebList.Contains(item))
                    {
                        if (item == SelBakSite)
                        {
                            this.BakDateLab += "<option value='" + item + "' selected='selected'>" + item + "</option>";
                        }
                        else
                        {
                            this.BakDateLab += "<option value='" + item + "'>" + item + "</option>";
                        }
                    }
                }
                this.BakDateLab += "</select>";
            }



            List<string> BackupDateStrs = new DBLog().GetBakDates(SelBakSite);//DataTable DT = new DBLog().GetBakDateList();
            this.BakDateLab += "<select id='BakDateList' name='BakDateList' onchange=\"javascript:SelBakDate()\"><option value='-1'>---请选择备份时间---</option>";
            if(BackupDateStrs!=null&& BackupDateStrs.Count>0)// (DT != null && DT.Rows.Count > 0)
            {
                //DateTime[] TimeArray = null;
                List<int> TimeList = new List<int>();

                foreach(string BackupDateVal in BackupDateStrs)// (DataRow dr in DT.Rows)
                {
                    if (!string.IsNullOrEmpty(BackupDateVal))//dr[0].ToString()))
                    {
                        TimeList.Add(Convert.ToInt32(BackupDateVal));
                    }
                }
                TimeList.Sort();
                foreach (int TimeS in TimeList)
                {
                    string BakDate = TimeS.ToString().Insert(4, "年").Insert(7, "月") + "日";
                    if (SelBakDate == BakDate)
                    {
                        this.BakDateLab += "<option value='" + BakDate + "' selected='selected'>" + BakDate + "</option>";
                    }
                    else
                    {
                        this.BakDateLab += "<option value='" + BakDate + "'>" + BakDate + "</option>";
                    }
                }
            }
            this.BakDateLab += "</select>";

            if (!string.IsNullOrEmpty(this.SelBakDate) && !string.IsNullOrEmpty(this.SelBakSite))
            {
                //DataTable MarkDt = new DBLog().GetBakRollMark(Convert.ToDateTime(this.SelBakDate).ToString("yyyyMMdd"));
                List<Trans.Db.Model.NRoll_Action> RollbackActions = new DBLog().GetRollbackMarks(Convert.ToDateTime(this.SelBakDate).ToString("yyyyMMdd"), this.SelBakSite);
                this.BakMarkLab += "<table width='100%' border='1' cellspacing='0' cellpadding='0'>";
                this.BakMarkLab += "<tr><td width='20%'>站点</td><td width='20%'>备份号</td><td width='20%'>时间</td><td>是否已用</td><td width='20%'>操作</td></tr>";
                if (RollbackActions != null && RollbackActions.Count > 0)// (MarkDt != null && MarkDt.Rows.Count > 0)
                {
                    foreach (var item in RollbackActions)//(DataRow dr in MarkDt.Rows)
                    {
                        this.BakMarkLab += "<tr onmouseover=\"this.style.background='#ccc'\" onmouseout=\"this.style.background='#fff'\">";
                        this.BakMarkLab += "<td style='white-space:normal; word-break:break-all;word-wrap:break-word'>" + item.SiteName /*dr[1].ToString()*/ + "</td>";
                        this.BakMarkLab += "<td style='white-space:normal; word-break:break-all;word-wrap:break-word'>" + item.RollCode /*dr[0].ToString()*/ + "</td>";
                        this.BakMarkLab += "<td style='white-space:normal; word-break:break-all;word-wrap:break-word'>" + item.RollCode.Substring(item.RollCode.IndexOf('/') + 1).Replace('_', ':') /*dr[0].ToString().Substring(dr[0].ToString().IndexOf('/') + 1).Replace('_', ':')*/ + "</td>";
                        if (item.IsUsed == 1)// (Convert.ToInt32(dr[2].ToString()) == 1)
                        {
                            this.BakMarkLab += "<td>已回滚</td><td>已回滚  |  <a href='#' onclick=\"javascript:ViewRBOp('" + item.RollCode /*dr[0].ToString()*/ + "')\">察看回滚日志</a></td></tr>";
                        }
                        else
                        {
                            this.BakMarkLab += "<td>未回滚</td><td><a href='#' onclick=\"javascript:RollBack('" + item.RollCode /*dr[0].ToString()*/ + "')\">未回滚</a>  |  <a href='#' onclick=\"javascript:ViewRBOp('" + item.RollCode /*dr[0].ToString()*/ + "')\">察看回滚日志</a></td></tr>";
                        }
                    }
                }
                this.BakMarkLab += "</table>";
            }
            else {
                this.BakMarkLab += "";
            }
        }
        /// <summary>
        /// 获取回传参数
        /// </summary>
        private void GetParameters()
        {
            if (Request["BakDateList"] != null && Request["BakDateList"].Trim() != "" && Request["BakDateList"].Trim() != "-1")
            {
                this.SelBakDate = Request["BakDateList"].Trim();
            }
            if (Request["BakSiteList"] != null && Request["BakSiteList"].Trim() != "" && Request["BakSiteList"].Trim() != "-1")
            {
                this.SelBakSite = Request["BakSiteList"].Trim();
            }
            if (Request["RollBackMark"] != null && Request["RollBackMark"].Trim() != "")
            {
                this.RollBackMark = Request["RollBackMark"].Trim();
                this.RollBackOp();
            }
        }
        private void RollBackOp()
        {
            if ((!string.IsNullOrEmpty(this.RollBackMark)) && this.RollBackMark != "-1")
            {
              
                //DataTable DT = dbl.GetBakUps(this.RollBackMark);
                
                string SiteName = string.Empty;
                int IsUsed = 0;
                string BlockCode = string.Empty;
                if (new DBLog().GetRollSiteInfo(this.RollBackMark, out SiteName, out IsUsed, out BlockCode) && !string.IsNullOrEmpty(SiteName) && IsUsed == 0)
                {
                    //记录回归日志
                    Common.LogUtility.DBLog Userlog = new DBLog();
                    Userlog.UserOperLog("回滚操作", "站点：" + SiteName + ",批次：" + BlockCode, OpUser.UserId);

                    TaskInfo SiteObj = new TaskInfo();
                    string msg = "";
                    //执行回滚

                    SiteObj.SiteBaseInfo = Common.ConfigUtility.Config.GetSiteInfo(SiteName);
                     DBLog dbl = new DBLog();
                    dbl.BlockStr = BlockCode;

                    TaskWorker SyncAction = new TaskWorker(SiteObj, dbl);
                    if (SyncAction.RollBackSite())
                    {
                        new DBLog().UpdateBakUsedMark(this.RollBackMark);
                        Response.Write("<script>alert('回滚成功');</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('回滚失败!\\r\\n\\r\\n" + msg + "');</script>");
                    }
                }
                else
                {
                    this.Data_Bind();
                    Response.End();
                }
                //if (DT != null && DT.Rows.Count > 0)
                //{
                //    sitename = DT.Rows[0]["RBSiteName"].ToString();
                //}
                //TaskInfo SiteObj = new TaskInfo();

                //if (Convert.ToInt32(DT.Rows[0]["IsUsed"]) == 0)
                //{
                //    string msg = "";
                //    TaskWorker SyncAction = new TaskWorker(SiteObj, dbl);
                //    if (SyncAction.RollBackSite())
                //    {
                //        new DBLog().UpdateBakUsedMark(this.RollBackMark);
                //        Response.Write("<script>alert('回滚成功');</script>");
                //    }
                //    else
                //    {
                //        Response.Write("<script>alert('回滚失败!\\r\\n\\r\\n" + msg + "');</script>");
                //    }
                //}
                //else
                //{
                //    this.Data_Bind();
                //    Response.End();
                //}
            }
        }
    }
}