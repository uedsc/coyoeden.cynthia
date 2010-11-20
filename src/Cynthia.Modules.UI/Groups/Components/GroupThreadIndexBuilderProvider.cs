/// Author:					    Joe Audette
/// Created:				    2007-08-30
/// Last Modified:			    2009-07-22
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Threading;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web;

namespace Cynthia.Modules
{
    
    public class GroupTopicIndexBuilderProvider : IndexBuilderProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(GroupTopicIndexBuilderProvider));

        public GroupTopicIndexBuilderProvider()
        { }

        public override void RebuildIndex(
            PageSettings pageSettings,
            string indexPath)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            if ((pageSettings == null) || (indexPath == null))
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("pageSettings object or index path passed to GroupTopicIndexBuilderProvider.RebuildIndex was null");
                }
                return;

            }

            //don't index pending/unpublished pages
            if (pageSettings.IsPending) { return; }

            log.Info("GroupTopicIndexBuilderProvider indexing page - "
                + pageSettings.PageName);

            try
            {
                List<PageModule> pageModules
                        = PageModule.GetPageModulesByPage(pageSettings.PageId);

                Guid groupFeatureGuid = new Guid("E75BAF8C-7079-4d10-A122-1AA3624E26F2");
                ModuleDefinition groupFeature = new ModuleDefinition(groupFeatureGuid);

                DataTable dataTable = GroupTopic.GetPostsByPage(
                    pageSettings.SiteId,
                    pageSettings.PageId);

                foreach (DataRow row in dataTable.Rows)
                {
                    IndexItem indexItem = new IndexItem();
                    indexItem.SiteId = pageSettings.SiteId;
                    indexItem.PageId = pageSettings.PageId;
                    indexItem.PageName = pageSettings.PageName;
                    indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                    indexItem.ModuleViewRoles = row["ViewRoles"].ToString();
                    indexItem.FeatureId = groupFeatureGuid.ToString();
                    indexItem.FeatureName = groupFeature.FeatureName;
                    indexItem.FeatureResourceFile = groupFeature.ResourceFile;

                    indexItem.ItemId = Convert.ToInt32(row["ItemID"]);
                    indexItem.ModuleId = Convert.ToInt32(row["ModuleID"]);
                    indexItem.ModuleTitle = row["ModuleTitle"].ToString();
                    indexItem.Title = row["Subject"].ToString();
                    indexItem.Content = row["Post"].ToString();
                    indexItem.ViewPage = "Groups/Topic.aspx";
                    indexItem.QueryStringAddendum = "&topic="
                        + row["TopicID"].ToString()
                        + "&postid=" + row["PostID"].ToString();

                    // lookup publish dates
                    foreach (PageModule pageModule in pageModules)
                    {
                        if (indexItem.ModuleId == pageModule.ModuleId)
                        {
                            indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                            indexItem.PublishEndDate = pageModule.PublishEndDate;
                        }
                    }

                    IndexHelper.RebuildIndex(indexItem, indexPath);

                    if (log.IsDebugEnabled) log.Debug("Indexed " + indexItem.Title);

                }

            }
            catch (System.Data.Common.DbException ex)
            {
                log.Error(ex);
            }

        }

        public override void ContentChangedHandler(
            object sender,
            ContentChangedEventArgs e)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (sender == null) return;
            if (!(sender is GroupTopic)) return;


            GroupTopic groupTopic = (GroupTopic)sender;
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            groupTopic.SiteId = siteSettings.SiteId;
            groupTopic.SearchIndexPath = IndexHelper.GetSearchIndexPath(siteSettings.SiteId);

            if (e.IsDeleted)
            {
                if (ThreadPool.QueueUserWorkItem(new WaitCallback(RemoveGroupIndexItem), groupTopic))
                {
                    if (log.IsDebugEnabled) log.Debug("GroupTopicIndexBuilderProvider.RemoveGroupIndexItem queued");
                }
                else
                {
                    if (log.IsErrorEnabled) log.Error("Failed to queue a topic for GroupTopicIndexBuilderProvider.RemoveGroupIndexItem");
                }

                //RemoveGroupIndexItem(groupTopic);
            }
            else
            {
                if (ThreadPool.QueueUserWorkItem(new WaitCallback(IndexItem), groupTopic))
                {
                    if (log.IsDebugEnabled) log.Debug("GroupTopicIndexBuilderProvider.IndexItem queued");
                }
                else
                {
                    if (log.IsErrorEnabled) log.Error("Failed to queue a topic for GroupTopicIndexBuilderProvider.IndexItem");
                }

                //IndexItem(groupTopic);
            }

        }

        private static void IndexItem(object oGroupTopic)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (oGroupTopic == null) return;
            if (!(oGroupTopic is GroupTopic)) return;

            GroupTopic groupTopic = oGroupTopic as GroupTopic;
            IndexItem(groupTopic);

        }

        private static void IndexItem(GroupTopic groupTopic)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            //SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            //if (siteSettings == null)
            //{
            //    if (log.IsErrorEnabled)
            //    {
            //        log.Error("siteSettings object retrieved in GroupTopicIndexBuilderProvider.IndexItem was null");
            //    }
            //    return;
            //}

            if (groupTopic == null)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("groupTopic object passed in GroupTopicIndexBuilderProvider.IndexItem was null");
                }
                return;

            }

            Group group = new Group(groupTopic.GroupId);
            Module module = new Module(group.ModuleId);
            Guid groupFeatureGuid = new Guid("E75BAF8C-7079-4d10-A122-1AA3624E26F2");
            ModuleDefinition groupFeature = new ModuleDefinition(groupFeatureGuid);

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(group.ModuleId);

            // must update index for all pages containing
            // this module
            foreach (PageModule pageModule in pageModules)
            {
                PageSettings pageSettings
                    = new PageSettings(
                    groupTopic.SiteId,
                    pageModule.PageId);

                //don't index pending/unpublished pages
                if (pageSettings.IsPending) { continue; }

                IndexItem indexItem = new IndexItem();
                if (groupTopic.SearchIndexPath.Length > 0)
                {
                    indexItem.IndexPath = groupTopic.SearchIndexPath;
                }
                indexItem.SiteId = groupTopic.SiteId;
                indexItem.PageId = pageModule.PageId;
                indexItem.PageName = pageSettings.PageName;
                // permissions are kept in sync in search index
                // so that results are filtered by role correctly
                indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                indexItem.ModuleViewRoles = module.ViewRoles;
                indexItem.ItemId = groupTopic.GroupId;
                indexItem.ModuleId = group.ModuleId;
                indexItem.ModuleTitle = module.ModuleTitle;
                indexItem.ViewPage = "Groups/Topic.aspx";
                indexItem.QueryStringAddendum = "&topic="
                    + groupTopic.TopicId.ToString()
                    + "&postid=" + groupTopic.PostId.ToString();
                indexItem.FeatureId = groupFeatureGuid.ToString();
                indexItem.FeatureName = groupFeature.FeatureName;
                indexItem.FeatureResourceFile = groupFeature.ResourceFile;
                indexItem.Title = groupTopic.Subject;
                indexItem.Content = groupTopic.PostMessage;
                indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                indexItem.PublishEndDate = pageModule.PublishEndDate;

                IndexHelper.RebuildIndex(indexItem);

                if (log.IsDebugEnabled) log.Debug("Indexed "
                    + groupTopic.Subject);

            }

        }

        public void TopicMovedHandler(object sender, GroupTopicMovedArgs e)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            GroupTopic groupTopic = (GroupTopic)sender;
            DataTable postIDList = groupTopic.GetPostIdList();

            Group origGroup = new Group(e.OriginalGroupId);
            foreach (DataRow row in postIDList.Rows)
            {
                int postID = Convert.ToInt32(row["PostID"]);
                GroupTopic post = new GroupTopic(groupTopic.TopicId, postID);

                RemoveGroupIndexItem(
                    origGroup.ModuleId,
                    e.OriginalGroupId,
                    groupTopic.TopicId,
                    postID);

                IndexItem(post);

            }


        }

        public static void RemoveGroupIndexItem(object oGroupTopic)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            if (!(oGroupTopic is GroupTopic)) return;

            GroupTopic groupTopic = oGroupTopic as GroupTopic;

            //SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            //if (siteSettings == null)
            //{
            //    if (log.IsErrorEnabled)
            //    {
            //        log.Error("siteSettings object retrieved in GroupTopicIndexBuilderProvider.RemoveGroupIndexItem was null");
            //    }
            //    return;
            //}

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(groupTopic.ModuleId);

            // must update index for all pages containing
            // this module
            foreach (PageModule pageModule in pageModules)
            {
                IndexItem indexItem = new IndexItem();
                // note we are just assigning the properties 
                // needed to derive the key so it can be found and
                // deleted from the index
                indexItem.SiteId = groupTopic.SiteId;
                indexItem.PageId = pageModule.PageId;
                indexItem.ModuleId = groupTopic.ModuleId;
                indexItem.ItemId = groupTopic.GroupId;
                indexItem.QueryStringAddendum = "&topic="
                    + groupTopic.TopicId.ToString(CultureInfo.InvariantCulture)
                    + "&postid=" + groupTopic.PostId.ToString(CultureInfo.InvariantCulture);

                IndexHelper.RemoveIndex(indexItem);
            }

            if (log.IsDebugEnabled) log.Debug("Removed Index ");

        }

        public static void RemoveGroupIndexItem(
            int moduleId,
            int itemId,
            int topicId,
            int postId)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (siteSettings == null)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("siteSettings object retrieved in GroupTopicIndexBuilderProvider.RemoveGroupIndexItem was null");
                }
                return;
            }

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(moduleId);

            // must update index for all pages containing
            // this module
            foreach (PageModule pageModule in pageModules)
            {
                IndexItem indexItem = new IndexItem();
                // note we are just assigning the properties 
                // needed to derive the key so it can be found and
                // deleted from the index
                indexItem.SiteId = siteSettings.SiteId;
                indexItem.PageId = pageModule.PageId;
                indexItem.ModuleId = moduleId;
                indexItem.ItemId = itemId;
                indexItem.QueryStringAddendum = "&topic="
                    + topicId.ToString()
                    + "&postid=" + postId.ToString();

                IndexHelper.RemoveIndex(indexItem);
            }

            if (log.IsDebugEnabled) log.Debug("Removed Index ");

        }


    }
}
