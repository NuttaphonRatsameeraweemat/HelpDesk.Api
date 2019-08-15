using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Data.Pocos
{
    public partial class Customer
    {
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(255)]
        public string FirstNameTh { get; set; }
        [StringLength(255)]
        public string LastNameTh { get; set; }
        [StringLength(255)]
        public string FirstNameEn { get; set; }
        [StringLength(255)]
        public string LastNameEn { get; set; }
        [StringLength(10)]
        public string CompanyCode { get; set; }
        [StringLength(20)]
        public string UserType { get; set; }
        [StringLength(50)]
        public string TelNo { get; set; }
    }
}
