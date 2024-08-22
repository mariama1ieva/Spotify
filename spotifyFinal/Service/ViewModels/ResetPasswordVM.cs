using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels
{
    public class ResetPasswordVM
    {

        [Required]
        [DataType(DataType.Password)]
        [Display(Prompt = "New password")]
        public string Password { get; set; }
        [Required]
        [Display(Prompt = "Confirm password")]
        [DataType(DataType.Password), Compare(nameof(Password))]

        public string ConfirmPassword { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
