using AutoMapper;
using HelpDesk.Bll.Components;
using HelpDesk.Bll.Components.Interfaces;
using HelpDesk.Bll.Interfaces;
using HelpDesk.Bll.Models;
using HelpDesk.Data.Pocos;
using HelpDesk.Data.Repository.Interfaces;
using HelpDesk.Helper;
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
        /// Get Ticket available time.
        /// </summary>
        /// <param name="ticketId">The identity ticket.</param>
        /// <returns></returns>
        public int GetTime(int ticketId)
        {
            int result = 0;
            var transections = RedisCacheHandler.GetValue(ConstantValue.TicketTransectionKey + ticketId.ToString(), () =>
            {
                return this.FuncGetValue(ticketId).ToList();
            });
            foreach (var item in transections)
            {
                if (item.Status == ConstantValue.TicketStatusWaiting || item.Status == ConstantValue.TicketStatusClose ||
                    item.Status == ConstantValue.TicketStatusGetReq)
                {
                    continue;
                }
                var endTime = DateTime.Now;
                if (item.EndDate.HasValue)
                {
                    endTime = item.EndDate.Value;
                }

                var diffTime = endTime - item.StartDate.Value;
                result = Convert.ToInt32(diffTime.TotalMinutes);
            }
            return result;
        }

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
            this.SaveRedisCacheTicketTransection(data);
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
                x => x.TicketId == ticketId && x.Status == oldStatus && !x.EndDate.HasValue).FirstOrDefault();
            oldTransection.EndDate = DateTime.Now;
            _unitOfWork.GetRepository<TicketTransection>().Update(oldTransection);
            this.UpdateRedisCacheTicketTransection(oldTransection);
            _unitOfWork.Complete();
        }

        /// <summary>
        /// Save new ticket comment to redis cache.
        /// </summary>
        /// <param name="data">The ticket information.</param>
        private void SaveRedisCacheTicketTransection(TicketTransection data)
        {
            var ticketList = RedisCacheHandler.GetValue(ConstantValue.TicketTransectionKey + data.TicketId.ToString(), () =>
            {
                return this.FuncGetValue(data.TicketId.Value).ToList();
            });
            ticketList.Add(data);
            RedisCacheHandler.SetValue(ConstantValue.TicketTransectionKey + data.TicketId.ToString(), ticketList);
        }

        /// <summary>
        /// Function get value to redis cache.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<TicketTransection> FuncGetValue(int ticketId)
        {
            return _unitOfWork.GetRepository<TicketTransection>().Get(x => x.TicketId == ticketId, x => x.OrderBy(y => y.Id));
        }

        /// <summary>
        /// Update ticket to redis cache.
        /// </summary>
        /// <param name="data">The ticket information.</param>
        private void UpdateRedisCacheTicketTransection(TicketTransection model)
        {
            var ticketList = RedisCacheHandler.GetValue(ConstantValue.TicketTransectionKey + model.TicketId.ToString(), () =>
            {
                return this.FuncGetValue(model.TicketId.Value).ToList();
            });
            var item = ticketList.FirstOrDefault(x => x.Id == model.Id);
            item.EndDate = model.EndDate;
            RedisCacheHandler.SetValue(ConstantValue.TicketTransectionKey + model.TicketId.ToString(), ticketList);
        }

        #endregion

    }
}
