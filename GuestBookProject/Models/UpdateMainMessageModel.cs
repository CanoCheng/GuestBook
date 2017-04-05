using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuestBookProject.Models
{
    public class UpdateMainMessageModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public int MemberID { get; set; }

        /// <summary>
        /// 主留言編號
        /// </summary>
        public int GuestBookID { get; set; }

        /// <summary>
        /// 主留言內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 編輯留言時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}