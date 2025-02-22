///	Created:			    2007-04-13
///	Last Modified:		    2010-01-31
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.		

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;


namespace Cynthia.Web.UI
{
    
    public class LogoutLink : WebControl
    {
        // these separator properties are deprecated
        // it is recommended not to use these properties
        // but instead to use Cynthia.Web.Controls.SeparatorControl
        private bool useLeftSeparator = false;
        public bool UseLeftSeparator
        {
            get { return useLeftSeparator; }
            set { useLeftSeparator = value; }
        }

        private string leftSeparatorImageUrl = string.Empty;
        public string LeftSeparatorImageUrl
        {
            get { return leftSeparatorImageUrl; }
            set { leftSeparatorImageUrl = value; }
        }

        private bool renderAsListItem = false;
        public bool RenderAsListItem
        {
            get { return renderAsListItem; }
            set { renderAsListItem = value; }
        }

        private string listItemCSS = "topnavitem";
        public string ListItemCss
        {
            get { return listItemCSS; }
            set { listItemCSS = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if (!Page.Request.IsAuthenticated) { return; }
            if (Context.User.Identity.AuthenticationType != "Forms") { return; }
             
            if (renderAsListItem)
            {
                //writer.Write("<li class='" + listItemCSS + "'>");
                writer.WriteBeginTag("li");
                writer.WriteAttribute("class", listItemCSS);
                writer.Write(HtmlTextWriter.TagRightChar);

            }

            if (UseLeftSeparator)
            {
                if (leftSeparatorImageUrl.Length > 0)
                {
                    writer.Write("<img class='accent' alt='' src='" + Page.ResolveUrl(leftSeparatorImageUrl) + "' border='0' />");
                }
                else
                {
                    writer.Write("<span class='accent'>|</span>");
                }
            }

            string urlToUse = SiteUtils.GetNavigationSiteRoot() + "/Logoff.aspx";
            if ((!WebConfigSettings.SslIsRequiredByWebServer)&&(Page.Request.IsSecureConnection)) { urlToUse = urlToUse.Replace("https", "http"); }

            if (CssClass.Length == 0) { CssClass = "sitelink"; }
            
            writer.WriteBeginTag("a");
            writer.WriteAttribute("class", CssClass);
            //writer.WriteAttribute("title", Resource.LogoutLink);
            writer.WriteAttribute("href", Page.ResolveUrl(urlToUse));
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteEncodedText(Resource.LogoutLink);
            writer.WriteEndTag("a");



            if (renderAsListItem) writer.WriteEndTag("li");

                
            
        }
    }
}
