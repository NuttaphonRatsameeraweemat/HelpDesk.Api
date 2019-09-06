using HelpDesk.Bll.Interfaces;
using HelpDesk.Bll.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HelpDesk.Test
{
    public class TicketBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The Kpi service manager provides Kpi service functionality.
        /// </summary>
        private ITicketBll _ticket;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public TicketBllTest(IoCConfig io)
        {
            _ticket = io.ServiceProvider.GetRequiredService<ITicketBll>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void LoopCreate()
        {
            try
            {
                string ticketName = "Loop Insert #";
                var model = new TicketViewModel
                {
                    TicketName = ticketName,
                    Description = "Loop Insert",
                    Status = "OPEN",
                    PriorityId = 1
                };
                for (int i = 0; i < 10000; i++)
                {
                    model.TicketName = ticketName + i;
                    _ticket.OpenTicket(model);
                }
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        #endregion

    }
}
