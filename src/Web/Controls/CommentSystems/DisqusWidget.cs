﻿//  Author:                     Joe Audette
//  Created:                    2009-09-05
//	Last Modified:              2009-12-11
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// enables the main javascript for discqus and optionally renders the html widget
    /// </summary>
    public class DisqusWidget : WebControl
    {
        private string siteShortName = string.Empty;

        public string SiteShortName
        {
            get { return siteShortName; }
            set { siteShortName = value; }
        }

        private bool renderCommentCountScript = false;

        public bool RenderCommentCountScript
        {
            get { return renderCommentCountScript; }
            set { renderCommentCountScript = value; }
        }

        private bool renderWidget = false;

        public bool RenderWidget
        {
            get { return renderWidget; }
            set { renderWidget = value; }
        }

        private string widgetPageUrl = string.Empty;

        /// <summary>
        /// Defines the page URL associated with a comment topic. Disqus uses this URL to uniquely create and identity a comment topic.
        /// </summary>
        public string WidgetPageUrl
        {
            get { return widgetPageUrl; }
            set { widgetPageUrl = value; }
        }

        private string widgetPageId = string.Empty;

        /// <summary>
        /// Defines a custom identifier for Disqus to use in place of the page URL (disqus_topic).
        /// </summary>
        public string WidgetPageId
        {
            get { return widgetPageId; }
            set { widgetPageId = value; }
        }

        private bool renderPoweredBy = false;

        public bool RenderPoweredBy
        {
            get { return renderPoweredBy; }
            set { renderPoweredBy = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (siteShortName.Length == 0) { return; }

            if (!Visible) { return; }

            if(renderWidget)
            {
                SetupWidgetVars();
            }

            if (renderCommentCountScript) { SetupCommentCountScript(); }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //base.Render(writer);
            if (siteShortName.Length == 0) { return; }

            if (!Visible) { return; }

            //if (renderCommentCountScript) { SetupCommentCountScript(); }

            if (renderWidget) { RenderTopicWidget(writer); }
        }

        private void RenderTopicWidget(HtmlTextWriter writer)
        {
            writer.Write("<div class=\"cmwrapper\">");
            writer.Write("<div id=\"" + this.ClientID + "\">&nbsp;</div>");
            writer.Write("<noscript><a href=\"http://" + siteShortName + ".disqus.com/?url=ref\">" + Resource.DisqusViewDiscussion + "</a></noscript>");

            if (renderPoweredBy)
            {
                writer.Write("<a href=\"http://disqus.com\" class=\"dsq-brlink\">blog comments powered by <span class=\"logo-disqus\">Disqus</span></a>");
            }

            writer.Write("</div>");
        }

        private void SetupWidgetVars()
        {
            bool addScript = true;
            StringBuilder script = new StringBuilder();
            script.Append("\r\n<script type=\"text/javascript\">");

            script.Append("\r\n var disqus_container_id = \"" + this.ClientID + "\";");

            if (Page.Request.Url.ToString().Contains("localhost"))
            {
                script.Append("\r\n var disqus_developer = 1;");
                addScript = true;
            }

            if (widgetPageUrl.Length > 0)
            {
                script.Append("\r\n var disqus_url = \"" + widgetPageUrl + "\";");
                addScript = true;
            }
            if (widgetPageId.Length > 0)
            {
                script.Append("\r\n var disqus_identifier = \"" + widgetPageId + "\";");
                addScript = true;
            }



            script.Append("\r\n</script>");

            if (addScript)
            {
                Page.ClientScript.RegisterClientScriptBlock(
                    typeof(Page),
                    "disqusvars",
                    script.ToString());
            }

            
            Page.ClientScript.RegisterStartupScript(typeof(Page), "disqusmain", "<script type=\"text/javascript\" src=\"http://disqus.com/groups/" + siteShortName + "/embed.js\" ></script>", false);
        }

        private void SetupCommentCountScript()
        {

            StringBuilder script = new StringBuilder();

            script.Append("\n<script type=\"text/javascript\">");

            script.Append("\n");
            script.Append("//<![CDATA[");
            script.Append("\n");
            script.Append("(function() {");
            script.Append("var links = document.getElementsByTagName('a');");
            script.Append("var query = '?';");
            script.Append("for(var i = 0; i < links.length; i++) {");
            script.Append("if(links[i].href.indexOf('#disqus_topic') >= 0) {");
            script.Append("query += 'url' + i + '=' + encodeURIComponent(links[i].href) + '&';");
            script.Append("}");
            script.Append("}");

            script.Append("document.write('<script charset=\"utf-8\" type=\"text/javascript\" src=\"http://disqus.com/forums/" 
                + siteShortName + "/get_num_replies.js' + query + '\"></' + 'script>');");

            script.Append("})();");
            script.Append("\n");
            script.Append("//]]>");

            script.Append("\n</script>");

            Page.ClientScript.RegisterStartupScript(
                typeof(Page),
                "disqusmain",
                script.ToString());


        }

    }
}
