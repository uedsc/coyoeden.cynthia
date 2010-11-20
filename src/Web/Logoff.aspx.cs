/// Last Modified:		        2007-01-19 by aleyush
/// 2007-08-25
/// 2008-11-17 by Joe Audette
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Cynthia.Web;

namespace Cynthia.Web.UI.Pages 
{

	
    public partial class Logoff : Page 
	{
        const string WindowsLiveSecurityAlgorithm = "wsignin1.0";
        //private bool forceDelAuthNonProvisioned = true;

        protected void Page_Load(object sender, EventArgs e) 
		{
            DoLogout();
        }

        private void DoLogout()
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            string winliveCookieName = "winliveid"
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            string roleCookieName = SiteUtils.GetRoleCookieName(siteSettings);

            HttpCookie roleCookie = new HttpCookie(roleCookieName, string.Empty);
            roleCookie.Expires = DateTime.Now.AddMinutes(1);
            roleCookie.Path = "/";
            Response.Cookies.Add(roleCookie);

            HttpCookie displayNameCookie = new HttpCookie("DisplayName", string.Empty);
            displayNameCookie.Expires = DateTime.Now.AddMinutes(1);
            displayNameCookie.Path = "/";
            Response.Cookies.Add(displayNameCookie);

           
            bool useFolderForSiteDetection = ConfigHelper.GetBoolProperty("UseFoldersInsteadOfHostnamesForMultipleSites", false);
            if ((useFolderForSiteDetection)&&(!WebConfigSettings.UseRelatedSiteMode))
            {
                string cookieName = "siteguid" + siteSettings.SiteGuid.ToString();

                HttpCookie siteCookie = new HttpCookie(cookieName, string.Empty);
                siteCookie.Expires = DateTime.Now.AddMinutes(1);
                siteCookie.Path = "/";
                Response.Cookies.Add(siteCookie);

            }
            else
            {
                FormsAuthentication.SignOut();
            }

            string winLiveToken = CookieHelper.GetCookieValue(winliveCookieName);
            WindowsLiveLogin.User liveUser = null;
            if (winLiveToken.Length > 0)
            {
                WindowsLiveLogin windowsLive = WindowsLiveHelper.GetWindowsLiveLogin();

                try
                {
                    liveUser = windowsLive.ProcessToken(winLiveToken);
                    if (liveUser != null)
                    {
                        Response.Redirect(windowsLive.GetLogoutUrl());
                        Response.End();

                    }
                }
                catch (InvalidOperationException)
                {
                }

            }

            try
            {
                if (Session != null) { Session.Abandon(); }
            }
            catch (HttpException) { }

            WebUtils.SetupRedirect(this, SiteUtils.GetNavigationSiteRoot() + "/Default.aspx");
        


        }
    }
}
