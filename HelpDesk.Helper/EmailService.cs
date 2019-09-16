using HelpDesk.Helper.Interfaces;
using HelpDesk.Helper.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace HelpDesk.Helper
{
    public class EmailService : IEmailService
    {

        #region [Fields]

        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService" /> class.
        /// </summary>
        /// <param name="config">The config value.</param>
        public EmailService(IConfigSetting config)
        {
            _config = config;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Send email without specified template.  
        /// </summary>
        /// <param name="email">Email detail.</param>
        public void SendEmail(EmailModel email)
        {
            //Call method to send the email.
            this.SendTheEmail(email);
        }
        /// <summary>
        /// Send email with specified template.
        /// </summary>
        /// <param name="email">Email detail.</param>
        public void SendEmailForgotPasswordTemplate(EmailModel email)
        {
            //Call method to send the email.
            string htmlTemplate = string.Empty;
            string path = Directory.GetCurrentDirectory() + @"\EmailTemplate\ForgotPassword.html";
            using (var stream = new System.IO.StreamReader(path))
            {
                htmlTemplate = stream.ReadToEnd();
            }
            email.Body = htmlTemplate.Replace("%NEWPASSWORD%", email.Body);
            this.SendTheEmail(email);
        }
        /// <summary>
        /// Send email with specified template.
        /// </summary>
        /// <param name="email">Email detail.</param>
        public void SendEmailNotificationTemplate(EmailModel email, string ticketNo, string from, string ticketName, string description)
        {
            //Call method to send the email.
            string htmlTemplate = string.Empty;
            string path = Directory.GetCurrentDirectory() + @"\EmailTemplate\Notification.html";
            using (var stream = new System.IO.StreamReader(path))
            {
                htmlTemplate = stream.ReadToEnd();
            }
            htmlTemplate = htmlTemplate.Replace("%TICKETNO%", ticketNo);
            htmlTemplate = htmlTemplate.Replace("%FROM%", from);
            htmlTemplate = htmlTemplate.Replace("%TICKETNAME%", ticketName);
            htmlTemplate = htmlTemplate.Replace("%TICKETDESCRIPTION%", description);
            email.Body = htmlTemplate;
            this.SendTheEmail(email);
        }
        /// <summary>
        /// Send the email
        /// </summary>
        /// <param name="email">Email detail</param>
        private void SendTheEmail(EmailModel email)
        {
            //Get email configuration
            var smtpHost = _config.SmtpHost;
            var smtpPort = _config.SmtpPort;
            var requireCredential = _config.SmtpRequireCredential;
            var enableSSL = _config.SmtpEnableSSL;
            var user = _config.SmtpUser;
            var password = _config.SmtpPassword;

            SmtpClient client = new SmtpClient(smtpHost, int.Parse(smtpPort))
            {
                EnableSsl = Convert.ToBoolean(enableSSL),
                UseDefaultCredentials = false
            };
            if (requireCredential == "true")
            {
                client.Credentials = new NetworkCredential(user, password);
            }
            //Create an email.
            MailMessage mailItem = new MailMessage
            {
                From = new MailAddress(email.Sender) // must use organization email
            };
            mailItem.To.Add(email.Receiver);
            mailItem.Subject = email.Subject;
            mailItem.IsBodyHtml = true;
            mailItem.Body = email.Body;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                //Send an email 
                client.Send(mailItem);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message + ", Send :" + email.Sender + "Receiver :" + email.Receiver);
            }
        }

        #endregion

    }
}
