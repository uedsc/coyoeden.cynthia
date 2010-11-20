/// Author:				        Dean Brettle
/// Created:			        2005-09-06
///	Last Modified:              2009-06-27
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;
using Cynthia.Modules;

namespace Cynthia.Web.GroupUI
{
	
    public partial class GroupTopicEdit : CBasePage
	{
		private int topicId = -1;
		private SiteUser siteUser;
		private GroupTopic groupTopic;
        private bool isSiteEditor = false;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            base.OnInit(e);

            SuppressPageMenu();
        }

        #endregion


        private void Page_Load(object sender, EventArgs e)
		{
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            if ((!WebUser.IsAdminOrContentAdmin) && (!isSiteEditor))
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }
              
            SecurityHelper.DisableBrowserCache();

            LoadParams();
            groupTopic = new GroupTopic(topicId);
            
            GroupTopicIndexBuilderProvider indexBuilder 
                = (GroupTopicIndexBuilderProvider)IndexBuilderManager.Providers["GroupTopicIndexBuilderProvider"];
            
            if (indexBuilder != null)
            {
                groupTopic.TopicMoved += new GroupTopic.TopicMovedEventHandler(indexBuilder.TopicMovedHandler);
            }

            siteUser = SiteUtils.GetCurrentSiteUser();
			
			PopulateLabels();

			if (!IsPostBack) 
			{
				PopulateControls();
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancel.NavigateUrl = hdnReturnUrl.Value;

                }
			}

		}

        

		private void PopulateLabels()
		{
            Title = SiteUtils.FormatPageTitle(siteSettings, GroupResources.GroupTopicEditLabel);
            btnUpdate.Text = GroupResources.GroupTopicUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, GroupResources.GroupEditUpdateButtonAccessKey);

            lnkCancel.Text = GroupResources.GroupTopicCancelButton;
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            
            btnDelete.Text = GroupResources.GroupTopicDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, GroupResources.GroupEditDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, GroupResources.GroupDeleteTopicWarning);

            
		}

		private void PopulateControls()
		{
			Group group = new Group(groupTopic.GroupId);
			this.txtSubject.Text = groupTopic.Subject;
            using (IDataReader reader = Group.GetGroups(group.ModuleId, siteUser.UserId))
            {
                this.ddGroupList.DataSource = reader;
                this.ddGroupList.DataBind();
            }
			this.ddGroupList.SelectedValue = groupTopic.GroupId.ToString();
			
		}

		


		private void btnUpdate_Click(object sender, EventArgs e)
		{
			groupTopic.GroupId = int.Parse(this.ddGroupList.SelectedValue);
			groupTopic.Subject = this.txtSubject.Text;
			groupTopic.UpdateTopic();

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}


		private void btnDelete_Click(object sender, EventArgs e)
		{
			GroupTopic.Delete(this.topicId);
            Group.UpdateUserStats(-1); // updates all users

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}

        private void LoadParams()
        {
            topicId = WebUtils.ParseInt32FromQueryString("topic", -1);

           
        }

		
	}
}
