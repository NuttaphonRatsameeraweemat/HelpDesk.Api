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
    [Authorize]
    public class PasswordController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The Password manager provides Password functionality.
        /// </summary>
        private readonly IPasswordBll _password;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="PasswordController" /> class.
        /// </summary>
        /// <param name="login"></param>
        public PasswordController(IPasswordBll password)
        {
            _password = password;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        [Route("ChangePassword")]
        public IActionResult ChangePassword([FromBody]PasswordViewModel model)
        {
            return Ok(_password.ChangePassword(model));
        }
        
        [HttpPost]
        [Route("ForgetPassword")]
        [AllowAnonymous]
        public IActionResult ForgetPassword([FromBody]ForgetPasswordViewModel model)
        {
            return Ok(_password.ForgetPassword(model));
        }

        #endregion

    }
}