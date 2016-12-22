using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Common.LogUtility;

namespace Common.FileUtility
{
    /// <summary>
    /// FTP原子操作
    /// </summary>
    public class FtpAtomOperation
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private DBLog dbLog;

        public FtpAtomOperation(DBLog log)
        {
            this.dbLog = log;
        }

        public bool CreateFtpDirectory(string BaseUriStr, string AimUriStr, string UserName, string UserPwd)
        {
            Uri requestUri = new Uri(new Uri(BaseUriStr), AimUriStr);
            FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(requestUri);
            ftpWebRequest.KeepAlive = false;
            ftpWebRequest.UsePassive = true;
            ftpWebRequest.Timeout = 600000;
            ftpWebRequest.UsePassive = true;
            NetworkCredential networkCredential = new NetworkCredential(UserName, UserPwd);
            ftpWebRequest.Credentials = (ICredentials)new CredentialCache()
              {
                {
                  requestUri,
                  ((object) AuthType.Basic).ToString(),
                  networkCredential
                }
              };
            ftpWebRequest.Method = "MKD";
            try
            {
                FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                if (ftpWebResponse.StatusCode == FtpStatusCode.PathnameCreated)
                {
                    ftpWebResponse.Close();
                    return true;
                }
                else
                {
                    this.dbLog.Error("CreateFtpDirectory",
                        "NO_EXC|BASEURI:" + BaseUriStr +
                        "|AIMURI:" + AimUriStr +
                        "|STATUSCODE:" + ftpWebResponse.StatusCode.ToString() +
                        "|STATUSDESCRIPTION:" + ftpWebResponse.StatusDescription);
                    ftpWebResponse.Close();
                    return false;
                }
            }
            catch (WebException ex)
            {
                FtpWebResponse ftpWebResponse = (FtpWebResponse)ex.Response;
                if (ftpWebResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable || ftpWebResponse.StatusCode == FtpStatusCode.Undefined && AimUriStr.Trim() == "/")
                {
                    ftpWebResponse.Close();
                    return true;
                }
                else
                {
                    this.dbLog.Error("CreateFtpDirectory",
                        "EXCEPTION|BASEURI:" + BaseUriStr +
                        "|AIMURI:" + AimUriStr +
                        "|STATUSCODE:" + ftpWebResponse.StatusCode.ToString() +
                        "|STATUSDESCRIPTION:" + ftpWebResponse.StatusDescription +
                        "|EXCDESCRIPTION:" + ex.Message);
                    ftpWebResponse.Close();
                    return false;
                }
            }
        }

        public bool CreateFtpListDirectory(string BaseUriStr, string AimUriStr, string UserName, string UserPwd)
        {
            this.CreateFtpDirectory(BaseUriStr, "/", UserName, UserPwd);
            string[] strArray = AimUriStr.TrimStart(new char[1]
              {
                '/'
              }).Split(new char[1]
              {
                '/'
              });
            string AimUriStr1 = string.Empty;
            for (int index = 0; index < strArray.Length; ++index)
            {
                if (!string.IsNullOrEmpty(strArray[index]))
                {
                    AimUriStr1 = AimUriStr1 + "/" + strArray[index];
                    this.CreateFtpDirectory(BaseUriStr, AimUriStr1, UserName, UserPwd);
                }
            }
            return true;
        }

