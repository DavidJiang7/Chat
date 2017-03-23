using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Chat
{
    /// <summary>
    /// 客户端通讯集线器类
    /// </summary>
    public class ChatHub : Hub
    {
        #region 客户User
        /// <summary>
        /// 客户登录用户池
        /// </summary>
        private static List<Models.User> ConnectedUsers = new List<Models.User>();

        /// <summary>
        /// user -> service 发送消息方法
        /// </summary>
        /// <param name="toConnectionId">对方id</param>
        /// <param name="name">发送者用户名</param>
        /// <param name="message">消息内容</param>
        public void USend(string toConnectionId, string name, string message)
        {
            toConnectionId = toConnectionId.Trim();
            name = name.Trim();
            message = message.Trim();
            if (toConnectionId == "" || name == "" || message == "")
            {
                return;
            }
            var u = ConnectedUsers.FirstOrDefault(it => it.FromConnectionId == Context.ConnectionId);
            if (u == null)
            {
                Clients.Caller.loginWarm();
                return;
            }
            var s = ConnectedServices.FirstOrDefault(it => it.FromConnectionId == toConnectionId);
            if (s == null)
            {
                Clients.Caller.loseServiceWarm();
                return;
            }
            Clients.Client(toConnectionId).addNewMessageToPage(name, message);
            Servers.Mess.AddMessage(u.UserId, s.UserId, 1, message, Models.MessageAuthor.User);
        }


        ////广播信息到所有client
        //public void USendAllServer(string name, string message)
        //{
        //    Clients.All.SendAllClient("<li>[" + name + "]：" + message + " - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</li>");
        //}

        ///// <summary>
        ///// 服务器端退出的消息回调
        ///// </summary>
        //public void ULogoutFromServer()
        //{
        //    //做一些清理登录状态信息，例如ConnectedUsers中移除指定对象
        //}

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userID">客户id</param>
        /// <param name="userName">客户用户名</param>
        /// <param name="img">客户头像</param>
        public void UConnect(int userID, string userName, string img)
        {
            if (userID <= 0 || userName == "")
            {
                //提醒用户先登录，有可能用户把cookies清理了，则登录失效
                Clients.Caller.loginWarm();
                return;
            }
            //Context.ConnectionId 同一id表示同一链接状态，可以理解为sessionid，但两者不是一个东西
            lock (ConnectedUsers)
            {
                int index = ConnectedUsers.FindIndex(it => it.FromConnectionId == Context.ConnectionId);
                if (index < 0)
                //if (ConnectedUsers.Count(it => it.ConnectionId == id) == 0)
                {
                    //当前客户端的新登录链接
                    var items = ConnectedUsers.Where(it => it.UserId == userID).ToList();
                    if (items != null && items.Count > 0)
                    {
                        //下线同一账户的其他登录状态，并清理线程池(无效链接)
                        foreach (var item in items)
                        {
                            Clients.Client(item.FromConnectionId).onUserDisconnected(item.UserName);
                        }
                        ConnectedUsers.RemoveAll(it => it.UserId == userID);
                    }
                    Models.Service service = UGetRandService();
                    if (service == null)
                    {
                        Clients.Caller.busy();//未分配到客服，提醒客户，客服正忙
                    }
                    else
                    {
                        //添加在线人员，并分配服务人员 
                        ConnectedUsers.Add(new Models.User { FromConnectionId = Context.ConnectionId, ToConnectionId = service.FromConnectionId, UserId = userID, UserName = userName, Img = img });
                        //反馈登录成功信息给用户，并说明已为其分配的客服人员信息
                        Clients.Caller.onConnected(service.FromConnectionId, service.UserName, service.Img);
                        //提醒客服有新的客户需要咨询
                        Clients.Client(service.FromConnectionId).addNewClientTagToPage(Context.ConnectionId, userName, img);
                        ////通知其他所有用户，有新用户链接进来
                        //Clients.AllExcept(id).onNewUserConnected(id, userID, userName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
                else
                {
                    ////已登录，更新在线状态信息
                    //ConnectedUsers[index].LoginTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 为客户端随机分配已登录客服人员
        /// </summary>
        /// <param name="FromUserName">客户用户名</param>
        /// <returns>返回一位已登录的客户信息</returns>
        public Models.Service UGetRandService()
        {
            try
            {
                lock (ConnectedServices)
                {
                    if (ConnectedServices != null && ConnectedServices.Count > 0)
                    {
                        Models.Service result = ConnectedServices.Where(it => it.ServiceMax > it.ServiceNow).OrderBy(it => it.ServiceNow).FirstOrDefault();
                        if (result != null)
                        {
                            //int index = ConnectedServices.FindIndex(it => it.FromConnectionId == result.FromConnectionId);
                            //if (index >= 0)
                            //{
                            //    ConnectedServices[index].ServiceNow += 1;
                            //    result.ServiceNow += 1;
                            //    return result;
                            //}
                            result.ServiceNow += 1;
                            return result;
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion


        #region 客服Service
        /// <summary>
        /// 客服登录用户池
        /// </summary>
        private static List<Models.Service> ConnectedServices = new List<Models.Service>();

        /// <summary>
        /// user -> service 发送消息方法
        /// </summary>
        /// <param name="toConnectionId">对方id</param>
        /// <param name="name">发送者用户名</param>
        /// <param name="message">消息内容</param>
        public void SSend(string toConnectionId, string name, string message)
        {
            toConnectionId = toConnectionId.Trim();
            name = name.Trim();
            message = message.Trim();
            if (toConnectionId == "" || name == "" || message == "")
            {
                return;
            }
            var s = ConnectedServices.FirstOrDefault(it => it.FromConnectionId == Context.ConnectionId);
            if (s == null)
            {
                Clients.Caller.loginWarm();
                return;
            }
            var u = ConnectedUsers.FirstOrDefault(it => it.FromConnectionId == toConnectionId);
            if (u == null)
            {
                Clients.Caller.loseUserWarm();
                return;
            }
            Clients.Client(toConnectionId).addNewMessageToPage(name, message);
            Servers.Mess.AddMessage(u.UserId, s.UserId, 1, message, Models.MessageAuthor.Service);
        }


        ////广播信息到所有client
        //public void SSendAllServer(string name, string message)
        //{
        //    Clients.All.SendAllClient("<li>[" + name + "]：" + message + " - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</li>");
        //}

        ///// <summary>
        ///// 服务器端退出的消息回调
        ///// </summary>
        //public void SLogoutFromServer()
        //{
        //    //做一些清理登录状态信息，例如ConnectedServices中移除指定对象
        //}

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userID">用户id</param>
        /// <param name="userName">用户名</param>
        /// <param name="img">头像</param>
        /// <param name="serviceMax">同时服务最多客户人数</param>
        public void SConnect(int userID, string userName, string img, int serviceMax)
        {
            if (userID <= 0 || userName == "")
            {
                //提醒用户先登录，有可能用户把cookies清理了，则登录失效
                Clients.Caller.loginWarm();
                return;
            }
            string id = Context.ConnectionId;//同一id表示同一链接状态，可以理解为sessionid，但两者不是一个东西
            lock (ConnectedServices)
            {
                int index = ConnectedServices.FindIndex(it => it.FromConnectionId == id);
                if (index < 0)
                //if (ConnectedServices.Count(it => it.ConnectionId == id) == 0)
                {
                    //当前客户端的新登录链接
                    var items = ConnectedServices.Where(it => it.UserId == userID).ToList();
                    if (items != null && items.Count > 0)
                    {
                        //下线同一账户的其他登录状态，并清理线程池(无效链接)
                        foreach (var item in items)
                        {
                            Clients.Client(item.FromConnectionId).onUserDisconnected(item.UserName);
                            Clients.Client(item.FromConnectionId).onDisconnected(true);
                        }
                        ConnectedServices.RemoveAll(it => it.UserId == userID);
                    }
                    //添加在线人员，并分配服务人员 
                    ConnectedServices.Add(new Models.Service { FromConnectionId = id, UserId = userID, UserName = userName, Img = img, ServiceMax = serviceMax });
                }
                else
                {
                    ////已登录，更新在线状态信息
                    //ConnectedServices[index].LoginTime = DateTime.Now;
                }
            }
        }

        #endregion


        #region 集线器hub函数重载
        /// <summary>
        /// 建立链接
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnConnected()
        {
            // 在这添加你的代码.   
            // 例如:在一个聊天程序中,记录当前连接的用户ID和名称,并标记用户在线.
            // 在该方法中的代码完成后,通知客户端建立连接,客户端代码
            // start().done(function(){//你的代码});
            return base.OnConnected();
        }
        /// <summary>
        /// 断开链接
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            try
            {
                lock (ConnectedUsers)
                {
                    var item = ConnectedUsers.FirstOrDefault(it => it.FromConnectionId == Context.ConnectionId);
                    if (item != null)
                    {
                        //发送用户离线通知到客户端
                        Clients.Client(item.FromConnectionId).onUserDisconnected(item.UserName);
                        ConnectedUsers.Remove(item);
                        //客服同时服务人数更新
                        lock (ConnectedServices)
                        {
                            var res = ConnectedServices.FindIndex(it => it.FromConnectionId == item.ToConnectionId);
                            if (res >= 0)
                            {
                                ConnectedServices[res].ServiceNow = ConnectedServices[res].ServiceNow - 1 >= 0 ? ConnectedServices[res].ServiceNow - 1 : 0;
                            }
                        }
                    }
                }
                lock (ConnectedServices)
                {
                    var item = ConnectedServices.FirstOrDefault(it => it.FromConnectionId == Context.ConnectionId);
                    if (item != null)
                    {
                        //发送用户离线通知到客户端
                        Clients.Client(item.FromConnectionId).onUserDisconnected(item.UserName);
                        ConnectedServices.Remove(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// 重新建立链接
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnReconnected()
        {
            // 在这添加你的代码.
            // 例如:你可以标记用户离线后重新连接,标记为在线
            return base.OnReconnected();
        }
        #endregion
    }
}