using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.Controllers
{
    public class ServiceController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">账户名</param>
        /// <param name="pass">密码</param>
        /// <returns></returns>
        public ActionResult LoginApi(string username, string password)
        {
            Models.RespObject ret = new Models.RespObject();
            var res = Servers.Service.Login(username, password);
            if (res == null)
            {
                ret.Code = 101;
                ret.Message = "用户名或密码错误！请重试！";
                ret.Data = null;
            }
            else
            {
                ret.Code = 0;
                ret.Message = "";
                ret.Data = new { UserName = res.A_UserName, UserId = res.id, NickName = res.A_RealName, Img = res.A_Icon };
            }
            return Json(ret);
        }

        public ActionResult CurrentMess(int userId)
        {
            return null;
        }
    }
}