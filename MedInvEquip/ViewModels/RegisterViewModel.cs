using System.ComponentModel.DataAnnotations;

namespace MedInvEquip.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }



        [Required(ErrorMessage = "Password is required.")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 40 characters.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Password doesn't match.")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confrim Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password.")]
        public string ConfirmPassword { get; set; }
    }
}