        public List<string> ListFtpGeneralDirectory(string BaseUriStr, string AimUriStr, string UserName, string UserPwd)
        {
            Uri requestUri = new Uri(new Uri(BaseUriStr), AimUriStr);
            FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(requestUri);
            ftpWebRequest.KeepAlive = false;
            ftpWebRequest.ReadWriteTimeout = 600000;
            ftpWebRequest.Timeout = 600000;
            ftpWebRequest.UsePassive = true;
            NetworkCredential networkCredential = new NetworkCredential(UserName, UserPwd);
            ftpWebRequest.Credentials = (ICredentials)new CredentialCache()
              {
                {
                  requestUri,
                  ((object) AuthType.Basic).ToString(),
                  networkCredential
                }
              };
            ftpWebRequest.Method = "NLST";
            try
            {
                FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(ftpWebResponse.GetResponseStream(), Encoding.GetEncoding("GB2312"));
                string str1 = streamReader.ReadToEnd();
                streamReader.Close();
                ftpWebResponse.Close();
                string[] strArray = str1.Split(new string[1]
                {
                  "\r\n"
                }, StringSplitOptions.RemoveEmptyEntries);
                List<string> list = new List<string>();
                foreach (string str2 in strArray)
                    list.Add(str2);
                return list;
            }
            catch (WebException ex)
            {
                FtpWebResponse ftpWebResponse = (FtpWebResponse)ex.Response;
                this.dbLog.Error("ListFtpGeneralDirectory",
                    "EXCEPTION|BASEURI:" + BaseUriStr +
                    "|AIMURI:" + AimUriStr +
                    "|STATUSCODE:" + ftpWebResponse.StatusCode.ToString() +
                    "|STATUSDESCRIPTION:" + ftpWebResponse.StatusDescription +
                    "|EXCDESCRIPTION:" + ex.Message);
                ftpWebResponse.Close();
                return new List<string>();
            }
        }

        public List<string> ListFtpDirectory(string BaseUriStr, string AimUriStr, string UserName, string UserPwd)
        {
            Uri requestUri = new Uri(new Uri(BaseUriStr), AimUriStr);
            FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(requestUri);
            ftpWebRequest.KeepAlive = false;
            ftpWebRequest.ReadWriteTimeout = 600000;
            ftpWebRequest.Timeout = 600000;
            ftpWebRequest.UsePassive = true;
            NetworkCredential networkCredential = new NetworkCredential(UserName, UserPwd);
            ftpWebRequest.Credentials = (ICredentials)new CredentialCache()
              {
                {
                  requestUri,
                  ((object) AuthType.Basic).ToString(),
                  networkCredential
                }
              };
            ftpWebRequest.Method = "LIST";
            try
            {
                FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(ftpWebResponse.GetResponseStream(), Encoding.GetEncoding("GB2312"));
                string str1 = streamReader.ReadToEnd();
                streamReader.Close();
                ftpWebResponse.Close();
                string[] strArray = str1.Split(new string[1]
                {
                  "\r\n"
                }, StringSplitOptions.RemoveEmptyEntries);
                List<string> list = new List<string>();
                foreach (string str2 in strArray)
                {
                    if (str2.StartsWith("d") && !str2.EndsWith("."))
                    {
                        string str3 = str2.Substring(str2.IndexOf(':') + 3).TrimStart(new char[0]);
                        list.Add(str3 + "|D");
                    }
                    else if (str2.StartsWith("-"))
                    {
                        string str3 = str2.Substring(str2.IndexOf(':') + 3).TrimStart(new char[0]);
                        list.Add(str3 + "|F");
                    }
                }
                return list;
            }
            catch (WebException ex)
            {
                FtpWebResponse ftpWebResponse = (FtpWebResponse)ex.Response;
                this.dbLog.Error("ListFtpDirectory",
                    "EXCEPTION|BASEURI:" + BaseUriStr +
                    "|AIMURI:" + AimUriStr +
                    "|STATUSCODE:" + ftpWebResponse.StatusCode.ToString() +
                    "|STATUSDESCRIPTION:" + ftpWebResponse.StatusDescription +
                    "|EXCDESCRIPTION:" + ex.Message);
                ftpWebResponse.Close();
                return new List<string>();
            }
        }

