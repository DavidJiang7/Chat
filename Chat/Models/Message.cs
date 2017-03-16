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
    /// 客服登录成功后，查询最近聊天记录，此类为一个客户的聊天信息类
    /// （返回的RespObject中的Data对象类）
    /// </summary>
    public class RespServiceCurrentMess
    {
        /// <summary>
        /// 客户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 客户用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 客户头像
        /// </summary>
        public string Img { get; set; }
        /// <summary>
        /// 客户最新20条聊天记录
        /// </summary>
        public Message[] Mess { get; set; }
    }
    
}