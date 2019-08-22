using AutoMapper;
using HelpDesk.Bll.Components;
using HelpDesk.Bll.Components.Interfaces;
using HelpDesk.Bll.Interfaces;
using HelpDesk.Bll.Models;
using HelpDesk.Data.Pocos;
using HelpDesk.Data.Repository.Interfaces;
using HelpDesk.Helper.Interfaces;
using HelpDesk.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace HelpDesk.Bll
{
    public class TicketBll : ITicketBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// The auto mapper.
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;
        /// <summary>
        /// The ticket comment provides ticket comment functionality.
        /// </summary>
        private readonly ITicketCommentBll _ticketComment;
        /// <summary>
        /// The ticket transection provides ticket transection functionality.
        /// </summary>
        private readonly ITicketTransectionBll _ticketTransection;
        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;
        /// <summary>
        /// The email service provides email functionality.
        /// </summary>
        private readonly IEmailService _emailService;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public TicketBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token,
            ITicketCommentBll ticketComment, ITicketTransectionBll ticketTransection, IConfigSetting config, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
            _ticketComment = ticketComment;
            _ticketTransection = ticketTransection;
            _config = config;
            _emailService = emailService;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Ticket List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TicketViewModel> GetList()
        {
            return this.InitialTicketViewModel(_mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketViewModel>>(
                _unitOfWork.GetRepository<Ticket>().Get(x => x.CreateBy == _token.Email)));
        }

        /// <summary>
        /// Get Ticket List by company code.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TicketViewModel> GetCompanyTicket()
        {
            return this.InitialTicketViewModel(_mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketViewModel>>(
                _unitOfWork.GetRepository<Ticket>().Get(x => x.CompanyCode == _token.ComCode)));
        }

        /// <summary>
        /// Get All ticket list in system.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TicketViewModel> GetAllTicket()
        {
            return this.InitialTicketViewModel(_mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketViewModel>>(
                _unitOfWork.GetRepository<Ticket>().Get()));
        }

        /// <summary>
        /// Initial Ticket view model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private IEnumerable<TicketViewModel> InitialTicketViewModel(IEnumerable<TicketViewModel> model)
        {
            var priority = _unitOfWork.GetRepository<Priority>().GetCache();
            foreach (var item in model)
            {
                var temp = priority.FirstOrDefault(x => x.Id == item.PriorityId);
                item.PriorityName = temp.PriorityName;
            }
            return model;
        }

        /// <summary>
        /// Insert new ticket to table.
        /// </summary>
        /// <param name="model">The ticket information.</param>
        /// <returns></returns>
        public ResultViewModel OpenTicket(TicketViewModel model)
        {
            var result = new ResultViewModel();
            model.TicketNo = this.GenerateTicketNo();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _mapper.Map<TicketViewModel, Ticket>(model);
                data.CreateBy = _token.Email;
                data.CreateDate = DateTime.Now;
                data.CompanyCode = _token.ComCode;
                _unitOfWork.GetRepository<Ticket>().Add(data);
                _unitOfWork.Complete();
                _ticketTransection.SaveTicketTransection(data.Id, data.Status);
                _unitOfWork.Complete(scope);
            }
            result = this.SendEmailOpenTicket(model);
            return result;
        }

        /// <summary>
        /// Update Ticket status.
        /// </summary>
        /// <param name="model">The ticket information.</param>
        /// <returns></returns>
        public ResultViewModel UpdateTicketStatus(TicketViewModel model)
        {
            var result = new ResultViewModel();
            string receiver = string.Empty;
            string emailDear = string.Empty;
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<Ticket>().Get(x => x.Id == model.Id).FirstOrDefault();
                _ticketTransection.UpdateTicketStatus(data.Id, data.Status, model.Status);
                _ticketComment.SaveTicketComment(data.Id, model.Comment);
                data.Status = model.Status;
                receiver = this.GetEmailReceiver(data.CreateBy);
                emailDear = this.GetDearEmailBody(data.CreateBy);
                _unitOfWork.GetRepository<Ticket>().Update(data);
                _unitOfWork.Complete(scope);
            }
            result = this.SendEmailUpdateTicket(model, model.Comment, receiver, emailDear);
            return result;
        }

        /// <summary>
        /// Send Notification Email Open new ticket issue.
        /// </summary>
        /// <param name="model"></param>
        private ResultViewModel SendEmailOpenTicket(TicketViewModel model)
        {
            var result = new ResultViewModel();
            var emailModel = new EmailModel
            {
                Sender = _config.SmtpEmail,
                Receiver = _config.EmailSupport,
                Subject = string.Format(ConstantValue.EmailNotificationNewTicketSubject, model.TicketNo, _token.FullName),
                Body = string.Format(ConstantValue.EmailNotificationNewTicketBody, _token.FullName,
                                                                                   model.TicketNo,
                                                                                   model.TicketName,
                                                                                   model.Description,
                                                                                   this.GetPriorityText(model.PriorityId))
            };
            try
            {
                _emailService.SendEmail(emailModel);
            }
            catch (Exception)
            {
                result.Message = ConstantValue.EmailCannotSending;
            }
            return result;
        }

        /// <summary>
        /// Send Notification Email Update ticket issue.
        /// </summary>
        /// <param name="model"></param>
        private ResultViewModel SendEmailUpdateTicket(TicketViewModel model, string comment, string receiver, string emailDear)
        {
            var result = new ResultViewModel();
            var emailModel = new EmailModel
            {
                Sender = _config.SmtpEmail,
                Receiver = receiver,
                Subject = string.Format(ConstantValue.EmailNotificationUpdateTicketSubject, model.TicketNo, model.Status),
                Body = string.Format(ConstantValue.EmailNotificationUpdateTicketBody, emailDear,
                                                                                   model.TicketNo,
                                                                                   model.TicketName,
                                                                                   model.Description,
                                                                                   this.GetPriorityText(model.PriorityId),
                                                                                   model.Status,
                                                                                   _token.FullName,
                                                                                   comment)
            };
            try
            {
                _emailService.SendEmail(emailModel);
            }
            catch (Exception)
            {
                result.Message = ConstantValue.EmailCannotSending;
            }
            return result;
        }

        /// <summary>
        /// Generate Ticket number.
        /// </summary>
        /// <returns></returns>
        private string GenerateTicketNo()
        {
            return $"TK-{_token.ComCode}-{DateTime.Now.ToString("yyyyMMddHHmmss")}";
        }

        /// <summary>
        /// Get Priority Text to display.
        /// </summary>
        /// <param name="priorityId">The identity of priority.</param>
        /// <returns></returns>
        private string GetPriorityText(int priorityId)
        {
            return _unitOfWork.GetRepository<Priority>().GetCache(x => x.Id == priorityId).FirstOrDefault()?.PriorityName;
        }

        /// <summary>
        /// Get Eamil receiver ticket notification.
        /// </summary>
        /// <param name="ticketOwner">The ticket owner email.</param>
        /// <returns></returns>
        private string GetEmailReceiver(string ticketOwner)
        {
            string result = string.Empty;
            if (ticketOwner == _token.Email)
            {
                result = _config.EmailSupport;
            }
            else result = ticketOwner;
            return result;
        }

        /// <summary>
        /// Get Eamil receiver ticket notification.
        /// </summary>
        /// <param name="ticketOwner">The ticket owner email.</param>
        /// <returns></returns>
        private string GetDearEmailBody(string ticketOwner)
        {
            string result = string.Empty;
            if (ticketOwner == _token.Email)
            {
                result = "SupportTeam";
            }
            else result = this.GetCustomerName(ticketOwner);
            return result;
        }

        /// <summary>
        /// Get customer name.
        /// </summary>
        /// <param name="email">The customer email.</param>
        /// <returns></returns>
        private string GetCustomerName(string email)
        {
            var customer = _unitOfWork.GetRepository<Customer>().GetCache(x => x.Email == email).FirstOrDefault();
            return string.Format(ConstantValue.EmpTemplate, customer.FirstNameEn, customer.LastNameEn);
        }

        #endregion

    }
}
