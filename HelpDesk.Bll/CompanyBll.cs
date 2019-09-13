using AutoMapper;
using HelpDesk.Bll.Components;
using HelpDesk.Bll.Interfaces;
using HelpDesk.Bll.Models;
using HelpDesk.Data.Pocos;
using HelpDesk.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll
{
    public class CompanyBll : ICompanyBll
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
        /// Initializes a new instance of the <see cref="CompanyBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public CompanyBll(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Company List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CompanyViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<Company>, IEnumerable<CompanyViewModel>>(_unitOfWork.GetRepository<Company>().GetCache());
        }

        /// <summary>
        /// Get Company List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CompanyViewModel> GetPartner()
        {
            return _mapper.Map<IEnumerable<Company>, IEnumerable<CompanyViewModel>>(
                _unitOfWork.GetRepository<Company>().GetCache(x => x.Type == ConstantValue.CompanyTypePartner));
        }

        #endregion

    }
}
