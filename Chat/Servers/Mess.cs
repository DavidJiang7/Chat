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
        /// <param name="dayOffset">最近多少天内</param>
        /// <returns></returns>
        public static Models.RespServiceCurrentMess[] GetCurrentMess(int userId, int dayOffset)
        {
            try
            {
                using (ChatEntities db = new ChatEntities())
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}