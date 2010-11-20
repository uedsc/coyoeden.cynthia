///	Created:			    2007-05-23
///	Last Modified:		    2010-01-18
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.		

using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;


namespace Cynthia.Web.UI
{
    public class CynthiaLink : WebControl
    {
        private bool useImage = true;
        private bool openInNewWindow = false;

        public bool UseImage
        {
            get { return useImage; }
            set { useImage = value; }
        }

        public bool OpenInNewWindow
        {
            get { return openInNewWindow; }
            set { openInNewWindow = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // TODO: show a bmp or some other design time thing?
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                string urlToUse = "http://www.vivasky.com";
                string innerMarkup = Resource.PoweredByLink;
                if (useImage)
                {
                    innerMarkup = "<img  src='"
                        + Page.ResolveUrl("~/Data/SiteImages/poweredbyCynthia3.gif")
                        + "' alt='" + Resource.PoweredByAltText + "' />";

                }

                string css = string.Empty;
                if (CssClass.Length > 0) css = " class='" + CssClass + "' ";
                string onclick = string.Empty;
                if (openInNewWindow)
                {
                    onclick = " onclick=\"window.open(this.href,'_blank');return false;\" ";
                }

                writer.Write(string.Format(CultureInfo.InvariantCulture,
                                 "<a href='{0}' "
                                 + onclick
                                 + css
                                 + ">"
                                 + innerMarkup
                                 + "</a>",
                                 urlToUse));
            }
        }

    }
}
