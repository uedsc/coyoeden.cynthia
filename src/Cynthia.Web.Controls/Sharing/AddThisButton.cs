﻿

using System;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;
using Cynthia.Web.Framework;


namespace Cynthia.Web.Controls
{
    /// <summary>
    /// See http://www.addthis.com
    /// </summary>
    public class AddThisButton : HyperLink
    {
        #region Private Properties

        private string accountId = string.Empty;
        private bool useMouseOverWidget = true;
        private string customLogoUrl = string.Empty;
        private string customLogoBackgroundColor = string.Empty;
        private string customLogoColor = string.Empty;
        private string customBrand = string.Empty;
        private string customOptions = string.Empty;
        private int customOffsetTop = -999;
        private int customOffsetLeft = -999;
        private string buttonImageUrl = "~/Data/SiteImages/addthissharebutton.gif";
        private string protocol = "http";

        private string urlToShare = string.Empty;
        private string titleOfUrlToShare = string.Empty;

        #endregion


        #region Public Properties


        /// <summary>
        /// Your addthis.com username.
        /// If this is not set the control will not render.
        /// </summary>
        public string AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        /// <summary>
        /// if true will show widget in the page
        /// </summary>
        public bool UseMouseOverWidget
        {
            get { return useMouseOverWidget; }
            set { useMouseOverWidget = value; }
        }

        /// <summary>
        /// The logo to display on the popup window (about 200x50 pixels). 
        /// The popup window is show when the user selects the 'More' choice
        /// </summary>
        public string CustomLogoUrl
        {
            get { return customLogoUrl; }
            set { customLogoUrl = value; }
        }

        /// <summary>
        /// The color to use as a background around the logo in the popup 
        /// </summary>
        public string CustomLogoBackgroundColor
        {
            get { return customLogoBackgroundColor; }
            set { customLogoBackgroundColor = value; }
        }


        /// <summary>
        /// The color to use for the text next to the logo in the popup 
        /// </summary>
        public string CustomLogoColor
        {
            get { return customLogoColor; }
            set { customLogoColor = value; }
        }


        /// <summary>
        /// The brand name to display in the drop-down (top right)
        /// </summary>
        public string CustomBrand
        {
            get { return customBrand; }
            set { customBrand = value; }
        }


        /// <summary>
        /// A comma-separated ordered list of options to include in the drop-down
        /// Example: addthis_options = 'favorites, email, digg, delicious, more'; 
        /// Currently supported options:
        /// delicious, digg, email, favorites, facebook, fark, furl, google, live, myweb, myspace, 
        /// newsvine, reddit, slashdot, stumbleupon, technorati, twitter, more 
        /// (the default is currently 'favorites, digg, delicious, google, myspace, facebook, 
        /// reddit, newsvine, 
        /// live, more', in that order).
        /// </summary>
        public string CustomOptions
        {
            get { return customOptions; }
            set { customOptions = value; }
        }

        /// <summary>
        /// Vertical offset for the drop-down window widget (in pixels) 
        /// thiscontrol defaults to -999 which means unsepcified
        /// this will result in the defaults from addthis.com
        /// not sure what the defsault is
        /// </summary>
        public int CustomOffsetTop
        {
            get { return customOffsetTop; }
            set { customOffsetTop = value; }
        }

        /// <summary>
        /// Horizontal offset for the drop-down window widget (in pixels) 
        /// thiscontrol defaults to -999 which means unsepcified
        /// this will result in the defaults from addthis.com
        /// not sure what the defsault is
        /// </summary>
        public int CustomOffsetLeft
        {
            get { return customOffsetLeft; }
            set { customOffsetLeft = value; }
        }

        public string ButtonImageUrl
        {
            get { return buttonImageUrl; }
            set { buttonImageUrl = value; }
        }

        public string UrlToShare
        {
            get { return urlToShare; }
            set { urlToShare = value; }
        }

        public string TitleOfUrlToShare
        {
            get { return titleOfUrlToShare; }
            set { titleOfUrlToShare = value; }
        }


        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (accountId.Length == 0)
            {
                this.Visible = false;
                return;
            }

