using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Utils
{
    /// <summary>
    /// 错误集合类
    /// </summary>
    public class Error
    {
        /// <summary>
        /// 1000 - 参数错误！
        /// </summary>
        public static Models.RespError Error1000 = new Models.RespError { Code = 1000, Message = "参数错误！" };
        /// <summary>
        /// 1001 - 用户名或密码错误！请重试！
        /// </summary>
        public static Models.RespError Error1001 = new Models.RespError { Code = 1001, Message = "用户名或密码错误！请重试！" };
        /// <summary>
        /// 1002 - 抱歉！登录状态已失效！请重新登录！
        /// </summary>
        public static Models.RespError Error1002 = new Models.RespError { Code = 1002, Message = "抱歉！登录状态已失效！请重新登录！" };
    }
}