
using System;
using System.Web.UI;
using Resources;
using Cynthia.Business;

namespace Cynthia.Web.UI
{
    public class GroupMyTopicsLink : GroupUserTopicLink
    {
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


        protected override void OnPreRender(EventArgs e)
        {
            if (Page.Request.IsAuthenticated)
            {
                SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                if (siteUser != null)
                {
                    this.UserId = siteUser.UserId;
                    this.TotalPosts = siteUser.TotalPosts;

                }

            }

            
            this.Text = Resource.GroupMyPostsLink;

            if (renderAsListItem)
                if (CssClass.Length == 0) CssClass = "sitelink";

            base.OnPreRender(e);

        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (renderAsListItem)
            {
  
                writer.WriteBeginTag("li");
                writer.WriteAttribute("class", listItemCSS);
                writer.Write(HtmlTextWriter.TagRightChar);

            }

            base.Render(writer);

            if (renderAsListItem) writer.WriteEndTag("li");
        }

    }
}
