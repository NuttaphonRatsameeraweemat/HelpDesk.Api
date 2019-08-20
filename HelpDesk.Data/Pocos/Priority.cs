using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Data.Pocos
{
    public partial class Priority
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string PriorityName { get; set; }
        public int? PriorityFixed { get; set; }
    }
}
