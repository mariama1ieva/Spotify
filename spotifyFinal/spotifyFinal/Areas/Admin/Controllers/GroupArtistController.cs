using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.GroupArtistVMs;
using spotifyFinal.Helpers.Extentions;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GroupArtistController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IGroupService _groupService;
        private readonly IGroupArtistService _groupArtistService;


        public GroupArtistController(IWebHostEnvironment env, IGroupArtistService groupArtistService, IGroupService groupService)
        {
            _env = env;
            _groupArtistService = groupArtistService;
            _groupService = groupService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _groupArtistService.GetAllWithGroup());
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {

            return View(await _groupArtistService.GetByIdAsync(id));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Groups = await _groupService.GetALlBySelectedAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupArtistCreateVM request)
        {
            ViewBag.Groups = await _groupService.GetALlBySelectedAsync();

            if (!ModelState.IsValid) return View(request);

            if (await _groupArtistService.AnyAsync(request.FullName.Trim().ToLower()))
            {
                ModelState.AddModelError("FullName", $"{request.FullName} is already exist!");
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
                return View(request);
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
            string path = Path.Combine(_env.WebRootPath, "assets/images", fileName);
            await request.Photo.SaveFileToLocalAsync(path);

            request.ImageUrl = fileName;

            await _groupArtistService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();
            var setting = await _groupArtistService.GetByIdAsync((int)id);

            if (setting == null) return NotFound();

            await _groupArtistService.DeleteAsync((int)id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Groups = await _groupService.GetALlBySelectedAsync();

            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            var groupArtist = await _groupArtistService.GetAllWithGroup((int)id);



            GroupArtistEditVM model = new()
            {
                FullName = groupArtist.FullName,
                GroupName = groupArtist.GroupName,
                ImageUrl = groupArtist.ImageUrl,

            };

            if (groupArtist == null) return NotFound();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, GroupArtistEditVM request)
        {
            ViewBag.Groups = await _groupService.GetALlBySelectedAsync();

            var existGroupArtist = await _groupArtistService.GetAllWithGroup((int)id);


            if (!ModelState.IsValid) return View(request);

            if (id == null) return BadRequest();


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

                FileExtention.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "img"), request.ImageUrl);
            }
            if (!await _groupArtistService.AnyAsync(request.FullName))
            {
                ModelState.AddModelError("FullName", $"{request.FullName} is already exist!");
                return View(request);
            }
            else
            {
                existGroupArtist.ImageUrl = request.ImageUrl;
                existGroupArtist.FullName = request.FullName;
                existGroupArtist.GroupName = request.GroupName;

            }



            await _groupArtistService.UpdateAsync((int)id, request);

            return RedirectToAction(nameof(Index));


        }
    }
}
