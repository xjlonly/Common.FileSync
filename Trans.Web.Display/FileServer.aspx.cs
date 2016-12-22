using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.Net;

namespace Trans.Web.Display
{

    public partial class FileServer : System.Web.UI.Page
    {
        private string username = "qiuyingguo1";
        private string password = "ghHUo4Hv8";
        private string uri = "124.251.46.241";
        protected void Page_Load(object sender, EventArgs e)
        {
            //ftp://qiuyingguo1:ghHUo4Hv8@124.251.46.241  kehutest.home.soufun.com
            List<FileStruct> list = GetFileList("kehutest.home.soufun.com").OrderByDescending(p=>p.IsDirectory).ToList();
            this.Repeater1.DataSource = list;
            Repeater1.DataBind();
        }

        #region ftp下载浏览文件夹信息
        /// <summary>
        /// 得到当前目录下的所有目录和文件
        /// </summary>
        /// <param name="srcpath">浏览的目录</param>
        /// <returns></returns>
        public List<FileStruct> GetFileList(string srcpath)
        {
            List<FileStruct> list = new List<FileStruct>();

            FtpWebRequest reqFtp;
            WebResponse response = null;


            string ftpuri = string.Format("ftp://{0}/{1}", uri, srcpath);
            //try
            //{
                reqFtp = (FtpWebRequest)FtpWebRequest.Create(ftpuri);
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(username, password);


                reqFtp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                response = reqFtp.GetResponse();
                list = ListFilesAndDirectories((FtpWebResponse)response).ToList();
                response.Close();
           // }
            //catch(Exception e)
            //{
                //if (response != null)
                //{
                //    response.Close();
                //}
            //}
            return list;
        }
        #endregion
        #region 列出目录文件信息
        /// <summary>
        /// 列出FTP服务器上面当前目录的所有文件和目录
        /// </summary>

        public FileStruct[] ListFilesAndDirectories(FtpWebResponse Response)
        {
            //Response = Open(this.Uri, WebRequestMethods.Ftp.ListDirectoryDetails);
            StreamReader stream = new StreamReader(Response.GetResponseStream(), Encoding.Default);
            string Datastring = stream.ReadToEnd();
            FileStruct[] list = GetList(Datastring);
            return list;
        }
    
        /// <summary>
        /// 获得文件和目录列表
        /// </summary>
        /// <param name="datastring">FTP返回的列表字符信息</param>
        private FileStruct[] GetList(string datastring)
        {
            List<FileStruct> myListArray = new List<FileStruct>();
            string[] tmpdataRecords = datastring.Split('\n');
            FileListStyle _directoryListStyle = GuessFileListStyle(tmpdataRecords);
            List<string> dataRecords = tmpdataRecords.ToList();
            if(dataRecords[0].IndexOf("total") >= 0)
            {
                dataRecords.RemoveAt(0);
            }
            foreach (string s in dataRecords)
            {
                
                if (_directoryListStyle != FileListStyle.Unknown && s != "")
                {
                    FileStruct f = new FileStruct();
                    f.Name = "..";
                    switch (_directoryListStyle)
                    {
                        case FileListStyle.UnixStyle:
                            f = ParseFileStructFromUnixStyleRecord(s);
                            break;
                        case FileListStyle.WindowsStyle:
                            f = ParseFileStructFromWindowsStyleRecord(s);
                            break;
                    }
                    if (!(f.Name == "." || f.Name == ".."))
                    {
                        myListArray.Add(f);
                    }
                }
            }
            return myListArray.ToArray();
        }


        /// <summary>
        /// 从Windows格式中返回文件信息
        /// </summary>
        /// <param name="Record">文件信息</param>
        private FileStruct ParseFileStructFromWindowsStyleRecord(string Record)
        {
            FileStruct f = new FileStruct();
            string processstr = Record.Trim();
            string dateStr = processstr.Substring(0, 8);
            processstr = (processstr.Substring(8, processstr.Length - 8)).Trim();
            string timeStr = processstr.Substring(0, 7);
            processstr = (processstr.Substring(7, processstr.Length - 7)).Trim();
            DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;
            myDTFI.ShortTimePattern = "t";
            f.CreateTime = DateTime.Parse(dateStr + " " + timeStr, myDTFI);
            if (processstr.Substring(0, 5) == "<DIR>")
            {
                f.IsDirectory = true;
                processstr = (processstr.Substring(5, processstr.Length - 5)).Trim();
            }
            else
            {
                string[] strs = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);   // true);
                processstr = strs[1];
                f.IsDirectory = false;
            }
            f.Name = processstr;
            return f;
        }




        /// <summary>
        /// 判断文件列表的方式Window方式还是Unix方式
        /// </summary>
        /// <param name="recordList">文件信息列表</param>
        private FileListStyle GuessFileListStyle(string[] recordList)
        {
            foreach (string s in recordList)
            {
                if (s.Length > 10
                 && Regex.IsMatch(s.Substring(0, 10), "(-|d)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)"))
                {
                    return FileListStyle.UnixStyle;
                }
                else if (s.Length > 8
                 && Regex.IsMatch(s.Substring(0, 8), "[0-9][0-9]-[0-9][0-9]-[0-9][0-9]"))
                {
                    return FileListStyle.WindowsStyle;
                }
            }
            return FileListStyle.Unknown;
        }


        /// <summary>
        /// 从Unix格式中返回文件信息
        /// </summary>
        /// <param name="Record">文件信息</param>
        private FileStruct ParseFileStructFromUnixStyleRecord(string Record)
        {
            FileStruct f = new FileStruct();
            string processstr = Record.Trim();
            f.Flags = processstr.Substring(0, 10);
            f.IsDirectory = (f.Flags[0] == 'd');
            processstr = (processstr.Substring(11)).Trim();
            _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);   //跳过一部分
            f.Owner = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            f.Group = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            f.Size = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            string yearOrTime = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2];
            int m_index = processstr.IndexOf(yearOrTime);
            if (yearOrTime.IndexOf(":") >= 0)  //time
            {
                processstr = processstr.Replace(yearOrTime, DateTime.Now.Year.ToString());
            }
            else
            {
                yearOrTime = "";
            }

            string tmptime = _cutSubstringFromStringWithTrim(ref processstr, ' ', 8) + " " + yearOrTime;
            f.CreateTime = DateTime.Parse(tmptime);

            f.Name = processstr;   //最后就是名称
            return f;
        }


        /// <summary>
        /// 按照一定的规则进行字符串截取
        /// </summary>
        /// <param name="s">截取的字符串</param>
        /// <param name="c">查找的字符</param>
        /// <param name="startIndex">查找的位置</param>
        private string _cutSubstringFromStringWithTrim(ref string s, char c, int startIndex)
        {
            int pos1 = s.IndexOf(c, startIndex);
            string retString = s.Substring(0, pos1);
            s = (s.Substring(pos1)).Trim();
            return retString;
        }
        #endregion


        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Uri
        {
            get { return uri; }
            set { uri = value; }
        }
    }


    public class FileStruct
    {
        public string Flags;
        public string Owner;
        public string Group;
        public bool IsDirectory;
        public DateTime CreateTime;
        public string Name;
        public string Size;
    }

    public enum FileListStyle
    {
        UnixStyle,
        WindowsStyle,
        Unknown
    }
}