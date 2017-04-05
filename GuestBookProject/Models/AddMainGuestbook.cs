using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuestBookProject.Models
{
    /// <summary>
    /// 新增主留言
    /// </summary>
    public class AddMainGuestbook
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public int MemberID { get; set; }        

        /// <summary>
        /// 主留言內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 留言者IP位址
        /// </summary>
        public string IP { get; set; }
    }
}