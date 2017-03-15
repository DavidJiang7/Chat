using System;

namespace Chat.Utils
{
    /// <summary>
    /// 随机数生成器
    /// </summary>
    public class Rand
    {
        /// <summary>
        /// 返回[min,max)之间的任意整数
        /// </summary>
        /// <param name="min">最小值，可取</param>
        /// <param name="max">最大值，不可取</param>
        /// <returns></returns>
        public static int Int(int min, int max)
        {
            Random r = new Random();
            return r.Next(min, max);
        }
    }
}
