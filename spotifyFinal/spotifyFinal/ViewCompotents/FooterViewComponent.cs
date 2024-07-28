using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace spotifyFinal.ViewCompotents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;
        public FooterViewComponent(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            return await Task.FromResult(View(await _settingService.GetAllAsync()));
        }
    }
}
