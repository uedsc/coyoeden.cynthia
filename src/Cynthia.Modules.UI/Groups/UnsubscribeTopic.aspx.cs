/// Last Modified:      2008-11-07
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using log4net;
using Cynthia.Business;
using Cynthia.Web.Framework;

namespace Cynthia.Web.GroupUI
{
    public partial class UnsubscribeGroupTopic : CBasePage
	{
		// Create a logger for use in this class
		private static readonly ILog log = LogManager.GetLogger(typeof(UnsubscribeGroupTopic));

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);
            base.OnInit(e);
        }
        #endregion


        private void Page_Load(object sender, System.EventArgs e)
		{
            
			if (!Request.IsAuthenticated)
			{
				lblUnsubscribe.Text = ResourceHelper.GetMessageTemplate("AccessDeniedMessage.config");
				return;
			}

            int topicID = WebUtils.ParseInt32FromQueryString("topicid", -1);

            if (topicID > -1)
            {
                UnsubscribeUser(topicID);
            }

					
		}

        private void UnsubscribeUser(int topicId)
        {
            SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
            if (siteUser == null) return;
            if (!GroupTopic.Unsubscribe(topicId, siteUser.UserId))
            {
                log.ErrorFormat("GroupTopic.UnSubscribe({0}, {1}) failed", topicId, siteUser.UserId);
                lblUnsubscribe.Text = Resources.GroupResources.GroupTopicUnsubscribeFailed;
                return;
            }
            lblUnsubscribe.Text = Resources.GroupResources.GroupTopicUnsubscribeCompleted;	

        }


		
	}
}
