using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.Services.Interfaces;
using Service.ViewModels.ArtistVMs;
using spotifyFinal.Helpers.Extentions;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArtistController : Controller
    {
        private readonly IArtistPositionService _positionArtistService;
        private readonly IArtistService _artistService;
        private readonly IPositionService _positionService;
        private readonly ISongArtistService _songArtistService;
        private readonly ISongService _songService;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ArtistController(IArtistPositionService positionArtistService,
               IPositionService positionService, IArtistService artistService, IWebHostEnvironment env, ISongArtistService songArtistService, ISongService songService, AppDbContext context)
        {
            _positionArtistService = positionArtistService;
            _positionService = positionService;
            _artistService = artistService;
            _env = env;
            _songArtistService = songArtistService;
            _songService = songService;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _artistService.GetAllWithDatas();
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            ArtistCreateVM model = new ArtistCreateVM
            {
                Positions = await _context.Positions.ToListAsync(),

            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ArtistCreateVM artistCreateVM)
        {
            ArtistCreateVM model = new ArtistCreateVM
            {
                Positions = await _context.Positions.ToListAsync(),

            };
            if (!ModelState.IsValid) return View(model);



            Artist newArtist = new Artist();
            newArtist.ArtistPositions = new List<ArtistPosition>();



            if (artistCreateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please Input Image");
                return View();

            }
            if (!artistCreateVM.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "please input only image");
                return View();
            }

            if (artistCreateVM.AboutImg == null)
            {
                ModelState.AddModelError("AboutImg", "Please Input Image");
                return View();

            }

            if (!artistCreateVM.AboutImg.IsImage())
            {
                ModelState.AddModelError("AboutImg", "please input only image");
                return View();
            }

            foreach (int id in artistCreateVM.PositionIds)
            {
                ArtistPosition position = new ArtistPosition()
                {
                    PositionId = id,
                    Artist = newArtist
                };
                newArtist.ArtistPositions.Add(position);
            }
            newArtist.FullName = artistCreateVM.FullName;
            newArtist.ImageUrl = artistCreateVM.Photo.SaveImage(_env, "assets/images", artistCreateVM.Photo.FileName);
            newArtist.AboutImg = artistCreateVM.AboutImg.SaveImage(_env, "assets/images", artistCreateVM.AboutImg.FileName);

            await _context.Artists.AddAsync(newArtist);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var artist = await _context.Artists
                .Include(a => a.Albums)
                .ThenInclude(al => al.Songs)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artist == null) return NotFound();

            string fullpath = Path.Combine(_env.WebRootPath, "assets/images", artist.ImageUrl);
            string aboutImg = Path.Combine(_env.WebRootPath, "assets/images", artist.AboutImg);

            if (System.IO.File.Exists(fullpath))
            {
                System.IO.File.Delete(fullpath);
            }

            if (System.IO.File.Exists(aboutImg))
            {
                System.IO.File.Delete(aboutImg);
            }

            foreach (var album in artist.Albums)
            {
                _context.Songs.RemoveRange(album.Songs);
            }

            _context.Albums.RemoveRange(artist.Albums);

            _context.Artists.Remove(artist);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {

            return View(await _artistService.GetAllWithDatasById(id));
        }

        public IActionResult Update(int id)
        {
            if (id == 0) return BadRequest();

            ArtistUpdateVM model = UpdatedArtist(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, ArtistUpdateVM artistUpdateVM)
        {
            if (id == 0) return BadRequest();
            ArtistUpdateVM model = UpdatedArtist(id);
            if (!ModelState.IsValid) return View(model);

            Artist dbArtist = _context.Artists.Include(p => p.ArtistPositions).FirstOrDefault(p => p.Id == id)!;
            if (dbArtist == null)
                return NotFound();

            dbArtist.FullName = artistUpdateVM.FullName;
            var removableArtist = dbArtist.ArtistPositions.Where(p => !artistUpdateVM.PositionIds.Contains(p.PositionId)).ToList();

            foreach (var item in removableArtist)
            {
                dbArtist.ArtistPositions.Remove(item);
            }

            foreach (var positionId in artistUpdateVM.PositionIds)
            {
                if (!dbArtist.ArtistPositions.Any(p => p.PositionId == positionId))
                {
                    ArtistPosition newposition = new ArtistPosition
                    {
                        PositionId = positionId,
                        ArtistId = dbArtist.Id
                    };
                    dbArtist.ArtistPositions.Add(newposition);
                }
            }
            if (artistUpdateVM.Photo != null)
            {
                if (artistUpdateVM.Photo == null)
                {
                    ModelState.AddModelError("Photo", "Can not be empty!");
                }
                if (!artistUpdateVM.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "only image select");
                }
                string fullPath = Path.Combine(_env.WebRootPath, "assets/images", dbArtist.ImageUrl);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                dbArtist.ImageUrl = artistUpdateVM.Photo.SaveImage(_env, "assets/images", artistUpdateVM.Photo.FileName);
                _context.SaveChanges();
            }
            if (artistUpdateVM.AboutPhoto != null)
            {
                if (artistUpdateVM.AboutPhoto == null)
                {
                    ModelState.AddModelError("AboutPhoto", "Can not be empty!");
                }
                if (!artistUpdateVM.AboutPhoto.IsImage())
                {
                    ModelState.AddModelError("AboutPhoto", "only image select");
                }
                string fullPath = Path.Combine(_env.WebRootPath, "assets/images", dbArtist.AboutImg);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                dbArtist.AboutImg = artistUpdateVM.AboutPhoto.SaveImage(_env, "assets/images", artistUpdateVM.AboutPhoto.FileName);
                _context.SaveChanges();
            }


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        ArtistUpdateVM UpdatedArtist(int id)
        {
            var artist = _context.Artists
                .Include(p => p.ArtistPositions)
                .FirstOrDefault(p => p.Id == id);

            if (artist == null)
            {

                return new ArtistUpdateVM();
            }


            ArtistUpdateVM model = _context.Artists.Select(s => new ArtistUpdateVM()
            {
                Id = s.Id,
                FullName = s.FullName,
                ImageUrl = s.ImageUrl,
                AboutImg = s.AboutImg,
                ArtistPositions = s.ArtistPositions.ToList()
            }).FirstOrDefault(s => s.Id == id);
            model.PositionIds = artist.ArtistPositions.Select(ap => ap.PositionId).ToList();
            model.Positions = _context.Positions.ToList();

            return model;
        }
    }
}
