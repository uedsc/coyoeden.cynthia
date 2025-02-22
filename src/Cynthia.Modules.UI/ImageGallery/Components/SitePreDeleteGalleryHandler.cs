﻿using System;
using System.Configuration.Provider;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using log4net;

namespace Cynthia.Modules
{
    /// <summary>
    /// Author:                     Joe Audette
    /// Created:                    2008-11-12
    ///	Last Modified:              2008-11-12
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    ///  
    /// </summary>
    public class SitePreDeleteGalleryHandler : SitePreDeleteHandlerProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(SitePreDeleteGalleryHandler));

        public SitePreDeleteGalleryHandler()
        { }

        public override void DeleteSiteContent(int siteId)
        {
            Gallery.DeleteBySite(siteId);

        }
    }
}
