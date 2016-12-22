using Common.ConfigUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Trans.Web.Display
{
    public partial class Grant : BasePage
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        public static string UserList = "";
        /// <summary>
        /// 站点权限
        /// </summary>
        public static string SiteAuthor = "";
        /// <summary>
        /// 站点列表
        /// </summary>
        public static string  WebListText ="";
        /// <summary>
        /// 人员信息
        /// </summary>
        public static Trans.Db.Model.NUser_Info NUser_Info = new Db.Model.NUser_Info();
        /// <summary>
        /// 是否可访问
        /// </summary>
        public static bool CanAcce = true;
        public string SelectedItem = "";

        
        protected void Page_Load(object sender, EventArgs e)
        {
            //设置网站授权页面的访问权限
            CanAcce = true;
            if (OpUser != null && OpUser.UserId > 0)
            {
                //获取可访问的Email权限
                string AuthUser = System.Configuration.ConfigurationManager.AppSettings["AuthUser"];
                if (string.IsNullOrEmpty(AuthUser))
                {
                    CanAcce = false;
                    Response.Write("<script>alert('请配置用户')</script>");
                    return;
                }
                string[] EmailArry = AuthUser.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                string EmailStr = "";
                if (OpUser != null)
                {
                    EmailStr = OpUser.UserName.ToString();
                }
                if (string.IsNullOrEmpty(EmailStr) || !EmailArry.Contains(EmailStr))
                {
                    CanAcce = false;
                    Response.Write("<script>alert('无访问权限')</script>");
                    return;
                }
            }
            else
            {
                CanAcce = false;
                return;
            }
            //获得用户列表
            GetUserList();
            //获得站点以及文件夹列表
            WebSiteList();
        }
        #region 获得用户列表
        /// <summary>
        /// 获得用户列表
        /// </summary>
        private void GetUserList()
        {
            string  UserId = Request["UserId"];
            NUser_Info = Trans.Db.Data.NUser_Info.Get("UserId=@UserId","",new object[] { UserId },true);
        }
        #endregion

        #region 加载站点全部列表
        private void WebSiteList()
        {
            string UserId = Request["UserId"];
            List<string> WebList = Common.ConfigUtility.Config.GetSiteList();
            StringBuilder Sb = new StringBuilder();
            if (WebList != null && WebList.Count > 0)
            {
                List<Trans.Db.Model.NUser_AuthSite> NUserSiteList = Trans.Db.Data.NUser_AuthSite.GetList("UserID=@UserID and isdel=0", "", new object[] { UserId });
                foreach (var item in WebList)
                {
                    Trans.Db.Model.NUser_AuthSite SiteInfo = new Db.Model.NUser_AuthSite();
                    SiteInfo = NUserSiteList.Find(X => X.SiteName == item);
                    Sb.AppendFormat("<tr ><td >{0}</ td >", item);
                    
                        if (SiteInfo != null&&SiteInfo.AllowList == 1)
                        {
                            Sb.Append(@" <td ><input  type='checkbox' value = '0'  checked='checked' /></ td >");
                        }
                        else
                        {
                            Sb.Append(@" <td ><input  type='checkbox' value = '0'  /></ td >");
                        }
                        if (SiteInfo != null && SiteInfo.AllowSync == 1)
                        {
                            Sb.Append(@"<td ><input  type = 'checkbox' value = '0'  checked='checked' /></ td >");
                        }
                        else
                        {
                            Sb.Append(@"<td ><input  type = 'checkbox' value = '0'   /></ td >");
                        }
                        if (SiteInfo != null && SiteInfo.AllowRoll == 1)
                        {
                            Sb.Append(@" <td ><input  type = 'checkbox' value = '0'  checked= 'checked' /></ td >");
                        }
                        else
                        {
                            Sb.Append(@" <td ><input  type = 'checkbox' value = '0'  /></ td >");
                        }
                        
                    Sb.Append(@"<td ><input type = 'button' value = '确认' onclick = 'Save(this);' /></ td ></tr> ");
                }
                WebListText = Sb.ToString();
            }
        }

        #endregion

        #region 获得站点以及文件夹列表
        /// <summary>
        /// 获得站点
        /// </summary>
        private void GetWebSiteList()
        {
            List<string> SitesList = Common.FileUtility.PopedomCmd.GetUserSiteNameList();
            StringBuilder sb = new StringBuilder();
            if (SitesList != null && SitesList.Count > 0)
            {
                //加载节点
                foreach (var item in SitesList)
                {
                    if (Common.ConfigUtility.Config.GetSiteList().Contains(item))
                    {
                        string FolderStr = GetFolderOfSite(item);
                        //获取站点基本信息
                        WebSiteInfo TaskSiteBaseInfo = Common.ConfigUtility.Config.GetSiteInfo(item);
                        //站点根目录
                        //string BaseDir = TaskSiteBaseInfo.LOCALPATH;
                        //string CurrDir = BaseDir;
                        //BaseDir = BaseDir.Replace("\\", "/");
                        ////当前目录标准化，将相对路径转换成绝对路径
                        //CurrDir = CurrDir.Replace("\\", "/").Replace(BaseDir, "..") + "/";
                        if (string.IsNullOrEmpty(FolderStr))
                        {
                            sb.Append("{\"id\":\";;" + item + "\",\"text\":\"" + item + "\",\"children\":true},");
                        }
                        else
                        {
                            sb.Append("{\"id\":\";;" + item + "\",\"text\":\"" + item + "\",\"children\":[" + FolderStr + "]},");
                            // sb.Append("{\"id\":\"" +CurrDir+";;" + item + "\"},\"text\":\"" + item + "\",\"children\":[" + FolderStr + "]},");
                        }
                    }
                }
                SiteAuthor = "{\"id\":\"-1\",\"text\":\"所有站点\",\"children\":[" + sb.ToString().TrimEnd(',') + "]}";

                //设置默认选中
                var result = Trans.Db.Data.NUser_AuthDetail.GetList();
                if (result != null && result.Count > 0)
                {
                    List<string> SelectedNode = new List<string>();
                    foreach (var item in result)
                    {
                        SelectedNode.Add(item.DetailName + ";;" + item.SiteName + ";;" + item.IsDir);
                    }
                    SelectedItem = string.Join(",", SelectedNode);
                }
            }
        }
        /// <summary>
        /// 获得每个站点下的文件夹
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        private string GetFolderOfSite(string site = "")
        {
            string foldeStr = "";
            //获取站点基本信息
            WebSiteInfo TaskSiteBaseInfo = Common.ConfigUtility.Config.GetSiteInfo(site);
            //站点根目录
            string BaseDir = TaskSiteBaseInfo.LOCALPATH;
            string CurrDir = BaseDir;
            //当前目录标准化，将相对路径转换成绝对路径
            CurrDir = CurrDir.Replace("..", BaseDir);
            BaseDir = BaseDir.Replace("\\", "/");
            //获取和添加子文件夹
            foldeStr = GetSubDirectoryList(CurrDir, BaseDir, site);
            if (foldeStr.ToUpper().Contains("BAK_") || foldeStr.ToUpper().Contains("BACK_"))
            {
                return "";
            }
            return foldeStr;
        }
        /// <summary>
        /// 获得站点下的文件夹
        /// </summary>
        /// <param name="RootDirectoryPath"></param> 
        private string GetSubDirectoryList(string RootDirectoryPath, string BaseDir, string SiteName)
        {
            DirectoryInfo RootDI = new DirectoryInfo(RootDirectoryPath);
            DirectoryInfo[] DD = RootDI.GetDirectories();
            StringBuilder sb = new StringBuilder();
            //json格式 [{"id":1,"text":"Root node","children":[{"id":2,"text":"Child node 1","children":true},{"id":3,"text":"Child node 2"}]}]
            //确定目录路径
            string DyOutPath = RootDirectoryPath.Replace("\\", "/").Replace(BaseDir, "..");
            foreach (DirectoryInfo SubDI in RootDI.GetDirectories())
            {
                if (SubDI.Name.Contains("BAK_") || SubDI.Name.Contains("BACK_"))
                {
                    continue;
                }
                //子文件夹绝对路径转相对路径
                string DyInPath = SubDI.FullName.Replace("\\", "/").Replace(BaseDir, "..") + "/";
                string subStr = GetSubDirectoryList(SubDI.FullName, BaseDir, SiteName);
                if (string.IsNullOrEmpty(subStr))
                {
                    //站点,文件目录,是否是根目录以“;;”分隔
                    sb.Append("{\"id\":\"" + DyInPath + ";;" + SiteName + ";;true\",\"text\":\"" + SubDI.Name + "\",\"children\":true},");
                }
                else
                {
                    sb.Append("{\"id\":\"" + DyInPath + ";;" + SiteName + ";;true\",\"text\":\"" + SubDI.Name + "\",\"children\":[" + subStr + "]},");
                }
            }
            //获取和添加子文件
            string FileJson = GetSubFileList(RootDirectoryPath, BaseDir, SiteName);
            string SiteJson = sb.ToString().TrimEnd(',') + "," + FileJson;
            return SiteJson;
        }
        /// <summary>
        /// 获取站点下的子文件Json格式
        /// </summary>
        /// <param name="DirectoryPath"></param>
        private string GetSubFileList(string DirectoryPath, string BaseDir, string SiteName)
        {
            StringBuilder sb = new StringBuilder();
            DirectoryInfo ParentDI = new DirectoryInfo(DirectoryPath);
            FileInfo[] Files = ParentDI.GetFiles();
            foreach (var SubFile in Files)
            {
                //站点,文件目录,是否是根目录以“;;”分隔
                string FilePath = SubFile.FullName.Replace("\\", "/").Replace(BaseDir, "..") + "/";
                sb.Append("{\"id\":\"" + FilePath + ";;" + SiteName + ";;false\",\"text\":\"" + SubFile.Name + "\",\"icon\" : \"jstree-file\"},");
            }
            return sb.ToString().TrimEnd(',');
        }
        #endregion
    }
}