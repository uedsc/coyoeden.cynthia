// Author:					Joe Audette
// Created:					2009-10-23
// Last Modified:			2009-10-23
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using Cynthia.Web.Framework;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Resources;



namespace Cynthia.Web.ELetterUI
{

    public partial class LetterViewPage : CBasePage
    {
        
        private LetterInfo letterInfo = null;
        private Letter letter = null;
        protected Guid letterGuid = Guid.Empty;
        protected string unavailableReason = Resource.NewsletterUnavailable;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if (letter != null)
            {
                RenderLetter();
            }
            else
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, Resource.NewsletterUnavailable);
                lblMessage.Text = unavailableReason;
                if (!Request.IsAuthenticated)
                {
                    litLoginMessage.Text = Resource.NewsletterMayRequireSignIn;
                }
            }
        }

        private void RenderLetter()
        {
            Response.Buffer = false;
            Response.BufferOutput = false;
            Response.Write(letter.HtmlBody);
            Response.End();
        }

        private void LoadSettings()
        {
            letterGuid = WebUtils.ParseGuidFromQueryString("letter", Guid.Empty);
            
            if (letterGuid != Guid.Empty)
            {
                letter = new Letter(letterGuid);
                if (letter.LetterGuid != Guid.Empty)
                {
                    letterInfo = new LetterInfo(letter.LetterInfoGuid);
                    if (letterInfo.SiteGuid != siteSettings.SiteGuid) { letter = null; }
                    if (!WebUser.IsInRoles(letterInfo.AvailableToRoles)) { letter = null; }
                    if ((!letterInfo.AllowArchiveView)&&(!WebUser.IsNewsletterAdmin))
                    {
                        letter = null;
                        unavailableReason = Resource.NewsletterArchivesNotAllowed;

                    }

                }
                else
                {
                    letter = null;
                }

            }

        }

       

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            SuppressMenuSelection();

        }

        #endregion
    }
}
