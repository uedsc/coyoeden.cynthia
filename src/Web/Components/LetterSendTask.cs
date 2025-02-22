﻿// Author:					Joe Audette
// Created:				    2007-12-30
// Last Modified:			2009-11-01
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Threading;
using log4net;
using Cynthia.Net;
using Cynthia.Business;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web
{
    /// <summary>
    ///
    /// </summary>
    [Serializable()]
    public class LetterSendTask : ITaskQueueTask
    {

        #region ITaskQueueTask

        private Guid taskGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid queuedBy = Guid.Empty;
        private string taskName = "Newsletter Send Task";
        private bool notifyOnCompletion = false;
        private string notificationToEmail = String.Empty;
        private string notificationFromEmail = String.Empty;
        private string notificationSubject = String.Empty;
        private string taskCompleteMessage = string.Empty;
        private string statusQueuedMessage = "Queued";
        private string statusStartedMessage = "Started";
        private string statusRunningMessage = "Running. Sent {0} of {1}";
        private string statusCompleteMessage = "Complete";
        private bool canStop = false;
        private bool canResume = true;
        // report status every 15 seconds by default
        private int updateFrequency = 15;

        #region Public ITaskQueueTask Properties

        public Guid TaskGuid
        {
            get { return taskGuid; }
            set { taskGuid = value; }
        }

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        public Guid QueuedBy
        {
            get { return queuedBy; }
            set { queuedBy = value; }
        }

        
        public string TaskName
        {
            get { return taskName; }
            set {
                //foo = value; //don't localize it can cause problems but need to keep th setter for backward compat
            } 
        }

        public bool NotifyOnCompletion
        {
            get { return notifyOnCompletion; }
            set { notifyOnCompletion = value; }
        }

        public string NotificationToEmail
        {
            get { return notificationToEmail; }
            set { notificationToEmail = value; }
        }

        public string NotificationFromEmail
        {
            get { return notificationFromEmail; }
            set { notificationFromEmail = value; }
        }

        public string NotificationSubject
        {
            get { return notificationSubject; }
            set { notificationSubject = value; }
        }

        public string TaskCompleteMessage
        {
            get { return taskCompleteMessage; }
            set { taskCompleteMessage = value; }
        }

        public string StatusQueuedMessage
        {
            get { return statusQueuedMessage; }
            set { statusQueuedMessage = value; }
        }

        public string StatusStartedMessage
        {
            get { return statusStartedMessage; }
            set { statusStartedMessage = value; }
        }

        public string StatusRunningMessage
        {
            get { return statusRunningMessage; }
            set { statusRunningMessage = value; }
        }

        public string StatusCompleteMessage
        {
            get { return statusCompleteMessage; }
            set { statusCompleteMessage = value; }
        }


        /// <summary>
        /// The frequency in second at which task status updates are expected.
        /// If no update to taskqueue status for 3x this value the taks is considered stalled.
        /// </summary>
        public int UpdateFrequency
        {
            get { return updateFrequency; }

        }

        public bool CanStop
        {
            get { return canStop; }

        }

        public bool CanResume
        {
            get { return canResume; }

        }

        #endregion

        public void QueueTask()
        {
            if (this.siteGuid == Guid.Empty) return;

            if (this.taskGuid != Guid.Empty) return;

            if (this.letterGuid == Guid.Empty) return;


            TaskQueue task = new TaskQueue();
            task.SiteGuid = this.siteGuid;
            task.QueuedBy = this.queuedBy;
            task.TaskName = this.taskName;
            task.NotifyOnCompletion = this.notifyOnCompletion;
            task.NotificationToEmail = this.notificationToEmail;
            task.NotificationFromEmail = this.notificationFromEmail;
            task.NotificationSubject = this.notificationSubject;
            task.TaskCompleteMessage = this.taskCompleteMessage;
            task.CanResume = this.canResume;
            task.CanStop = this.canStop;
            task.UpdateFrequency = this.updateFrequency;
            task.Status = statusQueuedMessage;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            this.taskGuid = task.NewGuid;
            task.SerializedTaskObject = SerializationHelper.SerializeToString(this);
            task.SerializedTaskType = this.GetType().AssemblyQualifiedName;
            task.Save();

            this.taskGuid = task.Guid;


        }

        public void StartTask()
        {
            if (this.taskGuid == Guid.Empty) return;

            TaskQueue task = new TaskQueue(this.taskGuid);

            if (task.Guid == Guid.Empty) return; // task not found

            if (!ThreadPool.QueueUserWorkItem(new WaitCallback(RunTaskOnNewTopic), this))
            {
                throw new Exception("Couldn't queue the task on a new topic.");
            }

            task.Status = statusStartedMessage;
            task.StartUTC = DateTime.UtcNow;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.Save();

            log.Info("Queued LetterSendTask on a new topic");


        }

        public void StopTask()
        {
            throw new System.NotImplementedException("This feature is not implemented");

        }

        public void ResumeTask()
        {
            StartTask();

        }

        #endregion



        private static readonly ILog log = LogManager.GetLogger(typeof(LetterSendTask));

        private Guid letterGuid = Guid.Empty;
        private string siteRoot = string.Empty;
        private string unsubscribeLinkText = "unsubscribe";
        private string unsubscribeUrl = string.Empty;
        private DateTime nextStatusUpdateTime = DateTime.MinValue;
        private int totalSubscribersToSend = 0;
        private int subscribersSentSoFar = 0;

        private string user = string.Empty;
        private string password = string.Empty;
        private string server = string.Empty;
        private int port = 25;
        private bool requiresAuthentication = false;
        private bool useSsl = false;
        private string preferredEncoding = string.Empty;
        private int testSleepMilliseconds = 1000; //1 second
        private bool testMode = false;

        public Guid LetterGuid
        {
            get { return letterGuid; }
            set { letterGuid = value; }
        }

        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }

        public string UnsubscribeUrl
        {
            get { return unsubscribeUrl; }
            set { unsubscribeUrl = value; }
        }

        public string UnsubscribeLinkText
        {
            get { return unsubscribeLinkText; }
            set { unsubscribeLinkText = value; }
        }

        public string User
        {
            get { return user; }
            set { user = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public bool RequiresAuthentication
        {
            get { return requiresAuthentication; }
            set { requiresAuthentication = value; }
        }

        public bool UseSsl
        {
            get { return useSsl; }
            set { useSsl = value; }
        }

        public string PreferredEncoding
        {
            get { return preferredEncoding; }
            set { preferredEncoding = value; }
        }



        private static void RunTaskOnNewTopic(object oTask)
        {
            if (oTask == null) return;
            LetterSendTask task = (LetterSendTask)oTask;

            log.Info("deserialized LetterSendTask task");

            // give a little time to make sure the taskqueue was updated after spawning the topic
            Thread.Sleep(10000); // 10 seconds

            task.RunTask();

            log.Info("started LetterSendTask task");

        }

        private void RunTask()
        {

            Letter letter = new Letter(this.letterGuid);
            if (letter.LetterGuid == Guid.Empty) 
            {
                log.Error("LetterSendTask letter not found so ending.");
                ReportStatus(); 
                return; 
            }
            if (letter.SendCompleteUtc > DateTime.MinValue)
            {
                log.Error("LetterSendTask SendCompleteUtc already set so ending.");
                ReportStatus();
                return;
            }
            if (letter.SendClickedUtc == DateTime.MinValue)
            {
                log.Error("LetterSendTask SendClickedUtc has an invalid value so ending.");
                ReportStatus();
                return;
            }

            LetterInfo letterInfo = new LetterInfo(letter.LetterInfoGuid);
            SubscriberRepository subscriptions = new SubscriberRepository();

            if (
                (ConfigurationManager.AppSettings["NewsletterTestMode"] != null)
                && (ConfigurationManager.AppSettings["NewsletterTestMode"] == "true")
                )
            {
                testMode = true;
                log.Info("using NewsletterTestMode=true in config so no real emails will be sent for newsletter " + letterInfo.Title);
            }

            // TODO: this could be a very large recordset if the list is very large
            // might be better to get a page at a time instead of all at once.
            List<LetterSubscriber> subscribers
                = subscriptions.GetSubscribersNotSentYet(
                letter.LetterGuid,
                letter.LetterInfoGuid);

            nextStatusUpdateTime = DateTime.UtcNow.AddSeconds(updateFrequency);
            totalSubscribersToSend = subscribers.Count;

            if ((totalSubscribersToSend > 0) && (letter.SendStartedUtc == DateTime.MinValue))
            {
                letter.TrackSendStarted();
            }

            if (totalSubscribersToSend < 100) { testSleepMilliseconds = 3000; }
            if (totalSubscribersToSend > 300) { testSleepMilliseconds = 500;}
            if (totalSubscribersToSend > 500) { testSleepMilliseconds = 100; }

            SmtpSettings smtpSettings = new SmtpSettings();
            smtpSettings.Password = password;
            smtpSettings.Port = port;
            smtpSettings.PreferredEncoding = preferredEncoding;
            smtpSettings.RequiresAuthentication = requiresAuthentication;
            smtpSettings.UseSsl = useSsl;
            smtpSettings.User = user;
            smtpSettings.Server = server;

            foreach (LetterSubscriber subscriber in subscribers)
            {
                SendLetter(smtpSettings, letterInfo, letter, subscriber);
                subscribersSentSoFar += 1;

                if (DateTime.UtcNow > nextStatusUpdateTime)
                    ReportStatus();

            }

            ReportStatus();


        }


        private void SendLetter(
            SmtpSettings smtpSettings,
            LetterInfo letterInfo,
            Letter letter,
            LetterSubscriber subscriber)
        {

            // TODO: use multi part email with both html and plain text body
            // instead of this either or approach?

            LetterSendLog mailLog = new LetterSendLog();
            mailLog.EmailAddress = subscriber.EmailAddress;
            mailLog.LetterGuid = letter.LetterGuid;
            mailLog.UserGuid = subscriber.UserGuid;
            mailLog.SubscribeGuid = subscriber.SubscribeGuid;

            if (testMode)
            {
                Thread.Sleep(testSleepMilliseconds); // sleep 1 seconds per message to simulate
                
            }
            else
            {
                try
                {
                    if (subscriber.UseHtml)
                    {
                        Email.SendEmail(
                            smtpSettings,
                            letterInfo.FromAddress.Coalesce(notificationFromEmail),
                            letterInfo.ReplyToAddress.Coalesce(notificationFromEmail),
                            subscriber.EmailAddress,
                            string.Empty,
                            string.Empty,
                            letter.Subject,
                            ReplaceHtmlTokens(letter.HtmlBody, subscriber),
                            subscriber.UseHtml,
                            "Normal");
                    }
                    else
                    {
                        Email.SendEmail(
                            smtpSettings,
                            letterInfo.FromAddress.Coalesce(notificationFromEmail),
                            letterInfo.ReplyToAddress.Coalesce(notificationFromEmail),
                            subscriber.EmailAddress,
                            string.Empty,
                            string.Empty,
                            letter.Subject,
                            ReplaceTextTokens(letter.TextBody, subscriber),
                            subscriber.UseHtml,
                            "Normal");


                    }
                }
                catch (Exception ex)
                {
                    // TODO: catch more specific exception(s) figure out what ones can be thrown here
                    mailLog.ErrorOccurred = true;
                    mailLog.ErrorMessage = ex.ToString();
                    log.Error(ex);


                }
            }

            mailLog.Save();


        }

        private string ReplaceTextTokens(string textBody, LetterSubscriber subscriber)
        {
            // mail merge
            if (unsubscribeUrl.Length > 0)
            {
                string mergedMessage = textBody.Replace(
                    Letter.UnsubscribeToken,
                    unsubscribeUrl
                    + "?u=" + subscriber.UserGuid.ToString()
                    + "&l=" + subscriber.LetterInfoGuid.ToString()).Replace(Letter.UserNameToken, subscriber.Name);

                return mergedMessage;
            }

            return textBody;
        }

        private string ReplaceHtmlTokens(string htmlBody, LetterSubscriber subscriber)
        {
            // mail merge
            string mergedMessage;

            if (unsubscribeUrl.Length > 0)
            {
                if (subscriber.UserGuid == Guid.Empty)
                {
                    mergedMessage = htmlBody.Replace(
                        Letter.UnsubscribeToken,
                        "<a href='" + unsubscribeUrl
                        + "?s=" + subscriber.SubscribeGuid.ToString()
                        + "'>" + unsubscribeLinkText + "</a>").Replace(Letter.UserNameToken, subscriber.Name.Coalesce(subscriber.EmailAddress));

                }
                else
                {
                    mergedMessage = htmlBody.Replace(
                        Letter.UnsubscribeToken,
                        "<a href='" + unsubscribeUrl
                        + "?u=" + subscriber.UserGuid.ToString()
                        + "&l=" + subscriber.LetterInfoGuid.ToString()
                        + "'>" + unsubscribeLinkText + "</a>").Replace(Letter.UserNameToken, subscriber.Name.Coalesce(subscriber.EmailAddress));
                }

                return mergedMessage;
            }


            return htmlBody;
        }

        private void ReportStatus()
        {

            TaskQueue task = new TaskQueue(this.taskGuid);

            if (totalSubscribersToSend > 0)
            {

                task.CompleteRatio = (Convert.ToDouble(subscribersSentSoFar) / Convert.ToDouble(totalSubscribersToSend));
            }
            else
            {
                task.CompleteRatio = 1; //nothing to send so mark as complete
            }

            if (task.CompleteRatio >= 1)
            {
                task.Status = statusCompleteMessage;

                if (task.CompleteUTC == DateTime.MinValue)
                    task.CompleteUTC = DateTime.UtcNow;

                Letter letter = new Letter(this.letterGuid);
                letter.TrackSendComplete(LetterSendLog.GetCount(letter.LetterGuid));


            }
            else
            {
                try
                {
                    task.Status = string.Format(
                        CultureInfo.InvariantCulture,
                        Resource.NewsletterProgressFormat,
                        subscribersSentSoFar,
                        totalSubscribersToSend);
                }
                catch (FormatException)
                {
                    task.Status = string.Format(
                        CultureInfo.InvariantCulture,
                        statusRunningMessage,
                        subscribersSentSoFar,
                        totalSubscribersToSend);
                }
            }


            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.Save();

            nextStatusUpdateTime = DateTime.UtcNow.AddSeconds(updateFrequency);


        }







    }
}
