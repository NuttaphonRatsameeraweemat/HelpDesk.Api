using HelpDesk.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll.Interfaces
{
    public interface ITicketTransectionBll
    {
        /// <summary>
        /// Get Ticket available time.
        /// </summary>
        /// <param name="ticketId">The identity ticket.</param>
        /// <returns></returns>
        int GetTime(int ticketId);
        /// <summary>
        /// Insert new ticket transection to table.
        /// </summary>
        /// <param name="ticketId">The identity ticket.</param>
        /// <param name="status">The ticket status in transection.</param>
        /// <returns></returns>
        ResultViewModel SaveTicketTransection(int ticketId, string status);
        /// <summary>
        /// Close old ticket transection and insert new transection.
        /// </summary>
        /// <param name="ticketId">The identity ticket.</param>
        /// <param name="oldStatus">The old ticket status transection.</param>
        /// <param name="newStatus">The new ticket status transection.</param>
        /// <returns></returns>
        ResultViewModel UpdateTicketStatus(int ticketId, string oldStatus, string newStatus);
    }
}
