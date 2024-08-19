using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.AccountVMs
{
    public class LoginVM
    {
        [Required, StringLength(30, MinimumLength = 3), Display(Prompt = "Email or username")]
        public string? UserName { get; set; }

        [Required, DataType(DataType.Password)]
        [Display(Prompt = "Password")]
        public string? Password { get; set; }
        [Display(Name = "Remember me", Prompt = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
