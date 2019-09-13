using HelpDesk.Bll.Interfaces;
using HelpDesk.Data.Pocos;
using HelpDesk.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll
{
    public class CacheManagement : ICacheManagement
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManagement" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public CacheManagement(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Reload z entity framework cache.
        /// </summary>
        public void ReloadAllCache()
        {
            _unitOfWork.GetRepository<Company>().ReCache();
            _unitOfWork.GetRepository<Customer>().ReCache();
            _unitOfWork.GetRepository<Password>().ReCache();
            _unitOfWork.GetRepository<Priority>().ReCache();
        }

        #endregion

    }
}
