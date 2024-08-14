using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.GroupVMs;
using spotifyFinal.Helpers.Extentions;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IWebHostEnvironment _env;
        public GroupController(IGroupService categoryService, IWebHostEnvironment env)
        {
            _groupService = categoryService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(await _groupService.GetAllAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {

            return View(await _groupService.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            if (await _groupService.AnyAsync(request.Name))
            {
                ModelState.AddModelError("Name", $"{request.Name} is already exist!");
                return View(request);
            }

            if (!request.Photo.CheckFileFormat("image/"))
            {
                ModelState.AddModelError("Photo", "File must be Image Format");
                return View(request);
            }

            if (!request.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Max File capacity must be 200KB");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
            string path = Path.Combine(_env.WebRootPath, "assets/images", fileName);
            await request.Photo.SaveFileToLocalAsync(path);

            request.ImageUrl = fileName;

            await _groupService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();
            var setting = await _groupService.GetByIdAsync((int)id);

            if (setting == null) return NotFound();

            await _groupService.DeleteAsync((int)id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!ModelState.IsValid) return View();
            if (id == null) return BadRequest();

            var group = await _groupService.GetByIdAsync((int)id);

            GroupEditVM model = new()
            {
                ImageUrl = group.ImageUrl,
                Name = group.Name,

            };

            if (group == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, GroupEditVM request)
        {
            if (!ModelState.IsValid) return View(request);
            if (id == null) return BadRequest();

            if (await _groupService.AnyAsync(request.Name))
            {
                ModelState.AddModelError("Name", $"{request.Name} is already exist!");
                return View(request);
            }

            if (request.Photo != null)
            {
                if (!request.Photo.CheckFileFormat("image/"))
                {
                    ModelState.AddModelError("Photo", "File must be Image Format");
                    return View(request);
                }

                if (!request.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Max File capacity must be 200KB");
                    return View(request);
                }

                string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
                string path = Path.Combine(_env.WebRootPath, "assets/images", fileName);
                await request.Photo.SaveFileToLocalAsync(path);

                request.ImageUrl = fileName;
            }
            FileExtention.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "img"), request.ImageUrl);

            await _groupService.UpdateAsync((int)id, request);

            return RedirectToAction(nameof(Index));
        }
    }
}
