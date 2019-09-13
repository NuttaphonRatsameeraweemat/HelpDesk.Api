using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll.Interfaces
{
    public interface ICacheManagement
    {
        /// <summary>
        /// Reload z entity framework cache.
        /// </summary>
        void ReloadAllCache();
    }
}
