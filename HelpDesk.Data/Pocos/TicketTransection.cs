using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Data.Pocos
{
    public partial class TicketTransection
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("TicketID")]
        public int? TicketId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }
        [StringLength(255)]
        public string Status { get; set; }
    }
}
