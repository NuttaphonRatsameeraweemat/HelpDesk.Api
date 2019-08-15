using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Bll.Interfaces;
using HelpDesk.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The Login manager provides Login functionality.
        /// </summary>
        private readonly ILoginBll _login;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="LoginController" /> class.
        /// </summary>
        /// <param name="login"></param>
        public LoginController(ILoginBll login)
        {
            _login = login;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody]LoginViewModel auth)
        {
            return Ok();
        }

        #endregion

    }
}