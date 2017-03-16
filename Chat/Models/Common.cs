using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    /// <summary>
    /// http请求的返回结果对象
    /// </summary>
    public class RespObject : RespError
    {
        /// <summary>
        /// 请求成功后返回的数据对象
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// http请求错误类
    /// </summary>
    public class RespError
    {
        /// <summary>
        /// 请求处理结果代号。一般 0 表示成功，其他未错误码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }
    }

}