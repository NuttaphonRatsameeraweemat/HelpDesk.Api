using HelpDesk.Helper.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HelpDesk.Helper
{
    public class ConfigSetting : IConfigSetting
    {

        #region [Fields]

        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfiguration _config;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="AdService" /> class.
        /// </summary>
        /// <param name="config">The config value.</param>
        public ConfigSetting(IConfiguration config)
        {
            _config = config;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Read value in appsetting config with key name;
        /// </summary>
        /// <param name="name">The key name parameter in appsetting.</param>
        /// <returns></returns>
        private string GetAppSetting(string name) => _config[name];
        
        /// <summary>
        /// Get SMTP Host Url.
        /// </summary>
        public string SmtpHost => this.GetAppSetting("SMTP:Host");
        /// <summary>
        /// Get SMTP Port.
        /// </summary>
        public string SmtpPort => this.GetAppSetting("SMTP:Port");
        /// <summary>
        /// Get RequireCredential SMTP.
        /// </summary>
        public string SmtpRequireCredential => this.GetAppSetting("SMTP:RequireCredential");
        /// <summary>
        /// Get SMTP EnableSSL.
        /// </summary>
        public string SmtpEnableSSL => this.GetAppSetting("SMTP:EnableSSL");
        /// <summary>
        /// Get User Authenticate SMTP Server.
        /// </summary>
        public string SmtpUser => this.GetAppSetting("SMTP:User");
        /// <summary>
        /// Get Password Authenticate SMTP Server.
        /// </summary>
        public string SmtpPassword => this.GetAppSetting("SMTP:Password");
        /// <summary>
        /// Get Json Web Token Config Issuer value.
        /// </summary>
        public string JwtIssuer => this.GetAppSetting("Jwt:Issuer");
        /// <summary>
        /// Get Json Web Token Config Key value
        /// </summary>
        public string JwtKey => this.GetAppSetting("Jwt:Key");

        #endregion

    }
}
