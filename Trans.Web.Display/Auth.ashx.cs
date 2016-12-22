using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trans.Web.Display
{
    /// <summary>
    /// Auth 的摘要说明
    /// </summary>
    public class Auth : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string userChecked = context.Request["uid"];//用户id
                                                        //站点及其目录,站点和文件目录以“;;”分隔
            string authchecked = context.Request["aid"];
            if (!string.IsNullOrEmpty(userChecked) && !string.IsNullOrEmpty(authchecked))
            {
                bool isSuccess = UserAuthorize(userChecked, authchecked);
                if (isSuccess)
                {
                    context.Response.Write("授权成功");
                }
                else
                {
                    context.Response.Write("部分授权成功");
                }
            }
            else
            {
                context.Response.Write("请选择需要授权的用户以及站点");
            }
            context.Response.End();
        }

        /// <summary>
        /// 用户授权
        /// </summary>
        /// <param name="userChecked">需授权的用户</param>
        /// <param name="authchecked">站点及其目录</param>
        /// <returns></returns>
        private bool UserAuthorize(string userChecked, string authchecked)
        {
            string[] userIds = userChecked.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] authArry = authchecked.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            bool isSuccess = true;
            if (userIds != null && userIds.Length > 0)
            {
                foreach (var item in userIds)
                {
                    if (!AddUserSite(item, authArry))
                    {
                        isSuccess = false;
                    }
                }
                return isSuccess;
            }
            return false;
        }
        /// <summary>
        ///为单个用户授权
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        private bool AddUserSite(string userid, string[] authArry)
        {
            List<string> siteList = new List<string>();
            bool isSuccess = true;
            if (authArry != null && authArry.Length > 0)
            {
                foreach (var item in authArry)
                {
                    //分隔站点与文件目录
                    string[] siteAndAuth = item.Split(new string[] { ";;" }, StringSplitOptions.RemoveEmptyEntries);
                    if (siteAndAuth.Length < 3)
                    {
                        continue;
                    }
                    string site = siteAndAuth[1];
                    string detailName = siteAndAuth[0];
                    string isDir = siteAndAuth[2];
                    //添加站点权限
                    if (!siteList.Contains(site))
                    {
                        siteList.Add(site);
                        //批量删除该用户下的该站点下文件权限
                        //Trans.DataLayer.Data.DBOper.w_UsersAndDetails.DBDelete(" UserID=@id  ", new object[] { userid, site });
                        Trans.Db.Data.NUser_AuthDetail.Delete("UserId=@UserId AND SiteName=@SiteName", new object[] { userid, site });
                        //检测该用户是否有该站点权限，如无则添加站点至w_UsersAndSites
                        if (!CheckAndAddSite(userid, site))
                        {
                            isSuccess = false;
                        }
                    }
                    // 检测该用户是否有该站点下的文件权限，如无则添加至w_UsersAndDetails
                    if (!CheckAndAddDetail(userid, detailName, site, isDir))
                    {
                        isSuccess = false;
                    }
                }
                return isSuccess;
            }
            return false;
        }
        /// <summary>
        /// 检测该用户是否有该站点权限，如无则添加站点至w_UsersAndSites
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="siteName"></param>
        /// <returns></returns>
        private bool CheckAndAddSite(string userid, string siteName)
        {
            var entity = Trans.Db.Data.NUser_AuthSite.Get("UserID=@UserID and SiteName=@SiteName ", "", new object[] { userid, siteName });
            //Trans.DataLayer.Data.DBOper.w_UsersAndSites.Get(" UserID=@id and SiteName=@site ", "", new object[] { userid, siteName });
            if (entity != null && entity.UserID > 0)
            {
                return true;
            }
            else
            {
                //bool isSuccess = Trans.DataLayer.Data.DBOper.w_UsersAndSites.insert(
                //    new Trans.DataLayer.Model.w_UsersAndSites()
                //    {
                //        SiteName = siteName,
                //        UserID = Convert.ToInt32(userid),
                //        AllowList = "true",
                //        AllowRoll = "true",
                //        AllowSync = "true"
                //    });
                bool isSuccess = Trans.Db.Data.NUser_AuthSite.insert(new Db.Model.NUser_AuthSite()
                {
                    UserID = Convert.ToInt32(userid),
                    SiteName = siteName,
                    AllowList = 1,
                    AllowRoll = 1,
                    AllowSync = 1
                });
                return isSuccess;
            }

        }
        /// <summary>
        /// 检测该用户是否有该站点下的文件权限，如无则添加至w_UsersAndDetails
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="detailName"></param>
        /// <param name="siteName"></param>
        /// <returns></returns>
        private bool CheckAndAddDetail(string userId, string detailName, string siteName, string IsDir)
        {
            var entity = Trans.Db.Data.NUser_AuthDetail.Get("userId=@userId and siteName=@siteName and DetailName=@detail ", "", new object[] { userId, siteName, detailName });
            if (entity != null && entity.ID > 0)
            {
                return true;
            }
            else
            {
                //bool isSuccess = Trans.DataLayer.Data.DBOper.w_UsersAndDetails.insert(
                //    new Trans.DataLayer.Model.w_UsersAndDetails()
                //    {
                //        UserID = Convert.ToInt32(userId),
                //        SiteName = siteName,
                //        DetailName = detailName,
                //        AllowList = "true",
                //        AllowRoll = "true",
                //        AllowSync = "true",
                //        IsDir = IsDir
                //    });
                bool isSuccess = Trans.Db.Data.NUser_AuthDetail.insert(
                    new Trans.Db.Model.NUser_AuthDetail()
                    {
                         UserID = Convert.ToInt32(userId),
                        SiteName = siteName,
                        DetailName = detailName,
                        AllowList = 1,
                        AllowRoll = 1,
                        AllowSync = 1,
                        IsDir = Convert.ToInt32(IsDir)
                    });
                return false;
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