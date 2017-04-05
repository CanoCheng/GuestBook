
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuestBookProject.Models.ViewModel
{
    public class MemberLoginModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public int Member_ID { get; set; } 

        /// <summary>
        /// 會員暱稱
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 會員角色
        /// </summary>
        public bool Role { get; set; }

        public MemberLoginModel()
        {
            this.Role = true;
        }
    }
}