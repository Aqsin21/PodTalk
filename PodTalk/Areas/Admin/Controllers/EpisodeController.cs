using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PodTalk.Areas.Admin.Data;
using PodTalk.DataContext;
using PodTalk.DataContext.Entities;


namespace PodTalk.Areas.Admin.Controllers
{
    public class EpisodeController : AdminController
    {
        private readonly AppDbContext _dbContext;
public EpisodeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var episodes = _dbContext.Episodes.ToList();
            return View(episodes);

        }


        public async Task<IActionResult> Details(int id)
        {
            var episode= await _dbContext.Episodes
                  .Include(e => e.Topic)
                  .Include(e => e.Speaker)
                 .FirstOrDefaultAsync(e => e.Id == id);

            if (episode == null) return NotFound();

            return View(episode);
        }

        public async Task<IActionResult> Delete (int id)
        {
            var episode= await _dbContext.Episodes.FirstOrDefaultAsync(e=>e.Id==id);
            if (episode == null)
                return NotFound();

            return View(episode);
        }

        [HttpPost]
        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var episode = await _dbContext.Episodes.FirstOrDefaultAsync(e => e.Id==id);
            if(episode== null) return NotFound();


            _dbContext.Episodes.Remove(episode);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var episode = await _dbContext.Episodes.FirstOrDefaultAsync(x=>x.Id==id);
            if (episode == null) return NotFound();

            var viewmodel = new EpisodeEditViewModel
            {
                Id = episode.Id,
                Title = episode.Title,
                ExistingCoverImageUrl = episode.CoverImageUrl,
                Description = episode.Description,
                TopicId = episode.TopicId,
                SpeakerId = episode.SpeakerId,
             };
            return View(viewmodel);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EpisodeEditViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var existingEpisode = await _dbContext.Episodes.FindAsync(id);
            if (existingEpisode == null)
                return NotFound();

            existingEpisode.Title = model.Title;
            existingEpisode.Description = model.Description;
            existingEpisode.Duration = model.Duration;
            existingEpisode.TopicId = model.TopicId;      
            existingEpisode.SpeakerId = model.SpeakerId;

            if (model.NewCoverImageUrl != null && model.NewCoverImageUrl.Length > 0)
            {
                
                var oldImageFileName = existingEpisode.CoverImageUrl;

               
                var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.NewCoverImageUrl.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", newFileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.NewCoverImageUrl.CopyToAsync(stream);
                }

                existingEpisode.CoverImageUrl = newFileName;

              
                if (!string.IsNullOrEmpty(oldImageFileName))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", oldImageFileName);
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public  IActionResult Create()
        {
           

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EpisodeCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);



            string? imageFileName = null;
            if (model.ImageFile is { Length: > 0 })
            {
                imageFileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageFileName);

                using var stream = new FileStream(imagePath, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);
            }

            string? audioFileName = null;
            if (model.AudioFile is { Length: > 0 })
            {
                audioFileName = Guid.NewGuid() + Path.GetExtension(model.AudioFile.FileName);
                var audioPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "audios", audioFileName);

                using var stream = new FileStream(audioPath, FileMode.Create);
                await model.AudioFile.CopyToAsync(stream);
            }

            var episode = new Episode
            {
                Title = model.Title,
                Description = model.Description,
                Duration = model.Duration,
                TopicId = model.TopicId,
                SpeakerId = model.SpeakerId,
                CoverImageUrl = imageFileName ,
                AudioUrl = audioFileName ,
                LikeCount = 0,
                PlayCount = 0,
                DownloadCount = 0,
                CommentCount = 0
            };

            await _dbContext.Episodes.AddAsync(episode);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
    }

