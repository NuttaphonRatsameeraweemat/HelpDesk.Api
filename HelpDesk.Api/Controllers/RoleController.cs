using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Bll.Components;
using HelpDesk.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        #region [Fields]

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="RoleController" /> class.
        /// </summary>
        /// <param name="register"></param>
        public RoleController()
        {

        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetRole")]
        public IActionResult GetRole()
        {
            return Ok(new List<ValueHelpViewModel>
            {
                new ValueHelpViewModel{ ValueKey = ConstantValue.UserTypeUser, ValueText = "User" },
                new ValueHelpViewModel{ ValueKey = ConstantValue.UserTypeAdmin, ValueText = "Admin" }
            });
        }

        #endregion
    }
}