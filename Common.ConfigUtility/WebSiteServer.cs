using System.Net;

namespace Common.ConfigUtility
{
    /// <summary>
    /// 任务站点单个节点服务器信息
    /// </summary>
    public class WebSiteServer
    {
        /// <summary>
        /// 被同步服务器备份目录名，如：BACKUP_19
        /// </summary>
        public string BAKFOLDER { get; set; }
        /// <summary>
        /// 站点FTP根地址，如：FTP://192.168.0.1：10021
        /// </summary>
        public string BaseUri
        {
            get { return ("Ftp://" + this.IP.ToString() + ":" + this.PORT.ToString()); }
        }
        /// <summary>
        /// IP地址
        /// </summary>
        public IPAddress IP { get; set; }
        /// <summary>
        /// 服务器名
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 服务器密码
        /// </summary>
        public string PASS { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        public int PORT { get; set; }
        /// <summary>
        /// 本地传输文件目录
        /// </summary>
        public string TRANSFOLDER { get; set; }
        /// <summary>
        /// 服务器用户
        /// </summary>
        public string USER { get; set; }
        /// <summary>
        /// 服务器根相对FTP路径
        /// </summary>
        public string DIRPATH { get; set; }
    }
}