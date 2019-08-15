using AutoMapper;
using HelpDesk.Bll.Components;
using HelpDesk.Bll.Interfaces;
using HelpDesk.Bll.Models;
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
    public class RegisterBll : IRegisterBll
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

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public RegisterBll(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// The Register new employee function.
        /// </summary>
        /// <param name="formData">The employee data.</param>
        /// <returns></returns>
        public ResultViewModel Register(RegisterViewModel formData)
        {
            var result = ValidateEmail(formData.Email);
            if (!result.IsError)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    this.SaveCustomer(formData);
                    this.SavePassword(formData);
                    scope.Complete();
                }
            }
            return result;
        }

        /// <summary>
        /// The Method insert employee information.
        /// </summary>
        /// <param name="formData">The employee data.</param>
        private void SaveCustomer(RegisterViewModel formData)
        {
            var data = _mapper.Map<RegisterViewModel, Customer>(formData);
            _unitOfWork.GetRepository<Customer>().Add(data);
            _unitOfWork.Complete();
        }

        /// <summary>
        /// The Method insert password employee login.
        /// </summary>
        /// <param name="formData">The employee data.</param>
        private void SavePassword(RegisterViewModel formData)
        {
            var password = new PasswordGenerator(formData.Password);
            var data = new Password { Email = formData.Email, Password1 = password.GetHash() };
            _unitOfWork.GetRepository<Password>().Add(data);
            _unitOfWork.Complete();
        }

        /// <summary>
        /// Validate Email is already have in database or not.
        /// </summary>
        /// <param name="email">The employee email.</param>
        /// <returns></returns>
        public ResultViewModel ValidateEmail(string email)
        {
            var result = new ResultViewModel();
            var data = _unitOfWork.GetRepository<Customer>().Get(x => x.Email == email).FirstOrDefault();
            if (data != null)
            {
                result.IsError = true;
                result.Message = MessageValue.EmailAlreadyExist;
            }
            return result;
        }

        #endregion

    }
}
