using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HelpDesk.Bll.Models
{
    public class PasswordViewModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [RegularExpression(@"^\S*$", ErrorMessage = "Whitespace Not Allowed")]
        public string NewPassword { get; set; }

        [RegularExpression(@"^\S*$", ErrorMessage = "Whitespace Not Allowed")]
        [Compare("NewPassword", ErrorMessage = "NewPassword Confirm not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
