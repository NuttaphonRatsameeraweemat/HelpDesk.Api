using AutoMapper;
using HelpDesk.Bll.Models;
using HelpDesk.Data.Pocos;
using HelpDesk.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Api.Extensions
{
    public class MappingProfiles : Profile
    {

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfiles" /> class.
        /// </summary>
        public MappingProfiles()
        {
            CreateMap<RegisterViewModel, Customer>();
            CreateMap<Customer, RegisterViewModel>();
            CreateMap<CompanyViewModel, Company>();
            CreateMap<Company, CompanyViewModel>();
            CreateMap<TicketViewModel, Ticket>();
            CreateMap<Ticket, TicketViewModel>();
            CreateMap<PriorityViewModel, Priority>();
            CreateMap<Priority, PriorityViewModel>();
            CreateMap<EmailModel, EmailTask>();
            CreateMap<EmailTask, EmailModel>();
            CreateMap<ValueHelpViewModel, ValueHelp>();
            CreateMap<ValueHelp, ValueHelpViewModel>();
        }

        #endregion

    }
}
