﻿using HelpDesk.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll.Interfaces
{
    public interface IValueHelpBll
    {
        /// <summary>
        /// Get ValueHelp List by type.
        /// </summary>
        /// <param name="type">The type of value.</param>
        /// <returns></returns>
        IEnumerable<ValueHelpViewModel> Get(string type);
    }
}
