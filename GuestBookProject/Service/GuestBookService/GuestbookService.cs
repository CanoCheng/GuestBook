using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GuestBookProject.Models;
using GuestBookProject.Models.ViewModel;
using GuestBookProject.Models.ViewModel.GuestBook;
using GuestBookProject.Repositery;

namespace GuestBookProject.Service.GuestBookService
{
    public class GuestbookService
    {
        //GuestBookRepository物件
        GuestBookRepository guestbookRepository = new GuestBookRepository();

        /// <summary>
        /// 取得Client端 IP位址
        /// </summary>
        /// <returns>回傳IP位址</returns>
        public string IPAddress()
        {
            //###下面兩種取到的IP會是Client端，但因為本機對本機發Request所以會得到 ::1
            //1.
            string IPAddress = string.Empty;
            IPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

            //2.
            string hostName = HttpContext.Current.Request.UserHostName;
            string ClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            //這個取到的會是Server端的IP不是Client端的
            //string strHostName = System.Net.Dns.GetHostName();
            //string clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            
            return IPAddress;
        }   
        

        /// <summary>
        /// 取得登入會員資料
        /// </summary>
        /// <param name="loginData">Session["Login"]</param>
        /// <returns>MemberLoginModel(會員編號、暱稱、角色)</returns>
        public MemberLoginModel GetMember(string loginData)
        {
            //存放Session 登入會員資料 
            MemberLoginModel member = new MemberLoginModel();                        

            if (!string.IsNullOrWhiteSpace(loginData))
            {
                string [] loginArray = loginData.Split('_');

                if (loginArray.Length > 0)
                {
                    //存取會員編號
                    int memberID = 0;
                    bool b_MemberID = false;

                    //轉型會員編號
                    b_MemberID = int.TryParse(loginArray[0], out memberID);
                    int.TryParse(loginArray[0], out memberID);

                    //取得會員編號
                    if (b_MemberID && memberID != 0)
                    {
                        member.Member_ID = memberID;
                    }

                    //取得會員暱稱
                    if (!string.IsNullOrWhiteSpace(loginArray[1]))
                    {
                        member.NickName = loginArray[1];
                    }

                    //轉型會員角色
                    //int role = 10;
                    //bool b_role = false;
                    //if (!string.IsNullOrWhiteSpace(loginArray[2]))
                    //{
                    //    b_role = int.TryParse(loginArray[2], out role);
                    //}
                    ////取得會員角色
                    //if (b_role && role!=10)
                    //{
                    //    member.Role = role;
                    //}
                }
            }                        
            return member;
        }


        /// <summary>
        /// 新增主留言資料
        /// </summary>
        /// <param name="addData"></param>
        /// <returns></returns>
        public bool AddMainMessage(AddMainGuestbook addData)
        {
            bool IsAddSuccess = false;

            int effectCount = guestbookRepository.AddMainMessage(addData);

            if(effectCount == 1)
            {
                IsAddSuccess = true;
            }

            return IsAddSuccess; 
        }

        /// <summary>
        /// 取得主留言資料
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        public IEnumerable<GetMessageViewModel> GetMainMessage (bool Role,int MemberID)
        {
            IEnumerable<GetMessageViewModel> mainMessageListData = null;
            var mainMessageList = guestbookRepository.GetMainMessage(Role, MemberID);

            if(mainMessageList.Count() > 0)
            {
                foreach(var mainMessage in mainMessageList)
                {
                    mainMessage.MemberName = mainMessage.MemberName.Equals("??") ? "匿名" : mainMessage.MemberName;                    
                }

                mainMessageListData = mainMessageList;
            }
            return mainMessageListData;
        }


