using HelpDesk.Bll.Models;
using HelpDesk.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll.Interfaces
{
    public interface ITicketBll
    {
        /// <summary>
        /// Get Ticket List.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TicketViewModel> GetList();
        /// <summary>
        /// Get Ticket List by company code.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TicketViewModel> GetCompanyTicket();
        /// <summary>
        /// Get All ticket list in system.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TicketViewModel> GetAllTicket();
        /// <summary>
        /// Insert new ticket to table.
        /// </summary>
        /// <param name="model">The ticket information.</param>
        /// <returns></returns>
        ResultViewModel OpenTicket(TicketViewModel model);
        /// <summary>
        /// Update Ticket status.
        /// </summary>
        /// <param name="model">The ticket information.</param>
        /// <returns></returns>
        ResultViewModel UpdateTicketStatus(TicketViewModel model);
    }
}
