﻿/// Author:					Voir Hillaire/Joe Audette
/// Created:				2009-03-24
/// Last Modified:			2009-12-16
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Web.UI;
using Cynthia.Web.Framework;
using Cynthia.Business;
using Resources;


namespace Cynthia.Web.UI
{
    public partial class PlugNPayLogList : UserControl
    {
        private int pageId = -1;
        private int moduleId = -1;
        //private int totalPages = 1;
        private int pageNumber = 1;
        //private int pageSize = 10;
        private Guid storeGuid = Guid.Empty;
        private Guid cartGuid = Guid.Empty;

        public Guid StoreGuid
        {
            get { return storeGuid; }
            set { storeGuid = value; }
        }

        public Guid CartGuid
        {
            get { return cartGuid; }
            set { cartGuid = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();

            BindGrid();
        }

        private void BindGrid()
        {
            if (cartGuid == Guid.Empty) 
            {
                this.Visible = false;
                return; 
            }


            using (IDataReader reader = PlugNPayLog.GetByCart(cartGuid))
            {
                pgrCheckoutLog.Visible = false;

                grdCheckoutLog.DataSource = reader;
                grdCheckoutLog.DataBind();
            }

            if (grdCheckoutLog.Rows.Count == 0)
            {
                this.Visible = false;
            }

        }

        private void PopulateLabels()
        {
            litHeading.Text = Resource.PlugNPayLogHeading;
            grdCheckoutLog.Columns[0].HeaderText = Resource.PlugNPayLogTransactionType;
            grdCheckoutLog.Columns[1].HeaderText = Resource.PlugNPayLogTransactionId;
            grdCheckoutLog.Columns[2].HeaderText = Resource.PlugNPayLogMethod;
            grdCheckoutLog.Columns[3].HeaderText = Resource.PlugNPayLogResponseCode;
            grdCheckoutLog.Columns[4].HeaderText = Resource.PlugNPayLogReason;
            grdCheckoutLog.Columns[5].HeaderText = Resource.PlugNPayLogAuthCode;
            //grdCheckoutLog.Columns[6].HeaderText = Resource.PlugNPayLogProviderName;
            grdCheckoutLog.Columns[6].HeaderText = Resource.PlugNPayLogCreatedUtc;



        }


        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

        }

    }
}