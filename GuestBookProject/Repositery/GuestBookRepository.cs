using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using GuestBookProject.Models;
using GuestBookProject.Models.ViewModel.GuestBook;
using Dapper;

namespace GuestBookProject.Repositery
{
    public class GuestBookRepository
    {
        /// <summary>
        /// 連線字串
        /// </summary>
        private string connection { get; set; }        

        //建立連線物件，取得設定在 Web.config 的 connectionStrings
        public GuestBookRepository()
        {
            connection = ConfigurationManager.ConnectionStrings["GuestBookConnection"].ConnectionString;            
        }

        /// <summary>
        /// 新增主留言資料
        /// </summary>
        /// <param name="addData"></param>
        /// <returns></returns>
        public int AddMainMessage(AddMainGuestbook addData)
        {
            int addcount = 0;
            using (var conn = new SqlConnection(connection))
            {
                try
                {
                    addcount = conn.Execute(CommontSPName.InsertMainMessage,
                                        new { MemberID = addData.MemberID, Content = addData.Content, MemberIP = addData.IP },
                                        commandType: System.Data.CommandType.StoredProcedure);
                }
                catch(Exception ex)
                {

                }                
            }

            return addcount;
        }
        
        /// <summary>
        /// 取得主留言
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GetMessageViewModel> GetMainMessage(bool Role,int MemberID)
        {
            //List<GetMessageViewModel> mainMessage = new List<GetMessageViewModel>();
            IEnumerable<GetMessageViewModel> mainMessage = null;

            using (var conn = new SqlConnection(connection))
            {
                try
                {
                    var messagelist = conn.Query<GetMessageViewModel>(
                        CommontSPName.GetMainMessage,
                        new { Role = Role , MemberID =MemberID},
                        commandType: System.Data.CommandType.StoredProcedure
                        );

                    mainMessage = messagelist;
                }
                catch(Exception ex)
                {
                    
                }
            }
            return mainMessage;
        }

        /// <summary>
        /// 新增回覆留言後，查詢出此主留言下回覆留言資料
        /// </summary>
        /// <param name="addReplyMessage"></param>
        /// <returns></returns>
        public  IEnumerable<GetReplyMessageViewModel> AddReplyMessage(ReplyGuestbooksModel addReplyMessage,bool Role)
        {
            IEnumerable<GetReplyMessageViewModel> getNewReplyMessage =null;

            using (var conn = new SqlConnection(connection))
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("@GuestBookID", addReplyMessage.GuestBookID);
                parameters.Add("@MemberID", addReplyMessage.MemberID);
                parameters.Add("@ReplyContent", addReplyMessage.Reply_Content);
                parameters.Add("@ReplyTime", addReplyMessage.ReplyTime);
                parameters.Add("@SecurityMessage", addReplyMessage.SecurityMessage);
                parameters.Add("@MemberIP", addReplyMessage.Member_IP);
                parameters.Add("@Role", Role);

                try
                {
                    getNewReplyMessage = conn.Query<GetReplyMessageViewModel>(CommontSPName.InsertReplyMessage, parameters
                                            , commandType: System.Data.CommandType.StoredProcedure);
                }
                catch(Exception ex)
                {

                }
            }

                return getNewReplyMessage; 
        }

        /// <summary>
        /// 顯示主留言下的回覆留言清單
        /// </summary>
        /// <param name="guestBookID">主留言ID</param>
        /// <param name="memberID">會員ID</param>
        /// <param name="role">會員角色</param>
        /// <returns></returns>
        public IEnumerable<GetReplyMessageViewModel> GetReplyMessage(int guestBookID,int memberID,bool role )
        {
            IEnumerable<GetReplyMessageViewModel> getReplyMessage = null;

            using (var conn = new SqlConnection(connection))
            {
                getReplyMessage = conn.Query<GetReplyMessageViewModel>(CommontSPName.GetReplyMessage,
                                            new { GuestBookID = guestBookID, MemberID = memberID, Role = role }
                                            , commandType: System.Data.CommandType.StoredProcedure);
            }

            return getReplyMessage;
        }

        /// <summary>
        /// 更新回覆留言
        /// </summary>
        /// <param name="memberID">登入會員編號</param>
        /// <param name="role">會員角色</param>
        /// <param name="content">修改回覆留言內容</param>
        /// <returns>修改完成回覆留言的內容</returns>
        public string UpdateReplyMessageContent(int memberID,bool role,string content,int replyID)
        {
            string newReplyMessage = string.Empty;
            if (!string.IsNullOrWhiteSpace(content))
            {
                using (var conn = new SqlConnection(connection))
                {
                    var updatedContent = conn.QuerySingle<string>(CommontSPName.UpdateReplyMessage,
                                        new { MemberID = memberID, Role = role, UpdatedContent = content, ReplyID = replyID },
                                        commandType: System.Data.CommandType.StoredProcedure);

                    newReplyMessage = updatedContent;
                }
            }
            else
            {
                using (var conn = new SqlConnection(connection))
                {
                    var IsDeleted = conn.QuerySingle<int>(CommontSPName.DeleteReplyMessage,
                                        new { MemberID = memberID, Role = role, ReplyID = replyID },
                                        commandType: System.Data.CommandType.StoredProcedure);

                    newReplyMessage = IsDeleted.ToString();
                }
            }
            

            return newReplyMessage;
        }

        public string UpdateMainMessageContent(UpdateMainMessageModel updateModel)
        {
            string newMainMessage = string.Empty;

            using (var conn = new SqlConnection(connection))
            {
                var updatedContent = conn.QuerySingle<string>(CommontSPName.UpdateMainMessage,
                                    new { MemberID = updateModel.MemberID, UpdatedContent = updateModel.Content, MainID = updateModel.GuestBookID, UpdateTime = updateModel.UpdateTime },
                                    commandType: System.Data.CommandType.StoredProcedure);

                newMainMessage = updatedContent;
            }

            return newMainMessage;
        }
    }
}