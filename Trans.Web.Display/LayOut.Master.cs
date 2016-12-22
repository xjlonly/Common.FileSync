using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Trans.Web.Display
{
    public partial class LayOut : System.Web.UI.MasterPage
    {
        public int AllowRoll = 0;
        public int UserList = 0;
        public string UserName = "";
        public string MenuListText = "";
        public Trans.Db.Model.NUser_Info OpUser  = new Db.Model.NUser_Info();
        protected void Page_Load(object sender, EventArgs e)
        {
            BasePage base1 = new BasePage();
            UserName = base1.OpUser.UserName;
            OpUser = base1.OpUser;
            GetMenuList();

        }
       
        private void GetMenuList()
        {
            BasePage base1 = new BasePage();
            List<Trans.Db.Model.NUser_AuthSite> List = base1.GetCurrentUserSiteList();
            int AllowList = List.Count(a => a.AllowList == 1);
            int AllowSync = List.Count(a => a.AllowSync == 1);
            int AllowRoll = List.Count(a => a.AllowRoll == 1);
            StringBuilder SbStr = new StringBuilder();
            if (AllowList > 0 || AllowSync > 0)
            {
                SbStr.Append("<li ><a href = 'Main.aspx' ><i class='fa fa-desktop'></i> 文件同步</a></li> ");
            }
            SbStr.Append("<li><a href = 'Trace.aspx' ><i class='fa fa-font'></i> 同步日志</a></li> ");
            if (AllowRoll > 0)
            {
                SbStr.Append("<li><a href = 'Rolls.aspx' ><i class='fa fa-bar-chart-o'></i> 回滚操作</a></li>");
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
                SbStr.Append("<li><a href = 'Users.aspx' ><i class='fa fa-table'></i> 用户列表</a></li> ");
            }
            MenuListText = SbStr.ToString();
        }
    }

}