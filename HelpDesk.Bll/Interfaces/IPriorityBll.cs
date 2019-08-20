using HelpDesk.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll.Interfaces
{
    public interface IPriorityBll
    {
        /// <summary>
        /// Get Priority List.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PriorityViewModel> GetList();
    }
}
