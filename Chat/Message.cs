//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Chat
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public int MESS_Id { get; set; }
        public int MESS_UserId { get; set; }
        public int MESS_ServiceId { get; set; }
        public int MESS_Status { get; set; }
        public int MESS_TypeId { get; set; }
        public System.DateTime MESS_CreateTime { get; set; }
        public string MESS_Content { get; set; }
        public int MESS_Author { get; set; }
    }
}
