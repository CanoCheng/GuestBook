using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GuestBookProject.SessionManager;
using Newtonsoft.Json;
using GuestBookProject.Models.ViewModel;

namespace GuestBookProject.SessionManager
{
    public class LoginSession
    {
        /// <summary>
        /// 登入資料寫入Session
        /// </summary>
        /// <param name="model">登入資料</param>
        public void WriteLoginSession(MemberLoginModel model)
        {
            //會員資料寫入Session
            if (model != null)
            {
                HttpContext.Current.Session[SessionKey.SessionKeyName.MemberLogin] = JsonConvert.SerializeObject(model);                
            }
        }

        /// <summary>
        /// 取得登入Session["Login"] 登入資訊
        /// </summary>
        /// <returns>回傳登入資訊</returns>
        public string GetSessionData()
        {
            if (HttpContext.Current.Session[SessionKey.SessionKeyName.MemberLogin] !=null)
            {
                return HttpContext.Current.Session[SessionKey.SessionKeyName.MemberLogin].ToString();
            }
            return string.Empty;
        }
        
        
        
    }
}