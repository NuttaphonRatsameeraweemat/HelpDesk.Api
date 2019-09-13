using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HelpDesk.Bll.Components;
using HelpDesk.Bll.Components.Interfaces;
using HelpDesk.Bll.Interfaces;
using HelpDesk.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CacheController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The CacheManagement provides CacheManagement functionality.
        /// </summary>
        private readonly ICacheManagement _cacheManagement;
        /// <summary>
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="CacheController" /> class.
        /// </summary>
        /// <param name="cacheManagement"></param>
        public CacheController(ICacheManagement cacheManagement, IManageToken token)
        {
            _cacheManagement = cacheManagement;
            _token = token;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("ReloadAllCache")]
        public IActionResult ReloadAllCache()
        {
            if (_token.UserType != ConstantValue.UserTypeAdmin)
            {
                return Unauthorized(UtilityService.InitialResultError(null, (int)HttpStatusCode.Unauthorized));
            }
            else _cacheManagement.ReloadAllCache();
            return Ok();
        }

        #endregion

    }
}