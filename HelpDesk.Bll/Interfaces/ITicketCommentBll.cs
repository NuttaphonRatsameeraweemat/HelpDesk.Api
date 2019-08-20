using HelpDesk.Bll.Models;
using HelpDesk.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll.Interfaces
{
    public interface ITicketCommentBll
    {
        /// <summary>
        /// Insert new ticket comment to table.
        /// </summary>
        /// <param name="ticketId">The identity ticket.</param>
        /// <param name="comment">The ticket comment.</param>
        /// <returns></returns>
        ResultViewModel SaveTicketComment(int ticketId, string comment);
        /// <summary>
        /// Get comment by ticket id.
        /// </summary>
        /// <param name="ticketId">The identity ticket.</param>
        /// <returns></returns>
        IEnumerable<TicketCommentViewModel> LoadComment(int ticketId);
    }
}
