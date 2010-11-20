

using System;
using System.Data;
using log4net;
using Cynthia.Data;

namespace Cynthia.Business
{
    /// <summary>
    /// Represents a group
    /// </summary>
    public class Group
    {

        #region Constructors

        public Group()
        { }

        public Group(int groupId)
        {
            if (groupId > -1)
            {
                GetGroup(groupId);
            }

        }

        #endregion

        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(Group));

        private int itemID = -1;
        private int moduleID = -1;
        private DateTime createdDate = DateTime.UtcNow;
        private string createdBy = string.Empty;
        private int createdByUserID;
        private string title = string.Empty;
        private string description = string.Empty;
        private bool isModerated;
        private bool isActive = true;
        //sorted in db by SortOrder, ItemID
        private int sortOrder = 100;
        private int postsPerPage = 10;
        private int topicsPerPage = 20;
        private int topicCount;
        private int postCount;
        private int totalPages = 1;
        private DateTime mostRecentPostDate = DateTime.UtcNow;
        private string mostRecentPostUser = string.Empty;
        private bool allowAnonymousPosts = false;


        #endregion

        #region Public Properties

        public int ItemId
        {
            get { return itemID; }
        }

        public int ModuleId
        {
            get { return moduleID; }
            set { moduleID = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        public int CreatedByUserId
        {
            get { return createdByUserID; }
            set { createdByUserID = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public bool AllowAnonymousPosts
        {
            get { return allowAnonymousPosts; }
            set { allowAnonymousPosts = value; }
        }


        public bool IsModerated
        {
            get { return isModerated; }
            set { isModerated = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        public int PostsPerPage
        {
            get { return postsPerPage; }
            set { postsPerPage = value; }
        }

        public int TopicsPerPage
        {
            get { return topicsPerPage; }
            set { topicsPerPage = value; }
        }

        public int TopicCount
        {
            get { return topicCount; }
            set { topicCount = value; }
        }

        public int PostCount
        {
            get { return postCount; }
            set { postCount = value; }
        }

        public int TotalPages
        {
            get { return totalPages; }
        }

        public DateTime MostRecentPostDate
        {
            get { return mostRecentPostDate; }
        }

        public string MostRecentPostUser
        {
            get { return mostRecentPostUser; }
        }




        #endregion

        #region Private Methods

        private void GetGroup(int groupId)
        {
            using (IDataReader reader = DBGroups.GetGroup(groupId))
            {
                if (reader.Read())
                {

                    this.itemID = int.Parse(reader["ItemID"].ToString());
                    this.moduleID = int.Parse(reader["ModuleID"].ToString());
                    this.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                    this.createdBy = reader["CreatedByUser"].ToString();
                    this.title = reader["Title"].ToString();
                    this.description = reader["Description"].ToString();
                    // this is to support dbs that don't have bit data type
                    string anon = reader["AllowAnonymousPosts"].ToString();
                    this.allowAnonymousPosts = (anon == "True" || anon == "1");
                    string moderated = reader["IsModerated"].ToString();
                    this.isModerated = (moderated == "True" || moderated == "1");
                    string active = reader["IsActive"].ToString();
                    this.isActive = (active == "True" || active == "1");
                    this.sortOrder = int.Parse(reader["SortOrder"].ToString());
                    this.postsPerPage = int.Parse(reader["PostsPerPage"].ToString());
                    this.topicsPerPage = int.Parse(reader["TopicsPerPage"].ToString());
                    this.topicCount = int.Parse(reader["TopicCount"].ToString());
                    this.postCount = int.Parse(reader["PostCount"].ToString());
                    if (reader["MostRecentPostDate"] != DBNull.Value)
                    {
                        this.mostRecentPostDate = Convert.ToDateTime(reader["MostRecentPostDate"]);
                    }
                    this.mostRecentPostUser = reader["MostRecentPostUser"].ToString();

                    if (this.topicCount > this.topicsPerPage)
                    {
                        this.totalPages = this.topicCount / this.topicsPerPage;
                        int remainder = 0;
                        Math.DivRem(this.topicCount, this.topicsPerPage, out remainder);
                        if (remainder > 0)
                        {
                            this.totalPages += 1;
                        }
                    }
                    else
                    {
                        this.totalPages = 1;
                    }

                }
                else
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("IDataReader didn't read in Group.GetGroup");
                    }


                }
            }
            


        }

        private bool Create()
        {
            int newID = -1;

            newID = DBGroups.Create(
                this.moduleID,
                this.createdByUserID,
                this.title,
                this.description,
                this.isModerated,
                this.isActive,
                this.sortOrder,
                this.postsPerPage,
                this.topicsPerPage,
                this.allowAnonymousPosts);

            this.itemID = newID;

            return (newID > -1);

        }


        private bool Update()
        {

            return DBGroups.Update(
                this.itemID,
                this.createdByUserID,
                this.title,
                this.description,
                this.isModerated,
                this.isActive,
                this.sortOrder,
                this.postsPerPage,
                this.topicsPerPage,
                this.allowAnonymousPosts);
        }

        #endregion

        #region Public Methods


        public bool Save()
        {
            if (this.itemID > -1)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }


        public IDataReader GetTopics(int pageNumber)
        {
            return DBGroups.GetTopics(this.itemID, pageNumber);
        }

        public bool Subscribe(int userId)
        {
            return DBGroups.AddSubscriber(this.itemID, userId);
        }

        public bool Unsubscribe(int userId)
        {
            return DBGroups.Unsubscribe(this.itemID, userId);
        }

        #endregion

        #region Static Methods

        public static bool UnsubscribeAll(int userId)
        {
            return DBGroups.UnsubscribeAll(userId);
        }

        public static IDataReader GetGroups(int moduleId, int userId)
        {
            return DBGroups.GetGroups(moduleId, userId);
        }


        public static bool IncrementPostCount(int groupId, int mostRecentPostUserId, DateTime mostRecentPostDate)
        {
            return DBGroups.IncrementPostCount(groupId, mostRecentPostUserId, mostRecentPostDate);
        }

        public static bool IncrementPostCount(int groupId)
        {
            return DBGroups.IncrementPostCount(groupId);
        }

        public static bool DecrementPostCount(int groupId)
        {
            return DBGroups.DecrementPostCount(groupId);
        }

        public static bool RecalculatePostStats(int groupId)
        {
            //implemented for PostgreSQL. --Dean 9/11/05
            return DBGroups.RecalculatePostStats(groupId);

        }

        public static bool DecrementTopicCount(int groupId)
        {
            return DBGroups.DecrementTopicCount(groupId);
        }

        public static bool IncrementTopicCount(int groupId)
        {
            return DBGroups.IncrementTopicCount(groupId);
        }

        public static bool Delete(int itemId)
        {
            return DBGroups.Delete(itemId);
        }

        public static bool DeleteByModule(int moduleId)
        {
            return DBGroups.DeleteByModule(moduleId);
        }

        public static bool DeleteBySite(int siteId)
        {
            return DBGroups.DeleteBySite(siteId);
        }

        public static bool IsSubscribed(int groupId, int userId)
        {
            return DBGroups.GroupSubscriptionExists(groupId, userId);
        }

        /// <summary>
        /// passing in -1 for userId will update the stats of all users.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool UpdateUserStats(int userId)
        {
            return DBGroups.UpdateUserStats(userId);
        }

        public static IDataReader GetSubscriberPage(
            int groupId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBGroups.GetSubscriberPage(
                groupId,
                pageNumber,
                pageSize,
                out totalPages);
        }

        public static bool DeleteSubscription(int subscriptionId)
        {
            return DBGroups.DeleteSubscription(subscriptionId);
        }


        #endregion

    }
}
