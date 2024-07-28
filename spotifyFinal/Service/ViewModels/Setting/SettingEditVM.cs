using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Setting
{
    public class SettingEditVM
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
