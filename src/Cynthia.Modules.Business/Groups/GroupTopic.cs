

using System;
using System.Configuration;
using System.Data;
using log4net;
using Cynthia.Data;

namespace Cynthia.Business
{
    /// <summary>
    /// Encapsulates a topic and post
    /// </summary>
    public class GroupTopic : IIndexableContent
    {

        #region Constructors

        public GroupTopic()
        {

        }

        public GroupTopic(int topicId)
        {
            GetTopic(topicId);
        }

        public GroupTopic(int topicId, int postId)
        {
            GetTopic(topicId);
            GetPost(postId);
        }

        #endregion


        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(GroupTopic));


        private int topicID = -1;
        private int groupID = -1;
        private int moduleID = -1;
        private int origGroupID = 0;
        private DateTime topicDate = DateTime.UtcNow;
        private string startedBy = string.Empty;
        private int startedByUserID = -1;
        private string subject = string.Empty;
        private int totalViews = 0;
        private int totalReplies = 0;
        private bool isLocked = false;
        //sorted in db by SortOrder, ItemID
        private int sortOrder = 100;
        private int groupSequence = 1;
        private DateTime mostRecentPostDate = DateTime.UtcNow;
        private string mostRecentPostUser = string.Empty;
        private int mostRecentPostUserID = 0;
        private int postsPerPage = 0;
        private int totalPages = 1;

        //post properties
        private int postID = -1;
        private int topicSequence = 1;
        private string postSubject = string.Empty;
        private string postDate = string.Empty;
        private bool isApproved = true;
        private int postUserID = -1;
        private string postUserName = string.Empty;
        private int postSortOrder = 100;
        private string postMessage = string.Empty;
        private bool subscribeUserToTopic = false;
        private int siteId = -1;
        private string searchIndexPath = string.Empty;


        #endregion


        #region Public Properties

        /// <summary>
        /// This is not persisted to the db. It is only set and used when indexing group topics in the search index.
        /// Its a convenience because when we queue the task to index on a new topic we can only pass one object.
        /// So we store extra properties here so we don't need any other objects.
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        /// <summary>
        /// This is not persisted to the db. It is only set and used when indexing group topics in the search index.
        /// Its a convenience because when we queue the task to index on a new topic we can only pass one object.
        /// So we store extra properties here so we don't need any other objects.
        /// </summary>
        public string SearchIndexPath
        {
            get { return searchIndexPath; }
            set { searchIndexPath = value; }
        }

        public int TopicId
        {
            get { return topicID; }
        }

        public int GroupId
        {
            get { return groupID; }
            set { groupID = value; }
        }

        public int ModuleId
        {
            get { return moduleID; }

        }

        public DateTime TopicDate
        {
            get { return topicDate; }
        }

        public string StartedBy
        {
            get { return startedBy; }
            set { startedBy = value; }
        }

