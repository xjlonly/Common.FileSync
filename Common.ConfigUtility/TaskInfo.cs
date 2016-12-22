using System.Collections.Generic;

namespace Common.ConfigUtility
{
    /// <summary>
    /// 同步任务信息
    /// </summary>
    public class TaskInfo
    {
        /// <summary>
        /// 需要同步的文件列表
        /// </summary>
        public List<TaskFileInfo> FilePairList { get; set; }
        /// <summary>
        /// 站点基本信息
        /// </summary>
        public Common.ConfigUtility.WebSiteInfo SiteBaseInfo { get; set; }
    }
}