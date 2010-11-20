// Author:             Joe Audette
// Created:            2006-12-30
// Last Modified:      2009-01-09

using System;

namespace Cynthia.Web.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class CProfilePropertyOption
    {
        public CProfilePropertyOption()
        { }

        private String textResourceKey = String.Empty;
        private String optionValue = String.Empty;

        public String TextResourceKey
        {
            get { return textResourceKey; }
            set { textResourceKey = value; }
        }

        public String Value
        {
            get { return optionValue; }
            set { optionValue = value; }
        }


    }
}
