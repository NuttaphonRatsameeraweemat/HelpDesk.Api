using HelpDesk.Data.Repository.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Data
{
    /// <summary>
    /// EVFUnitOfWork class is a unit of work for manipulating about utility data in database via repository.
    /// </summary>
    public class HelpDeskUnitOfWork : EfUnitOfWork<HelpDeskContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpDeskUnitOfWork" /> class.
        /// </summary>
        /// <param name="helpDeskDbContext">The HelpDesk database context what inherits from DbContext of EF.</param>
        public HelpDeskUnitOfWork(HelpDeskContext helpDeskDbContext) : base(helpDeskDbContext)
        { }
    }
}