        public int StartedByUserId
        {
            get { return startedByUserID; }
            set { startedByUserID = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public int TotalViews
        {
            get { return totalViews; }
        }

        public int TotalReplies
        {
            get { return totalReplies; }
        }

        public int TotalPages
        {
            get { return totalPages; }
        }


        public bool IsLocked
        {
            get { return isLocked; }
            set { isLocked = value; }
        }

        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        public int GroupSequence
        {
            get { return groupSequence; }
        }


        public DateTime MostRecentPostDate
        {
            get { return mostRecentPostDate; }
        }

        public string MostRecentPostUser
        {
            get { return mostRecentPostUser; }
        }

        public int MostRecentPostUserId
        {
            get { return mostRecentPostUserID; }
        }

        //post properties

        public int PostId
        {
            get { return postID; }
        }

        public int TopicSequence
        {
            get { return topicSequence; }
        }

        public string PostSubject
        {
            get { return postSubject; }
            set { postSubject = value; }
        }

        public string PostDate
        {
            get { return postDate; }
        }

        public bool IsApproved
        {
            get { return isApproved; }
            set { isApproved = value; }
        }

        public int PostUserId
        {
            get { return postUserID; }
            set { postUserID = value; }
        }

        public string PostUserName
        {
            get { return postUserName; }
            set { postUserName = value; }
        }

        public int PostSortOrder
        {
            get { return postSortOrder; }
            set { postSortOrder = value; }
        }

        public string PostMessage
        {
            get { return postMessage; }
            set { postMessage = value; }
        }

        public bool SubscribeUserToTopic
        {
            get { return subscribeUserToTopic; }
            set { subscribeUserToTopic = value; }
        }


        #endregion


        #region Private Methods

        private void GetTopic(int topicId)
        {
            using (IDataReader reader = DBGroups.GroupTopicGetTopic(topicId))
            {
                if (reader.Read())
                {

                    this.topicID = int.Parse(reader["TopicID"].ToString());
                    if (reader["GroupID"] != DBNull.Value)
                    {
                        this.groupID = this.origGroupID = int.Parse(reader["GroupID"].ToString());
                    }
                    if (reader["TopicDate"] != DBNull.Value)
                    {
                        this.topicDate = Convert.ToDateTime(reader["TopicDate"].ToString());
                    }
                    this.startedBy = reader["StartedBy"].ToString();
                    if (reader["StartedByUserID"] != DBNull.Value)
                    {
                        this.startedByUserID = int.Parse(reader["StartedByUserID"].ToString());
                    }

                    this.subject = reader["TopicTitle"].ToString();
                    if (reader["TotalViews"] != DBNull.Value)
                    {
                        this.totalViews = Convert.ToInt32(reader["TotalViews"]);
                    }

                    if (reader["TotalReplies"] != DBNull.Value)
                    {
                        this.totalReplies = Convert.ToInt32(reader["TotalReplies"]);
                    }

                    if (reader["SortOrder"] != DBNull.Value)
                    {
                        this.sortOrder = Convert.ToInt32(reader["SortOrder"]);
                    }
                    if (reader["GroupSequence"] != DBNull.Value)
                    {
                        this.groupSequence = Convert.ToInt32(reader["GroupSequence"]);
                    }

                    if (reader["PostsPerPage"] != DBNull.Value)
                    {
                        this.postsPerPage = Convert.ToInt32(reader["PostsPerPage"]);
                    }

                    if (this.totalReplies + 1 > this.postsPerPage)
                    {
                        this.totalPages = this.totalReplies / this.postsPerPage;
                        int remainder = 0;
                        int pageCount = Math.DivRem(this.totalReplies + 1, this.postsPerPage, out remainder);
                        if ((remainder > 0) || (pageCount > this.totalPages))
                        {
                            this.totalPages += 1;
                        }
                    }
                    else
                    {
                        this.totalPages = 1;
                    }

                    // this is to support dbs that don't have bit data type
                    string locked = reader["IsLocked"].ToString();
                    this.isLocked = (locked == "True" || locked == "1");

                    if (reader["MostRecentPostDate"] != DBNull.Value)
                    {
                        this.mostRecentPostDate = Convert.ToDateTime(reader["MostRecentPostDate"]);
                    }

                    this.mostRecentPostUser = reader["MostRecentPostUser"].ToString();

                    if (reader["MostRecentPostUserID"] != DBNull.Value)
                    {
                        this.mostRecentPostUserID = Convert.ToInt32(reader["MostRecentPostUserID"]);
                    }

                }

            }


        }

        private void GetPost(int postId)
        {
            using (IDataReader reader = DBGroups.GroupTopicGetPost(postId))
            {
                if (reader.Read())
                {
                    this.postID = Convert.ToInt32(reader["PostID"]);
                    this.postUserID = Convert.ToInt32(reader["UserID"]);
                    this.postSubject = reader["Subject"].ToString();
                    this.postMessage = reader["Post"].ToString();

                    // this is to support dbs that don't have bit data type
                    string approved = reader["Approved"].ToString();
                    this.isApproved = (approved == "True" || approved == "1");
                }

            }

        }


        private bool CreateTopic()
        {
            int newID = -1;

            newID = DBGroups.GroupTopicCreate(
                this.groupID,
                this.postSubject,
                this.sortOrder,
                this.isLocked,
                this.postUserID,
                DateTime.UtcNow);


            this.topicID = newID;
            Group.IncrementTopicCount(this.groupID);

            return (newID > -1);

        }

        private bool CreatePost()
        {
            int newID = -1;
            bool approved = false;
            if (
                (ConfigurationManager.AppSettings["PostsApprovedByDefault"] != null)
                && (string.Equals(ConfigurationManager.AppSettings["PostsApprovedByDefault"], "true", StringComparison.InvariantCultureIgnoreCase))
                )
            {
                approved = true;
            }

            this.mostRecentPostDate = DateTime.UtcNow;
            newID = DBGroups.GroupPostCreate(
                this.topicID,
                this.postSubject,
                this.postMessage,
                approved,
                this.PostUserId,
                this.mostRecentPostDate);

            this.postID = newID;
            Group.IncrementPostCount(this.groupID, this.postUserID, this.mostRecentPostDate);
            SiteUser.IncrementTotalPosts(this.postUserID);
            //IndexHelper.IndexItem(this);

            bool result = (newID > -1);

            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;

        }

        private bool UpdatePost()
        {
            bool result = false;

            result = DBGroups.GroupPostUpdate(
                this.postID,
                this.postSubject,
                this.postMessage,
                this.sortOrder,
                this.isApproved);

            //IndexHelper.IndexItem(this);
            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;

        }



        private bool IncrementReplyStats()
        {
            return DBGroups.GroupTopicIncrementReplyStats(
                this.topicID,
                this.postUserID,
                this.mostRecentPostDate);

        }

        private void ResetTopicSequences()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PostID", typeof(int));

            using (IDataReader reader = DBGroups.GroupTopicGetPosts(this.topicID))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PostID"] = reader["PostID"];
                    dataTable.Rows.Add(row);
                }

            }

