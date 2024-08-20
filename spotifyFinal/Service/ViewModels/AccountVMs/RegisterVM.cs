using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Account
{
    public class RegisterVM
    {

        [Required, StringLength(100)]
        [Display(Prompt = "username")]
        public string? UserName { get; set; }
        [Display(Prompt = "fullname")]
        public string? Fullname { get; set; }
        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        [Display(Prompt = "name@domain.com")]
        public string? Email { get; set; }
        [Required, DataType(DataType.Password)]
        [Display(Prompt = "password")]
        public string? Password { get; set; }
        [Display(Name = "Confirm password", Prompt = "confirm password")]
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
    }
}
