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
    public class TicketController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The ticket provides ticket functionality.
        /// </summary>
        private readonly ITicketBll _ticket;
        /// <summary>
        /// The ticket comment provides ticket comment functionality.
        /// </summary>
        private readonly ITicketCommentBll _ticketComment;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="TicketController" /> class.
        /// </summary>
        /// <param name="login"></param>
        public TicketController(ITicketBll ticket, ITicketCommentBll ticketComment)
        {
            _ticket = ticket;
            _ticketComment = ticketComment;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_ticket.GetList());
        }

        [HttpGet]
        [Route("GetAllTicket")]
        public IActionResult GetAllTicket()
        {
            return Ok(_ticket.GetAllTicket());
        }

        [HttpGet]
        [Route("GetTicketComment")]
        public IActionResult GetTicketComment(int ticketId)
        {
            return Ok(_ticketComment.LoadComment(ticketId));
        }

        [HttpPost]
        [Route("OpenTicket")]
        public IActionResult OpenTicket([FromBody]TicketViewModel ticketDetail)
        {
            return Ok(_ticket.OpenTicket(ticketDetail));
        }

        [HttpPost]
        [Route("UpdateTicket")]
        public IActionResult UpdateTicket([FromBody]TicketViewModel ticketDetail)
        {
            return Ok(_ticket.UpdateTicketStatus(ticketDetail));
        }

        #endregion

    }
}