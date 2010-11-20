

using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Configuration;
using log4net;
using Cynthia.Web.Framework;

namespace Cynthia.Net
{
   
    /// <summary>
    /// this class is deprecated, better to create an EmailMessageTask messageTask = new EmailMessageTask(SiteUtils.GetSmtpSettings());
    /// </summary>
    public sealed class Notification
    {
        // Create a logger for use in this class
        private static readonly ILog log = LogManager.GetLogger(typeof(Notification));

        private Notification()
        {
            //only public static methods so no public constructor
        }

        public static void SendPassword(
            SmtpSettings smtpSettings,
            string messageTemplate,
            string fromEmail,
            string userEmail,
            string userPassword,
            string siteName,
            string siteLink)
        {

            StringBuilder message = new StringBuilder();
            message.Append(messageTemplate);
            message.Replace("{SiteName}", siteName);
            message.Replace("{AdminEmail}", fromEmail);
            message.Replace("{UserEmail}", userEmail);
            message.Replace("{UserPassword}", userPassword);
            message.Replace("{SiteLink}", siteLink);

            Email.SendEmail(
                smtpSettings,
                fromEmail,
                userEmail, "", "",
                siteName,
                message.ToString(), false, "Normal");


        }

        public static void SendRegistrationConfirmationLink(
            SmtpSettings smtpSettings,
            string messageTemplate,
            string fromEmail,
            string userEmail,
            string siteName,
            string confirmationLink)
        {

            StringBuilder message = new StringBuilder();
            message.Append(messageTemplate);
            message.Replace("{SiteName}", siteName);
            message.Replace("{AdminEmail}", fromEmail);
            message.Replace("{UserEmail}", userEmail);

            message.Replace("{ConfirmationLink}", confirmationLink);

            Email.SendEmail(
                smtpSettings,
                fromEmail,
                userEmail, "", "",
                siteName,
                message.ToString(), false, "Normal");


        }

        public static void SendGroupNotificationEmail(
            object oNotificationInfo)
        {
            if (oNotificationInfo == null) return;
            if (!(oNotificationInfo is GroupNotificationInfo)) return;

            GroupNotificationInfo notificationInfo = oNotificationInfo as GroupNotificationInfo;

            if (notificationInfo.Subscribers == null) return;

            if (log.IsDebugEnabled) log.Debug("In SendGroupNotificationEmail()");
            if (notificationInfo.Subscribers.Tables.Count > 0)
            {
                if (notificationInfo.Subscribers.Tables[0].Rows.Count > 0)
                {
                    int timeoutBetweenMessages = ConfigHelper.GetIntProperty("SmtpTimeoutBetweenMessages", 1000);

                    foreach (DataRow row in notificationInfo.Subscribers.Tables[0].Rows)
                    {
                        StringBuilder body = new StringBuilder();
                        body.Append(notificationInfo.BodyTemplate);
                        body.Replace("{SiteName}", notificationInfo.SiteName);
                        body.Replace("{ModuleName}", notificationInfo.ModuleName);
                        body.Replace("{GroupName}", notificationInfo.GroupName);
                        body.Replace("{AdminEmail}", notificationInfo.FromEmail);
                        body.Replace("{MessageLink}", notificationInfo.MessageLink);
                        body.Replace("{UnsubscribeGroupTopicLink}", notificationInfo.UnsubscribeGroupTopicLink);
                        body.Replace("{UnsubscribeGroupLink}", notificationInfo.UnsubscribeGroupLink);
                        StringBuilder emailSubject = new StringBuilder();
                        if (notificationInfo.SubjectTemplate.Length == 0)
                        {
                            notificationInfo.SubjectTemplate = "[{SiteName} - {GroupName}] {Subject}";
                        }
                        emailSubject.Append(notificationInfo.SubjectTemplate);
                        emailSubject.Replace("{SiteName}", notificationInfo.SiteName);
                        emailSubject.Replace("{ModuleName}", notificationInfo.ModuleName);
                        emailSubject.Replace("{GroupName}", notificationInfo.GroupName);
                        emailSubject.Replace("{Subject}", notificationInfo.Subject);

                        Email.SendEmail(
                            notificationInfo.SmtpSettings,
                            notificationInfo.FromEmail,
                            row["Email"].ToString(), "", "",
                            emailSubject.ToString(),
                            body.ToString(), false, "Normal");

                        Thread.Sleep(timeoutBetweenMessages);
                    }
                }
            }

        }


       

        public static void SendBlogCommentNotificationEmail(
            SmtpSettings smtpSettings,
            string messageTemplate,
            string fromEmail,
            string siteName,
            string authorEmail,
            string messageLink)
        {

            StringBuilder message = new StringBuilder();
            message.Append(messageTemplate);
            message.Replace("{SiteName}", siteName);
            message.Replace("{MessageLink}", messageLink);


            Email.SendEmail(
                smtpSettings,
                fromEmail,
                authorEmail, "", "",
                siteName,
                message.ToString(), false, "Normal");



        }



    }
}
