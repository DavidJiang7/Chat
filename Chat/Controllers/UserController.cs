using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">账户名</param>
        /// <param name="pass">密码</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LoginApi(string username, string password)
        {
            HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            try
            {
                Models.RespObject ret = new Models.RespObject();
                username = username.Trim();
                password = password.Trim();
                if (username == "" || password == "")
                {
                    ret.Code = Utils.Error.Error1000.Code;
                    ret.Message = Utils.Error.Error1000.Message;
                    ret.Data = null;
                    return Json(ret);
                }
                var res = Servers.User.Login(username, password);
                if (res == null)
                {
                    ret.Code = Utils.Error.Error1001.Code;
                    ret.Message = Utils.Error.Error1001.Message;
                    ret.Data = null;
                    return Json(ret);
                }
                else
                {
                    ret.Code = 0;
                    ret.Message = "";
                    ret.Data = new { UserName = res.U_UserName, UserId = res.id, NickName = res.U_NickName, Img = res.U_Icon };
                }
                return Json(ret);
            }
            catch
            {
                return null;
            }
        }
    }
}