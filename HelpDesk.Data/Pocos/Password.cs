using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Data.Pocos
{
    public partial class Password
    {
        [StringLength(255)]
        public string Email { get; set; }
        [Column("Password")]
        public byte[] Password1 { get; set; }
    }
}
