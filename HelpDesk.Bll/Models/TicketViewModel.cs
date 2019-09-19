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
        public string PriorityName { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string CompanyCode { get; set; }
        public string CreateBy { get; set; }
        public string CreateName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string AssignTo { get; set; }
        public int OnlineTime { get; set; }
        public string TicketType { get; set; }
        public string TicketTypeName { get; set; }
        public int? EstimateTime { get; set; }
    }
}
