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
    public class CompanyController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The Company manager provides Company functionality.
        /// </summary>
        private readonly ICompanyBll _company;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="CompanyController" /> class.
        /// </summary>
        /// <param name="login"></param>
        public CompanyController(ICompanyBll company)
        {
            _company = company;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_company.GetList());
        }

        #endregion

    }
}