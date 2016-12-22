using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Trans.Web.Display
{
    public partial class Left : BasePage
    {
        /// <summary>
        /// 菜单文字
        /// </summary>
        public string MenuListText = "";
        public string Excel = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            GetParameters();
            if (Excel != "1")
            {
                GetMenuList();
            }
            else//退出，删除cookie,返回到登录页
            {
                DelCookie();
                HttpContext.Current.Response.Write("<script>window.parent.location.href='Login.aspx'</script>");
            }
        }
        private void GetParameters()
        {
            if (Request["Excel"] != null && Request["Excel"].Trim() != "")
            {
                this.Excel = HttpUtility.UrlDecode(Request["Excel"]);
            }
        }
        private void GetMenuList()
        {
            List<Trans.Db.Model.NUser_AuthSite> List = GetCurrentUserSiteList();
            int AllowList = List.Count(a => a.AllowList == 1);
            int AllowSync = List.Count(a => a.AllowSync == 1);
            int AllowRoll = List.Count(a => a.AllowRoll == 1);
            StringBuilder SbStr = new StringBuilder();
            if (AllowList > 0 || AllowSync > 0)
            {
                SbStr.Append("<li><a href = 'Main.aspx' target = 'right' > 文件同步 </ a ></ li > ");
            }
            SbStr.Append("<li><a href = 'Trace.aspx' target = 'right' > 同步日志 </ a ></ li > ");
            if (AllowRoll > 0)
            {
                SbStr.Append("<li><a href = 'Rolls.aspx' target = 'right' > 回滚操作 </ a ></ li > ");
            }
            //获取可访问的Email权限
            string AuthUser = System.Configuration.ConfigurationManager.AppSettings["AuthUser"];
            string EmailStr = "";
            string[] EmailArry = AuthUser.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (OpUser != null)
            {
                EmailStr = OpUser.UserName.ToString();
            }
            if (EmailArry.Contains(EmailStr))
            {
                SbStr.Append("<li><a href = 'Users.aspx' target = 'right' > 用户列表 </ a ></ li > ");
            }
            SbStr.Append("<li><a href = 'Left.aspx?Excel=1' target = 'right' > 退出 </ a ></ li > ");
            MenuListText = SbStr.ToString();
        }
    }
}