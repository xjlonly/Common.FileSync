using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using Common.ConfigUtility;
using Common.LogUtility;

namespace Common.FileUtility
{
    public class TaskWorker
    {
        #region 内部变量
        /// <summary>
        /// 线程数量
        /// </summary>
        private int threadNumber = 4;
        /// <summary>
        /// 是否多线程执行覆盖,默认true
        /// </summary>
        private bool multiCover = true;
        /// <summary>
        /// 备份批次号码
        /// </summary>
        private string _BackupBlock = "";
        /// <summary>
        /// FTP操作原子对象
        /// </summary>
        private FtpAtomOperation ftpcmd;
        /// <summary>
        /// 任务信息
        /// </summary>
        private TaskInfo SiteObj;
        /// <summary>
        /// 批次信息
        /// </summary>
        private string block;
        /// <summary>
        /// 日志记录对象
        /// </summary>
        private DBLog dbLog;
        /// <summary>
        /// 成功计数
        /// </summary>
        private int successnum;
        /// <summary>
        /// 是否有错误
        /// </summary>
        private bool hasError;
        /// <summary>
        /// 文件二维数组,用与多线程传输
        /// </summary>
        private TaskFileInfo[][] file;
        /// <summary>
        /// 覆盖错误文件
        /// </summary>
        private TaskFileInfo _coverErrorFile;
        /// <summary>
        /// 覆盖失败服务器
        /// </summary>
        private WebSiteServer _coverErrorServer;
        #endregion

        #region 控制信息属性
        /// <summary>
        /// 是否多线程执行覆盖
        /// </summary>
        public bool MutiCover
        {
            get
            {
                return this.multiCover;
            }
            set
            {
                this.multiCover = value;
            }
        }
        /// <summary>
        /// 工作线程数量
        /// </summary>
        public int ThreadNumber
        {
            get
            {
                return this.threadNumber;
            }
            set
            {
                this.threadNumber = value;
            }
        }
        #endregion

        #region 构造函数
        public TaskWorker()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_TaskObj">任务对象</param>
        /// <param name="_dbLogObj">日志对象</param>
        public TaskWorker(TaskInfo _TaskObj, DBLog _dbLogObj)
        {
            this.SiteObj = _TaskObj;
            this.block = _dbLogObj.BlockStr;
            this.dbLog = _dbLogObj;
            this.ftpcmd = new FtpAtomOperation(_dbLogObj);
        }
        #endregion

