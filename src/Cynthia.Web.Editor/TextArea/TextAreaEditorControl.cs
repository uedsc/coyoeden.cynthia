﻿// Author:					Joe Audette
// Created:				    2009-05-06
// Last Modified:			2009-05-07
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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Security.Permissions;
using log4net;

namespace Cynthia.Web.Editor
{
    public class TextAreaEditorControl : Control, IPostBackDataHandler
    {

        public TextAreaEditorControl() { }

        public string Text
        {
            get { object o = ViewState["Text"]; return (o == null ? string.Empty : (string)o); }
            set { ViewState["Text"] = value; }
        }

        public Unit Width
        {
            get { object o = ViewState["Width"]; return (o == null ? Unit.Percentage(100) : (Unit)o); }
            set { ViewState["Width"] = value; }
        }

        public Unit Height
        {
            get { object o = ViewState["Height"]; return (o == null ? Unit.Pixel(200) : (Unit)o); }
            set { ViewState["Height"] = value; }
        }


        protected override void Render(HtmlTextWriter writer)
        {

            //writer.Write(
            //    "<textarea id=\"{0}\" name=\"{0}\" rows=\"4\" cols=\"40\" style=\"width: {1}; height: {2}\" >{3}</textarea>",
            //        this.UniqueID,
            //        this.Width,
            //        this.Height,
            //        System.Web.HttpUtility.HtmlEncode(this.Text));

            writer.Write(
                "<textarea id=\"{0}\" name=\"{1}\" rows=\"4\" cols=\"40\" style=\"width: {2}; height: {3}\" >{4}</textarea>",
                    this.ClientID,
                    this.UniqueID,
                    this.Width,
                    this.Height,
                    System.Web.HttpUtility.HtmlEncode(this.Text));


        }


        #region Postback Handling

        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            if (
                (postCollection[postDataKey] != null)
                && (postCollection[postDataKey] != this.Text)
                )
            {
                this.Text = postCollection[postDataKey];
                return true;
            }

            return false;
        }

        public void RaisePostDataChangedEvent()
        {
            // Do nothing
        }

        #endregion


    }
}
