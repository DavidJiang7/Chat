using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Servers
{
    public class Service
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">账户名</param>
        /// <param name="password">密码</param>
        /// <returns>错误码：0-登录成功</returns>
        public static Base_Admins Login(string userName, string password)
        {
            try
            {
                //登录验证，可调用麦子网后台api
                using (ChatEntities db = new ChatEntities())
                {
                    return db.Base_Admins.FirstOrDefault(it => it.A_UserName == userName && it.A_Loginpwd == password);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}