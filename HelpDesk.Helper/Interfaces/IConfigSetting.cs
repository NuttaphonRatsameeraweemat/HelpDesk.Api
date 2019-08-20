using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Helper.Interfaces
{
    public interface IConfigSetting
    {
        /// <summary>
        /// Get SMTP Host Url.
        /// </summary>
        string SmtpHost { get; }
        /// <summary>
        /// Get SMTP Port.
        /// </summary>
        string SmtpPort { get; }
        /// <summary>
        /// Get RequireCredential SMTP.
        /// </summary>
        string SmtpRequireCredential { get; }
        /// <summary>
        /// Get SMTP EnableSSL.
        /// </summary>
        string SmtpEnableSSL { get; }
        /// <summary>
        /// Get User Authenticate SMTP Server.
        /// </summary>
        string SmtpUser { get; }
        /// <summary>
        /// Get Password Authenticate SMTP Server.
        /// </summary>
        string SmtpPassword { get; }
        /// <summary>
        /// Get Email Sender.
        /// </summary>
        string SmtpEmail { get; }
        /// <summary>
        /// Get Json Web Token Config Issuer value.
        /// </summary>
        string JwtIssuer { get; }
        /// <summary>
        /// Get Json Web Token Config Key value
        /// </summary>
        string JwtKey { get; }
        /// <summary>
        /// Get Email Support for notification when ticket open or update.
        /// </summary>
        string EmailSupport { get; }
    }
}
