using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Servers
{
    public class Mess
    {
        /// <summary>
        /// 添加聊天信息
        /// </summary>
        /// <param name="userId">客户id</param>
        /// <param name="serviceId">客服id</param>
        /// <param name="typeId">信息类型：1-文字；2-图片</param>
        /// <param name="content">内容</param>
        /// <param name="author">消息作者</param>
        /// <returns></returns>
        public static int AddMessage(int userId, int serviceId, int typeId, string content, Models.MessageAuthor author)
        {
            try
            {
                using (ChatEntities db = new ChatEntities())
                {
                    Message m = new Message();
                    m.MESS_Author = (int)author;
                    m.MESS_Content = content;
                    m.MESS_CreateTime = DateTime.Now;
                    m.MESS_ServiceId = serviceId;
                    m.MESS_Status = 1;//0-未读，1-已读
                    m.MESS_TypeId = 1;//1-文字；2-图片
                    m.MESS_UserId = userId;
                    db.Message.Add(m);
                    db.SaveChanges();
                    return m.MESS_Id;
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 根据 客服id 获取最近 dayOffset 天数内的聊天记录
        /// </summary>
        /// <param name="userId">客服id</param>
        /// <param name="messSize">获取每位客户最新messSize条记录</param>
        /// <param name="dayOffset">最近多少天内</param>
        /// <returns></returns>
        public static List<Models.RespServiceCurrentMess> GetCurrentMess(int userId, int dayOffset, int messSize)
        {
            try
            {
                DateTime d = DateTime.Now.AddDays(-dayOffset);
                using (ChatEntities db = new ChatEntities())
                {
                    var res = (from m in db.Message
                              where m.MESS_ServiceId == userId
                              && m.MESS_CreateTime > d
                              group m by m.MESS_UserId into g
                              select new
                              {
                                  UserId = g.Key,
                                  Number = g.Count(),
                                  MaxCreateTime = g.Max(it => it.MESS_CreateTime)
                              }).OrderByDescending(it => it.MaxCreateTime).ToArray();
                    if (res == null || res.Length <= 0)
                    {
                        return null;
                    }
                    List<Models.RespServiceCurrentMess> resultArray = new List<Models.RespServiceCurrentMess>();
                    foreach (var r in res)
                    {
                        Models.RespServiceCurrentMess rscm = new Models.RespServiceCurrentMess();
                        User_Account ua = db.User_Account.FirstOrDefault(it => it.id == r.UserId);
                        if (ua == null)
                        {
                            continue;
                        }
                        rscm.UserId = ua.id;
                        rscm.UserName = ua.U_UserName;
                        rscm.Img = ua.U_Icon;
                        rscm.Mess = db.Message.Where(it => it.MESS_UserId == ua.id && it.MESS_ServiceId == userId).OrderByDescending(it => it.MESS_CreateTime).Take(messSize).ToArray();
                        resultArray.Add(rscm);
                    }
                    return resultArray;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据 客户id 和 客服id 获取更多的历史聊天记录（翻页）
        /// </summary>
        /// <param name="userId">客户id</param>
        /// <param name="ServiceId">客服id</param>
        /// <param name="offset">数据偏移量</param>
        /// <param name="messSize">获取数据量</param>
        /// <returns></returns>
        public static List<Message> GetMoreMess(int userId, int ServiceId, int offset, int messSize)
        {
            try
            {
                using (ChatEntities db = new ChatEntities())
                {
                    return db.Message.Where(it => it.MESS_UserId == userId && it.MESS_ServiceId == ServiceId).OrderByDescending(it => it.MESS_CreateTime).Skip(offset).Take(messSize).ToList();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}