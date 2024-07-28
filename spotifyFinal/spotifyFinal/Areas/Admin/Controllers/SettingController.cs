using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.Setting;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(await _settingService.GetAllAsync());
        }


        public async Task<IActionResult> Detail(int id)
        {

            return View(await _settingService.GetByIdAsync(id));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SettingVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _settingService.CreateAsync(request);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            var setting = await _settingService.GetByIdAsync((int)id);

            if (setting == null) return NotFound();

            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SettingVM request)
        {
            if (!ModelState.IsValid) return View(request);

            if (request.Id == null) return BadRequest();

            var setting = await _settingService.GetByIdAsync((int)request.Id);

            if (setting == null) return NotFound();

            await _settingService.UpdateAsync(request);

            return RedirectToAction(nameof(Index));
        }
    }
}
