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
    }
}
