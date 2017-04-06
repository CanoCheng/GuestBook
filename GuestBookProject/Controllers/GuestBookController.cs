using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuestBookProject.Models.ViewModel;
using GuestBookProject.Service.GuestBookService;
using GuestBookProject.Models;
using GuestBookProject.Models.ViewModel.GuestBook;
using PagedList;

namespace GuestBookProject.Controllers
{
    public class GuestBookController : Controller
    {
        GuestbookService _guestbookService = new GuestbookService();
        /// <summary>
        /// 取得會員登入相關資料
        /// </summary>
        /// <returns></returns>
        public string SessionLogin()
        {
            //取得會員登入相關資料
            string login = string.Empty;

            if (Session["Login"] != null)
            {
                login = Session["Login"].ToString();
            }

            return login;
        }
        
        /// <summary>
        /// 新增留言
        /// </summary>
        /// <param name="AddMessage">留言內容</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateMessage(string AddMessage)
        {            
            if (!string.IsNullOrWhiteSpace(AddMessage))
            {
                //使用者登入資料
                string login = SessionLogin();
                MemberLoginModel loginData = new MemberLoginModel();

                //取得會員登入資料
                if (!string.IsNullOrWhiteSpace(login))
                {
                    loginData = _guestbookService.GetMember(login);
                }

                //使用者IP 位址
                string IP = _guestbookService.IPAddress();


                //組合新增留言
                AddMainGuestbook addMessage = new AddMainGuestbook()
                {
                    MemberID = loginData.Member_ID,
                    Content = AddMessage,
                    IP = IP
                };     

                //新增留言
                bool IsAddSuccess = _guestbookService.AddMainMessage(addMessage);

                //主留言資料
                var mainMessage = _guestbookService.GetMainMessage(loginData.Role, loginData.Member_ID);

                return PartialView("_GetMainMessagePartialView",mainMessage);
            }
            
            return RedirectToAction("GetMessage","GuestBook");
            
        }

        /// <summary>
        /// 編輯主留言
        /// </summary>
        /// <param name="MainContent">編輯內容</param>
        /// <param name="MainMessageID">主留言ID</param>
        /// <returns>編輯後新主留言</returns>
        [HttpPost]
        public ActionResult UpdateMessage(string MainContent, string MainMessageID)
        {
            //資料庫編輯過後的留言
            string UpdatedMessage = string.Empty;

            //使用者登入資料
            string login = SessionLogin();

            //存放使用者登入資料物件
            MemberLoginModel loginData = new MemberLoginModel();

            //取得會員登入資料
            if (!string.IsNullOrWhiteSpace(login))
            {
                loginData = _guestbookService.GetMember(login);
            }

            if(!string.IsNullOrWhiteSpace(MainContent) && !string.IsNullOrWhiteSpace(MainMessageID))
            {
                UpdatedMessage = _guestbookService.UpdateMainMessage(loginData, MainContent, MainMessageID);
            }

            return Content(UpdatedMessage);
        }

        /// <summary>
        /// 刪除主留言
        /// </summary>
        /// <returns>回傳是否刪除成功</returns>
        [HttpPost]
        public ActionResult DeleteMessage(string MainMessageID)
        {
            //資料庫編輯過後的留言
            string DeletedMessage = string.Empty;

            //使用者登入資料
            string login = SessionLogin();

            //存放使用者登入資料物件
            MemberLoginModel loginData = new MemberLoginModel();

            //取得會員登入資料
            if (!string.IsNullOrWhiteSpace(login))
            {
                loginData = _guestbookService.GetMember(login);
            }

            if (loginData!=null & !string.IsNullOrWhiteSpace(MainMessageID))
            {
                DeletedMessage = _guestbookService.DeleteMessage(loginData, MainMessageID);
            }


            return Content(DeletedMessage);
        }

        //分頁一頁有幾筆資料
        private int pageSize = 5;

        /// <summary>
        /// 顯示留言板頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMessage(int page = 1)
        {
            //ETMall.Common.DataAccess.MSSQL.SqlHelper.ExecuteNonQuery()
            //Connection.Query
            //使用者登入資料
            string login = SessionLogin();
            MemberLoginModel loginData = new MemberLoginModel();

            //取得會員登入資料
            if (!string.IsNullOrWhiteSpace(login))
            {
                loginData = _guestbookService.GetMember(login);
            }
            
            //判斷現在第幾頁
            int currentPage = page < 1 ? 1 : page;

            var mainMessage = _guestbookService.GetMainMessage(loginData.Role,loginData.Member_ID);

            //return View(mainMessage);

            var result = mainMessage.ToPagedList(currentPage,pageSize);

            return View(result);
        }


