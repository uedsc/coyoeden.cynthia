﻿/// Author:					Joe Audette
/// Created:				2007-12-30
/// Last Modified:			2009-12-16
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
using System.Threading;
using System.Web.UI;
using Cynthia.Web.Framework;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.AdminUI
{
    public partial class TaskQueueMonitorPage : CBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TaskQueueMonitorPage));

        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 20;
        private Double timeOffset = 0;
        private bool isSiteEditor = false;

        /// <summary>
        /// time offset for the current user
        /// </summary>
        protected Double TimeOffset
        {
            get { return timeOffset; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            if ((!isSiteEditor) && (!WebUser.IsAdminOrNewsletterAdmin))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (Page.IsPostBack) return;
            BindGrid();

        }

        private void BindGrid()
        {

            List<TaskQueue> TaskQueueList;

            if (siteSettings.IsServerAdminSite && WebUser.IsAdmin)
            {
                TaskQueueList 
                    = TaskQueue.GetPageUnfinished(
                        pageNumber,
                        pageSize,
                        out totalPages);
            }
            else
            {
                TaskQueueList
                    = TaskQueue.GetPageUnfinishedBySite(
                        siteSettings.SiteGuid,
                        pageNumber,
                        pageSize,
                        out totalPages);

            }


            if (this.totalPages > 1)
            {
              
                string pageUrl = SiteRoot 
                    + "/Admin/TaskQueueMonitor.aspx?pagenumber={0}";

                pgrTaskQueue.PageURLFormat = pageUrl;
                pgrTaskQueue.ShowFirstLast = true;
                pgrTaskQueue.CurrentIndex = pageNumber;
                pgrTaskQueue.PageSize = pageSize;
                pgrTaskQueue.PageCount = totalPages;

            }
            else
            {
                pgrTaskQueue.Visible = false;
            }

            grdTaskQueue.DataSource = TaskQueueList;
            grdTaskQueue.PageIndex = pageNumber;
            grdTaskQueue.PageSize = pageSize;
            grdTaskQueue.DataBind();

            if (TaskQueueList.Count == 0)
            {
                lblStatus.Text = Resource.TaskQueueNoTasksRunningMessage;
            }

        }

        void btnTest_Click(object sender, EventArgs e)
        {
            TopicSleepTask testTask = new TopicSleepTask();
            testTask.SiteGuid = siteSettings.SiteGuid;
            testTask.TaskName = "Test task that just sleeps a bit";
            testTask.QueueTask();

            WebUtils.SetupRedirect(this, Request.RawUrl);


        }

        void btnStartTasks_Click(object sender, EventArgs e)
        {
            WebTaskManager.StartOrResumeTasks();

            WebUtils.SetupRedirect(this, Request.RawUrl);

           
        }

        protected string GetPercentComplete(object data)
        {
            Double d = Convert.ToDouble(data) * 100;

            return d.ToString("#0", CultureInfo.InvariantCulture) + "&#37;"; //%

        }


        private void PopulateLabels()
        {
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
            lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

            lnkThisPage.Text = Resource.TaskQueueMonitorHeading;
            lnkThisPage.ToolTip = Resource.TaskQueueMonitorHeading;
            lnkThisPage.NavigateUrl = SiteRoot + "/Admin/TaskQueueMonitor.aspx";

            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.TaskQueueMonitorHeading);
            litHeading.Text = Resource.TaskQueueMonitorHeading;

            lnkRefresh.Text = Resource.TaskQueueRefreshLink;
            lnkRefresh.ToolTip = Resource.TaskQueueRefreshLink;
            lnkRefresh.NavigateUrl = Request.RawUrl;

            lnkTaskQueueHistory.Text = Resource.TaskQueueHistoryHeading;
            lnkTaskQueueHistory.ToolTip = Resource.TaskQueueHistoryHeading;
            lnkTaskQueueHistory.NavigateUrl = SiteRoot + "/Admin/TaskQueueHistory.aspx";

            btnTest.Visible = WebConfigSettings.EnableTaskQueueTestLinks;
            btnTest.Text = "Create TopicSleepTask";

            btnStartTasks.Visible = WebConfigSettings.EnableTaskQueueTestLinks;

            btnStartTasks.Text = "Start Tasks";

            grdTaskQueue.Columns[0].HeaderText = Resource.TaskQueueGridTaksNameHeader;
            grdTaskQueue.Columns[1].HeaderText = Resource.TaskQueueGridQueuedHeader;
            grdTaskQueue.Columns[2].HeaderText = Resource.TaskQueueGridStartedHeader;
            grdTaskQueue.Columns[3].HeaderText = Resource.TaskQueueGridLastUpdateHeader;
            grdTaskQueue.Columns[4].HeaderText = Resource.TaskQueueGridCompleteProgressHeader;
            grdTaskQueue.Columns[5].HeaderText = Resource.TaskQueueGridStatusHeader;
            
            /*
             When GetAvailableTopics returns, the variable specified by workerTopics contains the number 
             * of additional worker topics that can be started, and the variable specified by completionPortTopics 
             * contains the number of additional asynchronous I/O topics that can be started.

                If there are no available topics, additional topic pool requests remain queued until topic pool 
             * topics become available.
             */
            int workerThreads = 0;
            int completionPortThreads = 0;
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);

            litAvailableTopics.Text = workerThreads.ToString(CultureInfo.InvariantCulture);

            

        }

        private void LoadSettings()
        {
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            timeOffset = SiteUtils.GetUserTimeOffset();

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnTest.Click += new EventHandler(btnTest_Click);
            this.btnStartTasks.Click += new EventHandler(btnStartTasks_Click);

            SuppressMenuSelection();
            SuppressPageMenu();

        }

        

        

        #endregion
    }
}
