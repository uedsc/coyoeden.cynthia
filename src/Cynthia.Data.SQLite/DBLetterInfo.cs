﻿/// Author:					Joe Audette
/// Created:				2007-12-27
/// Last Modified:			2009-10-31
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using Mono.Data.Sqlite;

namespace Cynthia.Data
{
    
    public static class DBLetterInfo
    {
       
        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
            if (connectionString == "defaultdblocation")
            {
                connectionString = "version=3,URI=file:"
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/Cynthia.db.config");

            }
            return connectionString;
        }


        /// <summary>
        /// Inserts a row in the cy_LetterInfo table. Returns rows affected count.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="title"> title </param>
        /// <param name="description"> description </param>
        /// <param name="availableToRoles"> availableToRoles </param>
        /// <param name="enabled"> enabled </param>
        /// <param name="allowUserFeedback"> allowUserFeedback </param>
        /// <param name="allowAnonFeedback"> allowAnonFeedback </param>
        /// <param name="fromAddress"> fromAddress </param>
        /// <param name="fromName"> fromName </param>
        /// <param name="replyToAddress"> replyToAddress </param>
        /// <param name="sendMode"> sendMode </param>
        /// <param name="enableViewAsWebPage"> enableViewAsWebPage </param>
        /// <param name="enableSendLog"> enableSendLog </param>
        /// <param name="rolesThatCanEdit"> rolesThatCanEdit </param>
        /// <param name="rolesThatCanApprove"> rolesThatCanApprove </param>
        /// <param name="rolesThatCanSend"> rolesThatCanSend </param>
        /// <param name="createdUTC"> createdUTC </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="lastModUTC"> lastModUTC </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid letterInfoGuid,
            Guid siteGuid,
            string title,
            string description,
            string availableToRoles,
            bool enabled,
            bool allowUserFeedback,
            bool allowAnonFeedback,
            string fromAddress,
            string fromName,
            string replyToAddress,
            int sendMode,
            bool enableViewAsWebPage,
            bool enableSendLog,
            string rolesThatCanEdit,
            string rolesThatCanApprove,
            string rolesThatCanSend,
            DateTime createdUtc,
            Guid createdBy,
            DateTime lastModUtc,
            Guid lastModBy,
            bool allowArchiveView,
            bool profileOptIn,
            int sortRank)
        {
            #region Bit Conversion

            int intAllowArchiveView = 0;
            if (allowArchiveView) { intAllowArchiveView = 1; }

            int intProfileOptIn = 0;
            if (profileOptIn) { intProfileOptIn = 1; }

            int intEnabled = 0;
            if (enabled) { intEnabled = 1; }

            int intAllowUserFeedback = 0;
            if (allowUserFeedback) { intAllowUserFeedback = 1; }

            int intAllowAnonFeedback = 0;
            if (allowAnonFeedback) { intAllowAnonFeedback = 1; }

            int intEnableViewAsWebPage = 0;
            if (enableViewAsWebPage) { intEnableViewAsWebPage = 1; }

            int intEnableSendLog = 0;
            if (enableSendLog) { intEnableSendLog = 1; }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_LetterInfo (");
            sqlCommand.Append("LetterInfoGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("AvailableToRoles, ");
            sqlCommand.Append("Enabled, ");
            sqlCommand.Append("AllowUserFeedback, ");
            sqlCommand.Append("AllowAnonFeedback, ");
            sqlCommand.Append("FromAddress, ");
            sqlCommand.Append("FromName, ");
            sqlCommand.Append("ReplyToAddress, ");
            sqlCommand.Append("SendMode, ");
            sqlCommand.Append("EnableViewAsWebPage, ");
            sqlCommand.Append("EnableSendLog, ");
            sqlCommand.Append("RolesThatCanEdit, ");
            sqlCommand.Append("RolesThatCanApprove, ");
            sqlCommand.Append("RolesThatCanSend, ");
            sqlCommand.Append("SubscriberCount, ");
            sqlCommand.Append("UnVerifiedCount, ");
            sqlCommand.Append("AllowArchiveView, ");
            sqlCommand.Append("ProfileOptIn, ");
            sqlCommand.Append("SortRank, ");

            sqlCommand.Append("CreatedUTC, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("LastModUTC, ");
            sqlCommand.Append("LastModBy )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":LetterInfoGuid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":Title, ");
            sqlCommand.Append(":Description, ");
            sqlCommand.Append(":AvailableToRoles, ");
            sqlCommand.Append(":Enabled, ");
            sqlCommand.Append(":AllowUserFeedback, ");
            sqlCommand.Append(":AllowAnonFeedback, ");
            sqlCommand.Append(":FromAddress, ");
            sqlCommand.Append(":FromName, ");
            sqlCommand.Append(":ReplyToAddress, ");
            sqlCommand.Append(":SendMode, ");
            sqlCommand.Append(":EnableViewAsWebPage, ");
            sqlCommand.Append(":EnableSendLog, ");
            sqlCommand.Append(":RolesThatCanEdit, ");
            sqlCommand.Append(":RolesThatCanApprove, ");
            sqlCommand.Append(":RolesThatCanSend, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append(":AllowArchiveView, ");
            sqlCommand.Append(":ProfileOptIn, ");
            sqlCommand.Append(":SortRank, ");

            sqlCommand.Append(":CreatedUTC, ");
            sqlCommand.Append(":CreatedBy, ");
            sqlCommand.Append(":LastModUTC, ");
            sqlCommand.Append(":LastModBy );");

            SqliteParameter[] arParams = new SqliteParameter[24];

            arParams[0] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":Title", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqliteParameter(":Description", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new SqliteParameter(":AvailableToRoles", DbType.Object);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = availableToRoles;

            arParams[5] = new SqliteParameter(":Enabled", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intEnabled;

            arParams[6] = new SqliteParameter(":AllowUserFeedback", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = intAllowUserFeedback;

            arParams[7] = new SqliteParameter(":AllowAnonFeedback", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intAllowAnonFeedback;

            arParams[8] = new SqliteParameter(":FromAddress", DbType.String, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = fromAddress;

            arParams[9] = new SqliteParameter(":FromName", DbType.String, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = fromName;

            arParams[10] = new SqliteParameter(":ReplyToAddress", DbType.String, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = replyToAddress;

            arParams[11] = new SqliteParameter(":SendMode", DbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = sendMode;

            arParams[12] = new SqliteParameter(":EnableViewAsWebPage", DbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intEnableViewAsWebPage;

            arParams[13] = new SqliteParameter(":EnableSendLog", DbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = intEnableSendLog;

            arParams[14] = new SqliteParameter(":RolesThatCanEdit", DbType.Object);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = rolesThatCanEdit;

            arParams[15] = new SqliteParameter(":RolesThatCanApprove", DbType.Object);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = rolesThatCanApprove;

            arParams[16] = new SqliteParameter(":RolesThatCanSend", DbType.Object);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = rolesThatCanSend;

            arParams[17] = new SqliteParameter(":CreatedUTC", DbType.DateTime);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = createdUtc;

            arParams[18] = new SqliteParameter(":CreatedBy", DbType.String, 36);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = createdBy.ToString();

            arParams[19] = new SqliteParameter(":LastModUTC", DbType.DateTime);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = lastModUtc;

            arParams[20] = new SqliteParameter(":LastModBy", DbType.String, 36);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = lastModBy.ToString();

            arParams[21] = new SqliteParameter(":AllowArchiveView", DbType.Int32);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = intAllowArchiveView;

            arParams[22] = new SqliteParameter(":ProfileOptIn", DbType.Int32);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = intProfileOptIn;

            arParams[23] = new SqliteParameter(":SortRank", DbType.Int32);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = sortRank;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the cy_LetterInfo table. Returns true if row updated.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="title"> title </param>
        /// <param name="description"> description </param>
        /// <param name="availableToRoles"> availableToRoles </param>
        /// <param name="enabled"> enabled </param>
        /// <param name="allowUserFeedback"> allowUserFeedback </param>
        /// <param name="allowAnonFeedback"> allowAnonFeedback </param>
        /// <param name="fromAddress"> fromAddress </param>
        /// <param name="fromName"> fromName </param>
        /// <param name="replyToAddress"> replyToAddress </param>
        /// <param name="sendMode"> sendMode </param>
        /// <param name="enableViewAsWebPage"> enableViewAsWebPage </param>
        /// <param name="enableSendLog"> enableSendLog </param>
        /// <param name="rolesThatCanEdit"> rolesThatCanEdit </param>
        /// <param name="rolesThatCanApprove"> rolesThatCanApprove </param>
        /// <param name="rolesThatCanSend"> rolesThatCanSend </param>
        /// <param name="createdUTC"> createdUTC </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="lastModUTC"> lastModUTC </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid letterInfoGuid,
            Guid siteGuid,
            string title,
            string description,
            string availableToRoles,
            bool enabled,
            bool allowUserFeedback,
            bool allowAnonFeedback,
            string fromAddress,
            string fromName,
            string replyToAddress,
            int sendMode,
            bool enableViewAsWebPage,
            bool enableSendLog,
            string rolesThatCanEdit,
            string rolesThatCanApprove,
            string rolesThatCanSend,
            DateTime createdUtc,
            Guid createdBy,
            DateTime lastModUtc,
            Guid lastModBy,
            bool allowArchiveView,
            bool profileOptIn,
            int sortRank)
        {
            #region Bit Conversion

            int intAllowArchiveView = 0;
            if (allowArchiveView) { intAllowArchiveView = 1; }

            int intProfileOptIn = 0;
            if (profileOptIn) { intProfileOptIn = 1; }

            int intEnabled = 0;
            if (enabled) { intEnabled = 1; }

            int intAllowUserFeedback = 0;
            if (allowUserFeedback) { intAllowUserFeedback = 1; }

            int intAllowAnonFeedback = 0;
            if (allowAnonFeedback) { intAllowAnonFeedback = 1; }

            int intEnableViewAsWebPage = 0;
            if (enableViewAsWebPage) { intEnableViewAsWebPage = 1; }

            int intEnableSendLog = 0;
            if (enableSendLog) { intEnableSendLog = 1; }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_LetterInfo ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SiteGuid = :SiteGuid, ");
            sqlCommand.Append("Title = :Title, ");
            sqlCommand.Append("Description = :Description, ");
            sqlCommand.Append("AvailableToRoles = :AvailableToRoles, ");
            sqlCommand.Append("Enabled = :Enabled, ");
            sqlCommand.Append("AllowUserFeedback = :AllowUserFeedback, ");
            sqlCommand.Append("AllowAnonFeedback = :AllowAnonFeedback, ");
            sqlCommand.Append("FromAddress = :FromAddress, ");
            sqlCommand.Append("FromName = :FromName, ");
            sqlCommand.Append("ReplyToAddress = :ReplyToAddress, ");
            sqlCommand.Append("SendMode = :SendMode, ");
            sqlCommand.Append("EnableViewAsWebPage = :EnableViewAsWebPage, ");
            sqlCommand.Append("EnableSendLog = :EnableSendLog, ");
            sqlCommand.Append("RolesThatCanEdit = :RolesThatCanEdit, ");
            sqlCommand.Append("RolesThatCanApprove = :RolesThatCanApprove, ");
            sqlCommand.Append("RolesThatCanSend = :RolesThatCanSend, ");

            sqlCommand.Append("AllowArchiveView = :AllowArchiveView, ");
            sqlCommand.Append("ProfileOptIn = :ProfileOptIn, ");
            sqlCommand.Append("SortRank = :SortRank, ");

            sqlCommand.Append("CreatedUTC = :CreatedUTC, ");
            sqlCommand.Append("CreatedBy = :CreatedBy, ");
            sqlCommand.Append("LastModUTC = :LastModUTC, ");
            sqlCommand.Append("LastModBy = :LastModBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterInfoGuid = :LetterInfoGuid ;");

            SqliteParameter[] arParams = new SqliteParameter[24];

            arParams[0] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":Title", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqliteParameter(":Description", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new SqliteParameter(":AvailableToRoles", DbType.Object);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = availableToRoles;

            arParams[5] = new SqliteParameter(":Enabled", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intEnabled;

            arParams[6] = new SqliteParameter(":AllowUserFeedback", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = intAllowUserFeedback;

            arParams[7] = new SqliteParameter(":AllowAnonFeedback", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intAllowAnonFeedback;

            arParams[8] = new SqliteParameter(":FromAddress", DbType.String, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = fromAddress;

            arParams[9] = new SqliteParameter(":FromName", DbType.String, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = fromName;

            arParams[10] = new SqliteParameter(":ReplyToAddress", DbType.String, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = replyToAddress;

            arParams[11] = new SqliteParameter(":SendMode", DbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = sendMode;

            arParams[12] = new SqliteParameter(":EnableViewAsWebPage", DbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intEnableViewAsWebPage;

            arParams[13] = new SqliteParameter(":EnableSendLog", DbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = intEnableSendLog;

            arParams[14] = new SqliteParameter(":RolesThatCanEdit", DbType.Object);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = rolesThatCanEdit;

            arParams[15] = new SqliteParameter(":RolesThatCanApprove", DbType.Object);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = rolesThatCanApprove;

            arParams[16] = new SqliteParameter(":RolesThatCanSend", DbType.Object);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = rolesThatCanSend;

            arParams[17] = new SqliteParameter(":CreatedUTC", DbType.DateTime);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = createdUtc;

            arParams[18] = new SqliteParameter(":CreatedBy", DbType.String, 36);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = createdBy.ToString();

            arParams[19] = new SqliteParameter(":LastModUTC", DbType.DateTime);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = lastModUtc;

            arParams[20] = new SqliteParameter(":LastModBy", DbType.String, 36);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = lastModBy.ToString();

            arParams[21] = new SqliteParameter(":AllowArchiveView", DbType.Int32);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = intAllowArchiveView;

            arParams[22] = new SqliteParameter(":ProfileOptIn", DbType.Int32);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = intProfileOptIn;

            arParams[23] = new SqliteParameter(":SortRank", DbType.Int32);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = sortRank;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Updates the subscriber count on a row in the cy_LetterInfo table. Returns true if row updated.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <returns>bool</returns>
        public static bool UpdateSubscriberCount(Guid letterInfoGuid)
        {
           
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_LetterInfo ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("SubscriberCount = (  ");
            sqlCommand.Append("SELECT COUNT(*) ");
            sqlCommand.Append("FROM cy_LetterSubscribe  ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterInfoGuid = :LetterInfoGuid  ");
            sqlCommand.Append("),  ");

            sqlCommand.Append("UnVerifiedCount = (  ");
            sqlCommand.Append("SELECT COUNT(*) ");
            sqlCommand.Append("FROM cy_LetterSubscribe  ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterInfoGuid = :LetterInfoGuid AND IsVerified = 0  ");
            sqlCommand.Append(")  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterInfoGuid = :LetterInfoGuid ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            


            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the cy_LetterInfo table. Returns true if row deleted.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_LetterInfo ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = :LetterInfoGuid ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the cy_LetterInfo table.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        public static IDataReader GetOne(
            Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_LetterInfo ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = :LetterInfoGuid ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the cy_LetterInfo table.
        /// </summary>
        public static IDataReader GetAll(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_LetterInfo ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("SortRank, Title ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets a count of rows in the cy_LetterInfo table.
        /// </summary>
        public static int GetCount(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	cy_LetterInfo ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        /// <summary>
        /// Gets a page of data from the cy_LetterInfo table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(siteGuid);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	cy_LetterInfo  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append("ORDER BY SortRank, Title ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString()
                + ", :PageSize  ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }


    }
}
