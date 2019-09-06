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

namespace HelpDesk.Bll
{
    public class TicketCommentBll : ITicketCommentBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketCommentBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public TicketCommentBll(IUnitOfWork unitOfWork, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Insert new ticket comment to table.
        /// </summary>
        /// <param name="ticketId">The identity ticket.</param>
        /// <param name="comment">The ticket comment.</param>
        /// <returns></returns>
        public ResultViewModel SaveTicketComment(int ticketId, string comment)
        {
            var result = new ResultViewModel();
            var data = new TicketComment
            {
                TicketId = ticketId,
                Comment = comment,
                CommentBy = _token.Email,
                CommentDate = DateTime.Now
            };
            _unitOfWork.GetRepository<TicketComment>().Add(data);
            _unitOfWork.Complete();
            this.SaveRedisCacheTicketComments(data);
            return result;
        }

        /// <summary>
        /// Save new ticket comment to redis cache.
        /// </summary>
        /// <param name="data">The ticket information.</param>
        private void SaveRedisCacheTicketComments(TicketComment data)
        {
            var ticketList = RedisCacheHandler.GetValue(ConstantValue.TicketCommentKey, () =>
            {
                return this.FuncGetValue().ToList();
            });
            ticketList.Add(this.InitialTicketCommentViewModel(data));
            RedisCacheHandler.SetValue(ConstantValue.TicketCommentKey, ticketList);
        }

        /// <summary>
        /// Function get value to redis cache.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<TicketCommentViewModel> FuncGetValue()
        {
            return this.InitialTicketCommentViewModel(_unitOfWork.GetRepository<TicketComment>().Get(orderBy: x => x.OrderBy(y => y.Id)));
        }

        /// <summary>
        /// Initial Mapping ticket comment to viewmodel.
        /// </summary>
        /// <param name="ticketComments">The ticket comment.</param>
        /// <returns></returns>
        private IEnumerable<TicketCommentViewModel> InitialTicketCommentViewModel(IEnumerable<TicketComment> ticketComments)
        {
            var result = new List<TicketCommentViewModel>();
            var customer = _unitOfWork.GetRepository<Customer>().GetCache();
            foreach (var item in ticketComments)
            {
                var temp = customer.FirstOrDefault(x => x.Email == item.CommentBy);
                var commentItem = new TicketCommentViewModel
                {
                    Id = item.Id,
                    TicketId = item.TicketId.Value,
                    Comment = item.Comment,
                    CommentBy = item.CommentBy,
                    CommentByName = string.Format(ConstantValue.EmpTemplate, temp?.FirstNameEn, temp?.LastNameEn),
                    CommentDate = item.CommentDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                };
                result.Add(commentItem);
            }
            return result;
        }

        /// <summary>
        /// Initial Mapping ticket comment to viewmodel.
        /// </summary>
        /// <param name="ticketComment">The ticket comment.</param>
        /// <returns></returns>
        private TicketCommentViewModel InitialTicketCommentViewModel(TicketComment ticketComment)
        {
            var customer = _unitOfWork.GetRepository<Customer>().GetCache(x => x.Email == ticketComment.CommentBy).FirstOrDefault();
            return new TicketCommentViewModel
            {
                Id = ticketComment.Id,
                TicketId = ticketComment.TicketId.Value,
                Comment = ticketComment.Comment,
                CommentBy = ticketComment.CommentBy,
                CommentByName = string.Format(ConstantValue.EmpTemplate, customer?.FirstNameEn, customer?.LastNameEn),
                CommentDate = ticketComment.CommentDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }

        /// <summary>
        /// Get comment by ticket id.
        /// </summary>
        /// <param name="ticketId">The identity ticket.</param>
        /// <returns></returns>
        public IEnumerable<TicketCommentViewModel> LoadComment(int ticketId)
        {
            var ticketList = RedisCacheHandler.GetValue(ConstantValue.TicketCommentKey, () =>
            {
                return this.FuncGetValue().ToList();
            }).Where(x => x.TicketId == ticketId).OrderBy(y => y.Id);
            foreach (var item in ticketList)
            {
                if (item.CommentBy == _token.Email)
                {
                    item.IsOwner = true;
                }
            }
            return ticketList;
        }

        #endregion

    }
}
