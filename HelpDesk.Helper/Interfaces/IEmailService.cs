using HelpDesk.Helper.Models;

namespace HelpDesk.Helper.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Send email without specified template.  
        /// </summary>
        /// <param name="email">Email detail.</param>
        void SendEmail(EmailModel email);
        /// <summary>
        /// Send email with specified template.
        /// </summary>
        /// <param name="email">Email detail.</param>
        void SendEmailForgotPasswordTemplate(EmailModel email);
        /// <summary>
        /// Send email with specified template.
        /// </summary>
        /// <param name="email">Email detail.</param>
        void SendEmailNotificationTemplate(EmailModel email, string ticketNo, string from);
    }
}
