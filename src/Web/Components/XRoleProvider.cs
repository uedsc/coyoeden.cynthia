// Author:             Joe Audette
// Created:            2006-04-08
// Last Modified:      2010-03-21

using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Collections;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;


namespace Cynthia.Web
{
    /// <summary>
    ///
    /// </summary>
    public class CRoleProvider : RoleProvider
    {
        private static readonly log4net.ILog log
            = log4net.LogManager.GetLogger(typeof(CRoleProvider));

        private string applicationName = "unknown";
        //private SiteSettings siteSettings;
        
        public CRoleProvider()
        {
            //if (HttpContext.Current != null)
            //{
            //    siteSettings = CacheHelper.GetCurrentSiteSettings();
            //    if (siteSettings != null)
            //    {
            //        this.applicationName = siteSettings.SiteName;
            //    }
            //    else
            //    {
            //        this.applicationName = "Site Not Configured";
            //        //throw new Exception("siteSettings object is null, can't initialize CRoleProvider. This could be caused by failure to connect to the database or other issues.");
            //    }
            //}

        }

        
        public override string ApplicationName
        {
          get
          {
              if (HttpContext.Current != null)
              {
                  SiteSettings siteSettings 
                      = CacheHelper.GetCurrentSiteSettings();
                  if (siteSettings != null)
                  {
                      this.applicationName = siteSettings.SiteName;
                  }

              }

              return applicationName;
          }
          set
          {
              applicationName = value;
          }
        }

        /// <summary>
        /// Get any needed parameters from config section
        /// </summary>
        /// <param name="name">name of the provider</param>
        /// <param name="config">configuration collection</param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
          //if (config["blah"] != null)
          //{
          //  String blah = config["blah"];
          //}

          base.Initialize(name, config);

        }

