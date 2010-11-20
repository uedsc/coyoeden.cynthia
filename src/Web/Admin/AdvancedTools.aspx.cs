﻿/// Author:					Joe Audette
/// Created:				2008-06-14
/// Last Modified:			2009-06-07
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using Cynthia.Web.Framework;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.AdminUI
{
    public partial class AdvnacedToolsPage : CBasePage
    {
        private bool isContentAdmin = false;
        private bool isAdmin = false;
        private bool isSiteEditor = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if ((!isAdmin)&&(!isContentAdmin)&&(!isSiteEditor))
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/AccessDenied.aspx");
                return;
            }

            
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {


        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings,Resource.AdvancedToolsHeading);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCurrentPage.Text = Resource.AdvancedToolsLink;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

            litAdminHeading.Text = Resource.AdvancedToolsHeading;

            liFeatureAdmin.Visible = (isAdmin && siteSettings.IsServerAdminSite);
            lnkFeatureAdmin.Text = Resource.AdminMenuFeatureModulesLink;
            lnkFeatureAdmin.NavigateUrl = SiteRoot + "/Admin/ModuleAdmin.aspx";

#if !MONO
            liWebPartAdmin.Visible = (isAdmin && siteSettings.IsServerAdminSite);
            lnkWebPartAdmin.Text = Resource.AdminMenuWebPartAdminLink;
            lnkWebPartAdmin.NavigateUrl = SiteRoot + "/Admin/WebPartAdmin.aspx";
#endif

            liRedirectManager.Visible = (isAdmin || isContentAdmin || isSiteEditor);
            lnkRedirectManager.Text = Resource.RedirectManagerLink;
            lnkRedirectManager.NavigateUrl = SiteRoot + "/Admin/RedirectManager.aspx";


            liUrlManager.Visible = (isAdmin || isContentAdmin || isSiteEditor);
            lnkUrlManager.Text = Resource.AdminMenuUrlManagerLink;
            lnkUrlManager.NavigateUrl = SiteRoot + "/Admin/UrlManager.aspx";

            liBannedIPs.Visible = isAdmin && siteSettings.IsServerAdminSite;
            lnkBannedIPs.Text = Resource.AdminMenuBannedIPAddressesLabel;
            lnkBannedIPs.NavigateUrl = SiteRoot + "/Admin/BannedIPAddresses.aspx";

            if (WebConfigSettings.EnableNewsletter)
            {
                liTaskQueue.Visible = isAdmin || WebUser.IsNewsletterAdmin;
                lnkTaskQueue.Text = Resource.TaskQueueMonitorHeading;
                lnkTaskQueue.NavigateUrl = SiteRoot + "/Admin/TaskQueueMonitor.aspx";

            }
            else
            {
                
                liTaskQueue.Visible = false;
            }

            liDevTools.Visible = isAdmin && siteSettings.IsServerAdminSite && WebConfigSettings.EnableDeveloperMenuInAdminMenu;
            lnkDevTools.Text = DevTools.DevToolsHeading;
            lnkDevTools.NavigateUrl = "~/DevAdmin/Default.aspx";

        }

        private void LoadSettings()
        {
            isAdmin = WebUser.IsAdmin;
            isContentAdmin = WebUser.IsContentAdmin;
            isSiteEditor = SiteUtils.UserIsSiteEditor();

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            SuppressMenuSelection();
            SuppressPageMenu();


        }

        #endregion
    }
}
