using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    /// <summary>
    /// 消息作者枚举
    /// </summary>
    public enum MessageAuthor
    {
        /// <summary>
        /// 1-客户：客户->客服
        /// </summary>
        User = 1,
        /// <summary>
        /// 2-客服：客服->客户
        /// </summary>
        Service = 2,
    }

    /// <summary>
    /// 客服登录成功后，查询最近聊天记录，返回的RespObject中的Data对象类
    /// </summary>
    public class RespServiceMessage
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string  Img { get; set; }
    }
}