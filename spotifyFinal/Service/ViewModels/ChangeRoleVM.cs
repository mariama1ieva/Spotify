

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Service.ViewModels
{
    public class ChangeRoleVM
    {
        public int Id { get; set; }
        public string SelectedUserId { get; set; }
        public string SelectedRole { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
