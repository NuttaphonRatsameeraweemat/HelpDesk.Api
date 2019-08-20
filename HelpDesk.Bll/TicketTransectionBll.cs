using AutoMapper;
using HelpDesk.Bll.Components;
using HelpDesk.Bll.Components.Interfaces;
using HelpDesk.Bll.Interfaces;
using HelpDesk.Data.Pocos;
using HelpDesk.Data.Repository.Interfaces;
using HelpDesk.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace HelpDesk.Bll
{
    public class TicketTransectionBll : ITicketTransectionBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketTransectionBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public TicketTransectionBll(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Insert new ticket transection to table.
        /// </summary>
        /// <param name="ticketId">The identity ticket.</param>
        /// <param name="status">The ticket status in transection.</param>
        /// <returns></returns>
        public ResultViewModel SaveTicketTransection(int ticketId, string status)
        {
            var result = new ResultViewModel();
            var data = new TicketTransection
            {
                TicketId = ticketId,
                Status = status,
                StartDate = DateTime.Now
            };
            _unitOfWork.GetRepository<TicketTransection>().Add(data);
            _unitOfWork.Complete();
            return result;
        }

        /// <summary>
        /// Close old ticket transection and insert new transection.
        /// </summary>
        /// <param name="ticketId">The identity ticket.</param>
        /// <param name="oldStatus">The old ticket status transection.</param>
        /// <param name="newStatus">The new ticket status transection.</param>
        /// <returns></returns>
        public ResultViewModel UpdateTicketStatus(int ticketId, string oldStatus, string newStatus)
        {
            var result = new ResultViewModel();
            this.UpdateTicket(ticketId, oldStatus);
            this.SaveTicketTransection(ticketId, newStatus);
            if (newStatus == ConstantValue.TicketStatusClose)
            {
                this.UpdateTicket(ticketId, newStatus);
            }
            return result;
        }

        /// <summary>
        /// Clsoe old ticket transection.
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="oldStatus"></param>
        private void UpdateTicket(int ticketId, string oldStatus)
        {
            var oldTransection = _unitOfWork.GetRepository<TicketTransection>().Get(
                x => x.TicketId == ticketId && x.Status == oldStatus).FirstOrDefault();
            oldTransection.EndDate = DateTime.Now;
            _unitOfWork.GetRepository<TicketTransection>().Update(oldTransection);
            _unitOfWork.Complete();
        }

        #endregion

    }
}
