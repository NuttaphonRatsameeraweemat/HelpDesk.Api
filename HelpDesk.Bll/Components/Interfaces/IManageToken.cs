using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll.Components.Interfaces
{
    public interface IManageToken
    {
        /// <summary>
        /// Get email from payload token.
        /// </summary>
        string Email { get; }
        /// <summary>
        /// Get company code from payload token.
        /// </summary>
        string ComCode { get; }
        /// <summary>
        /// Get Full Name from payload token.
        /// </summary>
        string FullName { get; }
        /// <summary>
        /// Get user type from payload token.
        /// </summary>
        string UserType { get; }
    }
}
