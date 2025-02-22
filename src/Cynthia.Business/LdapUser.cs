// Author:					TJ Fontaine
// Created:				    2005-09-30
// Last Modified:			2009-01-09
// 
// 1/7/2006 Haluk Eryuksel added convNull2Blank
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 

using System;
using Novell.Directory.Ldap;

namespace Cynthia.Business
{
	/// <summary>
	///
	/// </summary>
	public class LdapUser
	{
		private LdapAttribute email = null;
		private LdapAttribute commonname = null;
		private LdapAttribute password = null;
		private LdapAttribute userid = null;
		private LdapAttribute uidNumber = null;
		private string dn = String.Empty;

		public LdapUser() {}

        public String ConvNull2Blank(LdapAttribute str)
        {
            if (str == null)
                return " ";
            else
                return str.StringValue;
        }

        public LdapUser(LdapSettings ldapSettings, String userName)
        {
            // in some cases with Active Directory
            // we can't actually retrieve ldap entries
            // we really just need to create a Cynthia user
            // from the ldap user so if we can't read it, just create an ldap user
            // with the properties we do have
            // Active Directory allows us to bind a connection for authentication
            // even if we can't query for entries

            email = new LdapAttribute("email", userName + "@" + ldapSettings.Domain);
            commonname = new LdapAttribute("commonname", userName);
            userid = new LdapAttribute("userid", userName);

        }

		public LdapUser(LdapEntry entry)
		{
			dn = entry.DN;

			LdapAttributeSet las = entry.getAttributeSet();
			
			foreach(LdapAttribute a in las)
			{
				switch(a.Name)
				{
					case "mail":
						this.email = a;
						break;
					case "cn":
						this.commonname = a;
						break;
					case "userPassword":
						this.password = a;
						break;
					case "uidNumber":
						this.uidNumber = a;
						break;
					case "uid":
						this.userid = a;
                        break;
                    case "sAMAccountName":
                        this.userid = a;
						break;
				}
			}
		}

		public string Email
		{
            get { return ConvNull2Blank(email); }
		}

		public string UserId
		{
			get { return ConvNull2Blank(userid); }
		}

		public string CommonName
		{
            get { return ConvNull2Blank(commonname); }

		}

		public string Password
		{
            get { return ConvNull2Blank(password); }
		}

		public string DN
		{
			get { return dn; }
		}

	}
}
