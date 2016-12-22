using System;
using System.Text;
using System.Web;

namespace Trans.Web.Display
{
    public partial class Login : System.Web.UI.Page
    {
        private string UserName = string.Empty;
        private string Password = string.Empty;
        private string CookieDomain = System.Configuration.ConfigurationManager.AppSettings["COOKIE_DOMAIN"] ?? "www.trans.com";
        private string CookieName = System.Configuration.ConfigurationManager.AppSettings["COOKIE_NAME"] ?? "trans_us";
        private string LoginPage = System.Configuration.ConfigurationManager.AppSettings["LOGIN_PAGE"] ?? "Login.aspx";
        private string CookieMinutes = System.Configuration.ConfigurationManager.AppSettings["COOKIE_MINUTES"] ?? "30";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.GetParameters();
            this.SetParameters();
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
        }

        private void SetParameters()
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                Password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Password.Trim(), "MD5");
                Trans.Db.Model.NUser_Info user = Trans.Db.Data.NUser_Info.Get("UserName=@UserName AND Password=@Password AND Status=1 AND IsDel=0", "", new object[] { UserName.Trim(), Password }, true);
                if (user != null && user.UserId > 0)
                {
                    int CookieExpireMinutes = 0;
                    if(!int.TryParse(CookieMinutes,out CookieExpireMinutes))
                    {
                        CookieExpireMinutes = 30;
                    }
                    HttpCookie UserCookie = new HttpCookie(CookieName);
                    UserCookie.Domain = CookieDomain;
                    UserCookie.Expires = DateTime.Now.AddMinutes(CookieExpireMinutes);
                    UserCookie.Value = HttpUtility.UrlEncode(DES.Encrypt("{\"USERID\":" + user.UserId.ToString() + ",\"USERNAME\":\"" + user.UserName + "\"}"), Encoding.UTF8);
                    Response.Cookies.Add(UserCookie);

                    Common.LogUtility.DBLog Userlog = new Common.LogUtility.DBLog();

                    Userlog.UserOperLog("登陆访问", UserName+"登陆" , user.UserId);


                    Response.Write("<script>alert('登录成功');location.href='Default.aspx';</script>");
                }
                else
                {
                    Response.Write("<script>alert('登录失败');</script>");
                }
            }
        }
    }
}