using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HelpDesk.Bll.Models
{
    public class ForgetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstNameEn { get; set; }
        [Required]
        public string LastNameEn { get; set; }
    }
}
