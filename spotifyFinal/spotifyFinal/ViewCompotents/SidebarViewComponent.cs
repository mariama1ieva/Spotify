using Microsoft.AspNetCore.Mvc;

namespace spotifyFinal.ViewCompotents
{
    public class SidebarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View());
        }
    }
}
