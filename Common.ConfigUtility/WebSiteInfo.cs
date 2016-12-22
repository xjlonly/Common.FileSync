using System.Collections.Generic;

namespace Common.ConfigUtility
{
    /// <summary>
    /// 任务站点信息
    /// </summary>
    public class WebSiteInfo
    {
        /// <summary>
        /// 本地传输路径，如：D:/Trans
        /// </summary>
        public string LOCALBAKTRANSFOLDER{get;set;}
        /// <summary>
        /// 文件源地址根文件夹，如：D:/Web/Publish/www.x.com
        /// </summary>
        public string LOCALPATH { get; set; }
        /// <summary>
        /// 站点名，如：www.x.com
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 任务包含服务器数量
        /// </summary>
        public int SERVERCOUNT { get; set; }
        /// <summary>
        /// 任务包含服务器列表
        /// </summary>
        public List<WebSiteServer> SERVLIST { get; set; }
    }
}