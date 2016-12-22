using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Trans.Web.Display
{
    public class BasePage : System.Web.UI.Page
    {
        private string CookieDomain = System.Configuration.ConfigurationManager.AppSettings["COOKIE_DOMAIN"] ?? "www.trans.com";
        private string CookieName = System.Configuration.ConfigurationManager.AppSettings["COOKIE_NAME"] ?? "trans_us";
        private string LoginPage = System.Configuration.ConfigurationManager.AppSettings["LOGIN_PAGE"] ?? "Login.aspx";
        private string CookieMinutes = System.Configuration.ConfigurationManager.AppSettings["COOKIE_MINUTES"] ?? "30";

        private Trans.Db.Model.NUser_Info _opUser = new Trans.Db.Model.NUser_Info();
        /// <summary>
        /// 当前登陆用户
        /// </summary>
        public Trans.Db.Model.NUser_Info OpUser
        {
            get { return _opUser; }
            set { _opUser = value; }
        }
        
        #region 登陆
        public bool IsLogin()
        {
            string CookieValue = ReadCookie();
            if (!string.IsNullOrEmpty(CookieValue))
            {
                string CookieDEVal = DES.Decrypt(CookieValue);
                try
                {
                    CookieModel cm = Newtonsoft.Json.JsonConvert.DeserializeObject<CookieModel>(CookieDEVal);
                    var entity = Trans.Db.Data.NUser_Info.Get("UserName=@UserName AND UserId=@UserId AND Status=1 AND IsDel=0", "", new object[] { cm.USERNAME, cm.USERID });
                    if (entity != null && entity.UserId > 0)
                    {
                        this.OpUser = entity;
                        int CookieExpireMinutes = 0;
                        if (!int.TryParse(CookieMinutes, out CookieExpireMinutes))
                        {
                            CookieExpireMinutes = 30;
                        }
                        System.Web.HttpContext.Current.Response.Cookies[CookieName].Domain = CookieDomain;
                        System.Web.HttpContext.Current.Response.Cookies[CookieName].Expires = DateTime.Now.AddMinutes(CookieExpireMinutes);
                        System.Web.HttpContext.Current.Response.Cookies[CookieName].Value = CookieValue;
                        return true;
                    }
                }
                catch(Exception ex) { Console.WriteLine(ex.Message); }
            }
            return false;
        }

        internal class CookieModel
        {
            public string USERNAME { get; set; }

            public int USERID { get; set; }
        }

        /// <summary>
        /// 读取单点登录的Cookie
        /// </summary>
        /// <returns></returns>
        private string ReadCookie()
        {
            System.Web.HttpCookie cookie = GetCookie();
            if (cookie != null)
            {
                return System.Web.HttpUtility.UrlDecode(cookie.Value, Encoding.UTF8);
            }
            return null;
        }
        /// <summary>
        /// 获取单点登录的Cookie
        /// </summary>
        /// <returns></returns>
        private System.Web.HttpCookie GetCookie()
        {
            if (System.Web.HttpContext.Current.Request.Cookies == null || System.Web.HttpContext.Current.Request.Cookies.Count == 0)
            {
                return null;
            }
            for (int i = 0; i < System.Web.HttpContext.Current.Request.Cookies.Count; i++)
            {
                System.Web.HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[i];
                if (cookie.Name == CookieName)
                {
                    return cookie;
                }
            }
            return null;
        }

        /// <summary>
        /// 删除cookie值
        /// </summary>
        public   void DelCookie()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            System.Web.HttpCookie cookie = new System.Web.HttpCookie(CookieName, "");
            cookie.Domain = CookieDomain;
            cookie.Expires = DateTime.Now.AddHours(-1);
            context.Response.Cookies.Add(cookie);
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public BasePage()
        {
            if (!IsLogin())
            {
                DelCookie();
                HttpContext.Current.Response.Write("<script>alert('未登陆系统，请先登陆');window.parent.location.href='"+ LoginPage + "'</script>");
                return;
            }
        }
        /// <summary>
        /// 获取当前登录人可操作的站点名称列表
        /// </summary>
        /// <returns></returns>
        public  List<Trans.Db.Model.NUser_AuthSite> GetCurrentUserSiteList()
        {
            List<Trans.Db.Model.NUser_AuthSite> list = new List<Trans.Db.Model.NUser_AuthSite>();
            list = Trans.Db.Data.NUser_AuthSite.GetList("isdel=0 and Userid=@Userid ", "", new object[] { this.OpUser.UserId }, true);
            return list;
        }
       
    }
}