using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Data.Pocos
{
    public partial class TicketComment
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("TicketID")]
        public int? TicketId { get; set; }
        public string Comment { get; set; }
        [StringLength(255)]
        public string CommentBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CommentDate { get; set; }
    }
}
