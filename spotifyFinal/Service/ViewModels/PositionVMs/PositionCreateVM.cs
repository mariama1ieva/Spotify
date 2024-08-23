using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.PositionVMs
{
    public class PositionCreateVM
    {
        [Required(ErrorMessage = "Can not be empty.")]
        public string Name { get; set; }

    }
}
