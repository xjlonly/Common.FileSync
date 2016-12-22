using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trans.Web.Display
{
    /// <summary>
    /// AuthSava 的摘要说明
    /// </summary>
    public class AuthSava : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string WebName = context.Request["WebName"];
            string AllowList = context.Request["AllowList"];
            string AllowSync = context.Request["AllowSync"];
            string AllowRoll = context.Request["AllowRoll"];
            string UserID = context.Request["UserID"];//用户id链接字符串以‘，分割

            bool falg = false;

           
                if (AllowList == "0" && AllowSync == "1")
                {
                    context.Response.Write("如授权同步权限，需同时授权查看权限！");
                }
                else
                {
                    Trans.Db.Model.NUser_AuthSite NUserSite = Trans.Db.Data.NUser_AuthSite.Get("UserID=@UserID and SiteName=@SiteName and isdel=0", "", new object[] { UserID, WebName });
                    if (NUserSite.ID > 0)
                    {
                        NUserSite.AllowList = Convert.ToInt32(AllowList);
                        NUserSite.AllowSync = Convert.ToInt32(AllowSync);
                        NUserSite.AllowRoll = Convert.ToInt32(AllowRoll);
                        if (Trans.Db.Data.NUser_AuthSite.Update(NUserSite) > 0)

                        {
                            context.Response.Write("授权成功！");
                        }
                        else
                        {
                            context.Response.Write("授权失败！");
                        }
                    }
                    else
                    {
                        NUserSite.UserID = Convert.ToInt32(UserID);
                        NUserSite.SiteName = WebName;
                        NUserSite.AllowList = Convert.ToInt32(AllowList);
                        NUserSite.AllowSync = Convert.ToInt32(AllowSync);
                        NUserSite.AllowRoll = Convert.ToInt32(AllowRoll);
                        NUserSite.CreateTime = DateTime.Now;
                        if (Trans.Db.Data.NUser_AuthSite.insert(NUserSite))

                        {
                            context.Response.Write("授权成功！");
                        }
                        else
                        {
                            context.Response.Write("授权失败！");
                        }
                    }

            }
        }
            

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}