using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using System.Net;
using System.Web;

namespace Common.ConfigUtility
{
    public class Config
    {
        private enum CONFIGTYPE
        {
            FTP_AIM
        }
        private static string SrcConfig = string.Empty;
        private static List<WebSiteServer> GetAimServList(string AimSiteName)
        {
            XmlNode node = null;
            foreach (XmlNode node3 in GetNodeByName(CONFIGTYPE.FTP_AIM).ChildNodes)
            {
                if ((AimSiteName == node3.Attributes["NAME"].Value) && (node3.NodeType == XmlNodeType.Element))
                {
                    node = node3;
                    break;
                }
            }
            List<WebSiteServer> list = new List<WebSiteServer>();
            if (node != null)
            {
                foreach (XmlNode node4 in node.ChildNodes)
                {
                    if (node4.NodeType == XmlNodeType.Element)
                    {
                        WebSiteServer item = new WebSiteServer();
                        item.BAKFOLDER = node4.Attributes["FILEBAKROOT"].Value;
                        item.IP = IPAddress.Parse(node4.Attributes["IP"].Value);
                        item.NAME = node4.Attributes["NAME"].Value;
                        item.PORT = Convert.ToInt32(node4.Attributes["PORT"].Value);
                        item.TRANSFOLDER = node4.Attributes["TRANSFOLDER"].Value;
                        item.USER = node4.Attributes["USER"].Value;
                        item.PASS = node4.Attributes["PASS"].Value;
                        item.DIRPATH = node4.Attributes["DIRPATH"].Value;
                        list.Add(item);
                    }
                }
            }
            return list;
        }
        private static XmlDocument GetConfigDoc()
        {
            XmlDocument document = new XmlDocument();
            document.Load(ConfigFileLocation);
            return document;
        }
        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="ConfigNode"></param>
        /// <returns></returns>
        private static XmlNode GetNodeByName(CONFIGTYPE ConfigNode)
        {
            XmlDocument configDoc = GetConfigDoc();
            XmlNode node = null;
            if (ConfigNode == CONFIGTYPE.FTP_AIM)
            {
                node = configDoc.SelectSingleNode("FTP_ROOT/FTP_AIM");
            }
            return node;
        }
        /// <summary>
        /// 获取单个站点信息
        /// </summary>
        /// <param name="AimSiteName"></param>
        /// <returns></returns>
        public static WebSiteInfo GetSiteInfo(string AimSiteName)
        {
            WebSiteInfo info = new WebSiteInfo();
            XmlNode node = null;
            foreach (XmlNode node3 in GetNodeByName(CONFIGTYPE.FTP_AIM).ChildNodes)
            {
                if ((AimSiteName == node3.Attributes["NAME"].Value) && (node3.NodeType == XmlNodeType.Element))
                {
                    node = node3;
                    break;
                }
            }
            info.NAME = node.Attributes["NAME"].Value;
            info.LOCALPATH = node.Attributes["LOCALPATH"].Value;
            info.SERVLIST = GetAimServList(AimSiteName);
            info.SERVERCOUNT = info.SERVLIST.Count;
            info.LOCALBAKTRANSFOLDER = node.Attributes["LOCALBAKTRANSFOLDER"].Value;
            return info;
        }
        /// <summary>
        /// 获取站点名称列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSiteList()
        {
            List<string> list = new List<string>();
            foreach (XmlNode node2 in GetNodeByName(CONFIGTYPE.FTP_AIM).ChildNodes)
            {
                if (node2.NodeType == XmlNodeType.Element)
                {
                    list.Add(node2.Attributes["NAME"].Value);
                }
            }
            return list;
        }
        /// <summary>
        /// 配置文件路径,格式为站点相对路径:~/App_Data/Config.xml
        /// </summary>
        private static string ConfigFileLocation
        {
            get
            {
                if (string.IsNullOrEmpty(SrcConfig))
                {
                    SrcConfig = ConfigurationManager.AppSettings["ConfigFile"].ToString();
                }
                if (SrcConfig.StartsWith("~"))
                {
                    SrcConfig = HttpContext.Current.Server.MapPath(SrcConfig);
                }
                return SrcConfig;
            }
            set
            {
                SrcConfig = value;
            }
        }
    }
}