        /// <summary>
        /// 新增回覆留言後，查詢出此主留言下回覆留言資料
        /// </summary>
        /// <param name="Content">回覆留言內容</param>
        /// <param name="MainMessageID">主留言ID</param>
        /// <param name="IP">回覆留言者IP</param>
        /// <param name="Isprivate">是否為悄悄話</param>
        /// <param name="loginData">會員登入資料</param>
        /// <returns></returns>
        public List<GetReplyMessageViewModel> AddReplyMessage(string Content, string MainMessageID,string IP
                                                        , string Isprivate, MemberLoginModel loginData)
        {
            //組合新增回覆留言資料
            ReplyGuestbooksModel replyGuestModel = new ReplyGuestbooksModel()
            {
                GuestBookID = Convert.ToInt32(MainMessageID),
                MemberID = loginData.Member_ID,
                Member_IP = IP,
                ReplyTime = DateTime.Now,
                Reply_Content = Content,
                SecurityMessage = Isprivate.Equals("Private") ? true : false                
            };
           
            List<GetReplyMessageViewModel> replyMessageList = new List<GetReplyMessageViewModel>();

            //新增回覆留言後，查詢出此主留言下回覆留言資料
            var replyMessagelist = guestbookRepository.AddReplyMessage(replyGuestModel, loginData.Role);           

            if(replyMessagelist.Count() > 0)
            {
                foreach (var replymessage in replyMessagelist)
                {
                    replymessage.MemberName = replymessage.MemberName.Equals("??") ? "匿名" : replymessage.MemberName;

                    if (replymessage.IsPrivateMessage != "T")
                    {
                        replyMessageList.Add(replymessage);
                    }
                }
            }            
            return replyMessageList;
        }

        /// <summary>
        /// 取得主留言下的回覆留言清單
        /// </summary>
        /// <param name="guestBookID">主留言ID</param>
        /// <param name="loginData">會員登入資料</param>
        /// <returns></returns>
        public List<GetReplyMessageViewModel>GetReplyMessage(int guestBookID, MemberLoginModel loginData)
        {
            var getReplyMessageList = guestbookRepository.GetReplyMessage(guestBookID, loginData.Member_ID,loginData.Role);            

            var SelectedReplyMessageList = getReplyMessageList.Where(x => x.IsPrivateMessage != "T");

            //List<GetReplyMessageViewModel> replymessagelist = new List<GetReplyMessageViewModel>();

            //if(getReplyMessageList.Count() > 0)
            //{
            //    //foreach(var replymessage in getReplyMessageList)
            //    //{
            //    //    replymessage.MemberName = replymessage.MemberName.Equals("??") ? "匿名" : replymessage.MemberName;

            //    //    if (replymessage.IsPrivateMessage != "T")
            //    //    {                     
            //    //        replymessagelist.Add(replymessage);
            //    //    }
            //    //}

            //    
            //}
            return SelectedReplyMessageList.ToList();
        }

        /// <summary>
        /// 更新回覆留言
        /// </summary>
        /// <param name="loginData">登入會員資料</param>
        /// <param name="UpdateContent">修改回覆留言內容</param>
        /// <returns>修改完成回覆留言的內容</returns>
        public string UpdateReplyMessage(MemberLoginModel loginData,string UpdateContent,string replyID)
        {
            string UpdatedContent = string.Empty;

            int replyGuestbookID = 0;
            bool transReplyID = int.TryParse(replyID, out replyGuestbookID);

            if (transReplyID && replyGuestbookID != 0)
            {
                UpdatedContent = guestbookRepository.UpdateReplyMessageContent(loginData.Member_ID, loginData.Role, UpdateContent, replyGuestbookID);
            }
            
            if (!string.IsNullOrWhiteSpace(UpdatedContent))
            {
                UpdatedContent = UpdatedContent.Equals("1") ? "success" : UpdatedContent;

                return UpdatedContent;
            }

            return UpdatedContent;
        }

        public string UpdateMainMessage(MemberLoginModel loginData, string Content, string MainMessageID)
        {
            //資料庫編輯過後的留言
            string UpdatedMessage = string.Empty;

            //前端取得ID 為M_32
            string[] MainID = MainMessageID.Split('_');

            //判斷是否分析字串後是否長度大於0 & 是否可轉型 & 轉型後ID不可為0
            int guestbookID = 0;
            if(MainID.Length > 0 && int.TryParse(MainID[1], out guestbookID) && guestbookID != 0)
            {
                UpdateMainMessageModel updateModel = new UpdateMainMessageModel
                {
                    MemberID = loginData.Member_ID,
                    GuestBookID = guestbookID,
                    Content = Content,
                    UpdateTime = DateTime.Now
                };

                UpdatedMessage = guestbookRepository.UpdateMainMessageContent(updateModel);                
            }

            return UpdatedMessage;
        }
    }
}