        /// <summary>
        /// 新增回覆留言
        /// </summary>
        /// <param name="Content">回覆留言</param>
        /// <param name="MainMessageID">主留言ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddReplyMesage(string Content, string MainMessageID,string Isprivate)
        {
            //使用者登入資料
            string login = SessionLogin();

            //存放使用者登入資料物件
            MemberLoginModel loginData = new MemberLoginModel();

            //取得會員登入資料
            if (!string.IsNullOrWhiteSpace(login))
            {
                loginData = _guestbookService.GetMember(login);
            }

            //使用者IP 位址
            string IP = _guestbookService.IPAddress();


            //此主留言下回覆留言資料存放物件
            List<GetReplyMessageViewModel> replyMessage = new List<GetReplyMessageViewModel>();

            //新增回覆留言，並回傳新增的回覆留言
            if (!string.IsNullOrWhiteSpace(Content) && !string.IsNullOrWhiteSpace(MainMessageID))
            {
                //處理傳進來的主留言ID
                string[] mainMessageIDArray = MainMessageID.Split('_');
                if (mainMessageIDArray.Length == 2 && mainMessageIDArray[0].Equals("M"))
                {
                    MainMessageID = mainMessageIDArray[1];
                }

                //新增回覆留言後，查詢出此主留言下回覆留言資料
                replyMessage = _guestbookService.AddReplyMessage(Content,MainMessageID,IP,Isprivate,loginData);
            }

            return Json(replyMessage);
        }

        /// <summary>
        /// 取得主留言下的回覆留言
        /// </summary>
        /// <param name="guestBookID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReplyMessage(string guestBookID)
        {
            //使用者登入資料
            string login = SessionLogin();

            //存放使用者登入資料物件
            MemberLoginModel loginData = new MemberLoginModel();

            //取得會員登入資料
            if (!string.IsNullOrWhiteSpace(login))
            {
                loginData = _guestbookService.GetMember(login);
            }

            //此主留言下回覆留言資料存放物件
            List<GetReplyMessageViewModel> replyMessageList = new List<GetReplyMessageViewModel>();

            //GuestBookID 取得
            if (!string.IsNullOrWhiteSpace(guestBookID))
            {
                string[] mainGuestIDArray = guestBookID.Split('_');
                guestBookID = mainGuestIDArray[1];
            }
            //確認是否可轉型成 int 型態
            int realmainGuestID = 0;
            bool checkmainGuestID = int.TryParse(guestBookID, out realmainGuestID);

            if (checkmainGuestID)
            {
                replyMessageList = _guestbookService.GetReplyMessage(realmainGuestID, loginData);
            }
            return Json(replyMessageList);
        }

        /// <summary>
        /// 編輯回覆留言
        /// </summary>
        /// <param name="ReplyID">回覆留言ID</param>
        /// <param name="UpdateContent">回覆留言編輯內容</param>
        /// <returns>修改完成回覆留言的內容</returns>
        public ActionResult UpdateReplyMessage(string ReplyID,string UpdateContent)
        {
            //使用者登入資料
            string login = SessionLogin();

            //存放使用者登入資料物件
            MemberLoginModel loginData = new MemberLoginModel();

            //取得會員登入資料
            if (!string.IsNullOrWhiteSpace(login))
            {
                loginData = _guestbookService.GetMember(login);
            }

            string newReplyMessage = string.Empty;
            if (loginData!=null & !string.IsNullOrWhiteSpace(UpdateContent) &&　!string.IsNullOrWhiteSpace(ReplyID) && loginData.Member_ID !=0)
            {
                newReplyMessage = _guestbookService.UpdateReplyMessage(loginData, UpdateContent, ReplyID,false);
            }
            
            return Content(newReplyMessage);
        }

        /// <summary>
        /// 刪除回覆留言
        /// </summary>
        /// <param name="ReplyID">回覆留言編號</param>
        /// <returns>是否刪除成功字串 Y</returns>
        [HttpPost]
        public ActionResult DeleteReplyMessage(string ReplyID)
        {
            //使用者登入資料
            string login = SessionLogin();

            //存放使用者登入資料物件
            MemberLoginModel loginData = new MemberLoginModel();

            //取得會員登入資料
            if (!string.IsNullOrWhiteSpace(login))
            {
                loginData = _guestbookService.GetMember(login);
            }
            
            string IsSuccess = string.Empty;
            if(loginData!=null && !string.IsNullOrWhiteSpace(ReplyID))
            {
                IsSuccess = _guestbookService.UpdateReplyMessage(loginData, null, ReplyID,true);
            }
            return Content(IsSuccess);
        }
    }
}