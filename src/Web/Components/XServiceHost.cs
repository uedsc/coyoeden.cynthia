﻿// Author:					Joe Audette
// Created:				    2008-12-15
// Last Modified:		    2009-01-16
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

#if !MONO
using System;
using System.ServiceModel;

namespace Cynthia.Web
{
    /// <summary>
    ///
    /// </summary>
    public class CServiceHost : ServiceHost
    {
        public CServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        { }
        protected override void ApplyConfiguration()
        {
            base.ApplyConfiguration();
        }

    }
}
#endif
