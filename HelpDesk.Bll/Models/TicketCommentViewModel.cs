using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll.Models
{
    public class TicketCommentViewModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public bool IsOwner { get; set; }
    }
}
