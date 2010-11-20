
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
   
    public static class DBGroups
    {
        public static String DBPlatform()
        {
            return "SQLite";
        }

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



        public static int Create(
            int moduleId,
            int userId,
            string title,
            string description,
            bool isModerated,
            bool isActive,
            int sortOrder,
            int postsPerPage,
            int topicsPerPage,
            bool allowAnonymousPosts)
        {
            byte isMod = 1;
            if (!isModerated)
            {
                isMod = 0;
            }

            byte active = 1;
            if (!isActive)
            {
                active = 0;
            }

            byte allowAnonymous = 1;
            if (!allowAnonymousPosts)
            {
                allowAnonymous = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO	cy_Groups ( ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description , ");
            sqlCommand.Append("IsModerated , ");
            sqlCommand.Append("IsActive , ");
            sqlCommand.Append("SortOrder , ");
            sqlCommand.Append("PostsPerPage , ");
            sqlCommand.Append("TopicsPerPage , ");
            sqlCommand.Append("AllowAnonymousPosts  ");
            sqlCommand.Append(" ) ");

            sqlCommand.Append("VALUES (");

            sqlCommand.Append(" :ModuleID , ");
            sqlCommand.Append(" :UserID  , ");
            sqlCommand.Append(" datetime('now','localtime'), ");
            sqlCommand.Append(" :Title , ");
            sqlCommand.Append(" :Description , ");
            sqlCommand.Append(" :IsModerated , ");
            sqlCommand.Append(" :IsActive , ");
            sqlCommand.Append(" :SortOrder , ");
            sqlCommand.Append(" :PostsPerPage , ");
            sqlCommand.Append(" :TopicsPerPage , ");
            sqlCommand.Append(" :AllowAnonymousPosts  ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[10];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new SqliteParameter(":Title", DbType.String, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqliteParameter(":Description", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new SqliteParameter(":IsModerated", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = isMod;

            arParams[5] = new SqliteParameter(":IsActive", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = active;

            arParams[6] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = sortOrder;

            arParams[7] = new SqliteParameter(":PostsPerPage", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = postsPerPage;

            arParams[8] = new SqliteParameter(":TopicsPerPage", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = topicsPerPage;

            arParams[9] = new SqliteParameter(":AllowAnonymousPosts", DbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = allowAnonymous;

            int newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }


        public static bool Update(
            int itemId,
            int userId,
            string title,
            string description,
            bool isModerated,
            bool isActive,
            int sortOrder,
            int postsPerPage,
            int topicsPerPage,
            bool allowAnonymousPosts)
        {
            byte moderated = 1;
            if (!isModerated)
            {
                moderated = 0;
            }

            byte active = 1;
            if (!isActive)
            {
                active = 0;
            }

            byte allowAnonymous = 1;
            if (!allowAnonymousPosts)
            {
                allowAnonymous = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE		cy_Groups ");
            sqlCommand.Append("SET	Title = :Title, ");
            sqlCommand.Append("Description = :Description, ");
            sqlCommand.Append("IsModerated = :IsModerated, ");
            sqlCommand.Append("IsActive = :IsActive, ");
            sqlCommand.Append("SortOrder = :SortOrder, ");
            sqlCommand.Append("PostsPerPage = :PostsPerPage, ");
            sqlCommand.Append("TopicsPerPage = :TopicsPerPage, ");
            sqlCommand.Append("AllowAnonymousPosts = :AllowAnonymousPosts ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[9];


            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqliteParameter(":Title", DbType.String, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new SqliteParameter(":Description", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new SqliteParameter(":IsModerated", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moderated;

            arParams[4] = new SqliteParameter(":IsActive", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = active;

            arParams[5] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sortOrder;

            arParams[6] = new SqliteParameter(":PostsPerPage", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = postsPerPage;

            arParams[7] = new SqliteParameter(":TopicsPerPage", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = topicsPerPage;

            arParams[8] = new SqliteParameter(":AllowAnonymousPosts", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = allowAnonymous;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_Groups ");
            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_GroupPosts WHERE TopicID IN (SELECT TopicID FROM cy_GroupTopics WHERE GroupID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID = :ModuleID) );");
            sqlCommand.Append("DELETE FROM cy_GroupTopicSubscriptions WHERE TopicID IN (SELECT TopicID FROM cy_GroupTopics WHERE GroupID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID = :ModuleID) );");
            sqlCommand.Append("DELETE FROM cy_GroupTopics WHERE GroupID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID = :ModuleID);");
            sqlCommand.Append("DELETE FROM cy_GroupSubscriptions WHERE GroupID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID = :ModuleID) ;");
            sqlCommand.Append("DELETE FROM cy_Groups WHERE ModuleID = :ModuleID;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_GroupPosts WHERE TopicID IN (SELECT TopicID FROM cy_GroupTopics WHERE GroupID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID IN  (SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID)) );");
            sqlCommand.Append("DELETE FROM cy_GroupTopicSubscriptions WHERE TopicID IN (SELECT TopicID FROM cy_GroupTopics WHERE GroupID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID IN  (SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID)) );");
            sqlCommand.Append("DELETE FROM cy_GroupTopics WHERE GroupID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID IN  (SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID));");
            sqlCommand.Append("DELETE FROM cy_GroupSubscriptions WHERE GroupID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID IN  (SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID)) ;");
            sqlCommand.Append("DELETE FROM cy_Groups WHERE ModuleID IN (SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID);");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }



        public static IDataReader GetGroups(int moduleId, int userId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT f.*, ");
            sqlCommand.Append("u.Name As MostRecentPostUser, ");
            sqlCommand.Append("s.SubscribeDate IS NOT NULL AND s.UnSubscribeDate IS NULL As Subscribed, ");
            sqlCommand.Append("(SELECT COUNT(*) FROM cy_GroupSubscriptions fs WHERE fs.GroupID = f.ItemID AND fs.UnSubscribeDate IS NULL) As SubscriberCount  ");

            sqlCommand.Append("FROM	cy_Groups f ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON f.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_GroupSubscriptions s ");
            sqlCommand.Append("ON f.ItemID = s.GroupID AND s.UserID = :UserID ");

            sqlCommand.Append("WHERE f.ModuleID	= :ModuleID ");
            sqlCommand.Append("AND f.IsActive = 1 ");
            sqlCommand.Append("ORDER BY		f.SortOrder, f.ItemID ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetGroup(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT f.*, ");
            sqlCommand.Append("u.Name As CreatedByUser, ");
            sqlCommand.Append("up.Name As MostRecentPostUser ");
            sqlCommand.Append("FROM	cy_Groups f ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON f.CreatedBy = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users up ");
            sqlCommand.Append("ON f.MostRecentPostUserID = up.UserID ");
            sqlCommand.Append("WHERE f.ItemID	= :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool IncrementPostCount(
            int groupId,
            int mostRecentPostUserId,
            DateTime mostRecentPostDate)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Groups ");
            sqlCommand.Append("SET MostRecentPostDate = :MostRecentPostDate, ");
            sqlCommand.Append("MostRecentPostUserID = :MostRecentPostUserID, ");
            sqlCommand.Append("PostCount = PostCount + 1 ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            arParams[1] = new SqliteParameter(":MostRecentPostUserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostUserId;

            arParams[2] = new SqliteParameter(":MostRecentPostDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostDate;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateUserStats(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("TotalPosts = (SELECT COUNT(*) FROM cy_GroupPosts WHERE cy_GroupPosts.UserID = cy_Users.UserID) ");
            if (userId > -1)
            {
                sqlCommand.Append("WHERE UserID = :UserID ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool IncrementPostCount(int groupId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Groups ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("PostCount = PostCount + 1 ");
            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DecrementPostCount(int groupId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Groups ");
            sqlCommand.Append("SET PostCount = PostCount - 1 ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool RecalculatePostStats(int groupId)
        {
            DateTime mostRecentPostDate = DateTime.Now;
            int mostRecentPostUserID = 0;
            int postCount = 0;

            StringBuilder sqlCommand = new StringBuilder();
            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":GroupID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("MostRecentPostDate, ");
            sqlCommand.Append("MostRecentPostUserID ");
            sqlCommand.Append("FROM cy_GroupTopics ");
            sqlCommand.Append("WHERE GroupID = :GroupID ");
            sqlCommand.Append("ORDER BY MostRecentPostDate DESC ");
            sqlCommand.Append("LIMIT 1 ;");

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    mostRecentPostUserID = Convert.ToInt32(reader["MostRecentPostUserID"]);
                    mostRecentPostDate = Convert.ToDateTime(reader["MostRecentPostDate"]);
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("SUM(TotalReplies) + COUNT(*) As PostCount ");
            sqlCommand.Append("FROM cy_GroupTopics ");
            sqlCommand.Append("WHERE GroupID = :GroupID ;");

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    postCount = Convert.ToInt32(reader["PostCount"]);
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE	cy_Groups ");
            sqlCommand.Append("SET	 ");
            sqlCommand.Append("MostRecentPostDate = :MostRecentPostDate,	 ");
            sqlCommand.Append("MostRecentPostUserID = :MostRecentPostUserID,	 ");
            sqlCommand.Append("PostCount = :PostCount	 ");
            sqlCommand.Append("WHERE ItemID = :GroupID ;");

            arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":GroupID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            arParams[1] = new SqliteParameter(":MostRecentPostDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostDate;

            arParams[2] = new SqliteParameter(":MostRecentPostUserID", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostUserID;

            arParams[3] = new SqliteParameter(":PostCount", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = postCount;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool IncrementTopicCount(int groupId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE	cy_Groups ");
            sqlCommand.Append("SET	TopicCount = TopicCount + 1 ");
            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DecrementTopicCount(int groupId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Groups ");
            sqlCommand.Append("SET TopicCount = TopicCount - 1 ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static int GetUserTopicCount(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	cy_GroupTopics ");
            sqlCommand.Append("WHERE TopicID IN (Select DISTINCT TopicID FROM cy_GroupPosts WHERE cy_GroupPosts.UserID = :UserID) ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }


        public static IDataReader GetTopicPageByUser(
            int userId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetUserTopicCount(userId);

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
            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" t.*, ");
            sqlCommand.Append("f.Title As Group, ");
            sqlCommand.Append("f.ModuleID, ");
            sqlCommand.Append("(SELECT PageID FROM cy_PageModules WHERE cy_PageModules.ModuleID = f.ModuleID AND (PublishEndDate IS NULL OR PublishEndDate > :CurrentDate) LIMIT 1) As PageID, ");
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("s.Name As StartedBy ");

            sqlCommand.Append("FROM	cy_GroupTopics t ");

            sqlCommand.Append("JOIN	cy_Groups f ");
            sqlCommand.Append("ON t.GroupID = f.ItemID ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");

            sqlCommand.Append("WHERE t.TopicID IN (Select DISTINCT TopicID FROM cy_GroupPosts WHERE cy_GroupPosts.UserID = :UserID) ");

            sqlCommand.Append("ORDER BY	t.MostRecentPostDate DESC  ");

            sqlCommand.Append("LIMIT " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + ", :PageSize ");
            sqlCommand.Append(";");


            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new SqliteParameter(":CurrentDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }



        public static IDataReader GetTopics(int groupId, int pageNumber)
        {
            int topicsPerPage = 1;
            int totalTopics = 0;
            using (IDataReader reader = GetGroup(groupId))
            {
                if (reader.Read())
                {
                    topicsPerPage = Convert.ToInt32(reader["TopicsPerPage"]);
                    totalTopics = Convert.ToInt32(reader["TopicCount"]);
                }
            }

            int pageLowerBound = (topicsPerPage * pageNumber) - topicsPerPage;

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT	t.*, ");
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("s.Name As StartedBy ");
            sqlCommand.Append("FROM	cy_GroupTopics t ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");
            sqlCommand.Append("WHERE	t.GroupID = :GroupID ");
            sqlCommand.Append("ORDER BY	t.MostRecentPostDate DESC ");
            sqlCommand.Append("LIMIT		" + topicsPerPage + " ");
            sqlCommand.Append("OFFSET		" + pageLowerBound + " ");
            sqlCommand.Append(" ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":GroupID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            arParams[1] = new SqliteParameter(":PageNumber", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GroupTopicGetPostCount(int topicId)
        {

            StringBuilder sqlCommand = new StringBuilder();

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            sqlCommand.Append("SELECT COUNT(*) FROM cy_GroupPosts WHERE TopicID = :TopicID ");

            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public static int GetSubscriberCount(int groupId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	cy_GroupSubscriptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("GroupID = :GroupID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("UnSubscribeDate IS NULL");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":GroupID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetSubscriberPage(
            int groupId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetSubscriberCount(groupId);

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
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("fs.SubscriptionID, ");
            sqlCommand.Append("fs.SubscribeDate, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	cy_GroupSubscriptions fs ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("cy_Users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.UserID = fs.UserID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("fs.GroupID = :GroupID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("fs.UnSubscribeDate IS NULL ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("u.Name  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append(", :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":GroupID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static bool AddSubscriber(int groupId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COUNT(*) As SubscriptionCount ");
            sqlCommand.Append("FROM cy_GroupSubscriptions  ");
            sqlCommand.Append("WHERE GroupID = :GroupID AND UserID = :UserID ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":GroupID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int subscriptionCount = 0;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    subscriptionCount = Convert.ToInt32(reader["SubscriptionCount"]);
                }
            }

            sqlCommand = new StringBuilder();


            if (subscriptionCount > 0)
            {
                sqlCommand.Append("UPDATE cy_GroupSubscriptions ");
                sqlCommand.Append("SET SubscribeDate = :SubscribeDate, ");
                sqlCommand.Append("UnSubscribeDate = Null ");
                sqlCommand.Append("WHERE GroupID = :GroupID AND UserID = :UserID ;");

            }
            else
            {

                sqlCommand.Append("INSERT INTO	cy_GroupSubscriptions ( ");
                sqlCommand.Append("GroupID, ");
                sqlCommand.Append("UserID, ");
                sqlCommand.Append("SubscribeDate");
                sqlCommand.Append(") ");
                sqlCommand.Append("VALUES ( ");
                sqlCommand.Append(":GroupID, ");
                sqlCommand.Append(":UserID, ");
                sqlCommand.Append(":SubscribeDate");
                sqlCommand.Append(") ;");

            }

            arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":GroupID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new SqliteParameter(":SubscribeDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteSubscription(int subscriptionId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_GroupSubscriptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SubscriptionID = :SubscriptionID ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SubscriptionID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subscriptionId;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool Unsubscribe(int groupId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_GroupSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = :UnSubscribeDate ");
            sqlCommand.Append("WHERE GroupID = :GroupID AND UserID = :UserID ;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":GroupID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new SqliteParameter(":UnSubscribeDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UnsubscribeAll(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_GroupSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = :UnSubscribeDate ");
            sqlCommand.Append("WHERE UserID = :UserID ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SqliteParameter(":UnSubscribeDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool GroupSubscriptionExists(int groupId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	cy_GroupSubscriptions ");
            sqlCommand.Append("WHERE GroupID = :GroupID AND UserID = :UserID AND UnSubscribeDate IS NULL ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":GroupID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static bool GroupTopicSubscriptionExists(int topicId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	cy_GroupTopicSubscriptions ");
            sqlCommand.Append("WHERE TopicID = :TopicID AND UserID = :UserID AND UnSubscribeDate IS NULL ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static IDataReader GroupTopicGetTopic(int topicId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	t.*, ");
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("f.PostsPerPage As PostsPerPage ");
            sqlCommand.Append("FROM	cy_GroupTopics t ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");
            sqlCommand.Append("JOIN	cy_Groups f ");
            sqlCommand.Append("ON f.ItemID = t.GroupID ");
            sqlCommand.Append("WHERE t.TopicID = :TopicID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GroupTopicGetPost(int postId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	fp.* ");
            sqlCommand.Append("FROM	cy_GroupPosts fp ");
            sqlCommand.Append("WHERE fp.PostID = :PostID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":PostID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GroupTopicCreate(
            int groupId,
            string topicSubject,
            int sortOrder,
            bool isLocked,
            int startedByUserId,
            DateTime topicDate)
        {

            byte locked = 1;
            if (!isLocked)
            {
                locked = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();
            int groupSequence = 1;
            sqlCommand.Append("SELECT COALESCE(Max(GroupSequence) + 1,1) As GroupSequence FROM cy_GroupTopics WHERE GroupID = :GroupID ; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":GroupID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    groupSequence = Convert.ToInt32(reader["GroupSequence"]);
                }
            }


            sqlCommand = new StringBuilder();
            //sqlCommand.Append("SET @GroupSequence = " + GroupSequence.ToString() + " ; ");

            sqlCommand.Append("INSERT INTO cy_GroupTopics ( ");
            sqlCommand.Append("GroupID, ");
            sqlCommand.Append("TopicTitle, ");
            sqlCommand.Append("SortOrder, ");
            sqlCommand.Append("GroupSequence, ");
            sqlCommand.Append("IsLocked, ");
            sqlCommand.Append("StartedByUserID, ");
            sqlCommand.Append("TopicDate, ");
            sqlCommand.Append("MostRecentPostUserID, ");
            sqlCommand.Append("MostRecentPostDate ");
            sqlCommand.Append(" ) ");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append(" :GroupID , ");
            sqlCommand.Append(" :TopicTitle  , ");
            sqlCommand.Append(" :SortOrder, ");
            sqlCommand.Append(" :GroupSequence, ");
            sqlCommand.Append(" :IsLocked , ");
            sqlCommand.Append(" :StartedByUserID , ");
            sqlCommand.Append(" :TopicDate , ");
            sqlCommand.Append(" :StartedByUserID , ");
            sqlCommand.Append(" :TopicDate  ");
            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            arParams = new SqliteParameter[7];

            arParams[0] = new SqliteParameter(":GroupID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            arParams[1] = new SqliteParameter(":TopicTitle", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = topicSubject;

            arParams[2] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = sortOrder;

            arParams[3] = new SqliteParameter(":IsLocked", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = locked;

            arParams[4] = new SqliteParameter(":StartedByUserID", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startedByUserId;

            arParams[5] = new SqliteParameter(":GroupSequence", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = groupSequence;

            arParams[6] = new SqliteParameter(":TopicDate", DbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = topicDate;

            int newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_GroupTopicSubscriptions (TopicID, UserID) ");
            sqlCommand.Append("    SELECT :TopicID as TopicID, UserID from cy_GroupSubscriptions fs ");
            sqlCommand.Append("        WHERE fs.GroupID = :GroupID AND fs.SubscribeDate IS NOT NULL AND fs.UnSubscribeDate IS NULL;");

            arParams = new SqliteParameter[2];
            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = newID;

            arParams[1] = new SqliteParameter(":GroupID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = groupId;

            SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return newID;

        }

        public static bool GroupTopicDelete(int topicId)
        {
            GroupTopicDeletePosts(topicId);
            GroupTopicDeleteSubscriptions(topicId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_GroupTopics ");
            sqlCommand.Append("WHERE TopicID = :TopicID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool GroupTopicDeletePosts(int topicId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_GroupPosts ");
            sqlCommand.Append("WHERE TopicID = :TopicID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool GroupTopicDeleteSubscriptions(int topicId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_GroupTopicSubscriptions ");
            sqlCommand.Append("WHERE TopicID = :TopicID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool GroupTopicUpdate(
            int topicId,
            int groupId,
            string topicSubject,
            int sortOrder,
            bool isLocked)
        {
            byte locked = 1;
            if (!isLocked)
            {
                locked = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE	 cy_GroupTopics ");
            sqlCommand.Append("SET	GroupID = :GroupID, ");
            sqlCommand.Append("TopicTitle = :TopicTitle, ");
            sqlCommand.Append("SortOrder = :SortOrder, ");
            sqlCommand.Append("IsLocked = :IsLocked ");


            sqlCommand.Append("WHERE TopicID = :TopicID ;");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            arParams[1] = new SqliteParameter(":GroupID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = groupId;

            arParams[2] = new SqliteParameter(":TopicTitle", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = topicSubject;

            arParams[3] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new SqliteParameter(":IsLocked", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = locked;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool GroupTopicIncrementReplyStats(
            int topicId,
            int mostRecentPostUserId,
            DateTime mostRecentPostDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_GroupTopics ");
            sqlCommand.Append("SET MostRecentPostUserID = :MostRecentPostUserID, ");
            sqlCommand.Append("TotalReplies = TotalReplies + 1, ");
            sqlCommand.Append("MostRecentPostDate = :MostRecentPostDate ");
            sqlCommand.Append("WHERE TopicID = :TopicID ;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            arParams[1] = new SqliteParameter(":MostRecentPostUserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostUserId;

            arParams[2] = new SqliteParameter(":MostRecentPostDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostDate;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > -1);

        }

        public static bool GroupTopicDecrementReplyStats(int topicId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT UserID, PostDate ");
            sqlCommand.Append("FROM cy_GroupPosts ");
            sqlCommand.Append("WHERE TopicID = :TopicID ");
            sqlCommand.Append("ORDER BY PostID DESC ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            int userID = 0;
            DateTime postDate = DateTime.Now;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    userID = Convert.ToInt32(reader["UserID"]);
                    postDate = Convert.ToDateTime(reader["PostDate"]);
                }
            }

            sqlCommand = new StringBuilder();


            sqlCommand.Append("UPDATE cy_GroupTopics ");
            sqlCommand.Append("SET MostRecentPostUserID = :MostRecentPostUserID, ");
            sqlCommand.Append("TotalReplies = TotalReplies - 1, ");
            sqlCommand.Append("MostRecentPostDate = :MostRecentPostDate ");
            sqlCommand.Append("WHERE TopicID = :TopicID ;");

            arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            arParams[1] = new SqliteParameter(":MostRecentPostUserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userID;

            arParams[2] = new SqliteParameter(":MostRecentPostDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = postDate;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool GroupTopicUpdateViewStats(int topicId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_GroupTopics ");
            sqlCommand.Append("SET TotalViews = TotalViews + 1 ");
            sqlCommand.Append("WHERE TopicID = :TopicID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GroupTopicGetPosts(int topicId, int pageNumber)
        {

            StringBuilder sqlCommand = new StringBuilder();

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            int postsPerPage = 10;

            sqlCommand.Append("SELECT	f.PostsPerPage As PostsPerPage ");
            sqlCommand.Append("FROM		cy_GroupTopics ft ");
            sqlCommand.Append("JOIN		cy_Groups f ");
            sqlCommand.Append("ON		ft.GroupID = f.ItemID ");
            sqlCommand.Append("WHERE	ft.TopicID = :TopicID ;");

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                if (reader.Read())
                {
                    postsPerPage = Convert.ToInt32(reader["PostsPerPage"]);
                }
            }

            sqlCommand = new StringBuilder();
            int currentPageMaxTopicSequence = postsPerPage * pageNumber;
            int beginSequence = 0;
           
            if (currentPageMaxTopicSequence > postsPerPage)
            {
                beginSequence = currentPageMaxTopicSequence - postsPerPage;

            }


            sqlCommand.Append("SELECT	p.*, ");
            sqlCommand.Append("ft.GroupID As GroupID, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
            sqlCommand.Append("COALESCE(up.Email, '') As AuthorEmail, ");
            sqlCommand.Append("COALESCE(up.TotalPosts, 0) As PostAuthorTotalPosts, ");
            sqlCommand.Append("COALESCE(up.TotalRevenue, 0) As UserRevenue, ");
            sqlCommand.Append("COALESCE(up.Trusted, 0) As Trusted, ");
            sqlCommand.Append("COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, ");
            sqlCommand.Append("up.WebSiteURL As PostAuthorWebSiteUrl, ");
            sqlCommand.Append("up.Signature As PostAuthorSignature ");
            sqlCommand.Append("FROM	cy_GroupPosts p ");
            sqlCommand.Append("JOIN	cy_GroupTopics ft ");
            sqlCommand.Append("ON p.TopicID = ft.TopicID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON ft.MostRecentPostUserID = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN cy_Users s ");
            sqlCommand.Append("ON ft.StartedByUserID = s.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users up ");
            sqlCommand.Append("ON up.UserID = p.UserID ");
            sqlCommand.Append("WHERE ft.TopicID = :TopicID ");

            sqlCommand.Append("ORDER BY	p.SortOrder, p.PostID ");
            sqlCommand.Append("LIMIT		" + postsPerPage + " ");
            sqlCommand.Append("OFFSET		" + beginSequence + " ; ");

            arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            arParams[1] = new SqliteParameter(":PageNumber", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GroupTopicGetPosts(int topicId)
        {

            StringBuilder sqlCommand = new StringBuilder();

            SqliteParameter[] arParams;

            sqlCommand.Append("SELECT	p.*, ");
            sqlCommand.Append("ft.GroupID As GroupID, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
            sqlCommand.Append("up.TotalPosts As PostAuthorTotalPosts, ");
            sqlCommand.Append("COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, ");
            sqlCommand.Append("up.WebSiteURL As PostAuthorWebSiteUrl, ");
            sqlCommand.Append("up.Signature As PostAuthorSignature ");
            sqlCommand.Append("FROM	cy_GroupPosts p ");
            sqlCommand.Append("JOIN	cy_GroupTopics ft ");
            sqlCommand.Append("ON p.TopicID = ft.TopicID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON ft.MostRecentPostUserID = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN cy_Users s ");
            sqlCommand.Append("ON ft.StartedByUserID = s.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users up ");
            sqlCommand.Append("ON up.UserID = p.UserID ");
            sqlCommand.Append("WHERE ft.TopicID = :TopicID ");

            sqlCommand.Append("ORDER BY	p.PostID  ;");

            arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GroupTopicGetPostsReverseSorted(int topicId)
        {

            StringBuilder sqlCommand = new StringBuilder();

            SqliteParameter[] arParams;

            sqlCommand.Append("SELECT	p.*, ");
            sqlCommand.Append("ft.GroupID As GroupID, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
            sqlCommand.Append("COALESCE(up.Email, '') As AuthorEmail, ");
            sqlCommand.Append("up.TotalPosts As PostAuthorTotalPosts, ");
            sqlCommand.Append("COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, ");
            sqlCommand.Append("up.WebSiteURL As PostAuthorWebSiteUrl, ");
            sqlCommand.Append("up.Signature As PostAuthorSignature ");
            sqlCommand.Append("FROM	cy_GroupPosts p ");
            sqlCommand.Append("JOIN	cy_GroupTopics ft ");
            sqlCommand.Append("ON p.TopicID = ft.TopicID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON ft.MostRecentPostUserID = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN cy_Users s ");
            sqlCommand.Append("ON ft.StartedByUserID = s.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users up ");
            sqlCommand.Append("ON up.UserID = p.UserID ");
            sqlCommand.Append("WHERE ft.TopicID = :TopicID ");

            sqlCommand.Append("ORDER BY p.TopicSequence DESC  ;");

            arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GroupTopicGetPostsByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  fp.*, ");
            sqlCommand.Append("f.ModuleID As ModuleID, ");
            sqlCommand.Append("f.ItemID As ItemID, ");
            sqlCommand.Append("m.ModuleTitle As ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles As ViewRoles, ");
            sqlCommand.Append("md.FeatureName As FeatureName, ");
            sqlCommand.Append("md.ResourceFile As ResourceFile ");
            sqlCommand.Append("FROM	cy_GroupPosts fp ");
            sqlCommand.Append("JOIN	cy_GroupTopics ft ");
            sqlCommand.Append("ON fp.TopicID = ft.TopicID ");
            sqlCommand.Append("JOIN	cy_Groups f ");
            sqlCommand.Append("ON f.ItemID = ft.GroupID ");
            sqlCommand.Append("JOIN	cy_Modules m ");
            sqlCommand.Append("ON f.ModuleID = m.ModuleID ");
            sqlCommand.Append("JOIN	cy_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");
            sqlCommand.Append("JOIN	cy_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");
            sqlCommand.Append("JOIN	cy_Pages p ");
            sqlCommand.Append("ON p.PageID = pm.PageID ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.SiteID = :SiteID ");
            sqlCommand.Append("AND pm.PageID = :PageID ");

            sqlCommand.Append(" ; ");
            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":PageID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GroupTopicGetPostsForRss(int siteId, int pageId, int moduleId, int itemId, int topicId, int maximumDays)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT		fp.*, ");
            sqlCommand.Append("ft.TopicTitle As TopicTitle, ");
            sqlCommand.Append("ft.GroupID As GroupID, ");
            sqlCommand.Append("COALESCE(s.[Name],'Guest') as StartedBy, ");
            sqlCommand.Append("COALESCE(up.[Name], 'Guest') as PostAuthor, ");
            sqlCommand.Append("up.TotalPosts as PostAuthorTotalPosts,");
            sqlCommand.Append("up.AvatarUrl as PostAuthorAvatar, ");
            sqlCommand.Append("up.WebSiteURL as PostAuthorWebSiteUrl, ");
            sqlCommand.Append("up.Signature as PostAuthorSignature ");

            sqlCommand.Append("FROM		cy_GroupPosts fp ");
            sqlCommand.Append("JOIN		cy_GroupTopics ft ");
            sqlCommand.Append("ON		fp.TopicID = ft.TopicID ");

            sqlCommand.Append("JOIN		cy_Groups f ");
            sqlCommand.Append("ON		ft.GroupID = f.ItemID ");

            sqlCommand.Append("JOIN		cy_Modules m ");
            sqlCommand.Append("ON		f.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN		cy_PageModules pm ");
            sqlCommand.Append("ON		pm.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN		cy_Pages p ");
            sqlCommand.Append("ON		pm.PageID = p.PageID ");

            sqlCommand.Append("LEFT OUTER JOIN		cy_Users u ");
            sqlCommand.Append("ON		ft.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN		cy_Users s ");
            sqlCommand.Append("ON		ft.StartedByUserID = s.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN		cy_Users up ");
            sqlCommand.Append("ON		up.UserID = fp.UserID ");

            sqlCommand.Append("WHERE	p.SiteID = :SiteID ");
            sqlCommand.Append("AND	(:PageID = -1 OR p.PageID = :PageID) ");
            sqlCommand.Append("AND	(:ModuleID = -1 OR m.ModuleID = :ModuleID) ");
            sqlCommand.Append("AND	(:ItemID = -1 OR f.ItemID = :ItemID) ");
            sqlCommand.Append("AND	(:TopicID = -1 OR ft.TopicID = :TopicID) ");
            sqlCommand.Append("AND	(:MaximumDays = -1 OR datetime(fp.PostDate) >= datetime('now', '-" + maximumDays + " days')) ");

            sqlCommand.Append("ORDER BY	fp.PostDate DESC ; ");

            SqliteParameter[] arParams = new SqliteParameter[6];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":PageID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            arParams[2] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleId;

            arParams[3] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = itemId;

            arParams[4] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = topicId;

            arParams[5] = new SqliteParameter(":MaximumDays", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = maximumDays;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static DataSet GroupTopicGetSubscribers(int groupId, int topicId, int currentPostUserId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT u.Email As Email ");
            sqlCommand.Append("FROM	cy_Users u ");

            sqlCommand.Append("WHERE u.UserID <> ?CurrentPostUserID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("(");

            sqlCommand.Append("(");
            sqlCommand.Append("u.UserID IN (SELECT UserID FROM cy_GroupTopicSubscriptions ");
            sqlCommand.Append("WHERE TopicID = :TopicID ");
            sqlCommand.Append("AND UnSubscribeDate IS NULL) ");
            sqlCommand.Append(")");

            sqlCommand.Append("OR ");

            sqlCommand.Append("(");
            sqlCommand.Append("u.UserID IN (SELECT UserID FROM cy_GroupSubscriptions ");
            sqlCommand.Append("WHERE GroupID = :GroupID ");
            sqlCommand.Append("AND UnSubscribeDate IS NULL) ");
            sqlCommand.Append(")");

            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":GroupID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = groupId;

            arParams[1] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = topicId;

            arParams[2] = new SqliteParameter(":CurrentPostUserID", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentPostUserId;

            return SqliteHelper.ExecuteDataset(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool GroupTopicAddSubscriber(int topicId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COUNT(*) As SubscriptionCount ");
            sqlCommand.Append("FROM cy_GroupTopicSubscriptions  ");
            sqlCommand.Append("WHERE TopicID = :TopicID AND UserID = :UserID ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int subscriptionCount = 0;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    subscriptionCount = Convert.ToInt32(reader["SubscriptionCount"]);
                }
            }

            sqlCommand = new StringBuilder();


            if (subscriptionCount > 0)
            {
                sqlCommand.Append("UPDATE cy_GroupTopicSubscriptions ");
                sqlCommand.Append("SET SubscribeDate = datetime('now','localtime'), ");
                sqlCommand.Append("UnSubscribeDate = Null ");
                sqlCommand.Append("WHERE TopicID = :TopicID AND UserID = :UserID ;");

            }
            else
            {

                sqlCommand.Append("INSERT INTO	cy_GroupTopicSubscriptions ( ");
                sqlCommand.Append("TopicID, ");
                sqlCommand.Append("UserID ");
                sqlCommand.Append(") ");
                sqlCommand.Append("VALUES ( ");
                sqlCommand.Append(":TopicID, ");
                sqlCommand.Append(":UserID ");
                sqlCommand.Append(") ;");

            }

            arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool GroupTopicUNSubscribe(int topicId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_GroupTopicSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = datetime('now','localtime') ");
            sqlCommand.Append("WHERE TopicID = :TopicID AND UserID = :UserID ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool GroupTopicUnsubscribeAll(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_GroupTopicSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = datetime('now','localtime') ");
            sqlCommand.Append("WHERE UserID = :UserID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static int GroupPostCreate(
            int topicId,
            string subject,
            string post,
            bool approved,
            int userId,
            DateTime postDate)
        {

            byte approve = 1;
            if (!approved)
            {
                approve = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COALESCE(Max(TopicSequence) + 1,1) As TopicSequence FROM cy_GroupPosts WHERE TopicID = :TopicID ; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            int topicSequence = 1;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    topicSequence = Convert.ToInt32(reader["TopicSequence"]);
                }
            }

            sqlCommand = new StringBuilder();

            sqlCommand.Append("INSERT INTO cy_GroupPosts ( ");
            sqlCommand.Append("TopicID, ");
            sqlCommand.Append("Subject, ");
            sqlCommand.Append("Post, ");
            sqlCommand.Append("PostDate, ");
            sqlCommand.Append("Approved, ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("TopicSequence ");

            sqlCommand.Append(" ) ");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append(" :TopicID , ");
            sqlCommand.Append(" :Subject  , ");
            sqlCommand.Append(" :Post, ");
            sqlCommand.Append(" :PostDate, ");
            sqlCommand.Append(" :Approved , ");
            sqlCommand.Append(" :UserID , ");
            sqlCommand.Append(" :TopicSequence  ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            arParams = new SqliteParameter[7];

            arParams[0] = new SqliteParameter(":TopicID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = topicId;

            arParams[1] = new SqliteParameter(":Subject", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = subject;

            arParams[2] = new SqliteParameter(":Post", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = post;

            arParams[3] = new SqliteParameter(":Approved", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = approve;

            arParams[4] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userId;

            arParams[5] = new SqliteParameter(":TopicSequence", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = topicSequence;

            arParams[6] = new SqliteParameter(":PostDate", DbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = postDate;

            int newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }

        public static bool GroupPostUpdate(
            int postId,
            string subject,
            string post,
            int sortOrder,
            bool approved)
        {
            byte approve = 1;
            if (!approved)
            {
                approve = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_GroupPosts ");
            sqlCommand.Append("SET Subject = :Subject, ");
            sqlCommand.Append("Post = :Post, ");
            sqlCommand.Append("SortOrder = :SortOrder, ");
            sqlCommand.Append("Approved = :Approved ");
            sqlCommand.Append("WHERE PostID = :PostID ;");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":PostID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            arParams[1] = new SqliteParameter(":Subject", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = subject;

            arParams[2] = new SqliteParameter(":Post", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = post;

            arParams[3] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new SqliteParameter(":Approved", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = approve;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool GroupPostUpdateTopicSequence(
            int postId,
            int topicSequence)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_GroupPosts ");
            sqlCommand.Append("SET TopicSequence = :TopicSequence ");
            sqlCommand.Append("WHERE PostID = :PostID ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":PostID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            arParams[1] = new SqliteParameter(":TopicSequence", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = topicSequence;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool GroupPostDelete(int postId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_GroupPosts ");
            sqlCommand.Append("WHERE PostID = :PostID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":PostID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }






    }
}