        public bool UploadFtpFile(string BaseUriStr, string AimUriStr, string UserName, string UserPwd, byte[] FileByteArray)
        {
            int num = 0;
            while (num < 3)
            {
                Uri requestUri = new Uri(new Uri(BaseUriStr), AimUriStr);
                FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(requestUri);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.ReadWriteTimeout = 60000;
                ftpWebRequest.Timeout = 60000;
                ftpWebRequest.UsePassive = true;
                NetworkCredential networkCredential = new NetworkCredential(UserName, UserPwd);
                ftpWebRequest.Credentials = (ICredentials)new CredentialCache()
                {
                  {
                    requestUri,
                    ((object) AuthType.Basic).ToString(),
                    networkCredential
                  }
                };
                ftpWebRequest.Method = "STOR";
                try
                {
                    Stream requestStream = ftpWebRequest.GetRequestStream();
                    requestStream.Write(FileByteArray, 0, FileByteArray.Length);
                    requestStream.Close();
                    requestStream.Dispose();
                    GC.Collect();
                    FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                    if (ftpWebResponse.StatusCode == FtpStatusCode.ClosingData)
                    {
                        ftpWebResponse.Close();
                        return true;
                    }
                    else
                    {
                        this.dbLog.Error("UploadFtpFile",
                            "NO_EXC|BASEURI:" + BaseUriStr +
                            "|AIMURI:" + AimUriStr +
                            "|STATUSCODE:" + ftpWebResponse.StatusCode.ToString() +
                            "|STATUSDESCRIPTION:" + ftpWebResponse.StatusDescription);
                        ftpWebResponse.Close();
                        return false;
                    }
                }
                catch (WebException ex)
                {
                    FtpWebResponse ftpWebResponse = (FtpWebResponse)ex.Response;
                    this.dbLog.Error("UploadFtpFile",
                        "EXCEPTION|BASEURI:" + BaseUriStr +
                        "|AIMURI:" + AimUriStr +
                        "|STATUSCODE:" + ftpWebResponse.StatusCode.ToString() +
                        "|STATUSDESCRIPTION:" + ftpWebResponse.StatusDescription +
                        "|EXCDESCRIPTION:" + ex.Message + "|尝试第" + Convert.ToString(num + 1) + "次");
                    ftpWebResponse.Close();
                    ++num;
                }
            }
            return false;
        }

        public int GetFtpFileSize(string BaseUriStr, string AimUriStr, string UserName, string UserPwd)
        {
            Uri requestUri = new Uri(new Uri(BaseUriStr), AimUriStr);
            FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(requestUri);
            ftpWebRequest.KeepAlive = false;
            ftpWebRequest.ReadWriteTimeout = 600000;
            ftpWebRequest.Timeout = 600000;
            ftpWebRequest.UsePassive = true;
            NetworkCredential networkCredential = new NetworkCredential(UserName, UserPwd);
            ftpWebRequest.Credentials = (ICredentials)new CredentialCache()
                {
                  {
                    requestUri,
                    ((object) AuthType.Basic).ToString(),
                    networkCredential
                  }
                };
            ftpWebRequest.Method = "SIZE";
            try
            {
                FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long contentLength = ftpWebResponse.ContentLength;
                ftpWebResponse.Close();
                return Convert.ToInt32(contentLength);
            }
            catch (WebException ex)
            {
                ex.Response.Close();
                return -1;
            }
        }

