using HelpDesk.Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class PriorityController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The Priority manager provides Priority functionality.
        /// </summary>
        private readonly IPriorityBll _priority;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="PriorityController" /> class.
        /// </summary>
        /// <param name="login"></param>
        public PriorityController(IPriorityBll priority)
        {
            _priority = priority;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_priority.GetList());
        }

        #endregion

    }
}