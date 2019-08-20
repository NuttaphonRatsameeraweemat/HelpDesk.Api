using AutoMapper;
using HelpDesk.Bll.Interfaces;
using HelpDesk.Bll.Models;
using HelpDesk.Data.Pocos;
using HelpDesk.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll
{
    public class PriorityBll : IPriorityBll
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
        /// Initializes a new instance of the <see cref="PriorityBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public PriorityBll(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Priority List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PriorityViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<Priority>, IEnumerable<PriorityViewModel>>(_unitOfWork.GetRepository<Priority>().GetCache());
        }

        #endregion

    }
}
