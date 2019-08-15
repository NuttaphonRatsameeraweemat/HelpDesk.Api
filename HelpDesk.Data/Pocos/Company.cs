using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Data.Pocos
{
    public partial class Company
    {
        [StringLength(10)]
        public string CompanyCode { get; set; }
        [StringLength(255)]
        public string CompanyName { get; set; }
    }
}
