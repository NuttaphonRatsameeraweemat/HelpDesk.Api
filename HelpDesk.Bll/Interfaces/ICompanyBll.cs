using HelpDesk.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll.Interfaces
{
    public interface ICompanyBll
    {
        /// <summary>
        /// Get Company List.
        /// </summary>
        /// <returns></returns>
        IEnumerable<CompanyViewModel> GetList();
    }
}
