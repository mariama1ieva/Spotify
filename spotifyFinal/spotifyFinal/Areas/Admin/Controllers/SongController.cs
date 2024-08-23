using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.Services.Interfaces;
using Service.ViewModels.SongArtistVMs;
using Service.ViewModels.SongVMs;
using spotifyFinal.Helpers.Extentions;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SongController : Controller
    {
        private readonly ISongArtistService _songArtistService;
        private readonly ISongService _songService;
        private readonly IArtistService _artistService;
        private readonly ICategoryService _categoryService;
        private readonly IAlbumService _albumService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public SongController(ISongArtistService songArtistService, ISongService songService, IArtistService artistService, IWebHostEnvironment env,
                             ICategoryService categoryService, IAlbumService albumService, IMapper mapper, AppDbContext context)
        {
            _songArtistService = songArtistService;
            _songService = songService;
            _artistService = artistService;
            _env = env;
            _categoryService = categoryService;
            _albumService = albumService;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _songService.GetAllWithDatas();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Artists = await _artistService.GetALlBySelectedAsync();
            ViewBag.Categories = await _categoryService.GetALlBySelectedAsync();
            ViewBag.Albums = await _albumService.GetALlBySelectedAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SongCreateVM request)
        {
            ViewBag.Artists = await _artistService.GetALlBySelectedAsync();
            ViewBag.Categories = await _categoryService.GetALlBySelectedAsync();
            ViewBag.Albums = await _albumService.GetALlBySelectedAsync();

            if (!ModelState.IsValid) return View(request);

            if (await _songService.AnyAsync(request.Name))
            {
                ModelState.AddModelError("Name", $"{request.Name} is already exist!");
                return View(request);
            }

            if (!request.PhotoUrl.CheckFileFormat("image/"))
            {
                ModelState.AddModelError("PhotoUrl", "File must be Image Format");
                return View(request);
            }

            if (!request.PhotoUrl.CheckFileSize(200))
            {
                ModelState.AddModelError("PhotoUrl", "Max File capacity must be 200KB");
                return View(request);
            }
            if (!request.SongUrl.CheckFileFormat("audio/"))
            {
                ModelState.AddModelError("SongUrl", "File must be mp3 format");
                return View(request);
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.PhotoUrl.FileName;
            string path = Path.Combine(_env.WebRootPath, "assets/images", fileName);
            await request.PhotoUrl.SaveFileToLocalAsync(path);

            string fileName2 = Guid.NewGuid().ToString() + "-" + request.SongUrl.FileName;
            string path2 = Path.Combine(_env.WebRootPath, "assets/music", fileName2);
            await request.SongUrl.SaveFileToLocalAsync(path2);

            request.ImageUrl = fileName;
            request.Path = fileName2;

            int songId = await _songService.CreateAsync(request);

            foreach (var artistId in request.ArtistIds)
            {
                await _songArtistService.CreateAsync(new SongArtistCreateVM() { ArtistId = artistId, SongId = songId });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Song? song = await _context.Songs.FirstOrDefaultAsync(s => s.Id == id);
            if (song == null) NotFound();
            else
            {
                _context.Remove(song);
                await _context.SaveChangesAsync();
                string image = Path.Combine(_env.WebRootPath, "assets/images", song.ImageUrl);

                if (System.IO.File.Exists(image))
                {
                    System.IO.File.Delete(image);
                }
                string fullpath = Path.Combine(_env.WebRootPath, "assets/music", song.Path);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }

            };
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Category = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            ViewBag.Artists = new SelectList(await _context.Artists.Where(m => !m.SoftDelete).ToListAsync(), "Id", "FullName");
            ViewBag.Albums = new SelectList(await _context.Albums.Where(m => !m.SoftDelete).ToListAsync(), "Id", "Name");


            if (id == 0) return BadRequest();

            SongEditVM model = UpdateSong(id);
            if (model == null) return NotFound();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, SongEditVM SongEditVM)
        {
            if (id == 0) return BadRequest();
            SongEditVM model = UpdateSong(id);
            ViewBag.Category = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            ViewBag.Artists = new SelectList(await _context.Artists.Where(m => !m.SoftDelete).ToListAsync(), "Id", "FullName");
            ViewBag.Albums = new SelectList(await _context.Albums.Where(m => !m.SoftDelete).ToListAsync(), "Id", "Name");


            if (model == null) return NotFound();
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }


            Song dbSong = _context.Songs.Include(s => s.ArtistSongs)
                                                .Include(s => s.Category)
                                                .FirstOrDefault(p => p.Id == id)!;
            if (dbSong == null) return NotFound();
            var removableArtist = dbSong.ArtistSongs.Where(s => !SongEditVM.ArtistsIds.Contains(s.ArtistId)).ToList();

            foreach (var artist in removableArtist)
            {
                dbSong.ArtistSongs.Remove(artist);
            }

            foreach (var artistId in SongEditVM.ArtistsIds)
            {
                if (!dbSong.ArtistSongs.Any(s => s.ArtistId == artistId))
                {
                    dbSong.ArtistSongs.Add(new ArtistSong { ArtistId = artistId });
                }
            }
            if (dbSong == null) return NotFound();
            if (SongEditVM.Photo != null)
            {
                if (!SongEditVM.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please select only an image file.");
                    return View(model);
                }

                string fullPath = Path.Combine(_env.WebRootPath, "assets/images", dbSong.ImageUrl);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                dbSong.ImageUrl = SongEditVM.Photo.SaveImage(_env, "assets/images", SongEditVM.Photo.FileName);
            }

            if (SongEditVM.Audio != null)
            {
                if (!SongEditVM.Audio.IsAudio())
                {
                    ModelState.AddModelError("Audio", "Please select only an audio file with mp3 extension.");
                    return View(model);
                }

                if (SongEditVM.Audio.CheckAudioSize(20000))
                {
                    ModelState.AddModelError("Audio", "Song should not be bigger than 20mb.");
                    return View(model);
                }

                dbSong.Path = SongEditVM.Audio.SaveAudio(_env, "assets/music", SongEditVM.Audio.FileName);
            }

            dbSong.Name = SongEditVM.Name;
            dbSong.Color = SongEditVM.Color;
            dbSong.CategoryId = SongEditVM.CategoryId;
            dbSong.CreateDate = SongEditVM.CreateDate;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        SongEditVM UpdateSong(int id)
        {
            SongEditVM model = _context.Songs.Include(s => s.Category)
                .Include(s => s.ArtistSongs)
                .ThenInclude(s => s.Artist)
                .Include(s => s.Album)
                .Select(s => new SongEditVM
                {
                    Id = s.Id,
                    Name = s.Name,
                    ImageUrl = s.ImageUrl,
                    Path = s.Path,
                    Color = s.Color,
                    CreateDate = s.CreateDate,
                    CategoryId = s.CategoryId,
                    AlbumId = s.AlbumId,
                    ArtistsIds = s.ArtistSongs.Select(a => a.ArtistId).ToList(),
                    Category = s.Category,
                    Album = s.Album,
                    ArtistSongs = s.ArtistSongs.ToList()
                }).FirstOrDefault(s => s.Id == id)!;
            return model;
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {

            return View(await _songService.GetDataIdWithCategoryArtistAlbum(id));
        }
    }
}
