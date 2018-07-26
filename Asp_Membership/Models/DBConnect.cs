using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Asp_Membership.Models
{
    public class DBConnect
    {
        public static DataTable HasLocalAccount(string UserId)
        {
            SubSonic.Schema.StoredProcedure sp = new StoredProcedure("HasLocalAccount");

            sp.Command.AddParameter("@UserId", UserId, DbType.String);

            return sp.ExecuteDataSet().Tables[0];
        }

        public static string Changepassword(string UserId, string Password)
        {
            SubSonic.Schema.StoredProcedure sp = new StoredProcedure("changepassword");

            sp.Command.AddParameter("@userId", UserId, DbType.String);
            sp.Command.AddParameter("@password", Password, DbType.String);

            return sp.ExecuteScalar<String>();

        }

        static public DataTable selectUserId(string email)
        {
            SubSonic.Schema.StoredProcedure sp = new StoredProcedure("aspnet_Membership_GetUserId");

            sp.Command.AddParameter("@Username", email, DbType.String);

            return sp.ExecuteDataSet().Tables[0];

        }
    }
}