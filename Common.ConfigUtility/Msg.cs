using System;

namespace Common.ConfigUtility
{
    /// <summary>
    /// 同步消息
    /// </summary>
    public class Msg
    {
        public Msg()
        {
            this.MsgName = string.Empty;
            this.MsgContent = string.Empty;
        }
        /// <summary>
        /// 同步消息对象
        /// </summary>
        /// <param name="Name">消息名称</param>
        /// <param name="Content">描述信息</param>
        /// <param name="Time">发生时间</param>
        /// <param name="Result">是否成功</param>
        public Msg(string Name, string Content, DateTime Time, bool Result)
        {
            this.MsgName = Name;
            this.MsgContent = Content;
            this.MsgTime = Time;
            this.MsgResult = Result;
        }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string MsgContent { get; set; }
        /// <summary>
        /// 消息名称
        /// </summary>
        public string MsgName { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool MsgResult { get; set; }
        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime MsgTime { get; set; }
    }
}