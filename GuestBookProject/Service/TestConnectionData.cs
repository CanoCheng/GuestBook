using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;

namespace GuestBookProject.Service
{
    public class TestData
    {
        public int TestID { get; set; }
        public string TestWord { get; set; }
    }
    public class TestConnectionData
    {       
        public void Connecting()
        {
            //建立連線物件，取得設定在 Web.config 的 connectionStrings        
            //ConnectionStringSettings connection = ConfigurationManager.ConnectionStrings["GuestBookConnection"];
            string connection = ConfigurationManager.ConnectionStrings["GuestBookConnection"].ConnectionString;

            using (var conn = new SqlConnection(connection))
            {
                var sqlcommand = "SELECT * FROM　dbo.Test";
                //SqlCommand command = new SqlCommand(sqlcommand, conn);
                conn.Open();
                var query = conn.Query<TestData>(sqlcommand);

            }
        }
        


    }
}