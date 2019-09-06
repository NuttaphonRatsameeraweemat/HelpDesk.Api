using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll.Models
{
    public class TicketCommentViewModel
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Comment { get; set; }
        public string CommentBy { get; set; }
        public string CommentByName { get; set; }
        public string CommentDate { get; set; }
        public bool IsOwner { get; set; }
    }
}