            int sequence = 1;
            foreach (DataRow row in dataTable.Rows)
            {
                DBGroups.GroupPostUpdateTopicSequence(
                    Convert.ToInt32(row["PostID"]),
                    sequence);
                sequence += 1;
            }

        }


        #endregion


        #region Public Methods

        public int Post()
        {
            bool newTopic = (this.topicID < 0);
            if (newTopic)
            {
                this.CreateTopic();
            }

            if (this.postID > -1)
            {
                this.UpdatePost();
            }
            else
            {
                this.CreatePost();
                if (!newTopic)
                {
                    this.IncrementReplyStats();
                }
            }

            if (this.subscribeUserToTopic)
            {
                DBGroups.GroupTopicAddSubscriber(this.topicID, this.postUserID);

            }


            return this.postID;

        }

        public bool DeletePost(int postId)
        {
            bool deleted = DBGroups.GroupPostDelete(postId);
            if (deleted)
            {
                Group.DecrementPostCount(this.groupID);
                if (this.totalReplies > 0)
                {
                    DBGroups.GroupTopicDecrementReplyStats(this.topicID);
                }
                Group group = new Group(this.groupID);

                this.moduleID = group.ModuleId;
                this.postID = postId;

                ContentChangedEventArgs e = new ContentChangedEventArgs();
                e.IsDeleted = true;
                OnContentChanged(e);

                int topicPostCount = GroupTopic.GetPostCount(this.topicID);
                if (topicPostCount == 0)
                {
                    GroupTopic.Delete(this.topicID);
                    Group.DecrementTopicCount(this.groupID);

                }

                ResetTopicSequences();
            }


            return deleted;
        }



        public bool UpdateTopicViewStats()
        {
            return DBGroups.GroupTopicUpdateViewStats(this.topicID);


        }

        public IDataReader GetPosts(int pageNumber)
        {
            return DBGroups.GroupTopicGetPosts(this.topicID, pageNumber);
        }

        public IDataReader GetPosts()
        {
            return DBGroups.GroupTopicGetPosts(this.topicID);
        }

        public DataTable GetPostIdList()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PostID", typeof(int));

            using (IDataReader reader = DBGroups.GroupTopicGetPosts(this.topicID))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PostID"] = reader["PostID"];
                    dataTable.Rows.Add(row);

                }
            }

            return dataTable;

        }

        public IDataReader GetPostsReverseSorted()
        {
            return DBGroups.GroupTopicGetPostsReverseSorted(this.topicID);
        }


        public DataSet GetTopicSubscribers()
        {
            return DBGroups.GroupTopicGetSubscribers(this.groupID, this.topicID, this.postUserID);
        }

        public bool UpdateTopic()
        {
            bool result = false;

            result = DBGroups.GroupTopicUpdate(
                this.topicID,
                this.groupID,
                this.subject,
                this.sortOrder,
                this.isLocked);

            if (this.groupID != this.origGroupID)
            {

                Group.DecrementTopicCount(this.origGroupID);
                Group.IncrementTopicCount(this.groupID);

                GroupTopicMovedArgs e = new GroupTopicMovedArgs();
                e.GroupId = groupID;
                e.OriginalGroupId = origGroupID;
                OnTopicMoved(e);


                Group.RecalculatePostStats(this.origGroupID);
                Group.RecalculatePostStats(this.groupID);
            }

            return result;
        }



        #endregion


        #region Static Methods


        public static bool Unsubscribe(int topicId, int userId)
        {
            return DBGroups.GroupTopicUNSubscribe(topicId, userId);
        }

        public static bool UnsubscribeAll(int userId)
        {
            return DBGroups.GroupTopicUnsubscribeAll(userId);
        }



        public static bool Delete(int topicId)
        {
            bool status = false;

            GroupTopic groupTopic = new GroupTopic(topicId);

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PostID", typeof(int));

            using (IDataReader reader = DBGroups.GroupTopicGetPosts(topicId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PostID"] = reader["PostID"];
                    dataTable.Rows.Add(row);
                }
            }

            foreach (DataRow row in dataTable.Rows)
            {
                groupTopic.DeletePost(Convert.ToInt32(row["PostID"]));
            }

            status = DBGroups.GroupTopicDelete(topicId);

            return status;
        }

        public static int GetPostCount(int topicId)
        {
            return DBGroups.GroupTopicGetPostCount(topicId);
        }


        public static DataTable GetPostsByPage(int siteId, int pageId)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PostID", typeof(int));
            dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("TopicID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("Subject", typeof(string));
            dataTable.Columns.Add("Post", typeof(string));
            dataTable.Columns.Add("ViewRoles", typeof(string));

            using (IDataReader reader = DBGroups.GroupTopicGetPostsByPage(siteId, pageId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PostID"] = reader["PostID"];
                    row["ItemID"] = reader["ItemID"];
                    row["ModuleID"] = reader["ModuleID"];
                    row["TopicID"] = reader["TopicID"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["Subject"] = reader["Subject"];
                    row["Post"] = reader["Post"];
                    row["ViewRoles"] = reader["ViewRoles"];

                    dataTable.Rows.Add(row);

                }
            }

            return dataTable;
        }


        public static IDataReader GetPageByUser(
            int userId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {

            return DBGroups.GetTopicPageByUser(
                userId,
                pageNumber,
                pageSize,
                out totalPages);

        }



        public static bool IsSubscribed(int topicId, int userId)
        {
            return DBGroups.GroupTopicSubscriptionExists(topicId, userId);
        }


        #endregion

        #region IIndexableContent

        public event ContentChangedEventHandler ContentChanged;

        protected void OnContentChanged(ContentChangedEventArgs e)
        {
            if (ContentChanged != null)
            {
                ContentChanged(this, e);
            }
        }


        #endregion

        public delegate void TopicMovedEventHandler(object sender, GroupTopicMovedArgs e);

        public event TopicMovedEventHandler TopicMoved;

        protected void OnTopicMoved(GroupTopicMovedArgs e)
        {
            if (TopicMoved != null)
            {
                TopicMoved(this, e);
            }
        }

    }
}