        #region 任务总控制:同步
        /// <summary>
        /// 同步
        /// </summary>
        public void Execute()
        {
            try
            {
                //记录开始日志
                this.dbLog.Trace("站点同步", this.SiteObj.SiteBaseInfo.NAME, "开始同步");

                #region 服务器检查
                Msg msg = this.CheckServs(this.SiteObj);
                if (!msg.MsgResult)
                {
                    this.dbLog.Trace("检查服务器状态", this.SiteObj.SiteBaseInfo.NAME, "服务器网络连接出现问题:" + msg.MsgContent + "有服务器出现网络连接错误，请立即通知技术人员维护");
                    UploadTrace.TaskFailed(this.block);
                    return;
                }
                else
                {
                    this.dbLog.Trace("检查服务器状态", this.SiteObj.SiteBaseInfo.NAME, "一切正常");
                }
                #endregion

                #region 判定是否用户取消任务
                if (UploadTrace.IsTaskCancel(this.block))
                {
                    this.dbLog.Trace("上传站点", this.SiteObj.SiteBaseInfo.NAME, "失败，用户取消");
                    return;
                }
                #endregion

                #region 按照线程分割文件列表到线程任务中
                this.file = new TaskFileInfo[this.threadNumber][];
                for (int index = 0; index < this.threadNumber; ++index)
                {
                    this.file[index] = new TaskFileInfo[this.SiteObj.FilePairList.Count / this.threadNumber + 1];
                }

                for (int index = 0; index < this.SiteObj.FilePairList.Count; ++index)
                {
                    TaskFileInfo filePair = this.SiteObj.FilePairList[index];
                    this.file[index / (this.SiteObj.FilePairList.Count / this.threadNumber + 1)][index % (this.SiteObj.FilePairList.Count / this.threadNumber + 1)] = filePair;
                }
                #endregion

                #region 步骤一:上传文件到所有任务服务器
                if (!this.UploadSite())
                {
                    this.dbLog.Trace("上传站点", this.SiteObj.SiteBaseInfo.NAME, "失败，任务终止");
                    UploadTrace.TaskFailed(this.block);
                    return;
                }
                else
                {
                    this.dbLog.Trace("上传站点", this.SiteObj.SiteBaseInfo.NAME, "成功");
                }
                #endregion

                #region 判定是否用户取消任务
                if (UploadTrace.IsTaskCancel(this.block))
                {
                    this.dbLog.Trace("上传站点", this.SiteObj.SiteBaseInfo.NAME, "失败，用户取消");
                    return;
                }
                #endregion

                #region 步骤二:备份文件到所有任务服务器
                if (!this.BackupSite())
                {
                    this.dbLog.Trace("备份站点", this.SiteObj.SiteBaseInfo.NAME, "失败，任务终止");
                    UploadTrace.TaskFailed(this.block);
                    return;
                }
                else
                {
                    this.dbLog.Trace("备份站点", this.SiteObj.SiteBaseInfo.NAME, "成功");
                }
                UploadTrace.TaskBackuping(this.block);
                #endregion

                #region 判定用户是否取消任务
                if (UploadTrace.IsTaskCancel(this.block))
                {
                    this.dbLog.Trace("上传站点", this.SiteObj.SiteBaseInfo.NAME, "失败，用户取消");
                    return;
                }
                #endregion

                #region 步骤三:覆盖文件到所有任务服务器
                if (this.CoverSite())
                {
                    this.dbLog.Trace("覆盖站点", this.SiteObj.SiteBaseInfo.NAME, "成功");
                    this.dbLog.Trace("同步站点", this.SiteObj.SiteBaseInfo.NAME, "结束同步");
                    UploadTrace.TaskSuccess(this.block);
                    return;
                }
                else
                {
                    this.dbLog.Trace("覆盖站点", this.SiteObj.SiteBaseInfo.NAME, "失败，任务终止");
                }
                UploadTrace.TaskCovering(this.block);
                #endregion

                #region 步骤四:回滚文件到所有任务服务器
                if (this.RollBackSite())
                {
                    this.dbLog.Trace("覆盖回滚", this.SiteObj.SiteBaseInfo.NAME, "成功");
                }
                else
                {
                    this.dbLog.Trace("覆盖回滚", this.SiteObj.SiteBaseInfo.NAME, "失败，任务终止");
                }
                UploadTrace.TaskFailed(this.block);
                #endregion
            }
            catch (Exception ex)
            {
                this.dbLog.Trace("上传站点", this.SiteObj.SiteBaseInfo.NAME, "失败，任务终止。错误信息：" + ex.Message + ex.StackTrace);
                UploadTrace.TaskFailed(this.block);
            }
        }
        #endregion

        #region 服务器网络检查
        /// <summary>
        /// 服务器网络检查
        /// </summary>
        /// <param name="TaskObj">任务对象</param>
        /// <returns></returns>
        private Msg CheckServs(TaskInfo TaskObj)
        {
            Msg msg1 = new Msg("服务器网络检查", string.Empty, DateTime.Now, true);
            foreach (WebSiteServer aimServ in TaskObj.SiteBaseInfo.SERVLIST)
            {
                if (!this.ftpcmd.CheckFtpServer(aimServ.IP))
                {
                    Msg msg2 = msg1;
                    string str = msg2.MsgContent + "服务器[" + aimServ.NAME + "]出现网络问题，IP[" + aimServ.IP.ToString() + "]\\r\\n";
                    msg2.MsgContent = str;
                    msg1.MsgResult = false;
                }
            }
            return msg1;
        }
        #endregion

        #region 任务控制:上传
        /// <summary>
        /// 任务控制:上传
        /// </summary>
        /// <returns></returns>
        private bool UploadSite()
        {
            if (UploadTrace.IsTaskCancel(this.block))
            {
                return false;
            }
            try
            {
                for (int threadid = 0; threadid < this.file.Length; ++threadid)
                {
                    TaskWorker.doUpload doUpload = new TaskWorker.doUpload(this.UploadSiteThread);
                    doUpload.BeginInvoke(threadid, new AsyncCallback(this.AddMsgCallBack), (object)doUpload);
                }
            }
            catch
            {
                return false;
            }
            while (!this.hasError && this.successnum != this.SiteObj.FilePairList.Count)
            {
                Thread.Sleep(100);
            }
            return !this.hasError;
        }
        #endregion

