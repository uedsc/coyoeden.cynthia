/// Author:				        Joe Audette
/// Created:			        2004-09-18
///	Last Modified:              2010-02-02

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Net;
using Cynthia.Web.Controls;
using Cynthia.Web.Editor;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;

namespace Cynthia.Web.GroupUI
{
	
    public partial class GroupPostEdit : CBasePage
	{
        
		private int moduleId = -1;
        private int groupId = -1;
        private int topicId = -1;
        private int postId = -1;
        private int pageId = -1;
        private int pageNumber = 1;
        private SiteUser theUser;
        private Group group;
        private string virtualRoot;
        private Double timeOffset = 0;
        private bool isSubscribedToGroup = false;
        private bool isSubscribedToTopic = false;
        protected string allowedImageUrlRegexPattern = SecurityHelper.RegexRelativeImageUrlPatern;
        //Gravatar public enum RatingType { G, PG, R, X }
        protected Gravatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
        protected bool allowGravatars = false;
        private bool disableAvatars = true;
        private bool useSpamBlockingForAnonymous = true;
        protected Hashtable moduleSettings;
        private bool isSiteEditor = false;
        private bool includePostBodyInNotification = false;

        public double TimeOffset
        {
            get { return timeOffset; }
        }

        

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);

            SiteUtils.SetupEditor(edMessage);
            
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            //this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            SecurityHelper.DisableBrowserCache();
            isSiteEditor = SiteUtils.UserIsSiteEditor();

            if ((siteSettings != null) && (CurrentPage != null))
            {
                if ((SiteUtils.SslIsAvailable())
                    && ((siteSettings.UseSslOnAllPages) || (CurrentPage.RequireSsl))
                    )
                {
                    SiteUtils.ForceSsl();
                }
                else
                {
                    SiteUtils.ClearSsl();
                }

            }

            LoadSettings();

