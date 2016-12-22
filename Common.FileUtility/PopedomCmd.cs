using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Common.FileUtility
{
    /// <summary>
    /// 同步系统权限控制
    /// </summary>
    public class PopedomCmd
    {
        #region 添加用户可操作文件夹
        /// <summary>
        /// 添加用户可操作文件夹
        /// </summary>
        /// <param name="userid">用户编码</param>
        /// <param name="sitename">站点名称</param>
        /// <param name="detailname">文件夹名称</param>
        /// <returns></returns>
        public static bool AddUserDetail(int userid, string sitename, string detailname)
        {
            //try
            //{
            //    string cmdText = "INSERT INTO NUser_AuthDetail(UserID,DetailName,SiteName,IsDir,AllowSync,AllowRoll,AllowList) VALUES (@uid,@dname,@sname,@IsDir,'True','True','True')";
            //    SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@uid", SqlDbType.Int), new SqlParameter("@dname", SqlDbType.NVarChar, 200), new SqlParameter("@sname", SqlDbType.NVarChar, 50), new SqlParameter("@IsDir", SqlDbType.NVarChar, 10) };
            //    commandParameters[0].Value = userid;
            //    commandParameters[1].Value = detailname;
            //    commandParameters[2].Value = sitename;
            //    if (detailname.EndsWith("/"))
            //    {
            //        commandParameters[3].Value = "True";
            //    }
            //    else
            //    {
            //        commandParameters[3].Value = "False";
            //    }
            //    return (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters) == 1);
            //}
            //catch
            //{
            //    return false;
            //}
            return Trans.Db.Data.NUser_AuthDetail.insert(new Trans.Db.Model.NUser_AuthDetail()
            {
                AllowList = 1,
                AllowRoll = 1,
                AllowSync = 1,
                DetailName = detailname,
                IsDir = detailname.EndsWith("/") ? 1 : 0,
                UserID = userid,
                SiteName = sitename
            });
        }
        #endregion

        #region 添加用户可操作站点
        /// <summary>
        /// 添加用户可操作站点
        /// </summary>
        /// <param name="userid">用户编码</param>
        /// <param name="sitename">站点名称</param>
        /// <returns></returns>
        public static bool AddUserSite(int userid, string sitename)
        {
            //try
            //{
            //    string cmdText = "INSERT INTO NUser_AuthSite(UserID,SiteName) VALUES (@uid,@sname)";
            //    SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@uid", SqlDbType.Int), new SqlParameter("@sname", SqlDbType.NVarChar, 50) };
            //    commandParameters[0].Value = userid;
            //    commandParameters[1].Value = sitename;
            //    return (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters) == 1);
            //}
            //catch
            //{
            //    return false;
            //}
            return Trans.Db.Data.NUser_AuthSite.insert(new Trans.Db.Model.NUser_AuthSite()
            {
                SiteName = sitename,
                UserID = userid
            });
        }
        #endregion

        public static List<string> AllowCompleteListSite(int userid)
        {
            List<Trans.Db.Model.NUser_AuthSite> Sites = Trans.Db.Data.NUser_AuthSite.GetList("UserId=@UserId AND AllowList=1", "ID ASC", new object[] { userid }, true);
            List<string> list = new List<string>();
            foreach(var item in Sites)
            {
                list.Add(item.SiteName);
            }
            return list;
            //string cmdText = "SELECT SiteName From NUser_AuthSite(nolock) WHERE UserID=@UID AND AllowList='True'";
            //SqlParameter parameter = new SqlParameter("@UID", SqlDbType.Int);
            //parameter.Value = userid;
            //DataTable table = SqlHelper.ExecuteDataTable(SqlHelper.ConnectionString, CommandType.Text, cmdText, new SqlParameter[] { parameter });
            //if ((table == null) || (table.Rows.Count <= 0))
            //{
            //    return new List<string>();
            //}
            //List<string> list = new List<string>();
            //foreach (DataRow row in table.Rows)
            //{
            //    list.Add(row[0].ToString());
            //}
            //return list;
        }

        public static List<string> AllowListDetails(int userid, string sitename)
        {
            //string cmdText = "SELECT DetailName From NUser_AuthDetail(nolock) WHERE UserID=@UID AND SiteName=@sname AND AllowList='True'";
            //SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@UID", SqlDbType.Int), new SqlParameter("@sname", SqlDbType.NVarChar, 50) };
            //commandParameters[0].Value = userid;
            //commandParameters[1].Value = sitename;
            //DataTable table = SqlHelper.ExecuteDataTable(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
            //if ((table == null) || (table.Rows.Count <= 0))
            //{
            //    return new List<string>();
            //}
            //List<string> list = new List<string>();
            //foreach (DataRow row in table.Rows)
            //{
            //    list.Add(row[0].ToString().Replace("http://" + sitename, ".."));
            //}
            //return list;
            List<Trans.Db.Model.NUser_AuthDetail> Details = Trans.Db.Data.NUser_AuthDetail.GetList("UserId=@UserId AND SiteName=@SiteName AND AllowList=1", "ID ASC", new object[] { userid, sitename }, true);
            List<string> list = new List<string>();
            foreach (var item in Details)
            {
                list.Add(item.DetailName.Replace("http://" + sitename, ".."));
            }
            return list;
        }

        public static List<string> AllowListSite(int userid)
        {
            List<string> list = new List<string>();
            List<Trans.Db.Model.NUser_AuthSite> Sites = Trans.Db.Data.NUser_AuthSite.GetList("UserId=@UserId AND AllowList=1", "ID ASC", new object[] { userid }, true);            
            foreach (var item in Sites)
            {
                list.Add(item.SiteName);
            }
            List<Trans.Db.Model.NUser_AuthDetail> Details = Trans.Db.Data.NUser_AuthDetail.GetList<Trans.Db.Model.NUser_AuthDetail>("UserID=@UID AND AllowList=1", "ID ASC", "Distinct SiteName", new object[] { userid }, true);
            foreach (var item in Details)
            {
                if (!list.Contains(item.SiteName))
                {
                    list.Add(item.SiteName);
                }
            }
            return list;
            //string cmdText = "SELECT SiteName From NUser_AuthSite(nolock) WHERE UserID=@UID AND AllowList=1 union SELECT  Distinct SiteName FROM NUser_AuthDetail(nolock) WHERE UserID=@UID AND AllowList=1";
            //DataTable table = new Trans.Db.DBUtility.DBHelper().ExecTextDataTable(cmdText, new object[] { userid });
            //if ((table == null) || (table.Rows.Count <= 0))
            //{
            //    return new List<string>();
            //}
            //List<string> list = new List<string>();
            //foreach (DataRow row in table.Rows)
            //{
            //    list.Add(row[0].ToString());
            //}
            //return list;
        }
        /// <summary>
        /// 用户可以回滚的站点
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<string> AllowRollSite(int userid)
        {
            List<string> list = new List<string>();
            List<Trans.Db.Model.NUser_AuthSite> Sites = Trans.Db.Data.NUser_AuthSite.GetList("UserId=@UserId AND AllowRoll=1", "ID ASC", new object[] { userid }, true);
            foreach (var item in Sites)
            {
                list.Add(item.SiteName);
            }
            List<Trans.Db.Model.NUser_AuthDetail> Details = Trans.Db.Data.NUser_AuthDetail.GetList<Trans.Db.Model.NUser_AuthDetail>("UserID=@UID AND AllowRoll=1", "ID ASC", "Distinct SiteName", new object[] { userid }, true);
            foreach (var item in Details)
            {
                if (!list.Contains(item.SiteName))
                {
                    list.Add(item.SiteName);
                }
            }
            return list;
        }


        /// <summary>
        /// 获取所以用户可以操作的站点
        /// </summary>
        /// <returns></returns>
        public static List<string> AllSiteOfUser(int userid)
        {
            List<string> list = new List<string>();
            List<Trans.Db.Model.NUser_AuthSite> Sites = Trans.Db.Data.NUser_AuthSite.GetList("UserId=@UserId AND (AllowRoll=1 or AllowList=1 )", "ID ASC", new object[] { userid }, true);
            foreach (var item in Sites)
            {
                list.Add(item.SiteName);
            }
            return list;
        }


        #region 获取站点名称
        public static List<string> GetUserSiteNameList()
        {
            //string cmdText = "SELECT distinct siteName FROM NUser_AuthSite(nolock) ORDER BY siteName DESC";
            //DataTable table= SqlHelper.ExecuteDataTable(SqlHelper.ConnectionString, CommandType.Text, cmdText);
            DataTable table = Trans.Db.Data.NUser_AuthSite.GetTable("IsDel=0", "SiteName DESC", "DISTINCT SiteName", null, true);
            if ((table == null) || (table.Rows.Count <= 0))
            {
                return new List<string>();
            }
            List<string> list = new List<string>();
            foreach (DataRow item in table.Rows)
            {
                list.Add(item[0].ToString());
            }
            return list;
        }
        #endregion

        #region 查看用户是否可以回滚站点
        /// <summary>
        /// 查看用户是否可以回滚站点
        /// </summary>
        /// <param name="FunctionName"></param>
        /// <param name="SiteName"></param>
        /// <param name="AdminUserID"></param>
        /// <returns></returns>
        public static bool IsAllowed(string SiteName, int AdminUserID)
        {
            //string cmdText = "SELECT AllowRoll from NUser_AuthSite where SiteName=@SiteName AND UserID=@UserID";
            //SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@SiteName", SqlDbType.NVarChar, 50), new SqlParameter("@UserID", SqlDbType.Int) };
            //commandParameters[0].Value = SiteName;
            //commandParameters[1].Value = AdminUserID;
            //return Convert.ToBoolean(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters));
            return Trans.Db.Data.NUser_AuthSite.Count("SiteName=@SiteName AND UserId=@UserId AND AllowRoll=1", new object[] { SiteName, AdminUserID }, true) > 0;
        }
        #endregion
        #region 查看用户是否可以回滚站点
        /// <summary>
        /// 查看用户是否可以回滚站点
        /// </summary>
        /// <param name="FunctionName"></param>
        /// <param name="SiteName"></param>
        /// <param name="AdminUserID"></param>
        /// <returns></returns>
        public static bool IsAllowSync(string SiteName, int AdminUserID)
        {
            //string cmdText = "SELECT AllowRoll from NUser_AuthSite where SiteName=@SiteName AND UserID=@UserID";
            //SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@SiteName", SqlDbType.NVarChar, 50), new SqlParameter("@UserID", SqlDbType.Int) };
            //commandParameters[0].Value = SiteName;
            //commandParameters[1].Value = AdminUserID;
            //return Convert.ToBoolean(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters));
            return Trans.Db.Data.NUser_AuthSite.Count("SiteName=@SiteName AND UserId=@UserId AND AllowSync=1", new object[] { SiteName, AdminUserID }, true) > 0;
        }
        #endregion

        public static bool UpdUserDetail(int id, string listval, string syncval, string rollval)
        {
            //string cmdText = "UPDATE NUser_AuthDetail SET AllowList=@List,AllowSync=@Sync,AllowRoll=@Roll WHERE ID=@ID";
            //SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@List", SqlDbType.NVarChar, 10), new SqlParameter("@Sync", SqlDbType.NVarChar, 10), new SqlParameter("@Roll", SqlDbType.NVarChar, 10), new SqlParameter("@ID", SqlDbType.Int) };
            //commandParameters[0].Value = listval;
            //commandParameters[1].Value = syncval;
            //commandParameters[2].Value = rollval;
            //commandParameters[3].Value = id;
            //return (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters) == 1);
            return Trans.Db.Data.NUser_AuthDetail.Update("ID=@ID", "AllowList=@List, AllowSync=@Sync, AllowRoll=@Roll", new object[] { id, listval, syncval, rollval }) > 0;
        }

        public static bool UpdUserSite(int updid, int listval, int syncval, int rollval)
        {
            return Trans.Db.Data.NUser_AuthSite.Update("ID=@ID", "AllowList=@List,AllowSync=@Sync,AllowRoll=@Roll", new object[] { updid, listval, syncval, rollval }) > 0;
            //string cmdText = "UPDATE NUser_AuthSite SET AllowList=@List,AllowSync=@Sync,AllowRoll=@Roll WHERE ID=@ID";
            //SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@List", SqlDbType.NVarChar, 10), new SqlParameter("@Sync", SqlDbType.NVarChar, 10), new SqlParameter("@Roll", SqlDbType.NVarChar, 10), new SqlParameter("@ID", SqlDbType.Int) };
            //commandParameters[0].Value = listval;
            //commandParameters[1].Value = syncval;
            //commandParameters[2].Value = rollval;
            //commandParameters[3].Value = updid;
            //return (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters) == 1);
        }
    }
}