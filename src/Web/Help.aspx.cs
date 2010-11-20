using System;
using System.Web.UI;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.UI.Pages
{
    public partial class Help : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        private string helpKey = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params.Get("helpkey") != null)
            {
                helpKey = Request.Params.Get("helpkey");
            }

            // if we pass this param suppress edit link doesn't work in cluetip
            if (Request.Params.Get("e") == null)
            {
                if (WebUser.IsAdminOrContentAdmin)
                {

                    if (helpKey != String.Empty)
                    {
                        litEditLink.Text = "<a href='"
                            + SiteUtils.GetNavigationSiteRoot() + "/HelpEdit.aspx?helpkey="
                            + helpKey + "'>"
                            + Resource.HelpEditLink
                            + "</a><br />";
                    }

                }
            }

            if (helpKey != String.Empty)
            {
                ShowHelp();
            }

        }


        protected void ShowHelp()
        {
            String helpText = ResourceHelper.GetHelpFileText(helpKey);
            if (helpText == String.Empty)
            {
                helpText = WebUser.IsAdminOrContentAdmin
                               ? Resource.HelpNoHelpAvailableAdminUser
                               : Resource.HelpNoHelpAvailable;
            }
            litHelp.Text = helpText;
        }
    }
}
