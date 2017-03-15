using System.Text.RegularExpressions;

namespace Chat.Utils
{
    /// <summary>
    /// 处理字符串的扩展方法静态类
    /// </summary>
    public class Strings
    {
        /// <summary>
        /// 是否正整数
        /// </summary>
        /// <param name="s">需验证的字符串</param>
        /// <returns></returns>
        public static bool IsPositiveInt(string s)
        {
            Regex reg = new Regex(@"^[1-9]\d*$");
            return reg.IsMatch(s);
        }        
    }
}
