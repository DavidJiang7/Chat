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
            username = username.Trim();
            password = password.Trim();
            Models.RespObject ret = new Models.RespObject();
            if (username == "" || password == "")
            {
                ret.Code = Utils.Error.Error1000.Code;
                ret.Message = Utils.Error.Error1000.Message;
                ret.Data = null;
                return Json(ret);
            }
            var res = Servers.Service.Login(username, password);
            if (res == null)
            {
                ret.Code = Utils.Error.Error1001.Code;
                ret.Message = Utils.Error.Error1001.Message;
                ret.Data = null;
                return Json(ret);
            }
            else
            {
                Session["ServiceId"] = res.id;
                Session["ServiceUserName"] = res.A_UserName;
                ret.Code = 0;
                ret.Message = "";
                ret.Data = new { UserName = res.A_UserName, UserId = res.id, NickName = res.A_RealName, Img = res.A_Icon };
            }
            return Json(ret);
        }

        /// <summary>
        /// 根据 客服id 获取最近 dayOffset 天数内的聊天记录
        /// </summary>
        /// <param name="userId">客服id</param>
        /// <param name="dayOffset">最近多少天内</param>
        /// <returns></returns>
        public ActionResult GetCurrentMess(int userId, int dayOffset)
        {
            Models.RespObject ret = new Models.RespObject();
            if (userId <= 0 || dayOffset < 0)
            {
                ret.Code = Utils.Error.Error1000.Code;
                ret.Message = Utils.Error.Error1000.Message;
                ret.Data = null;
                return Json(ret);
            }
            var id = Session["ServiceId"];
            if (id == null || (int)id != userId)
            {
                ret.Code = Utils.Error.Error1002.Code;
                ret.Message = Utils.Error.Error1002.Message;
                ret.Data = null;
                return Json(ret);
            }
            else
            {
                dayOffset = dayOffset > 30 ? 30 : (dayOffset <= 0 ? 7 : dayOffset);
                var data = Servers.Mess.GetCurrentMess(userId, dayOffset);
                ret.Code = 0;
                ret.Message = "";
                ret.Data = data;
            }
            return Json(ret);
        }
    }
}