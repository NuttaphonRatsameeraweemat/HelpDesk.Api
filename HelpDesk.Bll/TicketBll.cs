using AutoMapper;
using HelpDesk.Bll.Components;
using HelpDesk.Bll.Components.Interfaces;
using HelpDesk.Bll.Interfaces;
using HelpDesk.Bll.Models;
using HelpDesk.Data.Pocos;
using HelpDesk.Data.Repository.Interfaces;
using HelpDesk.Helper;
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
            var data = RedisCacheHandler.GetValue(ConstantValue.TicketInfoKey, () =>
            {
                return this.FuncGetValue().ToList();
            }).Where(x => x.CreateBy == _token.Email).OrderByDescending(x => x.Id);
            foreach (var item in data)
            {
                item.OnlineTime = _ticketTransection.GetTime(item.Id);
            }
            return data;
        }

        /// <summary>
        /// Get Ticket List by company code.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TicketViewModel> GetCompanyTicket()
        {
            var data = RedisCacheHandler.GetValue(ConstantValue.TicketInfoKey, () =>
            {
                return this.FuncGetValue().ToList();
            }).Where(x => x.CompanyCode == _token.ComCode).OrderByDescending(x => x.Id);
            foreach (var item in data)
            {
                item.OnlineTime = _ticketTransection.GetTime(item.Id);
            }
            return data;
        }

        /// <summary>
        /// Get Ticket List by assign to code.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TicketViewModel> GetAssignTicket()
        {
            var data = RedisCacheHandler.GetValue(ConstantValue.TicketInfoKey, () =>
            {
                return this.FuncGetValue().ToList();
            }).Where(x => x.AssignTo == _token.ComCode).OrderByDescending(x => x.Id);
            foreach (var item in data)
            {
                item.OnlineTime = _ticketTransection.GetTime(item.Id);
            }
            return data;
        }

        /// <summary>
        /// Get All ticket list in system.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TicketViewModel> GetAllTicket()
        {
            var data = RedisCacheHandler.GetValue(ConstantValue.TicketInfoKey, () =>
            {
                return this.FuncGetValue().ToList();
            }).OrderByDescending(x => x.Id);
            foreach (var item in data)
            {
                item.OnlineTime = _ticketTransection.GetTime(item.Id);
            }
            return data;
        }

        /// <summary>
        /// Initial Ticket view model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private IEnumerable<TicketViewModel> InitialTicketViewModel(IEnumerable<TicketViewModel> model)
        {
            var priority = _unitOfWork.GetRepository<Priority>().GetCache();
            var customerInfo = _unitOfWork.GetRepository<Customer>().GetCache();
            var valueHelp = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeTicketType);
            foreach (var item in model)
            {
                var tempPriority = priority.FirstOrDefault(x => x.Id == item.PriorityId);
                var tempCustomer = customerInfo.FirstOrDefault(x => x.Email == item.CreateBy);
                var ticketType = valueHelp.FirstOrDefault(x => x.ValueKey == item.TicketType);
                item.PriorityName = tempPriority?.PriorityName;
                item.TicketTypeName = ticketType?.ValueText;
                item.CreateName = string.Format(ConstantValue.EmpTemplate, tempCustomer?.FirstNameEn, tempCustomer?.LastNameEn);
            }
            return model;
        }

        /// <summary>
        /// Initial Ticket view model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private TicketViewModel InitialTicketViewModel(TicketViewModel model)
        {
            var priority = _unitOfWork.GetRepository<Priority>().GetCache(x => x.Id == model.PriorityId).FirstOrDefault();
            var customerInfo = _unitOfWork.GetRepository<Customer>().GetCache(x => x.Email == model.CreateBy).FirstOrDefault();
            var valueHelp = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeTicketType && x.ValueKey == model.TicketType).FirstOrDefault();
            model.PriorityName = priority?.PriorityName;
            model.TicketTypeName = valueHelp?.ValueText;
            model.CreateName = string.Format(ConstantValue.EmpTemplate, customerInfo?.FirstNameEn, customerInfo?.LastNameEn);
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
                this.SaveRedisCacheTicket(data);
                _ticketTransection.SaveTicketTransection(data.Id, data.Status);
                _unitOfWork.Complete(scope);
            }
            result = this.SendEmailOpenTicket(model);
            return result;
        }

        /// <summary>
        /// Save new ticket to redis cache.
        /// </summary>
        /// <param name="data">The ticket information.</param>
        private void SaveRedisCacheTicket(Ticket data)
        {
            var ticketList = RedisCacheHandler.GetValue(ConstantValue.TicketInfoKey, () =>
            {
                return this.FuncGetValue().ToList();
            });
            ticketList.Add(this.InitialTicketViewModel(_mapper.Map<Ticket, TicketViewModel>(data)));
            RedisCacheHandler.SetValue(ConstantValue.TicketInfoKey, ticketList);
        }

        /// <summary>
        /// Function get value to redis cache.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<TicketViewModel> FuncGetValue()
        {
            return this.InitialTicketViewModel(_mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketViewModel>>(
                _unitOfWork.GetRepository<Ticket>().Get(orderBy: x => x.OrderBy(y => y.Id))));
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
            var data = _unitOfWork.GetRepository<Ticket>().GetById(model.Id);
            var ticketInfo = _mapper.Map<Ticket, TicketViewModel>(data);
            using (TransactionScope scope = new TransactionScope())
            {
                _ticketTransection.UpdateTicketStatus(data.Id, data.Status, model.Status);
                if (!string.IsNullOrEmpty(model.Comment))
                {
                    _ticketComment.SaveTicketComment(data.Id, model.Comment);
                }
                data.Status = model.Status;
                receiver = this.GetEmailReceiver(data.CreateBy);
                emailDear = this.GetDearEmailBody(data.CreateBy);
                _unitOfWork.GetRepository<Ticket>().Update(data);
                this.UpdateRedisCacheTicket(model);
                _unitOfWork.Complete(scope);
            }
            result = this.SendEmailUpdateTicket(ticketInfo, model.Comment, receiver, emailDear, model.Status);
            return result;
        }

        /// <summary>
        /// Save new ticket to redis cache.
        /// </summary>
        /// <param name="data">The ticket information.</param>
        private void UpdateRedisCacheTicket(TicketViewModel model)
        {
            var ticketList = RedisCacheHandler.GetValue(ConstantValue.TicketInfoKey, () =>
            {
                return this.FuncGetValue().ToList();
            });
            var item = ticketList.FirstOrDefault(x => x.Id == model.Id);
            item.Status = model.Status;
            RedisCacheHandler.SetValue(ConstantValue.TicketInfoKey, ticketList);
        }

        /// <summary>
        /// Send Notification Email Open new ticket issue.
        /// </summary>
        /// <param name="model"></param>
        private ResultViewModel SendEmailOpenTicket(TicketViewModel model)
        {
            var result = new ResultViewModel();
            string status = ConstantValue.EmailSendingError;
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
                _emailService.SendEmailNotificationTemplate(emailModel, model.TicketNo, _token.FullName, model.TicketName, model.Description);
                status = ConstantValue.EmailSendingComplete;
            }
            catch (Exception)
            {
                result.Message = ConstantValue.EmailCannotSending;
            }
            this.SaveEmailTask(_mapper.Map<EmailModel, EmailTask>(emailModel), status);
            return result;
        }

        /// <summary>
        /// Save the email task
        /// </summary>
        /// <param name="email">The email information value.</param>
        /// <param name="status">The eamil status sendding or not.</param>
        private void SaveEmailTask(EmailTask email, string status)
        {
            email.Status = status;
            email.CreateDate = DateTime.Now;
            _unitOfWork.GetRepository<EmailTask>().Add(email);
            _unitOfWork.Complete();
        }

        /// <summary>
        /// Send Notification Email Update ticket issue.
        /// </summary>
        /// <param name="model"></param>
        private ResultViewModel SendEmailUpdateTicket(TicketViewModel model, string comment, string receiver, string emailDear, string ticketStatus)
        {
            var result = new ResultViewModel();
            string status = ConstantValue.EmailSendingError;
            var emailModel = new EmailModel
            {
                Sender = _config.SmtpEmail,
                Receiver = receiver,
                Subject = string.Format(ConstantValue.EmailNotificationUpdateTicketSubject, model.TicketNo, ticketStatus),
                Body = string.Format(ConstantValue.EmailNotificationUpdateTicketBody, emailDear,
                                                                                   model.TicketNo,
                                                                                   model.TicketName,
                                                                                   model.Description,
                                                                                   this.GetPriorityText(model.PriorityId),
                                                                                   ticketStatus,
                                                                                   _token.FullName,
                                                                                   comment)
            };
            try
            {
                _emailService.SendEmailNotificationTemplate(emailModel, model.TicketNo, _token.FullName, model.TicketName, model.Description);
                status = ConstantValue.EmailSendingComplete;
            }
            catch (Exception)
            {
                result.Message = ConstantValue.EmailCannotSending;
            }
            this.SaveEmailTask(_mapper.Map<EmailModel, EmailTask>(emailModel), status);
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
