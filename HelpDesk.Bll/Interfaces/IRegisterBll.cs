using HelpDesk.Bll.Models;
using HelpDesk.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll.Interfaces
{
    public interface IRegisterBll
    {

        /// <summary>
        /// The Register new employee function.
        /// </summary>
        /// <param name="formData">The employee data.</param>
        /// <returns></returns>
        ResultViewModel Register(RegisterViewModel formData);
        /// <summary>
        /// Validate Email is already have in database or not.
        /// </summary>
        /// <param name="email">The employee email.</param>
        /// <returns></returns>
        ResultViewModel ValidateEmail(string email);

    }
}
