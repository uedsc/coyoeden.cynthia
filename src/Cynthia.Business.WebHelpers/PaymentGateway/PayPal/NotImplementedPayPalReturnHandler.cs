﻿//  Author:                     Joe Audette
//  Created:                    2008-06-27
//	Last Modified:              2008-06-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Web;

namespace Cynthia.Business.WebHelpers.PaymentGateway
{
    /// <summary>
    ///  
    /// </summary>
    public class NotImplementedPayPalReturnHandler : PayPalReturnHandlerProvider
    {
        public NotImplementedPayPalReturnHandler()
        { }

        public override string HandleRequestAndReturnUrlForRedirect(
            HttpContext context,
            string payPalToken,
            string payPalPayerId,
            PayPalLog setExpressCheckoutLog)
        {
            // do nothing
            return string.Empty;

        }

    }
}
