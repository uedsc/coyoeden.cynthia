
using System;
using System.Configuration;
using System.Globalization;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Web.Controls;
using Cynthia.Web.UI;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.UI.Pages
{
    public partial class LoginPage : CBasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(LoginPage));

        //Constituent controls inside LoginControl
        private SiteLabel lblUserID;
        private SiteLabel lblEmail;
      
        private string rpxApiKey = string.Empty;
        private string rpxApplicationName = string.Empty;
        private string returnUrlCookieName = string.Empty;
		

		private TextBox txtUserName;
		private CButton btnLogin;
		private TextBox txtPassword;

        /// <summary>
        /// whether show remember me checkbox
        /// </summary>
        protected bool ShowRememberMe {
            get {
                return WebConfigSettings.AllowPersistentLoginCookie;
            }
        }
        /// <summary>
        /// whether show pwd recovery
        /// </summary>
        protected bool ShowRecovery {
            get {
                return siteSettings.AllowPasswordRetrieval && !siteSettings.UseLdapAuth;
            }
        }
        /// <summary>
        /// whether show the register link
        /// </summary>
        protected bool ShowRegister {
            get {
                return siteSettings.AllowNewRegistration;
            }
        }
        protected string ReturnUrl {
            get {
                var url= Page.Request.Params.Get("returnurl");
                url = string.IsNullOrEmpty(url) ? "" : "?returnurl="+url;
                return url;
            }
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.AppendQueryStringToAction = false;

            rpxApiKey = siteSettings.RpxNowApiKey;
            rpxApplicationName = siteSettings.RpxNowApplicationName;
            //pnlOpenID.Visible = WebConfigSettings.EnableOpenIdAuthentication && siteSettings.AllowOpenIdAuth;

            if (WebConfigSettings.UseOpenIdRpxSettingsFromWebConfig)
            {
                if (WebConfigSettings.OpenIdRpxApiKey.Length > 0)
                {
                    rpxApiKey = WebConfigSettings.OpenIdRpxApiKey;
                }

                if (WebConfigSettings.OpenIdRpxApplicationName.Length > 0)
                {
                    rpxApplicationName = WebConfigSettings.OpenIdRpxApplicationName;
                }

            }

            returnUrlCookieName = "returnurl" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);


#if !MONO

            if ((WebConfigSettings.EnableOpenIdAuthentication) && (siteSettings.AllowOpenIdAuth))
            {
                pnlOpenID.Visible = true;
                OpenIdLoginControl oidLogin = (OpenIdLoginControl)Page.LoadControl("~/Controls/OpenIDLoginControl.ascx");
                oidLogin.ID = "oidLogin";
                pnlOpenID.Controls.Add(oidLogin);
            }

            if (rpxApiKey.Length > 0)
            {
                if ((!WebConfigSettings.DisableRpxAuthentication))
                {
                    pnlOpenID.Visible = true;
                    OpenIdRpxNowLink rpxNowLink = new OpenIdRpxNowLink();
                    pnlOpenID.Controls.Add(rpxNowLink);
                    
                }
            }
            //else
            //{
                
            //}
            
#endif

            //DotNetOpenAuth does not work on mono as of 2009-05-15 so only option is rpxnow.com
#if MONO
            if (rpxApiKey.Length > 0)
            {
                OpenIdRpxNowLink rpxNowLink = new OpenIdRpxNowLink();
                pnlOpenID.Controls.Add(rpxNowLink);
            }

