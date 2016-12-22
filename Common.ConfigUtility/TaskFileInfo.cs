namespace Common.ConfigUtility
{
    /// <summary>
    /// 任务文件信息
    /// </summary>
    public class TaskFileInfo
    {
        private string AimFilePath;
        private string SrcFilePath;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskFileInfo()
        {
            this.SrcFilePath = string.Empty;
            this.AimFilePath = string.Empty;
        }
        /// <summary>
        /// 构造函数，根据源文件路径和目标文件路径构建文件信息
        /// </summary>
        /// <param name="SrcPath">源文件地址，如：D:/Demoweb/www.x.com/xx.cshtml</param>
        /// <param name="AimPath">目标文件地址，如:../web/www.x.com/xx.cshtml</param>
        public TaskFileInfo(string SrcPath, string AimPath)
        {
            this.SrcFilePath = string.Empty;
            this.AimFilePath = string.Empty;
            this.SrcFilePath = SrcPath;
            this.AimFilePath = AimPath;
        }
        /// <summary>
        /// 目标文件，格式：../A/B/C/D.jpg
        /// </summary>
        public string AimFile
        {
            get
            {
                return this.AimFilePath;
            }
            set
            {
                this.AimFilePath = value;
            }
        }
        /// <summary>
        /// 完整文件名，无路径信息，如：D.jpg
        /// </summary>
        public string FileName
        {
            get
            {
                string str = this.AimFile.Replace("../", "/");
                return str.Substring(str.LastIndexOf("/") + 1);
            }
        }
        /// <summary>
        /// 目标相对目录,带/前缀，如：/A/B/C，无结尾/
        /// </summary>
        public string RelAimFolder
        {
            get
            {
                string str = this.AimFile.Replace("../", "/");
                return str.Substring(0, str.LastIndexOf('/'));
            }
        }
        /// <summary>
        /// 源文件路径,如：D:/Demoweb/www.x.com/xx.cshtml
        /// </summary>
        public string SrcFile
        {
            get
            {
                return this.SrcFilePath;
            }
            set
            {
                this.SrcFilePath = value;
            }
        }
    }
}