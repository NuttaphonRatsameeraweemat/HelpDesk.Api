using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll.Models
{
    public class TicketViewModel
    {
        public int Id { get; set; }
        public string TicketNo { get; set; }
        public string TicketName { get; set; }
        public string Description { get; set; }
        public int PriorityId { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
    }
}
