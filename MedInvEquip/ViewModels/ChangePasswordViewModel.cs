using System.ComponentModel.DataAnnotations;

namespace MedInvEquip.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 40 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password.")]
        [Compare("ConfirmNewPassword", ErrorMessage = "Password doesn't match.")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Conirim Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password.")]
        public string ConfirmNewPassword { get; set; }
    }
}
