using System;
using System.Collections.Generic;
using System.Text;

namespace Trans.Web.Display
{
    public partial class Users : System.Web.UI.Page
    {
        protected string ListStr = "";
        private string UserName = "";
        private string Password = "";
        private string SruePwd = "";
        private string OperStatus = "";
        private string UserId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Trans.Db.Model.NUser_Info> users = Trans.Db.Data.NUser_Info.GetList("IsDel=0");
            StringBuilder txt = new StringBuilder();
            txt.Append("<table width='100%' cellspacing='0' cellpadding='0' border='1'>");
            txt.Append("<tr><td>编号</td><td>用户名</td><td>状态</td><td>创建时间</td><td>外部编码</td><td>操作</td></tr>");
            foreach (Trans.Db.Model.NUser_Info item in users)
            {
                txt.Append("<tr>");
                txt.Append("<td>" + item.UserId.ToString() + "</td>");
                txt.Append("<td>" + item.UserName.ToString() + "</td>");
                txt.Append("<td>" + (item.Status == 1 ? "有效" : "无效") + "</td>");
                txt.Append("<td>" + item.CreateTime.ToString() + "</td>");
                txt.Append("<td>" + item.RefUserId.ToString() + "</td>");
                txt.Append("<td><a href='Grant.aspx?UserId=" + item.UserId.ToString() + "'>授权</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                if(item.Status==1)
                {
                txt.Append("<a href='Users.aspx?OperStatus=1&UserId=" + item.UserId.ToString() + "'>停用</a></td>");
                }else
                { 
                txt.Append("<a href='Users.aspx?OperStatus=1&UserId=" + item.UserId.ToString() + "'>启用</a></td>");
                }
                txt.Append("</tr>");
            }
            this.ListStr = txt.ToString();
            GetParameters();
            if (IsPostBack)
            {

                SetParameters();
            }
            if (OperStatus == "1")
            {
                
                UpdateStatus();
            }
        }
        private void UpdateStatus()
        {
            Trans.Db.Model.NUser_Info userinfo = Trans.Db.Data.NUser_Info.Get("Userid=@Userid  ", "", new object[] { UserId });
            userinfo.Status = Convert.ToInt32(!Convert.ToBoolean(userinfo.Status));
            if (Trans.Db.Data.NUser_Info.Update(userinfo) > 0)
            {
                Response.Write("<script>alert('操作成功');window.location.href='Users.aspx'</script>");
            }
            else
            {

                Response.Write("<script>alert('操作失败');window.location.href='Users.aspx'</script>");
            }
        }

        private void GetParameters()
        {
            if (!string.IsNullOrEmpty(Request["UserName"]))
            {
                UserName = Request["UserName"].Trim();
            }
            if (!string.IsNullOrEmpty(Request["Password"]))
            {
                Password = Request["Password"].Trim();
            }
            if (!string.IsNullOrEmpty(Request["SruePwd"]))
            {
                SruePwd = Request["SruePwd"].Trim();
            }
            if (!string.IsNullOrEmpty(Request["OperStatus"]))
            {
                OperStatus = Request["OperStatus"].Trim();
            }
            if (!string.IsNullOrEmpty(Request["UserId"]))
            {
                UserId = Request["UserId"].Trim();
            }
        }
        private void SetParameters()
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(SruePwd))
            {
                Trans.Db.Model.NUser_Info user = Trans.Db.Data.NUser_Info.Get(" UserName=@UserName and isdel=0 ", "", new object[] { UserName });
                if (user.UserId > 0)
                {
                    Response.Write("<script>alert('此用户已存在');href='Users.aspx';</script>");
                }
                else
                {
                    if (SruePwd != Password)
                    {

                        Response.Write("<script>alert('两次密码不同');</script>");

                    }
                    else
                    {

                    user.UserName = UserName;
                    user.Password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5");
                    user.CreateTime = DateTime.Now;
                    user.Status = 1;
                        if (Trans.Db.Data.NUser_Info.insert(user))
                        {
                            Response.Write("<script>alert('添加成功');href='Users.aspx'</script>");

                        }
                        else
                        {
                            Response.Write("<script>alert('添加失败');</script>");

                        }

                    }
                }
            }
            else
            {

                Response.Write("<script>alert('请完善信息');;</script>");

            }
        }

    }
}