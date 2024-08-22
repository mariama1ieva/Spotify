using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Prompt = "Enter your email")]
        public string Email { get; set; }
    }
}
