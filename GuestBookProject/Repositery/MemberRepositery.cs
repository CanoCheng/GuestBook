using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using GuestBookProject.Models;
using GuestBookProject.Models.ViewModel;
using GuestBookProject.Models.ViewModel.GuestBook;
using GuestBookProject.Models.ViewModel.Member;
using System.Data;

namespace GuestBookProject.Repositery
{
    public class MemberRepositery
    {
        /// <summary>
        /// 連線字串
        /// </summary>
        public string connection { get; set; }        

        //建立連線物件，取得設定在 Web.config 的 connectionStrings
        public MemberRepositery()
        {
            connection = ConfigurationManager.ConnectionStrings["GuestBookConnection"].ConnectionString;            
        }

        /// <summary>
        /// 查詢是否帳號存在
        /// </summary>
        /// <param name="account">輸入帳號</param>
        /// <returns></returns>
        public RegisterViewModel QueryAccount(string account)
        {
            RegisterViewModel query = new RegisterViewModel();   
                     
            using (var conn = new SqlConnection(connection))
            {
                try
                {
                    var queryresult = conn.Query<RegisterViewModel>(
                    CommontSPName.QueryAccountExisted,
                    new { Account = account },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                    query = queryresult;
                }
                catch(Exception ex)
                {
                    query = new RegisterViewModel { Account = "Error" , Password = "Error", ConfirmPassword = "Error"};
                }                
                
            }
            return query;                 
        }


        /// <summary>
        /// 新增會員資料
        /// </summary>
        /// <param name="registerdata">註冊資料</param>
        /// <returns>影響資料筆數</returns>
        public int InsertMember(RegisterViewModel registerdata)
        {
            int datacount = 0;

            using (var conn = new SqlConnection(connection))
            {
                try
                {
                    DynamicParameters patameters = new DynamicParameters();
                    patameters.Add("@Account",registerdata.Account);
                    patameters.Add("@Password", registerdata.Password);
                    patameters.Add("@NickName", registerdata.NickName);
                    //patameters.Add("@Role", registerdata.Role);
                    datacount = conn.Execute(CommontSPName.InsertMember, patameters, commandType: CommandType.StoredProcedure);
                }
                catch(Exception ex)
                {
                    
                }
            }

            return datacount;
        }

        /// <summary>
        /// 會員登入資料
        /// </summary>
        /// <param name="account">登入帳號</param>
        /// <param name="password">登入密碼</param>
        /// <returns>會員編號、會員暱稱、會員角色</returns>
        public MemberLoginModel GetMemberLoginData(string account,string password)
        {
            MemberLoginModel logindata = new MemberLoginModel();

            using (var conn = new SqlConnection(connection))
            {
                try
                {
                    logindata = conn.Query<MemberLoginModel>(CommontSPName.QueryMember,
                                new { Account = account, Password = password },
                                commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    
                }
            }

            return logindata;
        }

        /// <summary>
        /// 依會員編號查詢會員資料
        /// </summary>
        /// <param name="memberID">會員編號</param>
        /// <returns>會員資料</returns>
        public int UpdateMemberPassword(int memberID,string newpassword)
        {
            int updateDataCount = 0;

            using (var conn = new SqlConnection(connection))
            {
                try
                {
                    updateDataCount = conn.Execute(CommontSPName.QueryMemberWithMemberID,
                                new { memberID = memberID, newPassword = newpassword },
                                commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {

                }
            }

            return updateDataCount;
        }
    }
}