#endif

            if (WebConfigSettings.HideMenusOnLoginPage) { SuppressAllMenus(); }
            
        }


        private void Page_Load(object sender, EventArgs e)
		{
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            SecurityHelper.DisableBrowserCache();

            if (Request.IsAuthenticated)
            {
                // user is logged in
                WebUtils.SetupRedirect(this, SiteRoot + "/Default.aspx");
                return;
            }

            lblUserID = (SiteLabel)this.LoginCtrl.FindControl("lblUserID");
            lblEmail = (SiteLabel)this.LoginCtrl.FindControl("lblEmail");
            txtUserName = (TextBox)this.LoginCtrl.FindControl("UserName");
            txtPassword = (TextBox)this.LoginCtrl.FindControl("Password");
            btnLogin = (CButton)this.LoginCtrl.FindControl("Login");

            

            PopulateLabels();
            

            string wlAppId = siteSettings.WindowsLiveAppId;
            if (ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"] != null)
            {
                wlAppId = ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"];
                if (wlAppId.Length == 0) { wlAppId = siteSettings.WindowsLiveAppId; }
            }

            pnlWindowsLive.Visible 
                = WebConfigSettings.EnableWindowsLiveAuthentication 
                && siteSettings.AllowWindowsLiveAuth
                && (wlAppId.Length > 0);

            divLiteralOr.Visible = (pnlOpenID.Visible && pnlWindowsLive.Visible);

            if ((siteSettings.UseEmailForLogin)&&(!siteSettings.UseLdapAuth))
            {
                if (!WebConfigSettings.AllowLoginWithUsernameWhenSiteSettingIsUseEmailForLogin)
                {
                    RegularExpressionValidator regexEmail = new RegularExpressionValidator();
                    regexEmail.ControlToValidate = txtUserName.ID;
                    //regexEmail.ValidationExpression = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$";
                    regexEmail.ValidationExpression = SecurityHelper.RegexEmailValidationPattern;
                    regexEmail.ErrorMessage = Resource.LoginFailedInvalidEmailFormatMessage;
                    this.LoginCtrl.Controls.Add(regexEmail);
                }

            }

            SetupReturnUrlCookie();

            if (siteSettings.DisableDbAuth) { pnlStandardLogin.Visible = false; }
            

        }

        private void SetupReturnUrlCookie()
        {
            if (Page.IsPostBack) { return; }
            
            string returnUrl = string.Empty;

            if (Page.Request.UrlReferrer != null)
            {
                string urlReferrer = Page.Request.UrlReferrer.ToString();
                if ((urlReferrer.StartsWith(SiteRoot)) || (urlReferrer.StartsWith(SiteRoot.Replace("https://", "http://"))))
                {
                    returnUrl = urlReferrer;

                }
            }

            string returnUrlParam = Page.Request.Params.Get("returnurl");
            if (!String.IsNullOrEmpty(returnUrlParam))
            {
                returnUrlParam = SecurityHelper.RemoveMarkup(returnUrlParam);
                string redirectUrl = Page.ResolveUrl(SecurityHelper.RemoveMarkup(Page.Server.UrlDecode(returnUrlParam)));
                //string redirectUrl = Page.ResolveUrl(SecurityHelper.RemoveMarkup(returnUrlParam));
                if ((redirectUrl.StartsWith(SiteRoot)) || (redirectUrl.StartsWith(SiteRoot.Replace("https://", "http://"))))
                {
                    returnUrl = redirectUrl;
                }
            }

            
            if (returnUrl.Length > 0) { CookieHelper.SetCookie(returnUrlCookieName, returnUrl); }

            

        }


        private void PopulateLabels()
		{
            
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.LoginLink);

            MetaDescription = string.Format(CultureInfo.InvariantCulture,
                Resource.MetaDescriptionSignInPageFormat, siteSettings.SiteName);
            
			if (siteSettings.UseEmailForLogin && !siteSettings.UseLdapAuth)
			{
                this.lblUserID.Visible = false;
			}
			else
			{
                this.lblEmail.Visible = false;
			}

            txtUserName.Focus();

            btnLogin.Text = this.LoginCtrl.LoginButtonText;
            SiteUtils.SetButtonAccessKey(btnLogin, AccessKeys.LoginAccessKey);

            litOr.Text = Resource.LiteralOr;
        }

    }
}
