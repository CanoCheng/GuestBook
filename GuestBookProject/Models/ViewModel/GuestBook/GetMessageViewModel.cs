using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuestBookProject.Models.ViewModel.GuestBook
{
    /// <summary>
    /// 回覆留言ViewModel
    /// </summary>
    public class GetReplyMessageViewModel
    {
        /// <summary>
        /// 回覆留言ID
        /// </summary>
        public int Reply_ID { get; set; }

        /// <summary>
        /// 會員暱稱
        /// </summary>
        private string _memberName;
        public string MemberName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this._memberName))
                {
                   this._memberName = this._memberName.Equals("??") ? "匿名" : this._memberName;
                }
                return this._memberName;
            }
            set
            {
                this._memberName = value;
            }
        }

        /// <summary>
        /// 留言時間
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 留言內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否有權限觀看&編輯  T: 可顯示、F:不可顯示 
        /// </summary>
        public string IsOwner { get; set; } //顯示編輯 & 刪除按鍵

        /// <summary>
        /// 是否有權限觀看  T:可顯示、F:不可顯示
        /// </summary>
        public string IsPrivateMessage { get; set; } //顯示悄悄話
    }

    /// <summary>
    /// 顯示全部留言ViewModel
    /// </summary>
    public class GetMessageViewModel
    {
        /// <summary>
        /// 主留言ID
        /// </summary>
        public int GuestBook_ID { get; set; }

        /// <summary>
        /// 會員暱稱
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 留言時間
        /// </summary>
        public string CreateTime { get; set; }
        
        /// <summary>
        /// 主留言內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否有權限顯示 編輯&刪除 Button， T:可顯示 、F:不可顯示
        /// </summary>
        public string IsOwner { get; set; }  //顯示編輯 & 刪除按鍵

        /// <summary>
        /// 是否有權限顯示悄悄話，T:可顯示 、F:不可顯示
        /// </summary>
        public string IsPrivateMessage { get; set; } //顯示悄悄話

        /// <summary>
        /// 回覆留言集合
        /// </summary>
        public List<GetReplyMessageViewModel> ReplyMessages { get; set; }
        
        public GetMessageViewModel()
        {
            this.ReplyMessages = new List<GetReplyMessageViewModel>();
        }
    }
}