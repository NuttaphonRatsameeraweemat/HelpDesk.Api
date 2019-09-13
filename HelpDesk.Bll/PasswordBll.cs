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
using System.Net;
using System.Text;
using System.Transactions;

namespace HelpDesk.Bll
{
    public class PasswordBll : IPasswordBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// The email service provides email functionality.
        /// </summary>
        private readonly IEmailService _emailService;
        /// <summary>
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;
        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;
        /// <summary>
        /// The auto mapper.
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="emailService">The email service provides email functionality.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public PasswordBll(IUnitOfWork unitOfWork, IEmailService emailService, IManageToken token, IConfigSetting config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _token = token;
            _config = config;
            _mapper = mapper;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Change password function.
        /// </summary>
        /// <param name="model">The password data.</param>
        /// <returns></returns>
        public ResultViewModel ChangePassword(PasswordViewModel model)
        {
            var result = new ResultViewModel();
            if (this.ValidatePassword(model.OldPassword))
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    this.SavePassword(_token.Email, model.NewPassword);
                    this.UpdateFirstLogin(_token.Email, false);
                    _unitOfWork.Complete(scope);
                }
                this.ReloadCache();
            }
            else result = UtilityService.InitialResultError(MessageValue.WrongPassword, (int)HttpStatusCode.BadRequest);
            return result;
        }

        /// <summary>
        /// Update new password.
        /// </summary>
        /// <param name="email">The email username.</param>
        /// <param name="newPassword">The new password update.</param>
        private void SavePassword(string email, string newPassword)
        {
            var data = _unitOfWork.GetRepository<Password>().Get(x => x.Email == email).FirstOrDefault();
            var password = new PasswordGenerator(newPassword);
            var newData = new Password { Email = email, Password1 = password.GetHash() };
            _unitOfWork.GetRepository<Password>().Remove(data);
            _unitOfWork.GetRepository<Password>().Add(newData);
        }

        /// <summary>
        /// The Verify Password.
        /// </summary>
        /// <param name="login">The login value.</param>
        /// <returns></returns>
        private bool ValidatePassword(string oldPassword)
        {
            var password = _unitOfWork.GetRepository<Password>().Get(x => x.Email == _token.Email).FirstOrDefault();
            var verifyPassword = new PasswordGenerator(password != null ? password.Password1 : new byte[64]);
            return verifyPassword.Verify(oldPassword);
        }

        /// <summary>
        /// Forget password logic function.
        /// </summary>
        /// <param name="model">The forget password information.</param>
        /// <returns></returns>
        public ResultViewModel ForgetPassword(ForgetPasswordViewModel model)
        {
            var result = new ResultViewModel();
            if (this.ValidateInformation(model))
            {
                string newPassword = CodeGenerator.RandomString(8);
                using (TransactionScope scope = new TransactionScope())
                {
                    this.SavePassword(model.Email, newPassword);
                    this.UpdateFirstLogin(model.Email, true);
                    _unitOfWork.Complete(scope);
                }
                this.ReloadCache();
                result = this.SendEmailForgetPassword(model.Email, newPassword,
                    string.Format(ConstantValue.EmpTemplate, model.FirstNameEn, model.LastNameEn));
            }
            else result = UtilityService.InitialResultError(MessageValue.ForgetPasswordAlert, 200);
            return result;
        }

        /// <summary>
        /// Validate user data information forget password.
        /// </summary>
        /// <param name="model">The forget password information.</param>
        /// <returns></returns>
        private bool ValidateInformation(ForgetPasswordViewModel model)
        {
            var data = _unitOfWork.GetRepository<Customer>().GetCache(x =>
                        x.Email == model.Email &&
                        string.Equals(x.FirstNameEn, model.FirstNameEn, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(x.LastNameEn, model.LastNameEn, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            return (data != null) ? true : false;
        }

        /// <summary>
        /// When reset password update first login to true.
        /// </summary>
        /// <param name="email">The email user.</param>
        private void UpdateFirstLogin(string email, bool isFirst)
        {
            var data = _unitOfWork.GetRepository<Customer>().GetCache(x => x.Email == email).FirstOrDefault();
            data.FirstLogin = isFirst;
            _unitOfWork.GetRepository<Customer>().Update(data);
        }

        /// <summary>
        /// Send email forget password.
        /// </summary>
        /// <param name="receiver">The receiver email.</param>
        /// <param name="newPassword">The new password reset.</param>
        /// <param name="contactName">The contact full name.</param>
        /// <returns></returns>
        private ResultViewModel SendEmailForgetPassword(string receiver, string newPassword, string contactName)
        {
            var result = new ResultViewModel();
            string status = ConstantValue.EmailSendingError;
            var emailModel = new EmailModel
            {
                Sender = _config.SmtpEmail,
                Receiver = receiver,
                Subject = ConstantValue.EmailForgetPasswordSubject,
                //Body = string.Format(ConstantValue.EmailForgetPasswordBody, contactName, newPassword)
                Body = newPassword
            };
            try
            {
                //_emailService.SendEmail(emailModel);
                _emailService.SendEmailForgotPasswordTemplate(emailModel);
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
        /// Reload cahce when action change data.
        /// </summary>
        private void ReloadCache()
        {
            _unitOfWork.GetRepository<Customer>().ReCache();
            _unitOfWork.GetRepository<Password>().ReCache();
        }

        #endregion


    }
}
