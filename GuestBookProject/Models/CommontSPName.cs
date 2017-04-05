using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuestBookProject.Models
{
    public class CommontSPName
    {
        public const string QueryAccountExisted = "dbo.SP_CheckMemberExisted";

        public const string InsertMember = "dbo.SP_InsertMember";

        public const string QueryMember = "dbo.SP_QueryMember";

        public const string QueryMemberWithMemberID = "dbo.SP_UpdateMemberPassword";

        public const string InsertMainMessage = "dbo.SP_InsertMainMessage";

        public const string GetMainMessage = "dbo.SP_GetAllMessage";

        public const string InsertReplyMessage = "dbo.SP_InsertReplyMessage";

        public const string GetReplyMessage = "dbo.SP_GetReplyMessage";

        public const string UpdateReplyMessage = "dbo.SP_UpdateReplyMessage";

        public const string DeleteReplyMessage = "dbo.SP_DeleteReplyMessage";

        public const string UpdateMainMessage = "dbo.SP_UpdateMainMessage";
    }
}