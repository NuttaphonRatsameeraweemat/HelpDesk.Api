using AutoMapper;
using HelpDesk.Bll.Components;
using HelpDesk.Bll.Components.Interfaces;
using HelpDesk.Bll.Interfaces;
using HelpDesk.Bll.Models;
using HelpDesk.Data.Pocos;
using HelpDesk.Data.Repository.Interfaces;
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
            return result;
        }

        /// <summary>
        /// Get comment by ticket id.
        /// </summary>
        /// <param name="ticketId">The identity ticket.</param>
        /// <returns></returns>
        public IEnumerable<TicketCommentViewModel> LoadComment(int ticketId)
        {
            var result = new List<TicketCommentViewModel>();
            var comments = _unitOfWork.GetRepository<TicketComment>().Get(x => x.TicketId == ticketId, x => x.OrderBy(y => y.Id));
            var customer = _unitOfWork.GetRepository<Customer>().GetCache();
            foreach (var item in comments)
            {
                var temp = customer.FirstOrDefault(x => x.Email == item.CommentBy);
                var commentItem = new TicketCommentViewModel
                {
                    Id = item.Id,
                    Comment = item.Comment,
                    CommentByName = string.Format(ConstantValue.EmpTemplate, temp?.FirstNameEn, temp?.LastNameEn),
                    CommentDate = item.CommentDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                };
                if (item.CommentBy == _token.Email)
                {
                    commentItem.IsOwner = true;
                }
                result.Add(commentItem);
            }
            return result;
        }

        #endregion

    }
}
