using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    /// <summary>
    /// 客户信息
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户客户端链接id
        /// </summary>
        public string FromConnectionId { get; set; }
        /// <summary>
        /// 与用户聊天的对方客户端链接id
        /// </summary>
        public string ToConnectionId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 账户名
        /// </summary>
        public string UserName { get; set; }
        ///// <summary>
        ///// 用户昵称
        ///// </summary>
        //public string NickName { get; set; }
        /// <summary>
        /// 头像url
        /// </summary>
        public string Img { get; set; }
        ///// <summary>
        ///// 部门名称
        ///// </summary>
        //public string DeptName { get; set; }
        ///// <summary>
        ///// 登录时间
        ///// </summary>
        //public DateTime LoginTime { get; set; }
    }


    /// <summary>
    /// 客服信息
    /// </summary>
    public class Service
    {
        /// <summary>
        /// 用户客户端链接id
        /// </summary>
        public string FromConnectionId { get; set; }
        ///// <summary>
        ///// 与用户聊天的对方客户端链接id
        ///// （备注：客服用户该字段为空，因为一个客服可接受多个client用户访问，所以对于客服来说此字段无意义）
        ///// </summary>
        //public string ToConnectionId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 账户名
        /// </summary>
        public string UserName { get; set; }
        ///// <summary>
        ///// 用户昵称
        ///// </summary>
        //public string NickName { get; set; }
        /// <summary>
        /// 头像url
        /// </summary>
        public string Img { get; set; }
        ///// <summary>
        ///// 部门名称
        ///// </summary>
        //public string DeptName { get; set; }
        ///// <summary>
        ///// 登录时间
        ///// </summary>
        //public DateTime LoginTime { get; set; }
        /// <summary>
        /// 客服同时服务的最多客户人数
        /// </summary>
        public int ServiceMax { set; get; }
        /// <summary>
        /// 当前正在服务的客户人数
        /// </summary>
        public int ServiceNow { set; get; }
    }
}