            if (Page.Request.IsSecureConnection)
                protocol = "https";

            SetupScripts();

            this.ImageUrl = Page.ResolveUrl(buttonImageUrl);
            this.NavigateUrl = "http://www.addthis.com/bookmark.php";

            if (useMouseOverWidget)
                SetupWidget();
            else
                SetupNormalLink();

        }

        private void SetupNormalLink()
        {
            StringBuilder onClickAttribute = new StringBuilder();

            if (urlToShare.Length > 0)
            {
                onClickAttribute.Append(String.Format("addthis_url = '{0}'; ", urlToShare));
            }
            else
            {
                onClickAttribute.Append("addthis_url = location.href; ");
            }

            if (titleOfUrlToShare.Length > 0)
            {
                onClickAttribute.Append(String.Format("addthis_title ='{0}'; ", titleOfUrlToShare.HtmlEscapeQuotes()));
            }
            else
            {
                onClickAttribute.Append("addthis_title = document.title; ");

            }

            onClickAttribute.Append("return addthis_click(this); ");

            this.Attributes.Add("onclick", onClickAttribute.ToString());

            //this.Attributes.Add("onclick", "return addthis_click(this); ");

        }

        private void SetupWidget()
        {
            StringBuilder mouseOverAttribute = new StringBuilder();

            mouseOverAttribute.Append("try {return addthis_open(this, '',");

            if (urlToShare.Length > 0)
            {
                mouseOverAttribute.Append(String.Format("'{0}', ", urlToShare));
            }
            else
            {
                mouseOverAttribute.Append("'[URL]', ");
            }

            if (titleOfUrlToShare.Length > 0)
            {
                mouseOverAttribute.Append(String.Format("'{0}' ", titleOfUrlToShare.HtmlEscapeQuotes()));
            }
            else
            {
                mouseOverAttribute.Append("'[TITLE]' ");

            }

            mouseOverAttribute.Append(");}catch(ex){} ");

            
            this.Attributes.Add("onmouseover", mouseOverAttribute.ToString());

            this.Attributes.Add("onmouseout", "try { addthis_close(); }catch(ex){}");

        }

        private void SetupScripts()
        {
            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\"> ");
            script.Append("\n<!-- \n");

            script.Append(String.Format("var addthis_pub = '{0}';", accountId));

            if(customLogoUrl.Length > 0)
                script.Append(String.Format("var addthis_logo = '{0}';", customLogoUrl));

            if (customLogoBackgroundColor.Length > 0)
                script.Append(String.Format("var addthis_logo_background = '{0}';", customLogoBackgroundColor));

            if (customLogoColor.Length > 0)
                script.Append(String.Format("var addthis_logo_color = '{0}';", customLogoColor));

            if (customBrand.Length > 0)
                script.Append(String.Format("var addthis_brand = '{0}';", customBrand));

            if (customOptions.Length > 0)
                script.Append(String.Format("var addthis_options = '{0}';", customOptions));

            if (customOffsetTop != -999)
                script.Append(String.Format("var addthis_offset_top = {0};", customOffsetTop.ToString(CultureInfo.InvariantCulture)));

            if (customOffsetLeft != -999)
                script.Append(String.Format("var addthis_offset_left = {0};", customOffsetLeft.ToString(CultureInfo.InvariantCulture)));


            script.Append("\n//--> ");
            script.Append(" </script>");


            Page.ClientScript.RegisterClientScriptBlock(
                typeof(AddThisButton),
                "addthisbutton",
                script.ToString());

            if(useMouseOverWidget)
                Page.ClientScript.RegisterStartupScript(
                    typeof(AddThisButton),
                    "addthisbuttonsetup", String.Format("\n<script type=\"text/javascript\" src=\"{0}://s7.addthis.com/js/152/addthis_widget.js\" ></script>", protocol));
            else
                Page.ClientScript.RegisterStartupScript(
                    typeof(AddThisButton),
                    "addthisbuttonsetup", String.Format("\n<script type=\"text/javascript\" src=\"{0}://s9.addthis.com/js/widget.php?v=10\" ></script>", protocol));

        }

    }
}
