using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Utils
{
    /// <summary>
    /// 写日志静态类
    /// </summary>
    public class Log
    {
        /// <summary>
        /// 写入程序错误日志文件
        /// 默认路径示例：log\log2016-11-18-13-44-38-502.txt
        /// </summary>
        /// <param name="str">异常信息</param>
        public static void WriteLog(string str)
        {
            if (!Directory.Exists("log"))
            {
                Directory.CreateDirectory("log");
            }
            DateTime d = DateTime.Now;
            using (var sw = new StreamWriter(String.Format(@"log\log{0}-{1}-{2}-{3}-{4}-{5}-{6}.txt", d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, d.Millisecond), true))
            {
                sw.WriteLine(str);
                sw.WriteLine("---------------------------------------------------------");
                sw.Close();
            }
        }       
    }
}
