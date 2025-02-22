﻿// Author:					Joe Audette
// Created:				    2010-01-14
// Last Modified:			2010-01-14
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Cynthia.Business;
using Resources;

namespace Cynthia.Web.ContactUI
{
    public partial class MessageListDialog : CDialogBasePage, ICallbackEventHandler
    {
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 4;
        private Guid moduleGuid = Guid.Empty;
        private int moduleId = -1;
        private int pageId = -1;
        protected string sCallBackFunctionInvocation;
        private string callbackArg = string.Empty;
        private Hashtable moduleSettings;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if (!UserCanEditModule(moduleId))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

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
            if (moduleGuid == Guid.Empty) return;

            using (IDataReader reader = ContactFormMessage.GetPageReader(
                moduleGuid,
                pageNumber,
                pageSize,
                out totalPages))
            {

                string pageUrl = SiteRoot + "/ContactForm/MessageListDialog.aspx"
                        + "?pageid=" + pageId.ToInvariantString()
                        + "&amp;mid=" + moduleId.ToInvariantString()
                        + "&amp;pagenumber={0}";

                pgrContactFormMessage.PageURLFormat = pageUrl;
                pgrContactFormMessage.ShowFirstLast = true;
                pgrContactFormMessage.CurrentIndex = pageNumber;
                pgrContactFormMessage.PageSize = pageSize;
                pgrContactFormMessage.PageCount = totalPages;
                grdContactFormMessage.PageIndex = pageNumber;
 
                pgrContactFormMessage.Visible = (totalPages > 1);

                grdContactFormMessage.DataSource = reader;
                grdContactFormMessage.DataBind();
            }

        }

        void grdContactFormMessage_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string arg = e.CommandArgument.ToString();
            if (arg.Length != 36) return;
            Guid messageGuid = new Guid(arg);
            switch (e.CommandName)
            {
                case "remove":
                    ContactFormMessage.Delete(messageGuid);
                    WebUtils.SetupRedirect(this, Request.RawUrl);
                    break;

                case "view":
                default:

                    ContactFormMessage message = new ContactFormMessage(messageGuid);
                    litMessage.Text = SecurityHelper.SanitizeHtml(message.Message);
                    //upMessage.Update();
                    break;

            }


        }

        public string GetViewOnClick(string arg)
        {
            return "GetMessage('" + arg + "','');return false;";

        }

        public string GetDeleteOnClick(string arg)
        {

            return "return confirm('" + ContactFormResources.ContactFormConfirmDeleteMessage + "');";

        }

        public void RaiseCallbackEvent(string eventArgument)
        {

            callbackArg = eventArgument;

        }

        public string GetCallbackResult()
        {
            if (callbackArg.Length != 36) return string.Empty;

            Guid messageGuid = new Guid(callbackArg);
            ContactFormMessage message = new ContactFormMessage(messageGuid);
            return SecurityHelper.SanitizeHtml(message.Message);
        }



        private void PopulateLabels()
        {
            Title = ContactFormResources.ContactFormViewMessagesLink;

            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;

            }

           
            lnkRefresh.NavigateUrl = Request.RawUrl;
            lnkRefresh.Text = ContactFormResources.ContactFormMessageListRefreshLink;

           
            grdContactFormMessage.Columns[0].HeaderText = ContactFormResources.ContactFormMessageListFromHeader;
            

        }

        private void SetupScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("\n<script type='text/javascript'>");
            script.Append("var myLayout; ");
            script.Append("$(document).ready(function () {");

            script.Append("myLayout = $('body').layout({  ");
            script.Append("applyDefaultStyles: true");
            script.Append(",west__paneSelector:'#" + pnlLeft.ClientID + "'");
            script.Append(",center__paneSelector:'#" + pnlCenter.ClientID + "'");
            script.Append(",west__size: 420 ");
            
            script.Append("});");
            script.Append("});");

            
            script.Append("\n</script>");


            Page.ClientScript.RegisterStartupScript(typeof(Page), "cmessages", script.ToString());

        }

        

        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);

            if ((moduleId > -1) && (this.CurrentPage.ContainsModule(moduleId)))
            {
                Module m = new Module(moduleId);
                moduleGuid = m.ModuleGuid;

                moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

                pageSize = WebUtils.ParseInt32FromHashtable(moduleSettings, "ContactFormMessageListPageSizeSetting", pageSize);

            }

            sCallBackFunctionInvocation
                = ClientScript.GetCallbackEventReference(this, "messageGuid", "ShowMessage", "context", "OnError", true);

           SetupScript();

        }


        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(this.Page_Load);
            grdContactFormMessage.RowCommand += new GridViewCommandEventHandler(grdContactFormMessage_RowCommand);

        }
       
    }
}
