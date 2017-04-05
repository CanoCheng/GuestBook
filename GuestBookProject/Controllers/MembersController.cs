using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuestBookProject.Models.ViewModel.Member;
using GuestBookProject.Service.MemberService;
using GuestBookProject.Models.ViewModel;

namespace GuestBookProject.Controllers
{
    public class MembersController : Controller
    {
        //宣告 MemberService 物件
        MembersService memberservice = new MembersService();

        // GET: Members
        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        //POST : Members
        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="registerdata">註冊資料</param>
        /// <returns>有重複帳號導回註冊頁面; 註冊成功導去留言板頁面</returns>
        [HttpPost]
        public ActionResult Register(RegisterViewModel registerdata)
        {                                    
            if(ModelState.IsValid)
            {
                //帳號是否已經存在
                if(memberservice.CheckAccountExisted(registerdata.Account))
                {
                    //密碼加密
                    registerdata.Password = memberservice.HashPassword(registerdata.Password);

                    //寫入會員資料表
                    int rowcount = memberservice.AddMember(registerdata);

                    if (rowcount != 1)
                    {
                        ViewBag.Error="新增會員失敗";

                        return View();
                    }

                    //查詢會員資料
                    MemberLoginModel logindata = memberservice.GetMemberLoginData(registerdata.Account, registerdata.Password);

                    //會員資料寫入Session
                    if (logindata != null)
                    {
                        Session["Login"] = string.Format("{0}_{1}_{2}", logindata.Member_ID, logindata.NickName, logindata.Role);                                                
                    }

                    if (Session["Login"] !=null && !string.IsNullOrWhiteSpace(Session["Login"].ToString()))
                    {
                        //轉導頁面到留言板
                        return RedirectToAction("GetMessage", "GuestBook");
                    }                    
                }

            }
            ViewBag.Error = "已經有此帳號，請重新輸入帳號。";
            return View();
        }

        /// <summary>
        /// 會員登入
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if(ModelState.IsValid)
            {
                //密碼加密
                login.Password = memberservice.HashPassword(login.Password);

                //查詢會員資料
                MemberLoginModel logindata = memberservice.GetMemberLoginData(login.Account, login.Password);

                //會員資料寫入Session
                if (logindata != null)
                {
                    Session["Login"] = string.Format("{0}_{1}_{2}", logindata.Member_ID, logindata.NickName, logindata.Role);
                }

                if (Session["Login"] != null && !string.IsNullOrWhiteSpace(Session["Login"].ToString()))
                {
                    //轉導頁面到留言板
                    return RedirectToAction("GetMessage", "GuestBook");
                }                
            }

            ViewBag.Error = "密碼輸入錯誤，請重新輸入密碼。";

            return View();
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            Session.Remove("Login");

            return RedirectToAction("Login", "Members");
        }

        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            //取得會員登入相關資料
            string login = string.Empty;

            if(Session["Login"] != null)
            {
                login = Session["Login"].ToString();
            }
           
            if (!string.IsNullOrWhiteSpace(login))
            {
                return View();               
            }            
            return RedirectToAction("Login","Members");
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel changepassword)
        {
            //取得會員登入相關資料
            string login = string.Empty;

            if (Session["Login"] != null)
            {
                login = Session["Login"].ToString();
            }

            //更新成功資料筆數
            int datacount = 0;
            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(changepassword.NewPassword))
            {
                //密碼加密
                changepassword.NewPassword = memberservice.HashPassword(changepassword.NewPassword);
                //更新密碼
                datacount = memberservice.UpdateMemberPassword(login, changepassword.NewPassword);
            }

            if (datacount == 1)
            {
                string UpdateSuccess = "T";
                return RedirectToAction("UpdatePasswordSuccessPage", "Members", new { UpdateSuccess = UpdateSuccess } );
            }

            ViewBag.Error = "更新密碼失敗!!";
            return View();
        }

        /// <summary>
        /// 更新密碼成功頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdatePasswordSuccessPage(string UpdateSuccess)
        {
            if(!string.IsNullOrWhiteSpace(UpdateSuccess) && UpdateSuccess.Equals("T"))
            {
                return View();
            }

            return RedirectToAction("ChangePassword", "Members");
        }
    }
}