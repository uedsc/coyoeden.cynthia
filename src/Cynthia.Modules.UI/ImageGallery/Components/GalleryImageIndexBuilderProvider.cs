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
using System.Data;
using System.Globalization;
using System.Text;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web;

namespace Cynthia.Modules
{
    public class GalleryImageIndexBuilderProvider : IndexBuilderProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(GalleryImageIndexBuilderProvider));

        public GalleryImageIndexBuilderProvider()
        { }

        public override void RebuildIndex(
            PageSettings pageSettings,
            string indexPath)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (pageSettings == null) return;

            //don't index pending/unpublished pages
            if (pageSettings.IsPending) { return; }

            log.Info("GalleryImageIndexBuilderProvider indexing page - " 
                + pageSettings.PageName);

            try
            {
                Guid galleryFeatureGuid = new Guid("165517FD-461B-44d0-8897-3E5FA1A62097");
                ModuleDefinition galleryFeature = new ModuleDefinition(galleryFeatureGuid);

                List<PageModule> pageModules
                        = PageModule.GetPageModulesByPage(pageSettings.PageId);

                DataTable dataTable = GalleryImage.GetImagesByPage(pageSettings.SiteId, pageSettings.PageId);

                foreach (DataRow row in dataTable.Rows)
                {
                    IndexItem indexItem = new IndexItem();
                    indexItem.SiteId = pageSettings.SiteId;
                    indexItem.PageId = pageSettings.PageId;
                    indexItem.PageName = pageSettings.PageName;
                    indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                    indexItem.ModuleViewRoles = row["ViewRoles"].ToString();

                    if (pageSettings.UseUrl)
                    {
                        indexItem.ViewPage = pageSettings.Url.Replace("~/", string.Empty);
                        indexItem.UseQueryStringParams = false;
                    }
                    indexItem.FeatureId = galleryFeatureGuid.ToString();
                    indexItem.FeatureName = galleryFeature.FeatureName;
                    indexItem.FeatureResourceFile = galleryFeature.ResourceFile;

                    // TODO: it would be good to check the module settings and if not
                    // in compact mode use the GalleryBrowse.aspx page
                    //indexItem.ViewPage = "GalleryBrowse.aspx";

                    indexItem.ItemId = Convert.ToInt32(row["ItemID"]);
                    indexItem.ModuleId = Convert.ToInt32(row["ModuleID"]);
                    indexItem.ModuleTitle = row["ModuleTitle"].ToString();
                    indexItem.Title = row["Caption"].ToString();
                    indexItem.Content = row["Description"].ToString();

                    indexItem.QueryStringAddendum = "&ItemID"
                        + row["ModuleID"].ToString()
                        + "=" + row["ItemID"].ToString();

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
            GalleryImage galleryImage = (GalleryImage)sender;
            if (e.IsDeleted)
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                if (siteSettings == null) return;
                // get list of pages where this module is published
                List<PageModule> pageModules
                    = PageModule.GetPageModulesByModule(galleryImage.ModuleId);

                foreach (PageModule pageModule in pageModules)
                {
                    RemoveGalleryImageIndexItem(
                        siteSettings.SiteId,
                        pageModule.PageId,
                        galleryImage.ModuleId,
                        galleryImage.ItemId);
                }

            }
            else
            {
                IndexItem(galleryImage);
            }

        }

        private static void IndexItem(GalleryImage galleryImage)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if ((siteSettings == null)
                    || (galleryImage == null))
            {
                return;
            }

            Guid galleryFeatureGuid = new Guid("165517FD-461B-44d0-8897-3E5FA1A62097");
            ModuleDefinition galleryFeature = new ModuleDefinition(galleryFeatureGuid);
            Module module = new Module(galleryImage.ModuleId);

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(galleryImage.ModuleId);

            foreach (PageModule pageModule in pageModules)
            {
                PageSettings pageSettings
                = new PageSettings(
                siteSettings.SiteId,
                pageModule.PageId);

                //don't index pending/unpublished pages
                if (pageSettings.IsPending) { continue; }

                IndexItem indexItem = new IndexItem();
                indexItem.SiteId = siteSettings.SiteId;
                indexItem.PageId = pageSettings.PageId;
                indexItem.PageName = pageSettings.PageName;
                indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                indexItem.ModuleViewRoles = module.ViewRoles;
                indexItem.FeatureId = galleryFeatureGuid.ToString();
                indexItem.FeatureName = galleryFeature.FeatureName;
                indexItem.FeatureResourceFile = galleryFeature.ResourceFile;

                indexItem.ItemId = galleryImage.ItemId;
                indexItem.ModuleId = galleryImage.ModuleId;

                indexItem.QueryStringAddendum = "&ItemID" 
                    + galleryImage.ModuleId.ToString()
                    + "=" + galleryImage.ItemId.ToString();

                indexItem.ModuleTitle = module.ModuleTitle;
                indexItem.Title = galleryImage.Caption;
                indexItem.Content = galleryImage.Description;
                indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                indexItem.PublishEndDate = pageModule.PublishEndDate;

                IndexHelper.RebuildIndex(indexItem);

                if (log.IsDebugEnabled) log.Debug("Indexed " + galleryImage.Caption);
            }

        }


        private static void RemoveGalleryImageIndexItem(
            int siteId,
            int pageId,
            int moduleId, 
            int itemId)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            IndexItem indexItem = new IndexItem();
            indexItem.SiteId = siteId;
            indexItem.PageId = pageId;
            indexItem.ModuleId = moduleId;
            indexItem.ItemId = itemId;
            indexItem.QueryStringAddendum = "&ItemID" + moduleId.ToString()
                + "=" + itemId.ToString();

            IndexHelper.RemoveIndex(indexItem);

            if (log.IsDebugEnabled) log.Debug("Removed Index ");
            

        }

    }
}