        public bool RenameFtpFile(string BaseUriStr, string SrcUriStr, string AimUriStr, string UserName, string UserPwd)
        {
            Uri requestUri = new Uri(new Uri(BaseUriStr), SrcUriStr);
            FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(requestUri);
            ftpWebRequest.KeepAlive = false;
            ftpWebRequest.ReadWriteTimeout = 600000;
            ftpWebRequest.Timeout = 600000;
            ftpWebRequest.UsePassive = true;
            NetworkCredential networkCredential = new NetworkCredential(UserName, UserPwd);
            ftpWebRequest.Credentials = (ICredentials)new CredentialCache()
              {
                {
                  requestUri,
                  ((object) AuthType.Basic).ToString(),
                  networkCredential
                }
              };
            ftpWebRequest.Method = "RENAME";
            ftpWebRequest.RenameTo = AimUriStr;
            try
            {
                FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                if (ftpWebResponse.StatusCode == FtpStatusCode.FileActionOK)
                {
                    ftpWebResponse.Close();
                    return true;
                }
                else
                {
                    this.dbLog.Error("RenameFtpFile",
                        "NO_EXC|BASEURI:" + BaseUriStr +
                        "|SRCURI:" + SrcUriStr +
                        "|AIMURI:" + AimUriStr +
                        "|STATUSCODE:" + ftpWebResponse.StatusCode.ToString() +
                        "|STATUSDESCRIPTION:" + ftpWebResponse.StatusDescription);
                    ftpWebResponse.Close();
                    return false;
                }
            }
            catch (WebException ex)
            {
                int ftpFileSize = this.GetFtpFileSize(BaseUriStr, SrcUriStr, UserName, UserPwd);
                if (ftpFileSize == -1)
                {
                    this.dbLog.Error("RenameFtpFile",
                        "EXCEPTION|BASEURI:" + BaseUriStr +
                        "SRCURI:" + SrcUriStr +
                        "|AIMURI:" + AimUriStr +
                        "|DESCRIPTION:文件不存在");
                    return false;
                }
                else
                {
                    FtpWebResponse ftpWebResponse = (FtpWebResponse)ex.Response;
                    this.dbLog.Error("RenameFtpFile",
                        "EXCEPTION|BASEURI:" + BaseUriStr +
                        "SRCURI:" + SrcUriStr +
                        "|AIMURI:" + AimUriStr +
                        "|STATUSCODE:" + ftpWebResponse.StatusCode.ToString() +
                        "|STATUSDESCRIPTION:" + ftpWebResponse.StatusDescription +
                        "|EXCDESCRIPTION:" + ex.Message +
                        "|EXCREQUESTURI:" + ftpWebResponse.ResponseUri.OriginalString +
                        "|FILESIZE:" + ftpFileSize.ToString() +
                        "|EXCSTACKTRACE:" + ex.StackTrace);
                    ftpWebResponse.Close();
                    return false;
                }
            }
        }

        public bool DownLoadFtpFile(string BaseUriStr, string AimUriStr, string LocalPath, string UserName, string UserPwd)
        {
            int num = 0;
            while (num < 3)
            {
                Uri requestUri = new Uri(new Uri(BaseUriStr), AimUriStr);
                FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(requestUri);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.ReadWriteTimeout = 600000;
                ftpWebRequest.Timeout = 600000;
                ftpWebRequest.UsePassive = true;
                NetworkCredential networkCredential = new NetworkCredential(UserName, UserPwd);
                ftpWebRequest.Credentials = (ICredentials)new CredentialCache()
                {
                  {
                    requestUri,
                    ((object) AuthType.Basic).ToString(),
                    networkCredential
                  }
                };
                ftpWebRequest.Method = "RETR";
                try
                {
                    FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                    Stream responseStream = ftpWebResponse.GetResponseStream();
                    FileStream fileStream = System.IO.File.Create(LocalPath);
                    byte[] buffer = new byte[500];
                    int count;
                    while ((count = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                        fileStream.Write(buffer, 0, count);
                    responseStream.Close();
                    responseStream.Dispose();
                    fileStream.Close();
                    fileStream.Dispose();
                    ftpWebResponse.Close();
                    return true;
                }
                catch (WebException ex)
                {
                    FtpWebResponse ftpWebResponse = (FtpWebResponse)ex.Response;
                    this.dbLog.Error("DownLoadFtpFile",
                        "EXCEPTION|BASEURI:" + BaseUriStr +
                        "|AIMURI:" + AimUriStr +
                        "|LOCALFILE:" + LocalPath +
                        "|STATUSCODE:" + ftpWebResponse.StatusCode.ToString() +
                        "|STATUSDESCRIPTION:" + ftpWebResponse.StatusDescription +
                        "|EXCDESCRIPTION:" + ex.Message + "|尝试第" + (num + 1).ToString() + "次失败");
                    ftpWebResponse.Close();
                    ++num;
                }
            }
            return false;
        }

        public bool CheckFtpServer(IPAddress IPAddrStr)
        {
            Ping ping = new Ping();
            if (ping.Send(IPAddrStr).Status == IPStatus.Success)
            {
                ping.Dispose();
                return true;
            }
            else
            {
                ping.Dispose();
                return false;
            }
        }
    }
}