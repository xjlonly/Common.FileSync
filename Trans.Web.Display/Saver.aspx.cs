using System;
using System.Data;
using System.Text;

namespace Trans.Web.Display
{
    public partial class Saver : System.Web.UI.Page
    {
        protected string block = "";
        private int status = -1;
        protected string mainContent = "";
        protected string thiscontent = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["block"]))
            {
                block = Request["block"];
            }
            string msg = "";
            DateTime starttime = new DateTime(1900, 1, 1);
            //CallBackInit();
            DataTable dt = Common.LogUtility.UploadTrace.getTaskList(block, out status, out starttime, out msg);
            string timemark = "";
            if (starttime.Year > 2000)
            {
                TimeSpan ts = DateTime.Now.Subtract(starttime);
                timemark = " 开始时间: <span style='color:blue'>" + starttime.ToString("HH:mm:ss") + "</span> 已上传时间: <span style='color:blue'>" + ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds + "</span>";
            }
            System.Text.StringBuilder sb = new StringBuilder();
            if (dt != null)
            {
                sb.Append("<div>" + timemark + " </div>");
                sb.Append("<table width='100%' border='1' cellspadding='3'><tr><th style='text-align:left;'>文件名</th><th>状态</th></tr>");
                foreach (DataRow dr in dt.Rows)
                {
                    sb.AppendFormat("<tr onmouseOver=\"this.className='over'\" onmouseOut=\"this.className='out'\"><td>{0}</td><td><span class='s{2}'>{1}</span></td></tr>", dr["FilePath"], Common.LogUtility.UploadTrace.getFileStatus(int.Parse(dr["Status"].ToString())) + dr["CoverStatus"] + dr["UploadLog"].ToString().Replace("\r\n", "<br/>"), dr["Status"].ToString());
                }
                sb.Append("</table>");
                if (status == 1 || status == 5)
                {
                    sb.Append("<div>" + msg + "</div>");
                    sb.Append("<div>" + Status + "...</div>");
                    thiscontent = "<script>function r(){window.location=window.location} setTimeout('r()',1000);</script>";
                }
                else if (status == 6)
                {
                    sb.Append("<div>" + msg + "</div>");
                    sb.Append("<div>" + Status + "...</div>");
                    thiscontent = "<script>function r(){window.location=window.location} setTimeout('r()',1000);this.parent.document.getElementById('lkcancel').style.display='none';</script>";
                }
                else
                {
                    sb.Append("<div>" + msg + "</div>");
                    sb.Append("<div>" + Status + "  <a href='Main.aspx'>返回</a></div>");
                    thiscontent = "<script>alert('" + Status + "'); this.parent.document.getElementById('lkcancel').style.display='none';</script>";
                }
            }
            else
            {
                sb.Append("<div>需要查看的上传计划不存在</div>");
            }

            mainContent = sb.ToString();
        }
        protected string Status
        {
            get
            {
                return Common.LogUtility.UploadTrace.GetTaskStatus(status);
            }
        }
    }
}