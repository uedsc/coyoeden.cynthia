﻿// Author:					Joe Audette
// Created:					2009-10-27
// Last Modified:			2009-10-27
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
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cynthia.Web;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Resources;



namespace Cynthia.Web.ELetterUI
{

    public partial class ThankYouPage : CBasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {


        }


        private void PopulateLabels()
        {
            litThankYou.Text = Resource.NewsletterThankYouMessage;
        }

        private void LoadSettings()
        {


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
