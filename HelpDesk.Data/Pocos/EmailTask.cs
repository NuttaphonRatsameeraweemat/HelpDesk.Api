using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Data.Pocos
{
    public partial class EmailTask
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(255)]
        public string Sender { get; set; }
        [StringLength(255)]
        public string Receiver { get; set; }
        [StringLength(255)]
        public string Subject { get; set; }
        public string Body { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        [Column(TypeName = "timestamp(6) with time zone")]
        public DateTime? CreateDate { get; set; }
    }
}
