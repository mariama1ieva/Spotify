using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.PositionVMs
{
    public class PositionEditVM
    {
        [Required(ErrorMessage = "Can not be empty.")]
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