        /// <summary>
        /// required implementation
        /// </summary>
        /// <param name="userNames">a list of usernames</param>
        /// <param name="roleNames">a list of roles</param>
        public override void AddUsersToRoles(string[] userNames, string[] roleNames)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if((siteSettings != null)&&(userNames != null)&&(roleNames != null))
            {
                foreach (String userName in userNames)
                {
                    SiteUser siteUser = new SiteUser(siteSettings, userName);
                    if (siteUser.UserId > -1)
                    {
                        foreach (String roleName in roleNames)
                        {
                            Role role = new Role(siteSettings.SiteId, roleName);
                            if (role.RoleId > -1)
                            {
                                Role.AddUser(role.RoleId, siteUser.UserId, role.RoleGuid, siteUser.UserGuid);
                            }
                        }
                    }

                }
            }
           
        }

        /// <summary>
        /// required implementation
        /// </summary>
        /// <param name="roleName">a role name</param>
        public override void CreateRole(string roleName)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if ((siteSettings != null)&&(roleName != null)&&(roleName.Length > 0))
            {
                if (!Role.Exists(siteSettings.SiteId, roleName))
                {
                    Role role = new Role();
                    role.SiteId = siteSettings.SiteId;
                    role.SiteGuid = siteSettings.SiteGuid;
                    role.RoleName = roleName;
                    role.Save();
                }

            }
  
        }

        /// <summary>
        /// required implementation
        /// </summary>
        /// <param name="roleName">a role</param>
        /// <param name="throwOnPopulatedRole">get upset of users are in a role</param>
        /// <returns></returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            bool result = false;

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if((siteSettings != null)&&(roleName != null)&&(roleName.Length > 0))
            {
                Role role = new Role(siteSettings.SiteId, roleName);
                if (role.RoleId > 0)
                {
                    if ((throwOnPopulatedRole) && (role.HasUsers()))
                    {
                        throw new Exception("This role cannot be deleted because it has users.");
                    }

                    Role.DeleteRole(role.RoleId);
                    result = true;

                }

            }
            
            return result;
        }

        /// <summary>
        /// required implemention
        /// </summary>
        /// <param name="roleName">a role</param>
        /// <param name="usernameToMatch">a username to look for in the role</param>
        /// <returns></returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
          //return service.FindUsersInRole(_RemoteProviderName, _ApplicationName, roleName, usernameToMatch);
            
            
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// required implementation
        /// this should not be used to get data for a dropdown list
        /// because it doesn't have role id or display name
        /// </summary>
        /// <returns></returns>
        public override string[] GetAllRoles()
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings != null)
            {
                string[] roleList = new string[Role.CountOfRoles(siteSettings.SiteId)];
                int i = 0;
                using (IDataReader reader = Role.GetSiteRoles(siteSettings.SiteId))
                {
                    while (reader.Read())
                    {
                        roleList[i] = reader["RoleName"].ToString();
                        i += 1;

                    }
                }

                return roleList;

            }

            return new string[0];
            
        }

        /// <summary>
        /// required implementation
        /// </summary>
        /// <param name="username">a username</param>
        /// <returns>a list of roles</returns>
        public override string[] GetRolesForUser(string userName)
        {
            if (HttpContext.Current != null)
            {
                
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                string roleCookieName = SiteUtils.GetRoleCookieName(siteSettings);

                if ((HttpContext.Current.Request.IsAuthenticated)
                    && (HttpContext.Current.User.Identity.Name == userName)
                    &&(siteSettings != null)
                    )
                {
                    
                    if (
                        (CookieHelper.CookieExists(roleCookieName))
                        && (CookieHelper.GetCookieValue(roleCookieName).Length > 0)
                       )
                    {
                        return GetRolesFromCookie();
                    }
                    else
                    {
                        return GetRolesAndSetCookie();
                    }
                    
                }
                else
                {
                    // not current user or not authenticated
                    

                    if ((siteSettings != null) && (userName != null) && (userName.Length > 0))
                    {
                        return SiteUser.GetRoles(siteSettings, userName);
                    }
                }
            }

            return new string[0];

        }

        private string[] GetRolesAndSetCookie()
        {
            string[] currentUserRoles = new string[0];
            String hostName = WebUtils.GetHostName();

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings != null)
            {
                string roleCookieName = SiteUtils.GetRoleCookieName(siteSettings);
                currentUserRoles = SiteUser.GetRoles(siteSettings, HttpContext.Current.User.Identity.Name);
                string roleStr = "";
                foreach (string role in currentUserRoles)
                {
                    roleStr += role;
                    roleStr += ";";
                }

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,                              // version
                    HttpContext.Current.User.Identity.Name,     // user name
                    DateTime.Now,                   // issue time
                    DateTime.Now.AddHours(1),       // expires every hour
                    false,                          // don't persist cookie
                    roleStr                         // roles
                    );

                string cookieStr = FormsAuthentication.Encrypt(ticket);

                HttpCookie roleCookie = new HttpCookie(roleCookieName, cookieStr);
                //roleCookie.Expires = DateTime.Now.AddMinutes(20);
                roleCookie.HttpOnly = true;
                roleCookie.Path = "/";
                HttpContext.Current.Response.Cookies.Add(roleCookie);
            }

            return currentUserRoles;


        }

        private string[] GetRolesFromCookie()
        {
            string[] currentUserRoles = new string[0];
            String hostName = WebUtils.GetHostName();
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings != null)
            {
                string roleCookieName = SiteUtils.GetRoleCookieName(siteSettings);
                ArrayList userRoles = new ArrayList();
                HttpCookie roleCookie = HttpContext.Current.Request.Cookies[roleCookieName];
                if (roleCookie != null)
                {
                    FormsAuthenticationTicket ticket
                        = FormsAuthentication.Decrypt(roleCookie.Value);
                    
                    foreach (string role in ticket.UserData.Split(new char[] { ';' }))
                    {
                        userRoles.Add(role);
                    }
                }

                currentUserRoles = (string[])userRoles.ToArray(typeof(string));
            }
            
            

            return currentUserRoles;
        }


        

        /// <summary>
        /// required implementation
        /// </summary>
        /// <param name="roleName">a role</param>
        /// <returns>a list of users</returns>
        public override string[] GetUsersInRole(string roleName)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (siteSettings != null)
            {
                Role role = new Role(siteSettings.SiteId, roleName);
                if (role.RoleId > -1)
                {
                    string[] userList = new string[role.CountOfUsers()];
                    int i = 0;
                    using (IDataReader reader = Role.GetRoleMembers(role.RoleId))
                    {
                        while (reader.Read())
                        {
                            if (siteSettings.UseEmailForLogin)
                            {
                                userList[i] = reader["Email"].ToString();
                            }
                            else
                            {
                                userList[i] = reader["LoginName"].ToString();
                            }

                            i += 1;

                        }
                    }

                    return userList;

                }
            }

            return new string[0];

        }

        /// <summary>
        /// required implementation
        /// </summary>
        /// <param name="userName">a username</param>
        /// <param name="roleName">a role</param>
        /// <returns>true or false</returns>
        public override bool IsUserInRole(string userName, string roleName)
        {
            bool result = false;
            string[] userRoles = GetRolesForUser(userName);
            foreach (String role in userRoles)
            {
                if (role == roleName)
                    result = true;
            }
          
            return result;
        }

        /// <summary>
        /// required implementation
        /// </summary>
        /// <param name="userNames">a list of usernames</param>
        /// <param name="roleNames">a list of roles</param>
        public override void RemoveUsersFromRoles(string[] userNames, string[] roleNames)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if ((siteSettings != null) && (userNames != null) && (roleNames != null))
            {
                foreach (String userName in userNames)
                {
                    SiteUser siteUser = new SiteUser(siteSettings, userName);
                    if (siteUser.UserId > 0)
                    {
                        foreach (String roleName in roleNames)
                        {
                            Role role = new Role(siteSettings.SiteId, roleName);
                            if (role.RoleId > 0)
                            {
                                Role.RemoveUser(role.RoleId, siteUser.UserId);
                            }
                        }
                    }

                }
            }
          

        }

        /// <summary>
        /// required implementation
        /// </summary>
        /// <param name="roleName">a role</param>
        /// <returns>true or false</returns>
        public override bool RoleExists(string roleName)
        {
            bool result = false;
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if ((siteSettings != null) && (roleName != null) && (roleName.Length > 0))
            {
                result = Role.Exists(siteSettings.SiteId, roleName);
            }

            return result;
        }



        //public override string[] GetRolesForUser(string userName)
        //{
        //    if (HttpContext.Current != null)
        //    {
        //        siteSettings = CacheHelper.GetCurrentSiteSettings();

        //        if ((HttpContext.Current.Request.IsAuthenticated)
        //            && (HttpContext.Current.User.Identity.Name == userName)
        //            )
        //        {
        //            string[] currentUserRoles;
        //            try
        //            {
        //                if (log.IsDebugEnabled) log.Debug("Decrypting ticket");
        //                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies["portalroles"].Value);
        //                ArrayList userRoles = new ArrayList();

        //                foreach (string role in ticket.UserData.Split(new char[] { ';' }))
        //                {
        //                    userRoles.Add(role);
        //                }

        //                currentUserRoles = (string[])userRoles.ToArray(typeof(string));
        //                return currentUserRoles;
        //            }
        //            catch (Exception ex)
        //            {
        //                if (log.IsDebugEnabled) log.Debug("Caught " + ex + ", so creating a new cookie");
        //                // Exception occurs if cookie does not exist, or was issued to a different user (eg logging out
        //                // and logging back in as another user).
        //                siteSettings = CacheHelper.GetCurrentSiteSettings();
        //                currentUserRoles = SiteUser.GetRoles(siteSettings, HttpContext.Current.User.Identity.Name);
        //                string roleStr = "";
        //                foreach (string role in currentUserRoles)
        //                {
        //                    roleStr += role;
        //                    roleStr += ";";
        //                }

        //                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
        //                    1,                              // version
        //                    HttpContext.Current.User.Identity.Name,     // user name
        //                    DateTime.Now,                   // issue time
        //                    DateTime.Now.AddHours(1),       // expires every hour
        //                    false,                          // don't persist cookie
        //                    roleStr                         // roles
        //                    );

        //                string cookieStr = FormsAuthentication.Encrypt(ticket);

        //                HttpCookie roleCookie = new HttpCookie("portalroles", cookieStr);
        //                roleCookie.Expires = DateTime.Now.AddMinutes(20);
        //                roleCookie.Path = "/";
        //                HttpContext.Current.Response.Cookies.Add(roleCookie);

        //                return currentUserRoles;

        //            }
        //        }
        //        else
        //        {
        //            // not current user or not authenticated

        //            if ((siteSettings != null) && (userName != null) && (userName.Length > 0))
        //            {
        //                return SiteUser.GetRoles(siteSettings, userName);
        //            }
        //        }
        //    }

        //    return new string[0];

        //}


    }
}