            if (groupId > -1)
            {
                group = new Group(groupId);
                moduleId = group.ModuleId;
                if (group.ItemId == -1)
                {
                    Response.Redirect(siteSettings.SiteRoot);
                }
                if (!group.AllowAnonymousPosts)
                {
                    if (!Request.IsAuthenticated)
                    {
                        SiteUtils.RedirectToLoginPage(this);
                        return;
                    }

                    pnlAntiSpam.Visible = false;
                    pnlEdit.Controls.Remove(pnlAntiSpam);
                }
                else
                {
                    if ((!useSpamBlockingForAnonymous) || (Request.IsAuthenticated))
                    {
                        pnlAntiSpam.Visible = false;
                        pnlEdit.Controls.Remove(pnlAntiSpam);
                    }

                }
            }

            
        }

        #endregion

        private void Page_Load(object sender, EventArgs e)
		{
            

            if (!CurrentPage.ContainsModule(moduleId))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
           
			
            SetupCss();
			PopulateLabels();

			if(!Page.IsPostBack)
			{
				PopulateControls();
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancel.NavigateUrl = Request.UrlReferrer.ToString();

                }
			}
			

		}

		private void PopulateControls()
		{
            GroupTopic topic = null;
		    if(topicId == -1)
			{
				this.btnDelete.Visible = false;
				this.rptMessages.Visible = false;
                Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName + " - " + GroupResources.NewTopicLabel);
			}
			else
			{
			    
			    if(postId > -1)
				{
					topic = new GroupTopic(topicId, postId);
					if (WebUser.IsAdmin
                        ||(isSiteEditor)
                        || (WebUser.IsInRoles(CurrentPage.EditRoles))
                        ||((this.theUser != null)&&(this.theUser.UserId == topic.PostUserId))
                        )
					{
						this.txtSubject.Text = topic.PostSubject;
						edMessage.Text = topic.PostMessage;
					}
				}
				else
				{  
					topic = new GroupTopic(topicId);
                    this.txtSubject.Text
                        = ResourceHelper.GetMessageTemplate(ResourceHelper.GetDefaultCulture(), "GroupPostReplyPrefix.config") 
                        + topic.Subject;
                }

                if ((group != null) && (topic != null))
                {
                    Title = SiteUtils.FormatPageTitle(siteSettings, group.Title + " - " + topic.Subject);
                }

                if (groupId == -1)
                {
                    groupId = topic.GroupId;
                }

                using(IDataReader reader = topic.GetPostsReverseSorted())
                {
                    this.rptMessages.DataSource = reader;
                    this.rptMessages.DataBind();
                    
                }
			}

            if (group != null)
            {
                litGroupPostLabel.Text = group.Title;
                litGroupDescription.Text = group.Description;
                
            }

            if (postId == -1)
            {
                string hookupInputScript = "<script type=\"text/javascript\">"
                     + "document.getElementById('" + this.txtSubject.ClientID + "').focus();</script>";

                if (!Page.ClientScript.IsStartupScriptRegistered("finitscript"))
                {
                    this.Page.ClientScript.RegisterStartupScript(
                        typeof(Page),
                        "finitscript", hookupInputScript);
                }
            }

            chkNotifyOnReply.Checked = isSubscribedToTopic;

            lnkPageCrumb.Text = CurrentPage.PageName;
            lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            lnkGroup.HRef = SiteRoot + "/Groups/GroupView.aspx?ItemID="
                + group.ItemId.ToInvariantString()
                + "&amp;pageid=" + pageId.ToInvariantString()
                + "&amp;mid=" + group.ModuleId.ToInvariantString();

            lnkGroup.InnerHtml = group.Title;
            if (topic != null) { lblTopicDescription.Text = Server.HtmlEncode(topic.Subject); }
		}


		private void btnDelete_Click(object sender, EventArgs e)
		{
			GroupTopic topic = new GroupTopic(topicId,postId);
            topic.ContentChanged += new ContentChangedEventHandler(topic_ContentChanged);
            
            

			if(topic.DeletePost(postId))
			{
                CurrentPage.UpdateLastModifiedTime();
                //if (Request.IsAuthenticated)
                //{
                //    SiteUser user = SiteUtils.GetCurrentSiteUser();
                //    if(user != null)
                //    SiteUser.DecrementTotalPosts(user.UserId);
                //}
                if (topic.PostUserId > -1)
                {
                    Group.UpdateUserStats(topic.PostUserId);
                }

                SiteUtils.QueueIndexing();
			}

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}

        

		private void btnCancel_Click(object sender, EventArgs e)
		{
            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}


		private void btnUpdate_Click(object sender, EventArgs e)
		{
            if (group == null) { group = new Group(groupId); }

            if (!group.AllowAnonymousPosts)
            {
                captcha.Enabled = false;
                pnlAntiSpam.Visible = false;
                pnlEdit.Controls.Remove(pnlAntiSpam);
            }

			Page.Validate();
			if(!Page.IsValid)
			{
                PopulateControls();
                return;
            }
            else
            {
                if ((useSpamBlockingForAnonymous) && (pnlAntiSpam.Visible))
                {
                    if (!captcha.IsValid)
                    {
                        PopulateControls();
                        return;
                    }
                }

				GroupTopic topic;
				bool userIsAllowedToUpdateThisPost = false;
				if(topicId == -1)
				{
					topic = new GroupTopic();
					topic.GroupId = groupId;
				}
				else
				{

					if(postId > -1)
					{
						topic = new GroupTopic(topicId,postId);
						if (WebUser.IsAdmin
                            || WebUser.IsInRoles(CurrentPage.EditRoles)
                            || (this.theUser.UserId == topic.PostUserId)
                           )
						{
							userIsAllowedToUpdateThisPost = true;
						}
					}
					else
					{
						topic = new GroupTopic(topicId);
					}
					groupId = topic.GroupId;

				}

                topic.ContentChanged += new ContentChangedEventHandler(topic_ContentChanged);
				topic.PostSubject = this.txtSubject.Text;
                topic.PostMessage = edMessage.Text;
			
				if(Request.IsAuthenticated)
				{
                    SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                    if (siteUser != null) 
					topic.PostUserId = siteUser.UserId;
                    if (chkSubscribeToGroup.Checked)
                    {
                        group.Subscribe(siteUser.UserId);
                    }
                    else
                    {
                        topic.SubscribeUserToTopic = this.chkNotifyOnReply.Checked;
                    }
				
				}
				else
				{
					topic.PostUserId = 0; //guest
				}

				string topicViewUrl = SiteRoot + "/Groups/Topic.aspx?topic=" 
					+ topic.TopicId.ToInvariantString()
                    + "&mid=" + moduleId.ToInvariantString()
                    + "&pageid=" + pageId.ToInvariantString()
                    + "&ItemID=" + groupId.ToInvariantString()
                    + "&pagenumber=" + this.pageNumber.ToInvariantString();

				if((topic.PostId == -1)||(userIsAllowedToUpdateThisPost))
				{
					topic.Post();
                    CurrentPage.UpdateLastModifiedTime();

                    topicViewUrl = SiteRoot + "/Groups/Topic.aspx?topic="
                        + topic.TopicId.ToInvariantString()
                        + "&mid=" + moduleId.ToInvariantString()
                        + "&pageid=" + pageId.ToInvariantString()
                        + "&ItemID=" + groupId.ToInvariantString()
                        + "&pagenumber=" + this.pageNumber.ToInvariantString()
                        + "#post" + topic.PostId.ToInvariantString();

					// Send notification to subscribers
                    // this doesn't make sense it only gets topic subscribers not group subscribers and yet I get the emails
					DataSet dsTopicSubscribers = topic.GetTopicSubscribers();

                    //ConfigurationManager.AppSettings["DefaultEmailFrom"]
                    GroupNotificationInfo notificationInfo = new GroupNotificationInfo();

                    CultureInfo defaultCulture = SiteUtils.GetDefaultCulture();

                    notificationInfo.SubjectTemplate
                        = ResourceHelper.GetMessageTemplate(defaultCulture, 
                        "GroupNotificationEmailSubject.config");

                    if (includePostBodyInNotification)
                    {
                        notificationInfo.BodyTemplate = Server.HtmlDecode(SecurityHelper.RemoveMarkup(topic.PostMessage)) + "\n\n\n";
                    }

                    notificationInfo.BodyTemplate
                        += ResourceHelper.GetMessageTemplate(defaultCulture, 
                        "GroupNotificationEmail.config");

                    notificationInfo.FromEmail = siteSettings.DefaultEmailFromAddress;
                    notificationInfo.SiteName = siteSettings.SiteName;
                    notificationInfo.ModuleName = new Module(moduleId).ModuleTitle;
                    notificationInfo.GroupName = new Group(groupId).Title;
                    notificationInfo.Subject = topic.PostSubject;
                    notificationInfo.Subscribers = dsTopicSubscribers;
                    notificationInfo.MessageLink = topicViewUrl;
                    notificationInfo.UnsubscribeGroupTopicLink = SiteRoot + "/Groups/UnsubscribeTopic.aspx?topicid=" + topic.TopicId;
                    notificationInfo.UnsubscribeGroupLink = SiteRoot + "/Groups/UnsubscribeGroup.aspx?mid=" + moduleId + "&itemid=" + topic.GroupId;
                    notificationInfo.SmtpSettings = SiteUtils.GetSmtpSettings();

                    ThreadPool.QueueUserWorkItem(new WaitCallback(Notification.SendGroupNotificationEmail), notificationInfo);
                    

                    String cacheDependencyKey = "Module-" + moduleId.ToInvariantString();
                    CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                    SiteUtils.QueueIndexing();
                   
				}

				//WebUtils.SetupRedirect(this, topicViewUrl);
                Response.Redirect(topicViewUrl);
			}
		}

        void topic_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["GroupTopicIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        protected int GetUserId(object obj)
        {
            if (obj == null) { return -1; }
            if (obj == DBNull.Value) { return -1; }
            return Convert.ToInt32(obj, CultureInfo.InvariantCulture);

        }

        public String GetAvatarUrl(String avatar)
        {
            if (allowGravatars) { return string.Empty; }
            if (disableAvatars) { return string.Empty; }

            if ((avatar == null) || (avatar == String.Empty))
            {
                avatar = "blank.gif";
            }
            return "<img  alt='' src='"
                + Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/useravatars/" + avatar) + "' />";
        }

        private void PopulateLabels()
        {
            reqSubject.ErrorMessage = GroupResources.GroupEditSubjectRequiredHelp;
            lblTopicDescription.Text = GroupResources.NewTopicLabel;

            btnUpdate.Text = GroupResources.GroupPostEditUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, GroupResources.GroupPostEditUpdateButtonAccessKey);

            UIHelper.DisableButtonAfterClick(
                btnUpdate,
                GroupResources.ButtonDisabledPleaseWait,
                Page.ClientScript.GetPostBackEventReference(this.btnUpdate, string.Empty)
                );

            lnkCancel.Text = GroupResources.GroupPostEditCancelButton;
            //btnCancel.Text = GroupResources.GroupPostEditCancelButton;
            //SiteUtils.SetButtonAccessKey(btnCancel, GroupResources.GroupEditCancelButtonAccessKey);

            btnDelete.Text = GroupResources.GroupPostEditDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, GroupResources.GroupEditDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, GroupResources.GroupDeletePostWarning);

            if (postId == -1)
            {
                this.btnDelete.Visible = false;
            }

            if (!Request.IsAuthenticated) pnlNotify.Visible = false;
            if (isSubscribedToGroup) pnlNotify.Visible = false;

            if (groupId == -1) pnlEdit.Visible = false;
        }


        private void SetupCss()
        { 
            // older skins have this
            StyleSheet stylesheet = (StyleSheet)Page.Master.FindControl("StyleSheet");
            if (stylesheet != null)
            {
                if (stylesheet.FindControl("groupcss") == null)
                {
                    Literal cssLink = new Literal();
                    cssLink.ID = "groupcss";
                    cssLink.Text = "\n<link href='"
                    + SiteUtils.GetSkinBaseUrl()
                    + "groupmodule.css' type='text/css' rel='stylesheet' media='screen' />";

                    stylesheet.Controls.Add(cssLink);
                }
            }
           
        }


	    private void LoadSettings()
        {
            virtualRoot = WebUtils.GetApplicationRoot();

            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            groupId = WebUtils.ParseInt32FromQueryString("groupid", -1);
            topicId = WebUtils.ParseInt32FromQueryString("topic", -1);
            postId = WebUtils.ParseInt32FromQueryString("postid", -1);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            timeOffset = SiteUtils.GetUserTimeOffset();

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    allowGravatars = true;
                    disableAvatars = true;
                    break;

                case "internal":
                    allowGravatars = false;
                    disableAvatars = false;
                    break;

                case "none":
                default:
                    allowGravatars = false;
                    disableAvatars = true;
                    break;

            }

            if (Request.IsAuthenticated)
            {
                theUser = SiteUtils.GetCurrentSiteUser();
                if (groupId > -1)
                {
                    isSubscribedToGroup = Group.IsSubscribed(groupId, theUser.UserId);
                }
                if (topicId > -1)
                {
                    isSubscribedToTopic = GroupTopic.IsSubscribed(topicId, theUser.UserId);
                }
            }

            if (WebUser.IsAdminOrContentAdmin)
            {
                edMessage.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            }
            else if ((Request.IsAuthenticated)&&(WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles)))
            {
                edMessage.WebEditor.ToolBar = ToolBar.GroupWithImages;
            }
            else
            {
                edMessage.WebEditor.ToolBar = ToolBar.Group;
            }

            edMessage.WebEditor.SetFocusOnStart = true;
            edMessage.WebEditor.Height = Unit.Parse("350px");

            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

            useSpamBlockingForAnonymous = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "GroupEnableAntiSpamSetting", useSpamBlockingForAnonymous);

            includePostBodyInNotification = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "IncludePostBodyInNotificationEmail", includePostBodyInNotification);

           

            if (useSpamBlockingForAnonymous)
            {
                captcha.ProviderName = siteSettings.CaptchaProvider;
                captcha.Captcha.ControlID = "captcha" + moduleId.ToString(CultureInfo.InvariantCulture);
                captcha.RecaptchaPrivateKey = siteSettings.RecaptchaPrivateKey;
                captcha.RecaptchaPublicKey = siteSettings.RecaptchaPublicKey;
            }

            


        }

	}
}
