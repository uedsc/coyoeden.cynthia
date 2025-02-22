/// Author:		        Joe Audette
/// Created:            2007-08-16
/// Last Modified:      2010-01-21
/// 
/// Licensed under the terms of the GNU Lesser General Public License:
///	http://www.opensource.org/licenses/lgpl-license.php
///
/// You must not remove this notice, or any other, from this software.

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
    
    public class SubkismetCaptchaAdapter : ICaptcha
    {
        #region Constructors

        public SubkismetCaptchaAdapter() 
        {
            InitializeAdapter();
        }

        #endregion

        private Subkismet.Captcha.CaptchaControl captchaControl
            = new Subkismet.Captcha.CaptchaControl();

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
            if (HttpContext.Current == null) return;

            captchaControl.InstructionText = "Enter the code shown above:";
            captchaControl.ErrorMessage = "Incorrect, try again";
            //SubkismetCaptchFailureMessage

            try
            {
                object resource = HttpContext.GetGlobalResourceObject(
                    "Resource", "SubkismetCaptchaInstructions");

                if (resource != null)
                {
                    captchaControl.InstructionText = "&nbsp;" + resource.ToString();
                }

                resource = HttpContext.GetGlobalResourceObject(
                    "Resource", "SubkismetCaptchFailureMessage");

                if (resource != null)
                {
                    captchaControl.ErrorMessage = resource.ToString();
                }
                
            }
            catch { }

            
        }

        #region Public Methods

        public Control GetControl()
        {
            return captchaControl;
        }



        #endregion

    }
}
