﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Data.Pocos
{
    public partial class Ticket
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string TicketNo { get; set; }
        [StringLength(255)]
        public string TicketName { get; set; }
        public string Description { get; set; }
        [Column("PriorityID")]
        public int? PriorityId { get; set; }
        [StringLength(255)]
        public string Status { get; set; }
        [StringLength(255)]
        public string CreateBy { get; set; }
        [Column(TypeName = "timestamp(0) without time zone")]
        public DateTime? CreateDate { get; set; }
        [StringLength(10)]
        public string CompanyCode { get; set; }
        [StringLength(10)]
        public string AssignTo { get; set; }
        [StringLength(10)]
        public string TicketType { get; set; }
        public int? EstimateTime { get; set; }
    }
}
