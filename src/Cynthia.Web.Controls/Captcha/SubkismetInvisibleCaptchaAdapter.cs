using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web.Controls;
using Subkismet;
using Subkismet.Captcha;

namespace Cynthia.Web.Controls.Captcha
{
    /// <summary>
    /// Author:		        Joe Audette
    /// Created:            2007-08-17
    /// Last Modified:      2007-08-17
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public class SubkismetInvisibleCaptchaAdapter : ICaptcha
    {
        #region Constructors

        public SubkismetInvisibleCaptchaAdapter() 
        {
            InitializeAdapter();
        }

        #endregion

        private InvisibleCaptcha captchaControl
            = new InvisibleCaptcha();

        public bool IsValid
        {
            get
            {
                captchaControl.Validate();
                return captchaControl.IsValid;
            }
           
        }

        public bool Enabled
        {
            get { return captchaControl.Enabled; }
            set { captchaControl.Enabled = value; }

        }

        public string ControlID
        {
            get
            {
                return captchaControl.ID;
            }
            set
            {
                captchaControl.ID = value;
            }
        }

        private void InitializeAdapter()
        {
            //if (HttpContext.Current == null) return;

            captchaControl.Display = ValidatorDisplay.Dynamic;
            captchaControl.ValidationGroup = "Blog";

            //captchaControl.InstructionText = "Enter the code shown above:";
            //captchaControl.ErrorMessage = "Incorrect, try again";
            ////SubkismetCaptchFailureMessage

            //try
            //{
            //    object resource = HttpContext.GetGlobalResourceObject(
            //        "Resource", "SubkismetCaptchaInstructions");

            //    if (resource != null)
            //    {
            //        captchaControl.InstructionText = resource.ToString();
            //    }

            //    resource = HttpContext.GetGlobalResourceObject(
            //        "Resource", "SubkismetCaptchFailureMessage");

            //    if (resource != null)
            //    {
            //        captchaControl.ErrorMessage = resource.ToString();
            //    }
                
            //}
            //catch { }

            
        }

        #region Public Methods

        public Control GetControl()
        {
            return captchaControl;
        }



        #endregion
    }
}
