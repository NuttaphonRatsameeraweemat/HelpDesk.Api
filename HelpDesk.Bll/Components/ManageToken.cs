using HelpDesk.Bll.Components.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll.Components
{
    public class ManageToken : IManageToken
    {

        #region [Fields]

        /// <summary>
        /// The httpcontext.
        /// </summary>
        private readonly HttpContext _httpContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageToken" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The httpcontext value.</param>
        public ManageToken(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Ad User from payload token.
        /// </summary>
        public string Email => _httpContext.User.Identity.Name;

        #endregion

    }
}
