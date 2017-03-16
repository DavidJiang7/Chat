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
        [HttpPost]
        public JsonResult LoginApi(string username, string password)
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
        /// 客服获取最近 dayOffset 天数内的聊天记录
        /// </summary>
        /// <param name="messSize">获取每位客户最新messSize条记录，参数非必需，默认20条，最大50条</param>
        /// <param name="dayOffset">最近多少天内，参数非必需，默认7天内，最大30天内</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCurrentMess(int dayOffset = 7, int messSize = 20)
        {
            Models.RespObject ret = new Models.RespObject();
            if (dayOffset < 0)
            {
                ret.Code = Utils.Error.Error1000.Code;
                ret.Message = Utils.Error.Error1000.Message;
                ret.Data = null;
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            var id = Session["ServiceId"];
            if (id == null || (int)id <= 0)
            {
                ret.Code = Utils.Error.Error1002.Code;
                ret.Message = Utils.Error.Error1002.Message;
                ret.Data = null;
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            else
            {
                dayOffset = dayOffset > 30 ? 30 : (dayOffset <= 0 ? 7 : dayOffset);
                messSize = messSize > 50 ? 50 : (messSize <= 0 ? 20 : messSize);
                var data = Servers.Mess.GetCurrentMess((int)id, dayOffset, messSize);
                ret.Code = 0;
                ret.Message = "";
                ret.Data = data;
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据 客户id， 客服获取更多的历史聊天记录
        /// </summary>
        /// <param name="userId">客户id</param>
        /// <param name="offset">数据偏移量</param>
        /// <param name="messSize">获取数量，参数非必需，默认20条，最大50条</param>
        /// <returns></returns>
        public JsonResult GetMoreMess(int userId, int offset, int messSize = 20)
        {
            Models.RespObject ret = new Models.RespObject();
            if (userId < 0)
            {
                ret.Code = Utils.Error.Error1000.Code;
                ret.Message = Utils.Error.Error1000.Message;
                ret.Data = null;
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            var id = Session["ServiceId"];
            if (id == null || (int)id <= 0)
            {
                ret.Code = Utils.Error.Error1002.Code;
                ret.Message = Utils.Error.Error1002.Message;
                ret.Data = null;
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            else
            {
                offset = offset <= 0 ? 0 : offset;
                messSize = messSize > 50 ? 50 : (messSize <= 0 ? 20 : messSize);
                var data = Servers.Mess.GetMoreMess(userId, (int)id, offset, messSize);
                ret.Code = 0;
                ret.Message = "";
                ret.Data = data;
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
    }
}