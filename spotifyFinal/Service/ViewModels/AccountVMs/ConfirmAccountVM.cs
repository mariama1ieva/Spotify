using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.AccountVMs
{
    public class ConfirmAccountVM
    {
        public string Email { get; set; }
        [Required]
        public string? OTP { get; set; }
        public AppUser? AppUser { get; set; }

        public ConfirmAccountVM()
        {
            AppUser = new();
        }
    }
}
