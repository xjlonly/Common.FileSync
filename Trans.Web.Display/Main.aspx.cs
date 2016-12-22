using System;
using System.Web;
using Common.ConfigUtility;
using Common.FileUtility;
using Common.LogUtility;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Trans.Web.Display
{
    public partial class Main : BasePage
    {
        /// <summary>
        /// 页面部分数组
        /// </summary>
        private string[] PagePartArray = new string[8];
        /// <summary>
        /// 目标站点
        /// </summary>
        private Common.ConfigUtility.TaskInfo NewTask = null;
        /// <summary>
        /// 目标站点基本信息
        /// </summary>
        private WebSiteInfo TaskSiteBaseInfo = null;
        /// <summary>
        /// 文件列表
        /// </summary>
        private string FileTabStr = string.Empty;
        /// <summary>
        /// 当前文件夹
        /// </summary>
        protected string CurrDir = string.Empty;
        /// <summary>
        /// 站点根目录
        /// </summary>
        protected string BaseDir = string.Empty;
        /// <summary>
        /// 当前目录路径
        /// 格式:../Sub1/Sub2/Sub3
        /// </summary>
        private string UpDir = string.Empty;
        /// <summary>
        /// 已选择的文件页面代码
        /// </summary>
        private string SelFiles = string.Empty;
        /// <summary>
        /// 已选择文件列表
        /// </summary>
        private List<string> SelFileList = new List<string>();
        /// <summary>
        /// 已选择待删除文件页面源代码
        /// </summary>
        private string DelFiles = string.Empty;
        /// <summary>
        /// 已选择待删除文件列表
        /// </summary>
        private List<string> DelFileList = new List<string>();

        private bool AllowSync = false;
        /// <summary>
        /// 备选文件列表页面代码
        /// </summary>
        protected string ForSel = string.Empty;
        protected string Seled = string.Empty;
        protected string CanUseSiteList = string.Empty;
        private string SelSite = string.Empty;
        private bool IsPost = false;
        protected string CurrDirStr = string.Empty;
        private bool SelSiteChanged = false;
        /// <summary>
        /// 操作类型
        /// </summary>
        private string OpType = string.Empty;
        /// <summary>
        /// 用户ID
        /// </summary>
        private int UID = -1;

        public string OutPage = "";

        private DateTime TimeFilter = new DateTime(1900, 1, 1, 0, 0, 0);
        #region 页面入口
        /// <summary>
        /// 页面入口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //单点登陆
            if (OpUser != null && OpUser.UserId > 0)
            {
                UID = Convert.ToInt32(OpUser.UserId);
            }
            if (UID == -1)
            {
                HttpContext.Current.Response.Write("<script>alert('无同步权限');</script>");
                return;
            }

            //获取页面参数信息
            this.GetParameters();
            //页面Ftp同步
            this.FileFtpOp();
            //控制页面运行
            this.Work();
        }
        #endregion

        #region 消息脚本 PART——1
        /// <summary>
        /// 产生消息通知脚本 PART——1
        /// 位置：页面代码第一段
        /// 在页面加载段进行运行，防止页面重复提交
        /// </summary>
        /// <param name="MsgContent">消息体，消息提示的内容</param>
        private void Pre_MsgScript(string MsgContent)
        {
            PagePartArray[0] = "<script>alert('" + MsgContent + "');</script>\r\n";
        }
        #endregion
        #region 页面基本组成部分 PART——2
        /// <summary>
        /// 页面基本组成部分 PART——2
        /// 页面基本信息，页面头部，页面主体部分开头，如:body
        /// </summary>
        private void Pre_BasePage()
        {
            //修正代码产生方式：string 相加改 StringBuilder 时间：090709 11：19
            StringBuilder txt = new StringBuilder();
            //txt.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">");
            //txt.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            //txt.AppendLine("<head>");
            //txt.AppendLine("<title>文件同步管理</title>");
            //txt.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />");
            //txt.AppendLine("</head>");
            //txt.AppendLine("<body style='font-size:12px;font-famimly:arial;'>\r\n");
            ////txt.AppendLine("<%@ Page Language=\"C#\" MasterPageFile=\"~/test.Master\"  AutoEventWireup=\"true\" CodeBehind=\"Main.aspx.cs\" Inherits=\"Trans.Web.Display.Main\" %>");
            //txt.AppendLine(" <% Page Language=\"C#\" MasterPageFile=\"~/test.Master\"  AutoEventWireup=\"true\" CodeBehind=\"Main.aspx.cs\" Inherits=\"Trans.Web.Display.Main\" %> <asp:Content ID='Content1' ContentPlaceHolderID='ContentPlaceHolder1' runat='server'>");

            txt.AppendLine("<form id=\"form1\" name=\"form1\" method=\"post\" onsubmit=\"return false;\">");
            txt.AppendLine("<table width=\"100%\" border='1' cellpadding='1' cellspacing='1'>\r\n<caption>文件同步管理</caption>");

            PagePartArray[1] += txt.ToString();
        }
        #endregion
        #region 页面主体A 站点列表
        /// <summary>
        /// 页面主体A 站点列表
        /// </summary>
        private void Pre_SiteList()
        {
            //可选站点列表为空或已选站点为空
            if (string.IsNullOrEmpty(SelSite) || SelSite == "-1")
            {
                StringBuilder txt = new StringBuilder();
                txt.AppendLine("<tr>\r\n<td>\r\n可选站点<select id='SiteList' name='SiteList' onchange=\"javascript:SelSite()\">\r\n<option value='-1'>---请选择---</option>");
                //可选站点列表
                List<string> SiteList = Common.FileUtility.PopedomCmd.AllowListSite(this.UID);//授予操作权限的
                List<string> WebList = Common.ConfigUtility.Config.GetSiteList();//XML配置的

                foreach (string SL in SiteList)
                {
                    if (WebList.Contains(SL))
                    {
                        txt.AppendLine("<option value='" + SL + "'>" + SL + "</option>");
                    }
                }
                txt.AppendLine("</select>\r\n</td>\r\n</tr>");
                PagePartArray[2] = txt.ToString();
            }
            else//已选站点不为空
            {
                StringBuilder txt = new StringBuilder();
                txt.AppendLine("<tr>\r\n<td>\r\n可选站点<select id='SiteList' name='SiteList' onchange=\"javascript:SelSite()\">\r\n<option value='-1'>---请选择---</option>");
                //可选站点列表
                List<string> SiteList = Common.FileUtility.PopedomCmd.AllowListSite(this.UID);//授予操作权限的
                List<string> WebList = Common.ConfigUtility.Config.GetSiteList();//XML配置的

                foreach (string SL in SiteList)
                {
                    if (WebList.Contains(SL))
                    {
                        if (SL != SelSite)
                        {
                            txt.AppendLine("<option value='" + SL + "'>" + SL + "</option>");
                        }
                        else
                        {
                            txt.AppendLine("<option selected='selected' value='" + SL + "'>" + SL + "</option>");
                        }
                    }
                }
                txt.AppendLine("</select>过滤时间<input type='text' id='TimeFilter' name='TimeFilter' value='" + (TimeFilter == new DateTime(1900, 1, 1, 0, 0, 0) ? "" : TimeFilter.ToString("yyyy-MM-dd HH:mm:ss")) + "'/></td>\r\n</tr>");
                PagePartArray[2] = txt.ToString();
            }
        }
        #endregion
        #region 页面主体B 可选文件
        /// <summary>
        /// 页面主体B 可选文件
        /// </summary>
        private void Pre_ForSelFiles()
        {
            if (SelSite != "-1" && !string.IsNullOrEmpty(SelSite))
            {
                //获取站点基本信息
                TaskSiteBaseInfo = Common.ConfigUtility.Config.GetSiteInfo(SelSite);
                //站点根目录
                BaseDir = TaskSiteBaseInfo.LOCALPATH;
                //当前目录为空，或站点改变，则重新定义当前目录为站点根目录
                if (string.IsNullOrEmpty(CurrDir) || SelSiteChanged)
                {
                    CurrDir = BaseDir;
                }
                //当前目录标准化，将相对路径转换成绝对路径
                CurrDir = CurrDir.Replace("..", BaseDir);
                //当前目录信息保存到页面，保存信息为相对路径
                this.CurrDirStr = "<input type=\"hidden\"  id=\"CurrDir\" name=\"CurrDir\" value=\"" + HttpUtility.UrlEncode(CurrDir.Replace('\\', '/').Replace(BaseDir, "..")) + "\"/>\r\n";

                //获取和添加子文件夹
                GetSubDirectoryList(CurrDir);
                //获取和添加子文件
                GetSubFileList(CurrDir);

                //添加可选文件列表
                //当前目录为根目录
                if (string.IsNullOrEmpty(UpDir))
                {
                    this.ForSel =
                        "<table width=\"100%\" cellspacing='0' cellpadding='0' border='1'>\r\n" +
                        "<caption>\r\n<font color='red'>可添加文件</font>\r\n</caption>\r\n" +
                        "<tr>\r\n<td colspan='3'>\r\n<a href='#' onclick=\"javascript:DirIn('..')\">当前路径：根目录</a>\r\n" +
                        "<tr>\r\n<td>文件名</td>\r\n<td>文件路径</td>\r\n<td>上次修改时间</td>\r\n</tr>\r\n" + this.FileTabStr +
                        "</table>\r\n";
                }
                else
                {//当前目录为某子目录
                    this.ForSel =
                        "<table width=\"100%\" cellspacing='0' cellpadding='0' border='1'>\r\n" +
                        "<caption>\r\n<font color='red'>可添加文件</font></caption>\r\n<tr>\r\n<td colspan='3'>\r\n当前路径：";

                    //分割父级目录路径为目录数组
                    string[] DirArray = UpDir.Split('/');
                    string CacheDir = string.Empty;
                    for (int i = 0; i < DirArray.Length; i++)
                    {
                        //第一项是根目录
                        //目录路径格式:../Sub1/Sub2/Sub3
                        if (string.IsNullOrEmpty(CacheDir))
                        {
                            CacheDir = DirArray[i];
                        }
                        else
                        {
                            CacheDir += "/" + DirArray[i];
                        }
                        //第一项是根目录
                        if (CacheDir == "..")
                        {
                            this.ForSel += "<a href='#' onclick=\"javascript:DirIn('" + CacheDir + "')\">根目录</a>/";
                        }
                        else
                        {
                            //如果目录已经是最后一级（当前目录），则不提供进入链接，只显示名字
                            if (i == DirArray.Length - 1)
                            {
                                this.ForSel += DirArray[i];
                            }
                            else
                            {
                                this.ForSel += "<a href='#' onclick=\"javascript:DirIn('" + CacheDir + "')\">" + DirArray[i] + "</a>/";
                            }
                        }
                    }
                    //循环处理目录路径会在最后多余一个/
                    this.ForSel = this.ForSel.TrimEnd('/');
                    //为可选列表提供列头
                    this.ForSel += "<tr>\r\n<td>\r\n文件名\r\n</td>\r\n<td>\r\n文件路径\r\n</td>\r\n<td>\r\n上次修改时间\r\n</td>\r\n</tr>\r\n" + this.FileTabStr +
                        "</table>\r\n";
                }

                //页面操作按钮
                PagePartArray[3] = "<tr>\r\n<td>\r\n" + "<input id=\"Add_Btn\" name=\"Add_Btn\" type=\"button\" value=\"添加\"  onclick=\"Add()\" title=\"添加到上传列表\"/>  |  " +
                    "<input id=\"All_Btn\" name=\"All_Btn\" type=\"button\" value=\"全选\" onclick=\"AddAll()\" title=\"全部选择\"/>  |  " +
                    "<input id=\"No_Btn\" name=\"No_Btn\" type=\"button\" value=\"全取消\" onclick=\"CLRAll()\" title=\"全部取消\"/>  |  " +
                    "<input id=\"REA_Btn\" name=\"REA_Btn\" type=\"button\" value=\"反选\" onclick=\"REAAll()\" title=\"反向选择\"/>" +
                    "\r\n</td>\r\n</tr><tr><td>\r\n" + this.ForSel +
                    "<input id=\"Add_Btn\" name=\"Add_Btn\" type=\"button\" value=\"添加\"  onclick=\"Add()\" title=\"添加到上传列表\"/>  |  " +
                    "<input id=\"All_Btn\" name=\"All_Btn\" type=\"button\" value=\"全选\" onclick=\"AddAll()\" title=\"全部选择\"/>  |  " +
                    "<input id=\"No_Btn\" name=\"No_Btn\" type=\"button\" value=\"全取消\" onclick=\"CLRAll()\" title=\"全部取消\"/>  |  " +
                    "<input id=\"REA_Btn\" name=\"REA_Btn\" type=\"button\" value=\"反选\" onclick=\"REAAll()\" title=\"反向选择\"/>" +
                    "\r\n</td>\r\n</tr>\r\n";
            }
        }
        #endregion
        #region 页面主体C 已选文件
        /// <summary>
        /// 页面主体C 已选文件
        /// </summary>
        private void Pre_SeledFiles()
        {
            //当前站点已选文件列表不为空
            if (SelFileList.Count > 0 && !SelSiteChanged)
            {
                this.Seled +=
                    "<table width=\"100%\" cellspacing='0' cellpadding='0' border='1'id=\"tblSort\">\r\n<caption><font color='red'>上传队列</font></caption>\r\n<thead><tr>\r\n<th  style=\"cursor:pointer\"  onclick=\"sortTable('tblSort',0,'int')\" width='4%'>\r\n序号\r\n</th>\r\n<th width='16%'>\r\n站点\r\n</th>\r\n<th width='30%'>\r\n文件\r\n</th>\r\n<th width='30%'>\r\n路径\r\n</th>\r\n<th width='20%'>\r\n操作\r\n</th>\r\n</tr></thead><tbody>\r\n";
                //为显示方便，提供流水号
                int i = 1;
                foreach (string AddFile in SelFileList)
                {
                    this.Seled += "<tr  onmouseover=\"this.style.background='#ccc'\" onmouseout=\"this.style.background='#fff'\">\r\n" +
                                  "<td><input id='ipt_" + i +
                                  "' onfocus=\"this.select();\" onblur=\"if(! /^\\-?([1-9]\\d*|0)(\\.\\d+)?$/.test(this.value)){alert('排序请输入整数');this.focus(); return false ;}sortTable('tblSort',0,'int')\" type='text' value='" +
                                  i.ToString() + "' style='width:95%;'/></td>\r\n" +
                                  "<td style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + SelSite +
                                  "\r\n</td>\r\n" +
                                  "<td style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" +
                                  AddFile.Substring(AddFile.LastIndexOf('/') + 1) + "\r\n</td>\r\n" +
                                  "<td style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" +
                                  AddFile.Replace("../", "/") + "\r\n</td>\r\n" +
                                  "<td>\r\n<a href='#' onclick=\"javascript:DelFile('" + AddFile +
                                  "')\">从队列删除</a> <input type='checkbox' name='delfilelist' value='" + AddFile + "'/>\r\n<input type='hidden' id='" + AddFile + "' name='AddFileBox' value='" + SelSite +
                                  "|" + AddFile + "'/>\r\n</td>\r\n" +
                                  "</tr>\r\n";
                    i++;
                }
                this.Seled += "</tbody></table>";

                PagePartArray[4] += "<tr>\r\n<td>\r\n" + this.Seled + "\r\n</td>\r\n</tr>\r\n";
                //同步操作提交按钮
                PagePartArray[4] +=
                    "<tr>\r\n<td>\r\n<input id=\"ConfirmPost_Btn\" name=\"Post_Btn\" type=\"button\" value=\"确认上传\" onclick=\"ConfirmPost()\" />\r\n";
                PagePartArray[4] +=
                    "<input id=\"TrunSelFiles_Btn\" name=\"TrunSelFiles_Btn\" type=\"button\" value=\"清空已选文件列表\" onclick=\"ConfirmTrun()\" /> <input id=\"TrunSelFiles_Btn\" name=\"TrunSelFiles_Btn\" type=\"button\" value=\"删除选中文件\" onclick=\"ConfirmDelList()\" />\r\n</td>\r\n</tr>\r\n</table>\r\n";
            }
            else
            {
                PagePartArray[4] += "</table>\r\n";
            }
        }
        #endregion
        #region 页面主体D 隐藏变量
        /// <summary>
        /// 页面主体D 隐藏变量
        /// </summary>
        private void Pre_HiddenParams()
        {
            StringBuilder txt = new StringBuilder();
            txt.AppendLine("<input type=\"hidden\" value=\"\" id=\"SelFiles\" name=\"SelFiles\" />");
            txt.AppendLine("<input type=\"hidden\" value=\"\" id=\"DelFiles\" name=\"DelFiles\" />");
            txt.AppendLine("<input type=\"hidden\" value=\"\" id=\"SelSiteChanged\" name=\"SelSiteChanged\" />");
            txt.AppendLine(this.CurrDirStr);
            txt.AppendLine("<input type=\"hidden\" value=\"false\" id=\"IsPost\" name=\"IsPost\" />");
            txt.AppendLine("<input type=\"hidden\" value=\"0\" id=\"TrunMark\" name=\"TrunMark\" />");
            txt.AppendLine("<input type=\"hidden\" value=\"\" id=\"OpType\" name=\"OpType\" />");
            txt.AppendLine("</form>");
            PagePartArray[5] += txt.ToString();
        }
        #endregion
        #region 页面主体E 页面脚本
        private void Pre_PageScript()
        {
            StringBuilder txt = new StringBuilder();
            txt.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
            if (!Common.FileUtility.PopedomCmd.IsAllowSync(SelSite, OpUser.UserId))
            {
                txt.AppendLine("window.onload = function () {");
                txt.AppendLine("var objList = document.getElementsByTagName('input');");
                txt.AppendLine("for (var i = 0; i < objList.length; i++) {");
                txt.AppendLine("objList[i].disabled = true;");
                txt.AppendLine("};");
                txt.AppendLine("document.getElementById('SelFiles').disabled = false;");
                txt.AppendLine("document.getElementById('DelFiles').disabled = false;");
                txt.AppendLine("document.getElementById('CurrDir').disabled = false;");
                txt.AppendLine("document.getElementById('SelSiteChanged').disabled = false;");
                txt.AppendLine("document.getElementById('IsPost').disabled = false;");
                txt.AppendLine("document.getElementById('TrunMark').disabled = false;");
                txt.AppendLine("document.getElementById('OpType').disabled = false;");
                txt.AppendLine("};");

            }
            txt.AppendLine("function ConfirmPost()");
            txt.AppendLine("{");
            txt.AppendLine("    var txt=\"\"");
            txt.AppendLine("    var SelArray=new Array();");
            txt.AppendLine("    SelArray=document.getElementsByName(\"AddFileBox\");");
            txt.AppendLine("    var side=SelArray.length;");
            txt.AppendLine("    if(side>=10)");
            txt.AppendLine("    {");
            txt.AppendLine("        side=10;");
            txt.AppendLine("    }");
            txt.AppendLine("    for(var i=0;i<side;i++)");
            txt.AppendLine("    {");
            txt.AppendLine("        txt=txt+SelArray[i].value+\"\\r\\n\";");
            txt.AppendLine("    }");
            txt.AppendLine("    if(SelArray.length<=0)");
            txt.AppendLine("    {");
            txt.AppendLine("    alert(\"上传列表为空.\");");
            txt.AppendLine("    return false;");
            txt.AppendLine("    }");
            txt.AppendLine("    else{");
            txt.AppendLine("    alert(\"文件列表:\\r\\n\"+txt+\"...\\r\\n文件总数:\"+SelArray.length);");
            txt.AppendLine("    }");
            txt.AppendLine("    if(confirm(\"确认上传?\"))");
            txt.AppendLine("    {");
            txt.AppendLine("        document.getElementById(\"OpType\").value=\"POST\";");
            txt.AppendLine("        document.getElementById(\"IsPost\").value=\"true\";");
            txt.AppendLine("        document.form1.submit();");
            txt.AppendLine("    }");
            txt.AppendLine("}");
            txt.AppendLine("function ConfirmDelList()");
            txt.AppendLine("{");
            txt.AppendLine("	var objs= document.getElementsByName('delfilelist');");
            txt.AppendLine("	var DelFiles=document.getElementById(\"DelFiles\");");
            txt.AppendLine("	DelFiles.value='';");
            txt.AppendLine("for(i=0;i<objs.length;i++)");
            txt.AppendLine("{");
            txt.AppendLine("	if(objs[i].checked)");
            txt.AppendLine("		DelFiles.value=DelFiles.value+'|'+objs[i].value;");
            txt.AppendLine("}");
            txt.AppendLine("    document.getElementById(\"OpType\").value=\"\";");
            txt.AppendLine("	if(DelFiles.value=='')");
            txt.AppendLine("		alert('没有选中文件');");
            txt.AppendLine("	else");
            txt.AppendLine("		document.form1.submit();");
            txt.AppendLine("}");
            txt.AppendLine("function ConfirmDel()");
            txt.AppendLine("{");
            //txt.AppendLine("    var txt=\"\"");
            //txt.AppendLine("    var SelArray=new Array();");
            //txt.AppendLine("    SelArray=document.getElementsByName(\"AddFileBox\");");
            //txt.AppendLine("    var side=SelArray.length;");
            //txt.AppendLine("    if(side>=10)");
            //txt.AppendLine("    {");
            //txt.AppendLine("        side=10;");
            //txt.AppendLine("    }");
            //txt.AppendLine("    for(var i=0;i<side;i++)");
            //txt.AppendLine("    {");
            //txt.AppendLine("        txt=SelArray[i].value+\"\\r\\n\"+txt;");
            //txt.AppendLine("    }");
            //txt.AppendLine("    alert(\"文件列表:\\r\\n\"+txt+\"...\\r\\n文件总数:\"+SelArray.length);");
            //txt.AppendLine("    if(confirm(\"确认删除?\"))");
            //txt.AppendLine("    {");
            //txt.AppendLine("        document.getElementById(\"OpType\").value=\"DEL\";");
            //txt.AppendLine("        document.getElementById(\"IsPost\").value=\"true\";");
            //txt.AppendLine("        document.form1.submit();");
            //txt.AppendLine("    }");
            txt.AppendLine("}");
            txt.AppendLine("function SelSite()");
            txt.AppendLine("{");
            txt.AppendLine("    var selSite=document.getElementById(\"SiteList\");");
            txt.AppendLine("    if(selSite.value!=\"-1\")");
            txt.AppendLine("    {");
            txt.AppendLine("        var selSiteChange=document.getElementById(\"SelSiteChanged\");");
            txt.AppendLine("        selSiteChange.value=\"true\";");
            txt.AppendLine("        document.form1.submit();");
            txt.AppendLine("    }");
            txt.AppendLine("}");
            txt.AppendLine("function Add()");
            txt.AppendLine("{");
            txt.AppendLine("    var PreSelArray=new Array();");
            txt.AppendLine("    PreSelArray=document.getElementsByName(\"PreSelFileBox\");");
            txt.AppendLine("    var SelFiles=document.getElementById(\"SelFiles\");");
            txt.AppendLine("    for(var index=0;index<PreSelArray.length;index++)");
            txt.AppendLine("    {");
            txt.AppendLine("        if(PreSelArray[index].checked)");
            txt.AppendLine("        {");
            txt.AppendLine("            if(SelFiles.value==\"\")");
            txt.AppendLine("            {");
            txt.AppendLine("                SelFiles.value=PreSelArray[index].value;");
            txt.AppendLine("            }");
            txt.AppendLine("            else");
            txt.AppendLine("            {");
            txt.AppendLine("                SelFiles.value=SelFiles.value+\"|\"+PreSelArray[index].value;");
            txt.AppendLine("            }");
            txt.AppendLine("        }");
            txt.AppendLine("    }");
            txt.AppendLine("    document.form1.submit();");
            txt.AppendLine("}");
            txt.AppendLine("function DelFile(f)");
            txt.AppendLine("{");
            txt.AppendLine("    document.getElementById(\"OpType\").value=\"\";");
            txt.AppendLine("    var DelFiles=document.getElementById(\"DelFiles\");");
            txt.AppendLine("    DelFiles.value=f;");
            txt.AppendLine("    document.form1.submit();");
            txt.AppendLine("}");
            txt.AppendLine("function DirIn(d)");
            txt.AppendLine("{");
            txt.AppendLine("    var Curr=document.getElementById(\"CurrDir\");");
            txt.AppendLine("    Curr.value=encodeURIComponent(d);");
            txt.AppendLine("    document.form1.submit();");
            txt.AppendLine("}");
            txt.AppendLine("function ConfirmTrun()");
            txt.AppendLine("{");
            txt.AppendLine("    if(confirm(\"确认清空?\"))");
            txt.AppendLine("    {");
            txt.AppendLine("        document.getElementById(\"TrunMark\").value=\"1\";");
            txt.AppendLine("        document.form1.submit();");
            txt.AppendLine("    }");
            txt.AppendLine("}");
            txt.AppendLine("function AddAll()");
            txt.AppendLine("{");
            txt.AppendLine("   var forSelArray=new Array();");
            txt.AppendLine("   forSelArray=document.getElementsByName(\"PreSelFileBox\");");
            txt.AppendLine("   for(var i=0;i<forSelArray.length;i++)");
            txt.AppendLine("   {");
            txt.AppendLine("       forSelArray[i].checked=\"checked\";");
            txt.AppendLine("   }");
            txt.AppendLine("}");
            txt.AppendLine("function CLRAll()");
            txt.AppendLine("{");
            txt.AppendLine("   var forClrArray=new Array();");
            txt.AppendLine("   forClrArray=document.getElementsByName(\"PreSelFileBox\");");
            txt.AppendLine("   for(var j=0;j<forClrArray.length;j++)");
            txt.AppendLine("   {");
            txt.AppendLine("       forClrArray[j].checked=\"\";");
            txt.AppendLine("   }");
            txt.AppendLine("}");
            txt.AppendLine("function REAAll()");
            txt.AppendLine("{");
            txt.AppendLine("   var forClrArray=new Array();");
            txt.AppendLine("   forClrArray=document.getElementsByName(\"PreSelFileBox\");");
            txt.AppendLine("   for(var k=0;k<forClrArray.length;k++)");
            txt.AppendLine("   {");
            txt.AppendLine("       if(forClrArray[k].checked)");
            txt.AppendLine("       {");
            txt.AppendLine("           forClrArray[k].checked=\"\";");
            txt.AppendLine("       }");
            txt.AppendLine("       else");
            txt.AppendLine("       {");
            txt.AppendLine("           forClrArray[k].checked=\"checked\";");
            txt.AppendLine("       }");
            txt.AppendLine("   }");
            txt.AppendLine("}");

            txt.AppendLine("	function sortTable(sTableId,iCol,sDataType){	");
            txt.AppendLine("	var oTable=document.getElementById(sTableId);//获取表格的ID 	");
            txt.AppendLine("	var oTbody=oTable.tBodies[0]; //获取表格的tbody	");
            txt.AppendLine("	var colDataRows=oTbody.rows; //获取tbody里的所有行的引用	");
            txt.AppendLine("	var aTRs=new Array; //定义aTRs数组用于存放tbody里的行	");
            txt.AppendLine("	for(var i=0;i<colDataRows.length;i++){//依次把所有行放如aTRs数组/	");
            txt.AppendLine("	aTRs.push(colDataRows[i]);	");
            txt.AppendLine("	}	");
            txt.AppendLine("	/***********************************************************************	");
            txt.AppendLine("	sortCol属性是额外给table添加的属性，用于作顺反两种顺序排序时的判断，区分	");
            txt.AppendLine("	首次排序和后面的有序反转	");
            txt.AppendLine("	************************************************************************/	");
            txt.AppendLine("	aTRs.sort(generateCompareTRs(iCol,sDataType));	");
            txt.AppendLine("	var oFragment=document.createDocumentFragment();//创建文档碎片/	");
            txt.AppendLine("	for(var i=0;i<aTRs.length;i++){ //把排序过的aTRs数组成员依次添加到文档碎片	");
            txt.AppendLine("	oFragment.appendChild(aTRs[i]);	");
            txt.AppendLine("	}	");
            txt.AppendLine("	oTbody.appendChild(oFragment); //把文档碎片添加到tbody,完成排序后的显示更新 	");
            txt.AppendLine("	oTable.sortCol=iCol;//把当前列号赋值给sortCol,以此来区分首次排序和非首次排序,//sortCol的默认值为-1	");
            txt.AppendLine("		");
            txt.AppendLine("	};	");
            txt.AppendLine("		");
            txt.AppendLine("	//比较函数，用于两项之间的排序	");
            txt.AppendLine("	function generateCompareTRs(iCol,sDataType){	");
            txt.AppendLine("	return function compareTRs(oTR1,oTR2){");
            txt.AppendLine("	var vValue1=convert(oTR1.cells[iCol].firstChild.value,sDataType);	");
            txt.AppendLine("	var vValue2=convert(oTR2.cells[iCol].firstChild.value,sDataType);	");
            txt.AppendLine("	if(vValue1<vValue2){	");
            txt.AppendLine("	return -1;	");
            txt.AppendLine("	}	");
            txt.AppendLine("	else if(vValue1>vValue2){	");
            txt.AppendLine("	return 1;	");
            txt.AppendLine("	}	");
            txt.AppendLine("	else{	");
            txt.AppendLine("	return 0;	");
            txt.AppendLine("	}	");
            txt.AppendLine("	};	");
            txt.AppendLine("	};	");
            txt.AppendLine("	//数据类型转换函数	");
            txt.AppendLine("	function convert(sValue,sDataType){	");
            txt.AppendLine("	switch(sDataType){	");
            txt.AppendLine("	case \"int\":return parseInt(sValue);	");
            txt.AppendLine("	case \"float\": return parseFloat(sValue);	");
            txt.AppendLine("	case \"date\":return new Date(Date.parse(sValue));	");
            txt.AppendLine("	default:return sValue.toString();	");
            txt.AppendLine("	}	");
            txt.AppendLine("	};	");
            txt.AppendLine("</script>");
            PagePartArray[6] += txt.ToString();
        }
        #endregion
        #region 页面收尾
        private void Pre_PageEnd()
        {
            //PagePartArray[7] += "</body>\r\n</html>";
           // PagePartArray[7] += "</asp:Content>";
        }
        #endregion
        #region 内部函数
        /// <summary>
        /// 获取参数
        /// </summary>
        private void GetParameters()
        {
            if (Request["CurrDir"] != null && Request["CurrDir"].Trim() != "")
            {
                this.CurrDir = HttpUtility.UrlDecode(Request["CurrDir"]);
            }
            if (Request["SelFiles"] != null && Request["SelFiles"].Trim() != "")
            {
                this.SelFiles = Request["SelFiles"];
                string[] AddArray = SelFiles.Split('|');
                foreach (string AddFile in AddArray)
                {
                    SelFileList.Add(AddFile);
                }
            }

            if (Request.Form.GetValues("AddFileBox") != null)
            {
                string[] AddFileArray = Request.Form.GetValues("AddFileBox");
                foreach (string AddFile in AddFileArray)
                {
                    SelFileList.Add(AddFile.Substring(AddFile.IndexOf("..")));
                }
            }
            if (Request["TrunMark"] != null && Request["TrunMark"].Trim() != "" && Request["TrunMark"].Trim() == "1")
            {
                this.SelFileList.Clear();
            }
            if (Request["DelFiles"] != null && Request["DelFiles"].Trim() != "")
            {
                this.DelFiles = Request["DelFiles"];
                string[] AddArray = DelFiles.Split('|');
                foreach (string AddFile in AddArray)
                {
                    DelFileList.Add(AddFile);
                }
                if (DelFileList.Count > 0)
                {
                    foreach (string DelFile in DelFileList)
                    {
                        SelFileList.Remove(DelFile);
                    }
                }
            }
            if (Request["SiteList"] != null && Request["SiteList"].Trim() != "")
            {
                this.SelSite = Request["SiteList"];
            }
            if (Request["IsPost"] != null && Request["IsPost"].Trim() != "" && Request["IsPost"].Trim() == "true")
            {
                this.IsPost = true;
            }
            if (Request["SelSiteChanged"] != null && Request["SelSiteChanged"].Trim() != "" && Request["SelSiteChanged"].Trim() == "true")
            {
                this.SelSiteChanged = true;
            }
            if (!string.IsNullOrEmpty(Request["OpType"]))
            {
                this.OpType = Request["OpType"].Trim();
            }
            if (!string.IsNullOrEmpty(Request["TimeFilter"]))
            {
                DateTime TempTime = DateTime.Now;
                if (DateTime.TryParse(Request["TimeFilter"], out TempTime))
                {
                    this.TimeFilter = TempTime;
                }
                else
                {
                    this.TimeFilter = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 1, 1);
                }
            }
            else
            {
                this.TimeFilter = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 1, 1);
            }
        }
        /// <summary>
        /// 获取子文件夹列表
        /// </summary>
        /// <param name="RootDirectoryPath">根目录</param>
        /// <returns></returns>
        private void GetSubDirectoryList(string RootDirectoryPath)
        {
            DirectoryInfo RootDI = new DirectoryInfo(RootDirectoryPath);
            DirectoryInfo[] DD = RootDI.GetDirectories();

            //确定目录路径
            string DyOutPath = RootDirectoryPath.Replace("\\", "/").Replace(BaseDir, "..");
            if (DyOutPath.Contains(".."))
            {
                UpDir = DyOutPath;
            }
            else
            {
                UpDir = string.Empty;
            }
            //许可站点许可列表
            List<string> AllowListSite = Common.FileUtility.PopedomCmd.AllowCompleteListSite(this.UID);
            //许可文件夹列表
            List<string> AllowDirs = null;
            //已添加可查看文件夹列表
            List<string> ViewDirList = new List<string>();
            //bool NeedDetails = false;
            //如果站点不被许可，则获取被许可文件夹列表
            if (!AllowListSite.Contains(this.SelSite))
            {
                AllowDirs = Common.FileUtility.PopedomCmd.AllowListDetails(this.UID, this.SelSite);
            }

            foreach (DirectoryInfo SubDI in RootDI.GetDirectories())
            {
                //子文件夹绝对路径转相对路径
                string DyInPath = SubDI.FullName.Replace("\\", "/").Replace(BaseDir, "..");
                //没有整站浏览权限
                if (AllowDirs != null&&AllowDirs.Count>0)
                {
                    foreach (string AllowDirFullName in AllowDirs)
                    {
                        if (AllowDirFullName.Contains(DyInPath + "/") && (!ViewDirList.Contains(DyInPath)))
                        {
                            if (SubDI.LastWriteTime > this.TimeFilter && this.TimeFilter.Year > 1900)
                            {
                                this.FileTabStr +=
                                    "<tr style='background:#ff0;' onmouseover=\"this.style.background='#ccc'\" onmouseout=\"this.style.background='#ff0'\">\r\n" +
                                    "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'><img src='image/folder.png'>\r\n" + DyInPath + "\r\n</td>\r\n" +
                                    "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n<a href='#' onclick=\"javascript:DirIn('" + DyInPath + "')\">进入该目录</a>\r\n</td>\r\n" +
                                    "<td width='20%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + SubDI.LastWriteTime + "\r\n</td>\r\n" +
                                    "</tr>\r\n";
                                ViewDirList.Add(DyInPath);
                                break;
                            }
                            else
                            {
                                this.FileTabStr +=
                                    "<tr onmouseover=\"this.style.background='#ccc'\" onmouseout=\"this.style.background='#fff'\">\r\n" +
                                    "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'><img src='image/folder.png'>\r\n" + DyInPath + "\r\n</td>\r\n" +
                                    "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n<a href='#' onclick=\"javascript:DirIn('" + DyInPath + "')\">进入该目录</a>\r\n</td>\r\n" +
                                    "<td width='20%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + SubDI.LastWriteTime + "\r\n</td>\r\n" +
                                    "</tr>\r\n";
                                ViewDirList.Add(DyInPath);
                                break;
                            }
                        }
                    }
                }
                else //注销 7.24 by xl
                {
                    if (SubDI.LastWriteTime > this.TimeFilter && this.TimeFilter.Year > 1900)
                    {
                        this.FileTabStr +=
                            "<tr style='background:#ff0;' onmouseover=\"this.style.background='#ccc'\" onmouseout=\"this.style.background='#ff0'\">\r\n" +
                            "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'><img src='image/folder.png'>\r\n" + DyInPath + "\r\n</td>\r\n" +
                            "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n<a href='#' onclick=\"javascript:DirIn('" + DyInPath + "')\">进入该目录</a>\r\n</td>\r\n" +
                            "<td width='20%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + SubDI.LastWriteTime + "\r\n</td>\r\n" +
                            "</tr>\r\n";
                    }
                    else
                    {
                        this.FileTabStr +=
                            "<tr onmouseover=\"this.style.background='#ccc'\" onmouseout=\"this.style.background='#fff'\">\r\n" +
                            "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'><img src='image/folder.png'>\r\n" + DyInPath + "\r\n</td>\r\n" +
                            "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n<a href='#' onclick=\"javascript:DirIn('" + DyInPath + "')\">进入该目录</a>\r\n</td>\r\n" +
                            "<td width='20%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + SubDI.LastWriteTime + "\r\n</td>\r\n" +
                            "</tr>\r\n";
                    }
                }
            }
            //补充与文件关联的文加夹权限
            if (AllowDirs != null)//AllowDirs != null)
            {
                foreach (string ExistDir in AllowDirs)
                {
                    //先把已经存在的加入
                    if (ExistDir.EndsWith("/"))
                    {
                        ViewDirList.Add(ExistDir.Replace("http://" + this.SelSite, ".."));
                    }
                    else
                    {
                        //文件
                        string Path_A = ExistDir.Replace("http://" + this.SelSite, "..");//格式:../A/X/C.htm
                                                                                         //路径
                        string Path_B = Path_A.Substring(0, Path_A.LastIndexOf('/') + 1);//格式../A/X/
                                                                                         //加入
                        ViewDirList.Add(Path_B);
                        string Path_C = Path_B.Substring(0, Path_B.Length - 1);//格式../A/X
                                                                               //循环检查
                        string[] PathArray = Path_C.Split('/');
                        string CachePath = string.Empty;//格式../A
                        for (int i = 0; i < PathArray.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(CachePath))
                            {
                                CachePath = CachePath + PathArray[i] + "/";
                            }
                            else
                            {
                                CachePath = PathArray[i] + "/";
                            }
                            if (CachePath == RootDirectoryPath.Replace("\\", "/").Replace("//", "/").Replace(BaseDir, "..") + "/")
                            {
                                if (i < PathArray.Length - 1)
                                {
                                    string RelSubDIName = "../" + PathArray[i + 1];
                                    string ReaSubDIName = RelSubDIName.Replace("..", BaseDir);
                                    DirectoryInfo MyDir = new DirectoryInfo(ReaSubDIName);
                                    string ToAddDir = CachePath + PathArray[i + 1];
                                    if (!ViewDirList.Contains(ToAddDir))
                                    {
                                        if (MyDir.LastWriteTime > this.TimeFilter && this.TimeFilter.Year > 1900)
                                        {
                                            this.FileTabStr +=
                                                "<tr style='background:#ff0;' onmouseover=\"this.style.background='#ccc'\" onmouseout=\"this.style.background='#fff'\">\r\n" +
                                                "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'><img src='image/folder.png'>\r\n" + ToAddDir + "\r\n</td>\r\n" +
                                                "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n<a href='#' onclick=\"javascript:DirIn('" + ToAddDir + "')\">进入该目录</a>\r\n</td>\r\n" +
                                                "<td width='20%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + MyDir.LastWriteTime + "\r\n</td>\r\n" +
                                                "</tr>\r\n";
                                            ViewDirList.Add(ToAddDir);
                                        }
                                        else
                                        {
                                            this.FileTabStr +=
                                                "<tr onmouseover=\"this.style.background='#ccc'\" onmouseout=\"this.style.background='#ff0'\">\r\n" +
                                                "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'><img src='image/folder.png'>\r\n" + ToAddDir + "\r\n</td>\r\n" +
                                                "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n<a href='#' onclick=\"javascript:DirIn('" + ToAddDir + "')\">进入该目录</a>\r\n</td>\r\n" +
                                                "<td width='20%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + MyDir.LastWriteTime + "\r\n</td>\r\n" +
                                                "</tr>\r\n";
                                            ViewDirList.Add(ToAddDir);
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取自文件列表
        /// </summary>
        /// <param name="DirectoryPath">文件目录</param>
        /// <returns></returns>
        private void GetSubFileList(string DirectoryPath)
        {
            DirectoryInfo ParentDI = new DirectoryInfo(DirectoryPath);
            //先将文件名和事件存入链表，排序后再写页面
            List<string> PreSortFileList = new List<string>();
            foreach (FileInfo SubFile in ParentDI.GetFiles())
            {
                if (!SelFileList.Contains(SubFile.FullName.Replace("\\", "/").Replace(BaseDir.Replace("\\", "/"), "..")))
                {
                    PreSortFileList.Add(SubFile.FullName + "|" + SubFile.LastWriteTime + "|" + SubFile.Name);
                }
            }
            //排序
            string cacheFileinfo = string.Empty;
            bool CycleMoveOccured = false;
            for (int i = 0; i < PreSortFileList.Count; i++)
            {
                CycleMoveOccured = false;
                for (int j = 0; j < PreSortFileList.Count - 1; j++)
                {
                    DateTime ATime = DateTime.Parse(PreSortFileList[j].Split('|')[1]);
                    DateTime BTime = DateTime.Parse(PreSortFileList[j + 1].Split('|')[1]);

                    if (ATime.CompareTo(BTime) < 0)
                    {
                        cacheFileinfo = PreSortFileList[j];
                        PreSortFileList[j] = PreSortFileList[j + 1];
                        PreSortFileList[j + 1] = cacheFileinfo;
                        CycleMoveOccured = true;
                    }
                }
                if (!CycleMoveOccured)
                {
                    break;
                }
            }
            List<string> AllowListSite = Common.FileUtility.PopedomCmd.AllowCompleteListSite(this.UID);
            List<string> AllowFiles = null;
            if (!AllowListSite.Contains(this.SelSite))
            {
                AllowFiles = Common.FileUtility.PopedomCmd.AllowListDetails(this.UID, this.SelSite);
            }
            foreach (string SortedFileStr in PreSortFileList)
            {
                string FileFullName = SortedFileStr.Split('|')[0];
                string FileLatestTime = SortedFileStr.Split('|')[1];
                string FileName = SortedFileStr.Split('|')[2];

                if (AllowFiles != null)
                {
                    string WaitCheckFileName = FileFullName.Replace("\\", "/").Replace(BaseDir.Replace("\\", "/"), "..");
                    bool NoAllow = true;
                    foreach (string AllowFileCache in AllowFiles)
                    {
                        if (AllowFileCache.EndsWith("/") && WaitCheckFileName.Contains(AllowFileCache))
                        {
                            if (DateTime.Parse(FileLatestTime) > this.TimeFilter && this.TimeFilter.Year > 1900)
                            {
                                this.FileTabStr +=
                                    "<tr style='background:#ff0;' onmouseover=\"this.style.background='#ccc'\" onmouseout=\"this.style.background='#ff0'\">\r\n" +
                                    "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'><img src='image/File.png'>\r\n<input type='checkbox' id='PreSelFileBox' name='PreSelFileBox' value='" + FileFullName.Replace("\\", "/").Replace(BaseDir, "..") + "'>" + FileName + "</input>\r\n</td>\r\n" +
                                    "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + FileFullName.Replace("\\", "/").Replace(BaseDir, "..").Replace("../", "/") + "\r\n</td>\r\n" +
                                    "<td width='20%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + FileLatestTime + "\r\n</td>\r\n" +
                                    "</tr>\r\n";
                                NoAllow = false;
                            }
                            else
                            {
                                this.FileTabStr +=
                                    "<tr onmouseover=\"this.style.background='#ccc'\" onmouseout=\"this.style.background='#fff'\">\r\n" +
                                    "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'><img src='image/File.png'>\r\n<input type='checkbox' id='PreSelFileBox' name='PreSelFileBox' value='" + FileFullName.Replace("\\", "/").Replace(BaseDir, "..") + "'>" + FileName + "</input>\r\n</td>\r\n" +
                                    "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + FileFullName.Replace("\\", "/").Replace(BaseDir, "..").Replace("../", "/") + "\r\n</td>\r\n" +
                                    "<td width='20%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + FileLatestTime + "\r\n</td>\r\n" +
                                    "</tr>\r\n";
                                NoAllow = false;
                            }
                        }
                    }
                    if (NoAllow && AllowFiles.Contains(WaitCheckFileName))
                    {
                        if (DateTime.Parse(FileLatestTime) > this.TimeFilter && this.TimeFilter.Year > 1900)
                        {
                            this.FileTabStr +=
                                "<tr style='background:#ff0;' onmouseover=\"this.style.background='#ccc'\" onmouseout=\"this.style.background='#ff0'\">\r\n" +
                                "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'><img src='image/File.png'>\r\n<input type='checkbox' id='PreSelFileBox' name='PreSelFileBox' value='" + FileFullName.Replace("\\", "/").Replace(BaseDir, "..") + "'>" + FileName + "</input>\r\n</td>\r\n" +
                                "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + FileFullName.Replace("\\", "/").Replace(BaseDir, "..").Replace("../", "/") + "\r\n</td>\r\n" +
                                "<td width='20%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + FileLatestTime + "\r\n</td>\r\n" +
                                "</tr>\r\n";
                        }
                        else
                        {
                            this.FileTabStr +=
                                "<tr onmouseover=\"this.style.background='#ccc'\" onmouseout=\"this.style.background='#fff'\">\r\n" +
                                "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'><img src='image/File.png'>\r\n<input type='checkbox' id='PreSelFileBox' name='PreSelFileBox' value='" + FileFullName.Replace("\\", "/").Replace(BaseDir, "..") + "'>" + FileName + "</input>\r\n</td>\r\n" +
                                "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + FileFullName.Replace("\\", "/").Replace(BaseDir, "..").Replace("../", "/") + "\r\n</td>\r\n" +
                                "<td width='20%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + FileLatestTime + "\r\n</td>\r\n" +
                                "</tr>\r\n";
                        }
                    }
                }
                else
                {
                    if (DateTime.Parse(FileLatestTime) > this.TimeFilter && this.TimeFilter.Year > 1900)
                    {
                        this.FileTabStr +=
                            "<tr style='background:#ff0;' onmouseover=\"this.style.background='#ccc'\" onmouseout=\"this.style.background='#ff0'\">\r\n" +
                            "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'><img src='image/File.png'>\r\n<input type='checkbox' id='PreSelFileBox' name='PreSelFileBox' value='" + FileFullName.Replace("\\", "/").Replace(BaseDir, "..") + "'>" + FileName + "</input>\r\n</td>\r\n" +
                            "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + FileFullName.Replace("\\", "/").Replace(BaseDir, "..").Replace("../", "/") + "\r\n</td>\r\n" +
                            "<td width='20%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + FileLatestTime + "\r\n</td>\r\n" +
                            "</tr>\r\n";
                    }
                    else
                    {
                        this.FileTabStr +=
                            "<tr onmouseover=\"this.style.background='#ccc'\" onmouseout=\"this.style.background='#fff'\">\r\n" +
                            "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'><img src='image/File.png'>\r\n<input type='checkbox' id='PreSelFileBox' name='PreSelFileBox' value='" + FileFullName.Replace("\\", "/").Replace(BaseDir, "..") + "'>" + FileName + "</input>\r\n</td>\r\n" +
                            "<td width='40%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + FileFullName.Replace("\\", "/").Replace(BaseDir, "..").Replace("../", "/") + "\r\n</td>\r\n" +
                            "<td width='20%' style='white-space:normal; word-break:break-all;word-wrap:break-word'>\r\n" + FileLatestTime + "\r\n</td>\r\n" +
                            "</tr>\r\n";
                    }
                }
            }
        }
        #endregion
        #region 控制页面运行
        /// <summary>
        /// 控制页面运行
        /// </summary>
        private void Work()
        {
            this.Pre_BasePage();
            this.Pre_SiteList();
            this.Pre_ForSelFiles();
            this.Pre_SeledFiles();
            this.Pre_HiddenParams();
            this.Pre_PageScript();
            this.Pre_PageEnd();
            OutPage= string.Concat(PagePartArray); 
            //string OutPage = string.Concat(PagePartArray);

            //Response.Clear();
            //Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            //Response.ContentType = "text/html";
            //Response.Charset = "UTF-8";
            //Response.Write(OutPage);
            //Response.End();
        }
        #endregion
        #region 文件同步控制
        /// <summary>
        /// 文件同步控制
        /// </summary>
        private void FileFtpOp()
        {
            if (IsPost)
            {

                //目标站点
                NewTask = new Common.ConfigUtility.TaskInfo();
                if (TaskSiteBaseInfo == null)
                {
                    TaskSiteBaseInfo = Common.ConfigUtility.Config.GetSiteInfo(SelSite);
                    BaseDir = TaskSiteBaseInfo.LOCALPATH;
                }
                NewTask.SiteBaseInfo = TaskSiteBaseInfo;
                //文件对
                List<TaskFileInfo> AddFilePairList = new List<TaskFileInfo>();
                foreach (string AddFile in SelFileList)
                {
                    string SrcPath = AddFile.Replace("..", BaseDir);
                    string AimPath = AddFile;
                    AddFilePairList.Add(new TaskFileInfo(AddFile.Replace("..", BaseDir), AddFile));
                }
                NewTask.FilePairList = AddFilePairList;
                string M = string.Empty;

                if (OpType == "POST")
                {

                    DBLog log = new DBLog();
                    log.UpdateBlock(NewTask.SiteBaseInfo.NAME);

                    Common.LogUtility.DBLog Userlog = new DBLog();

                    Userlog.UserOperLog("同步操作", "站点：" +  NewTask.SiteBaseInfo.NAME+ ",批次：" + log.BlockStr, OpUser.UserId );

                    Common.LogUtility.UploadTrace.AddUploadTask(log.BlockStr, NewTask);
                    doPost t2 = new doPost(new TaskWorker(NewTask, log).Execute);
                    t2.BeginInvoke(new AsyncCallback(AddMsgCallBack), t2);


                    Response.Redirect("Monitor.aspx?block=" + log.BlockStr+"&SiteName="+ NewTask.SiteBaseInfo.NAME);
                    this.Pre_MsgScript("文件同步:\\r\\n" + M);
                }

                if (string.IsNullOrEmpty(M) && (!string.IsNullOrEmpty(OpType)))
                {
                    this.Pre_MsgScript("没有合适的操作可执行!");
                }
            }
        }
        private delegate void doPost();
        private void AddMsgCallBack(IAsyncResult IAR)
        {
            doPost t2 = (doPost)(IAR.AsyncState);
            t2.EndInvoke(IAR);
            t2.Clone();
        }
        #endregion
    }
}