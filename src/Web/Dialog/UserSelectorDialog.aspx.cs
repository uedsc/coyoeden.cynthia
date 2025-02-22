﻿// Author:					Joe Audette
// Created:				    2009-04-25
// Last Modified:			2009-05-02
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.UI
{
    public partial class UserSelectorDialog : CDialogBasePage
    {
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 15;
        private string userNameBeginsWith = string.Empty;
        private SiteSettings siteSettings = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            if (!IsPostBack)
            {
                PopulateControls();
            }
        }

        private void PopulateControls()
        {
            if (siteSettings == null) { return; }

            if (WebUser.IsInRoles(siteSettings.RolesThatCanLookupUsers))
            {
                pnlLookup.Visible = true;
                pnlNotAllowed.Visible = false;
                BindListForAdmin();
            }
            else
            {
                pnlLookup.Visible = false;
                pnlNotAllowed.Visible = true;
            }

        }

        private void BindListForAdmin()
        {
         

            List<SiteUser> siteUserPage = SiteUser.GetUserSearchPage(
                CurrentSite.SiteId,
                pageNumber,
                pageSize,
                userNameBeginsWith,
                out totalPages);

            if (pageNumber > totalPages)
            {
                pageNumber = 1;

            }

            if (userNameBeginsWith.Length > 1)
            {
                txtSearchUser.Text = Server.HtmlEncode(userNameBeginsWith);
            }

            Literal topPageLinks = new Literal();
            string pageUrl = SiteRoot
                + "/Dialog/UserSelectorDialog.aspx?"
                + "pagenumber=";

            String alphaChars;

            if (ConfigurationManager.AppSettings["GetAlphaPagerCharsFromResourceFile"].ToLower() == "true")
            {
                alphaChars = Resource.AlphaPagerChars;
            }
            else
            {
                alphaChars = ConfigurationManager.AppSettings["AlphaPagerChars"];
            }

            topPageLinks.Text = UIHelper.GetAlphaPagerLinks(
                pageUrl,
                pageNumber,
                alphaChars,
                userNameBeginsWith);

            this.spnTopPager.Controls.Add(topPageLinks);

            pageUrl = SiteRoot
                + "/Dialog/UserSelectorDialog.aspx?"
                + "letter=" + userNameBeginsWith
                + "&amp;pagenumber={0}";

            pgrMembers.PageURLFormat = pageUrl;
            pgrMembers.ShowFirstLast = true;
            pgrMembers.CurrentIndex = pageNumber;
            pgrMembers.PageSize = pageSize;
            pgrMembers.PageCount = totalPages;
            pgrMembers.Visible = (totalPages > 1);


            this.rptUsers.DataSource = siteUserPage;
#if MONO
            this.rptUsers.DataBind();
#else
            this.DataBind();
#endif

        }

        void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Button btnSelect = (Button)e.Item.FindControl("btnSelect");
            if (btnSelect == null) { return; }

            string[] pair = btnSelect.CommandArgument.Split('|');
            if (pair.Length != 2) { return; }
            if (pair[0].Length != 36) { return; }


            btnSelect.Attributes.Add("onclick", GetClientScriptForButton(pair[0], pair[1]));
            
        }

        private string GetClientScriptForButton(string userGuid, string userId)
        {
            return "top.window.SelectUser('" + userGuid
                + "'," + userId + ");  return false;";
        }

        void btnSearchUser_Click(object sender, EventArgs e)
        {
            string pageUrl = SiteRoot
                 + "/Dialog/UserSelectorDialog.aspx?"
                 + "letter=" + Server.HtmlEncode(txtSearchUser.Text)
                 + "&pagenumber=";

            WebUtils.SetupRedirect(this, pageUrl);

        }

        


        private void PopulateLabels()
        {
           
            lnkAllUsers.Text = Resource.MemberListAllUsersLink;
            lnkAllUsers.NavigateUrl = SiteRoot + "/Dialog/UserSelectorDialog.aspx";
            btnSearchUser.Text = Resource.MemberListSearchButton;
        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
            if (Request.Params["letter"] != null)
            {
                userNameBeginsWith = Request.Params["letter"];
            }

            
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            btnSearchUser.Click += new EventHandler(btnSearchUser_Click);
            rptUsers.ItemDataBound += new RepeaterItemEventHandler(rptUsers_ItemDataBound);
        }

        
    }
}
