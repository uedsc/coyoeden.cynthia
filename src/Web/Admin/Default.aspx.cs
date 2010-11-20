﻿///	Created:			    2009-04-16
///	Last Modified:		    2009-04-16
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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web;
using Cynthia.Web.Framework;

namespace Cynthia.Web.AdminUI
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // this is just a catch all redirect for /Admin/

            string siteRoot = SiteUtils.GetNavigationSiteRoot();
            string redirectUrl = string.Empty;

            if (Request.IsAuthenticated)
            {
                redirectUrl = "/Admin/AdminMenu.aspx";
            }
            else
            {
                redirectUrl = SiteUtils.GetLoginRelativeUrl() + "?returnurl=" + Server.UrlEncode(siteRoot + "/Admin/AdminMenu.aspx");
            }

            WebUtils.SetupRedirect(this, redirectUrl);

        }
    }
}
