using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GuestBookProject.Models.ViewModel.Member;
using System.Security.Cryptography;
using System.Text;
using GuestBookProject.Repositery;
using GuestBookProject.Models.ViewModel;
using System.Web.Mvc;

namespace GuestBookProject.Service.MemberService
{
    public class MembersService
    {
        /// <summary>
        /// TokenKey 鑰匙
        /// </summary>
        const string TK = "W1u9c8K7";
        
        //會員相關Repositery
        MemberRepositery repositery = new MemberRepositery();


        /// <summary>
        /// 判斷是否為空資料
        /// </summary>
        /// <param name="registerdata">註冊資料</param>
        /// <returns>回傳bool</returns>
        public bool IsNullData(RegisterViewModel registerdata)
        {
            bool IsNull = false;

            //判斷如果註冊資料不為空值，判斷旗標為true
            if(!string.IsNullOrWhiteSpace(registerdata.Account) 
                && !string.IsNullOrWhiteSpace(registerdata.Password)
                && !string.IsNullOrWhiteSpace(registerdata.ConfirmPassword))
            {
                IsNull = true;
            }

            return IsNull;
        }

        /// <summary>
        /// 密碼加密
        /// </summary>
        /// <param name="password">使用者輸入密碼</param>
        /// <returns>(原密碼+TK)後經過SHA256加密</returns>
        public string HashPassword(string password)
        {                                  
            password = string.Format("{0}{1}", password, TK);

            SHA256 sha256 = new SHA256CryptoServiceProvider();//建立一個SHA256
            byte[] source = Encoding.UTF8.GetBytes(password);//將字串轉為Byte[]
            byte[] crypto = sha256.ComputeHash(source);//進行SHA256加密

            string hashpassword = string.Empty;

            foreach (byte bt in crypto)
           {
               if (crypto.Length != 0)
               {
                   hashpassword = hashpassword + bt.ToString("x2");
               }
           }

           return hashpassword;                          
        }

        /// <summary>
        /// 檢查帳號是否重複
        /// </summary>
        /// <param name="account"></param>
        /// <returns>True:沒重複 ， False:重複</returns>
        public bool CheckAccountExisted(string account)
        {
            bool IsExisted = false;
            
            if (repositery.QueryAccount(account) == null)
            {
                IsExisted = true;
            }

            return IsExisted;
        }

        /// <summary>
        /// 新增會員資料
        /// </summary>
        /// <param name="addMemberdata">註冊會員資料</param>
        /// <returns>影響資料庫筆數</returns>
        public int AddMember(RegisterViewModel addMemberdata)
        {
            int datacount = 0;           

            datacount = repositery.InsertMember(addMemberdata);

            return datacount;
        }

        /// <summary>
        /// 會員登入查詢會員資料
        /// </summary>
        /// <param name="account">登入帳號</param>
        /// <param name="password">登入密碼</param>
        /// <returns>會員編號、會員暱稱、會員角色</returns>
        public MemberLoginModel GetMemberLoginData(string account, string password)
        {
            var logindata = repositery.GetMemberLoginData(account, password);

            return logindata;
        }
        

        /// <summary>
        /// 更新會員密碼
        /// </summary>
        /// <param name="loginData">session 紀錄的會員資料 </param>
        /// <returns>更新資料筆數</returns>
        public int UpdateMemberPassword(string loginData, string newpassword)
        {
            //存取會員編號
            int memberID = 0;
            bool b_MemberID = false;

            if (!string.IsNullOrWhiteSpace(loginData))
            {
                string[] loginArray = loginData.Split('_');

                if (loginArray.Length > 0)
                {
                    b_MemberID = int.TryParse(loginArray[0], out memberID);
                }
            }

            //影響資料筆數
            int datacount = 0;

            if (b_MemberID && memberID != 0 && !string.IsNullOrWhiteSpace(newpassword))
            {
                datacount = repositery.UpdateMemberPassword(memberID,newpassword);
            }
            
            return datacount;           
        }       
    }
}