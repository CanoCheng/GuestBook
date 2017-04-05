using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuestBookProject.Models
{
    /// <summary>
    /// 回覆留言資料表物件
    /// </summary>
    public class ReplyGuestbooksModel
    {
        public int ReplyGuestbook_ID { get; set; }

        public int GuestBookID { get; set; }

        public int MemberID { get; set; }

        public string Reply_Content { get; set; }

        public DateTime ReplyTime { get; set; }
        public bool SecurityMessage { get; set; }

        public string Member_IP { get; set; }

        public int UpdateMemberID { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}