        #region 任务控制:线程上传
        /// <summary>
        /// 任务控制:线程上传
        /// </summary>
        /// <param name="threadid"></param>
        private void UploadSiteThread(int threadid)
        {
            foreach (TaskFileInfo filePair in this.file[threadid])
            {
                if (filePair != null && !this.hasError)
                {
                    if (UploadTrace.IsTaskCancel(this.block))
                    {
                        this.hasError = true;
                        break;
                    }
                    else
                    {
                        try
                        {
                            FileStream fileStream = File.Open(filePair.SrcFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                            byte[] numArray = new byte[fileStream.Length];
                            fileStream.Read(numArray, 0, numArray.Length);
                            fileStream.Close();
                            fileStream.Dispose();
                            string name = this.SiteObj.SiteBaseInfo.NAME;
                            foreach (WebSiteServer ServObj in this.SiteObj.SiteBaseInfo.SERVLIST)
                            {
                                if (UploadTrace.IsTaskCancel(this.block))
                                {
                                    this.hasError = true;
                                    return;
                                }
                                else if (!this.UploadServ(ServObj, numArray, name, filePair, new FtpAtomOperation(this.dbLog)))
                                {
                                    this.hasError = true;
                                    return;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            UploadTrace.AddLog(filePair, this.block, "文件上传:" + filePair.FileName + "出错.出错原因:" + ex.Message);
                            UploadTrace.FileUploadFail(filePair, this.block, "");
                            this.hasError = true;
                            break;
                        }
                        UploadTrace.FileUploadSuccess(filePair, this.block);
                        ++this.successnum;
                    }
                }
            }
        }
        /// <summary>
        /// 上传文件到单个服务器
        /// </summary>
        /// <param name="ServObj"></param>
        /// <param name="FileBytes"></param>
        /// <param name="SiteName"></param>
        /// <param name="FileObj"></param>
        /// <param name="ftpcmdv3"></param>
        /// <returns></returns>
        private bool UploadServ(WebSiteServer ServObj, byte[] FileBytes, string SiteName, TaskFileInfo FileObj, FtpAtomOperation ftpcmdv3)
        {
            string AimUriStr1 = this.PreUpAimFolder(ServObj, FileObj);
            string AimUriStr2 = this.PreUpAimFile(ServObj, FileObj);
            if (!ftpcmdv3.CreateFtpListDirectory(ServObj.BaseUri, AimUriStr1, ServObj.USER, ServObj.PASS))
            {
                this.dbLog.Trace("[级联创建]单个服务器", SiteName, "[失败]___服务器名:" + ServObj.NAME + ",路径:" + AimUriStr1);
                return false;
            }
            else
            {
                try
                {
                    this.dbLog.Trace("[级联创建]单个服务器", SiteName, "[成功]___服务器名:" + ServObj.NAME + ",路径:" + AimUriStr1);
                    if (!ftpcmdv3.UploadFtpFile(ServObj.BaseUri, AimUriStr2, ServObj.USER, ServObj.PASS, FileBytes))
                    {
                        this.dbLog.Trace("[上传文件]单个服务器", SiteName, "[失败]___服务器名:" + ServObj.NAME + ",路径:" + AimUriStr2);
                        return false;
                    }
                    else
                    {
                        this.dbLog.Trace("[上传文件]单个服务器", SiteName, "[成功]___服务器名:" + ServObj.NAME + ",路径:" + AimUriStr2);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    this.dbLog.Trace("[上传文件]单个服务器", SiteName, "[失败]___服务器名:" + ServObj.NAME + ",路径:" + AimUriStr2 + ",错误信息:" + ex.Message);
                    return false;
                }
            }
        }

        private void AddMsgCallBack(IAsyncResult IAR)
        {
            ((TaskWorker.doUpload)IAR.AsyncState).EndInvoke(IAR);
        }
        /// <summary>
        /// 生成上传目录
        /// </summary>
        /// <param name="ServObj"></param>
        /// <param name="FileObj"></param>
        /// <returns></returns>
        private string PreUpAimFolder(WebSiteServer ServObj, TaskFileInfo FileObj)
        {
            if (!string.IsNullOrEmpty(ServObj.DIRPATH))
            {
                return ServObj.DIRPATH + "/" + ServObj.TRANSFOLDER + "/" + this.block + FileObj.RelAimFolder;
            }
            else
            {
                return ServObj.TRANSFOLDER + "/" + this.block + FileObj.RelAimFolder;
            }
        }
        /// <summary>
        /// 生成上传文件路径
        /// </summary>
        /// <param name="ServObj"></param>
        /// <param name="FileObj"></param>
        /// <returns></returns>
        private string PreUpAimFile(WebSiteServer ServObj, TaskFileInfo FileObj)
        {
            if (!string.IsNullOrEmpty(ServObj.DIRPATH))
            {
                return ServObj.DIRPATH + "/" + ServObj.TRANSFOLDER + "/" + this.block + FileObj.AimFile.Replace("../", "/");
            }
            else
            {
                return ServObj.TRANSFOLDER + "/" + this.block + FileObj.AimFile.Replace("../", "/");
            }
        }
        #endregion

        #region 任务控制:备份
        /// <summary>
        /// 任务控制:备份
        /// </summary>
        /// <returns></returns>
        private bool BackupSite()
        {
            this.successnum = 0;
            this._BackupBlock = this.dbLog.VMARKTEXT;
            try
            {
                for (int threadid = 0; threadid < this.file.Length; ++threadid)
                {
                    TaskWorker.doBackup doBackup = new TaskWorker.doBackup(this.BackSiteThread);
                    doBackup.BeginInvoke(threadid, new AsyncCallback(this.doBackupCallBack), (object)doBackup);
                }
            }
            catch
            {
                return false;
            }
            while (!this.hasError && this.successnum != this.SiteObj.FilePairList.Count)
                Thread.Sleep(100);
            return !this.hasError;
        }
        #endregion

        #region 任务控制:线程备份
        private void doBackupCallBack(IAsyncResult IAR)
        {
            ((TaskWorker.doBackup)IAR.AsyncState).EndInvoke(IAR);
        }

        private void BackSiteThread(int threadid)
        {
            string name = this.SiteObj.SiteBaseInfo.NAME;
            foreach (TaskFileInfo filePair in this.file[threadid])
            {
                if (filePair != null && !this.hasError)
                {
                    if (UploadTrace.IsTaskCancel(this.block))
                    {
                        this.hasError = true;
                        break;
                    }
                    else
                    {
                        try
                        {
                            string fileName = filePair.FileName;
                            byte[] backFileBytes = this.GetBackFileBytes(this.SiteObj.SiteBaseInfo.SERVLIST[0], filePair, this.SiteObj.SiteBaseInfo.LOCALBAKTRANSFOLDER, this.SiteObj.SiteBaseInfo.NAME);
                            if (backFileBytes != null)
                            {
                                foreach (WebSiteServer ServObj in this.SiteObj.SiteBaseInfo.SERVLIST)
                                {
                                    if (UploadTrace.IsTaskCancel(this.block))
                                    {
                                        this.hasError = true;
                                        return;
                                    }
                                    else if (this.BackupServ(ServObj, backFileBytes, name, filePair, this._BackupBlock, new FtpAtomOperation(this.dbLog)))
                                    {
                                        this.LogRollBack(ServObj, filePair, name, this._BackupBlock, "COVER");
                                        this.dbLog.Trace("[备份]单个服务器", name, "[成功]___服务器名:" + ServObj.NAME + ",文件名:" + fileName);
                                    }
                                    else
                                    {
                                        UploadTrace.FileBackupFail(filePair, this.block, ServObj.NAME);
                                        UploadTrace.AddLog(filePair, this.block, "文件备份:" + filePair.FileName + "到:" + ServObj.NAME + "出错.");
                                        this.dbLog.Trace("[备份]单个服务器", name, "[失败]___服务器名:" + ServObj.NAME + ",文件名:" + fileName);
                                        this.hasError = true;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            UploadTrace.AddLog(filePair, this.block, "文件备份:" + filePair.FileName + "出错.出错原因:" + ex.Message + ex.StackTrace);
                            UploadTrace.FileUploadFail(filePair, this.block, "");
                            this.hasError = true;
                            break;
                        }
                        UploadTrace.FileBackupSuccess(filePair, this.block);
                        ++this.successnum;
                    }
                }
            }
        }

        private bool BackupServ(WebSiteServer ServObj, byte[] FileBytes, string SiteName, TaskFileInfo FileObj, string BackupBlock, FtpAtomOperation ftpCmdV3)
        {
            string AimUriStr1 = this.PreBakAimFolder(ServObj, FileObj, BackupBlock);
            if (ftpCmdV3.CreateFtpListDirectory(ServObj.BaseUri, AimUriStr1, ServObj.USER, ServObj.PASS))
            {
                this.dbLog.Trace("[创建备份目录]", SiteName, "[成功]___服务器名:" + ServObj.NAME + ",路径:" + AimUriStr1);
                string AimUriStr2 = this.PreBakAimFile(ServObj, FileObj, BackupBlock);
                if (ftpCmdV3.UploadFtpFile(ServObj.BaseUri, AimUriStr2, ServObj.USER, ServObj.PASS, FileBytes))
                {
                    this.dbLog.Trace("[上传备份文件]单个服务器", SiteName, "[成功]___服务器名:" + ServObj.NAME + ",备份路径:" + AimUriStr2);
                    return true;
                }
                else
                {
                    this.dbLog.Trace("[上传备份文件]单个服务器", SiteName, "[失败]___服务器名:" + ServObj.NAME + ",备份路径:" + AimUriStr2);
                    return false;
                }
            }
            else
            {
                this.dbLog.Trace("[创建备份目录]", SiteName, "[失败]___服务器名:" + ServObj.NAME + ",路径:" + AimUriStr1);
                return false;
            }
        }
        /// <summary>
        /// 生成备份文件目录
        /// </summary>
        /// <param name="ServObj"></param>
        /// <param name="FileObj"></param>
        /// <param name="BackBlock"></param>
        /// <returns></returns>
        private string PreBakAimFolder(WebSiteServer ServObj, TaskFileInfo FileObj, string BackBlock)
        {
            if (!string.IsNullOrEmpty(ServObj.DIRPATH))
            {
                return ServObj.DIRPATH + "/" + ServObj.BAKFOLDER + "/" + BackBlock + FileObj.RelAimFolder;
            }
            else
            {
                return ServObj.BAKFOLDER + "/" + BackBlock + FileObj.RelAimFolder;
            }
        }
        /// <summary>
        /// 生成备份文件路径
        /// </summary>
        /// <param name="ServObj"></param>
        /// <param name="FileObj"></param>
        /// <param name="BackBlock"></param>
        /// <returns></returns>
        private string PreBakAimFile(WebSiteServer ServObj, TaskFileInfo FileObj, string BackBlock)
        {
            if (!string.IsNullOrEmpty(ServObj.DIRPATH))
            {
                return ServObj.DIRPATH + "/" + ServObj.BAKFOLDER + "/" + BackBlock + FileObj.AimFile.Replace("../", "/");
            }
            else
            {
                return ServObj.BAKFOLDER + "/" + BackBlock + FileObj.AimFile.Replace("../", "/");
            }
        }

        private byte[] GetBackFileBytes(WebSiteServer ServObj, TaskFileInfo FileObj, string LocalTransFolder, string SiteName)
        {
            bool flag = false;
            string ServRelFilePath = "/" + ServObj.DIRPATH + FileObj.AimFile.Replace("../", "/");
            if (this.ftpcmd.GetFtpFileSize(ServObj.BaseUri, ServRelFilePath, ServObj.USER, ServObj.PASS) != -1)
                flag = true;
            if (!flag)
                return (byte[])null;
            if (!Directory.Exists(LocalTransFolder))
                this.CreateLocalListDir(LocalTransFolder);
            if (!Directory.Exists(LocalTransFolder + "/" + this.block))
                this.CreateLocalListDir(LocalTransFolder + "/" + this.block);
            if (!Directory.Exists(LocalTransFolder + "/" + this.block + FileObj.RelAimFolder))
                this.CreateLocalListDir(LocalTransFolder + "/" + this.block + FileObj.RelAimFolder);
            //备份转存本地临时文件路径
            string str = LocalTransFolder + "/" + this.block + FileObj.RelAimFolder + "/" + FileObj.FileName;
            if (this.ftpcmd.DownLoadFtpFile(ServObj.BaseUri, ServRelFilePath, str, ServObj.USER, ServObj.PASS))
            {
                FileStream fileStream = File.Open(str, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                fileStream.Close();
                fileStream.Dispose();
                this.dbLog.Trace("[备份下载]下载文件", SiteName, "[成功]___服务器名:" + ServObj.NAME + ",文件名:" + ServRelFilePath);
                return buffer;
            }
            else
            {
                this.dbLog.Trace("[备份下载]下载文件", SiteName, "[失败]___服务器名:" + ServObj.NAME + ",文件名:" + ServRelFilePath);
                return (byte[])null;
            }
        }
        #endregion

        public bool RollBackSite()
        {
            try
            {
                //Trans.DataLayer.Model.Block BlockInfo = UploadTrace.GetBlockInfo(this.block);
                Trans.Db.Model.NBlock_Info BlockInfo = Trans.Db.Data.NBlock_Info.Get("BlockCode=@BlockCode", "", new object[] { this.block }, true);
                //string VMark = BlockInfo.VMark;
                string VMark = BlockInfo.ActionMark;
                this._BackupBlock = VMark;
                string name = this.SiteObj.SiteBaseInfo.NAME;
                DataTable rollBackTaskList = UploadTrace.getRollBackTaskList(this.block);
                List<TaskFileInfo> list = new List<TaskFileInfo>();
                foreach (DataRow dataRow in (InternalDataCollectionBase)rollBackTaskList.Rows)
                {
                    string SrcPath = dataRow["FilePath"].ToString().Replace("..", "");
                    string AimPath = dataRow["FilePath"].ToString();
                    list.Add(new TaskFileInfo(SrcPath, AimPath));
                }
                int count = this.SiteObj.SiteBaseInfo.SERVLIST.Count;
                int num = 1;
                foreach (WebSiteServer ServObj in this.SiteObj.SiteBaseInfo.SERVLIST)
                {
                    string serverstatus = num.ToString() + "/" + count.ToString();
                    foreach (TaskFileInfo FileObj in list)
                    {
                        if (this._coverErrorServer == null)// || !(FileObj.FileName == this._coverErrorFile.FileName) || !(ServObj.NAME == this._coverErrorServer.NAME))
                        {
                            this.RollbackServ(ServObj, FileObj, name, this.block, serverstatus);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    ++num;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool RollbackServ(WebSiteServer ServObj, TaskFileInfo FileObj, string SiteName, string block, string serverstatus)
        {
            string SrcUriStr = "";
            //如果_BackupBlock不为空，则为同步任务失败的回滚，否则为延后回滚
            if (!string.IsNullOrEmpty(this._BackupBlock))
            {
                SrcUriStr = this.PreBakAimFile(ServObj, FileObj, this._BackupBlock).Replace("//", "/");
            }
            else
            {
                SrcUriStr = this.PreBakAimFile(ServObj, FileObj, this.block).Replace("//", "/");
            }
            string AimUriStr = this.PreCoverAimFile(ServObj, FileObj);
            try
            {
                this.dbLog.Trace("[级联检查/回滚]覆盖目标目录", SiteName, "[成功]___服务器名:" + ServObj.NAME + ",路径:" + FileObj.RelAimFolder);
                if (this.ftpcmd.RenameFtpFile(ServObj.BaseUri, SrcUriStr, AimUriStr, ServObj.USER, ServObj.PASS))
                {
                    UploadTrace.FileRollBackSuccess(FileObj, block, serverstatus);
                    this.AutoRollBackSuccess(ServObj, FileObj, SiteName, this._BackupBlock, "COVER", block);
                    this.dbLog.Trace("[文件回滚]单个服务器", SiteName, "[成功]___服务器名:" + ServObj.NAME + ",文件源位置:" + SrcUriStr + ",文件目标位置:" + AimUriStr);
                    return true;
                }
                else
                {
                    UploadTrace.FileRollBackFail(FileObj, block, ServObj.NAME, serverstatus);
                    UploadTrace.AddLog(FileObj, block, "文件回滚:" + FileObj.FileName + "到:" + ServObj.NAME + "出错.");
                    this.dbLog.Trace("[文件回滚]单个服务器", SiteName, "[失败]___服务器名:" + ServObj.NAME + ",文件源位置:" + SrcUriStr + ",文件目标位置:" + AimUriStr);
                    this._coverErrorServer = ServObj;
                    this._coverErrorFile = FileObj;
                    return false;
                }
            }
            catch
            {
                UploadTrace.FileRollBackFail(FileObj, block, ServObj.NAME, serverstatus);
                UploadTrace.AddLog(FileObj, block, "文件回滚:" + FileObj.FileName + "到:" + ServObj.NAME + "出错.");
                this.dbLog.Trace("[文件回滚]单个服务器", SiteName, "[失败]___服务器名:" + ServObj.NAME + ",文件源位置:" + SrcUriStr + ",文件目标位置:" + AimUriStr);
                return false;
            }
        }

        private void CreateLocalListDir(string LocalDir)
        {
            if (Directory.Exists(LocalDir))
            {
                return;
            }
            LocalDir = LocalDir.TrimEnd('/');
            string ParentPath = LocalDir.Substring(0, LocalDir.LastIndexOf('/'));
            if (!Directory.Exists(ParentPath))
            {
                CreateLocalListDir(ParentPath);
            }
            Directory.CreateDirectory(LocalDir);
        }

        #region 记录回滚系统所需操作
        /// <summary>
        /// 记录回滚系统所需操作
        /// </summary>
        /// <param name="ServObj"></param>
        /// <param name="FileObj"></param>
        /// <param name="SiteName"></param>
        /// <param name="MarkBlock"></param>
        /// <param name="RollOp"></param>
        private void LogRollBack(WebSiteServer ServObj, TaskFileInfo FileObj, string SiteName, string MarkBlock, string RollOp)
        {
            string str1 = FileObj.RelAimFolder + "/" + FileObj.FileName;
            string str2 = "/" + ServObj.BAKFOLDER + "/" + MarkBlock + str1;
            this.dbLog.RollBakLog(MarkBlock, ServObj.IP.ToString(), ServObj.PORT, str2, str1, SiteName, RollOp, this.block);
        }
        #endregion

        private void AutoRollBackSuccess(WebSiteServer ServObj, TaskFileInfo FileObj, string SiteName, string MarkBlock, string RollOp, string block)
        {
            string str1 = FileObj.RelAimFolder + "/" + FileObj.FileName;
            string str2 = "/" + ServObj.BAKFOLDER + "/" + MarkBlock + str1;
            this.dbLog.RollBakAutoSuccess(MarkBlock, ServObj.IP.ToString(), ServObj.PORT, str2, str1, SiteName, RollOp, this.block);
        }

        #region 任务控制:覆盖
        /// <summary>
        /// 任务控制:覆盖
        /// </summary>
        /// <returns></returns>
        private bool CoverSite()
        {
            if (!this.multiCover)
            {
                string name = this.SiteObj.SiteBaseInfo.NAME;
                int count = this.SiteObj.SiteBaseInfo.SERVLIST.Count;
                int num = 0;
                foreach (WebSiteServer ServObj in this.SiteObj.SiteBaseInfo.SERVLIST)
                {
                    string coverStatus = num.ToString() + (object)"/" + (string)(object)count;
                    foreach (TaskFileInfo filePair in this.SiteObj.FilePairList)
                    {
                        if (this.CoverServ(ServObj, filePair, name, new FtpAtomOperation(this.dbLog)))
                        {
                            this.dbLog.Trace("[覆盖]单个服务器", name, "[成功]___服务器名:" + ServObj.NAME + "文件路径:" + filePair.AimFile);
                            UploadTrace.FileCoverSuccess(filePair, this.block, coverStatus);
                        }
                        else
                        {
                            UploadTrace.FileCoverFail(filePair, this.block, ServObj.NAME, coverStatus);
                            this._coverErrorFile = filePair;
                            this._coverErrorServer = ServObj;
                            UploadTrace.AddLog(filePair, this.block, "覆盖文件:" + filePair.FileName + "到:" + ServObj.NAME + "出错.准备回滚");
                            this.dbLog.Trace("[覆盖]单个服务器", name, "[失败]___服务器名:" + ServObj.NAME + "文件路径:" + filePair.AimFile);
                            return false;
                        }
                    }
                    ++num;
                }
                return true;
            }
            else
            {
                this.successnum = 0;
                if (UploadTrace.IsTaskCancel(this.block))
                    return false;
                try
                {
                    foreach (WebSiteServer serv in this.SiteObj.SiteBaseInfo.SERVLIST)
                    {
                        TaskWorker.doCover doCover = new TaskWorker.doCover(this.CoverSiteThread);
                        doCover.BeginInvoke(serv, new AsyncCallback(this.doCoverCallBack), (object)doCover);
                    }
                }
                catch
                {
                    return false;
                }
                while (!this.hasError && this.successnum != this.SiteObj.FilePairList.Count * this.SiteObj.SiteBaseInfo.SERVLIST.Count)
                    Thread.Sleep(100);
                return !this.hasError;
            }
        }
        #endregion

        #region 任务控制:线程覆盖
        private void doCoverCallBack(IAsyncResult IAR)
        {
            TaskWorker.doCover doCover = (TaskWorker.doCover)IAR.AsyncState;
            doCover.EndInvoke(IAR);
            doCover.Clone();
        }

        private void CoverSiteThread(WebSiteServer ServObj)
        {
            string name = this.SiteObj.SiteBaseInfo.NAME;
            int count = this.SiteObj.SiteBaseInfo.SERVLIST.Count;
            foreach (TaskFileInfo filePair in this.SiteObj.FilePairList)
            {
                if (!this.hasError)
                {
                    if (this.CoverServ(ServObj, filePair, name, new FtpAtomOperation(this.dbLog)))
                    {
                        this.dbLog.Trace("[覆盖]单个服务器", name, "[成功]___服务器名:" + ServObj.NAME + "文件路径:" + filePair.AimFile);
                        UploadTrace.AddLog(filePair, this.block, "覆盖文件:" + filePair.FileName + "到:" + ServObj.NAME + "成功.");
                        ++this.successnum;
                        UploadTrace.FileCoverSuccess(filePair, this.block, "");
                    }
                    else
                    {
                        UploadTrace.FileCoverFail(filePair, this.block, ServObj.NAME, "");
                        this._coverErrorFile = filePair;
                        this._coverErrorServer = ServObj;
                        UploadTrace.AddLog(filePair, this.block, "覆盖文件:" + filePair.FileName + "到:" + ServObj.NAME + "出错.准备回滚");
                        this.dbLog.Trace("[覆盖]单个服务器", name, "[失败]___服务器名:" + ServObj.NAME + "文件路径:" + filePair.AimFile);
                        this.hasError = true;
                        break;
                    }
                }
            }
        }

        private bool CoverServ(WebSiteServer ServObj, TaskFileInfo FileObj, string SiteName, FtpAtomOperation ftpCmdV3)
        {
            string SrcUriStr = this.PreCoverSrcFile(ServObj, FileObj);
            string AimUriStr = this.PreCoverAimFile(ServObj, FileObj);
            if (ftpCmdV3.CreateFtpListDirectory(ServObj.BaseUri, "/" + ServObj.DIRPATH + FileObj.RelAimFolder, ServObj.USER, ServObj.PASS))
            {
                this.dbLog.Trace("[级联检查/创建]覆盖目标目录", SiteName, "[成功]___服务器名:" + ServObj.NAME + ",路径:" + FileObj.RelAimFolder);
                if (ftpCmdV3.RenameFtpFile(ServObj.BaseUri, SrcUriStr, AimUriStr, ServObj.USER, ServObj.PASS))
                {
                    this.dbLog.Trace("[文件移动]单个服务器", SiteName, "[成功]___服务器名:" + ServObj.NAME + ",文件源位置:" + SrcUriStr + ",文件目标位置:" + AimUriStr);
                    return true;
                }
                else
                {
                    this.dbLog.Trace("[文件移动]单个服务器", SiteName, "[失败]___服务器名:" + ServObj.NAME + ",文件源位置:" + SrcUriStr + ",文件目标位置:" + AimUriStr);
                    return false;
                }
            }
            else
            {
                this.dbLog.Trace("[级联检查/创建]覆盖目标目录", SiteName, "[失败]___服务器名:" + ServObj.NAME + ",路径:" + FileObj.RelAimFolder);
                return false;
            }
        }
        /// <summary>
        /// 生成覆盖操作源文件目录
        /// </summary>
        /// <param name="ServObj"></param>
        /// <param name="FileObj"></param>
        /// <returns></returns>
        private string PreCoverSrcFile(WebSiteServer ServObj, TaskFileInfo FileObj)
        {
            if (!string.IsNullOrEmpty(ServObj.DIRPATH))
            {
                return "/" + ServObj.DIRPATH + "/" + ServObj.TRANSFOLDER + "/" + this.block + (string.IsNullOrEmpty(FileObj.RelAimFolder) ? "" : FileObj.RelAimFolder) + "/" + FileObj.FileName;
            }
            else
            {
                return "/" + ServObj.TRANSFOLDER + "/" + this.block + (string.IsNullOrEmpty(FileObj.RelAimFolder) ? "" : FileObj.RelAimFolder) + "/" + FileObj.FileName;
            }
        }
        /// <summary>
        /// 生成覆盖操作目标文件路径
        /// </summary>
        /// <param name="ServObj"></param>
        /// <param name="FileObj"></param>
        /// <returns></returns>
        private string PreCoverAimFile(WebSiteServer ServObj, TaskFileInfo FileObj)
        {
            if (!string.IsNullOrEmpty(ServObj.DIRPATH))
            {
                return "/" + ServObj.DIRPATH + (string.IsNullOrEmpty(FileObj.RelAimFolder) ? "" : FileObj.RelAimFolder) + "/" + FileObj.FileName;
            }
            else
            {
                return (string.IsNullOrEmpty(FileObj.RelAimFolder) ? "" : FileObj.RelAimFolder) + "/" + FileObj.FileName;
            }
        }
        #endregion

        private delegate void doUpload(int threadid);

        private delegate void doBackup(int threadid);

        private delegate void doCover(WebSiteServer serv);
    }
}