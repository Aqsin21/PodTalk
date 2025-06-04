using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PodTalk.Areas.Admin.Data;
using PodTalk.DataContext;
using PodTalk.DataContext.Entities;
using System.Threading.Tasks;

namespace PodTalk.Areas.Admin.Controllers
{
    public class SpeakerController : AdminController
    {
        private readonly AppDbContext _dbContext;

        public SpeakerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var speakers = _dbContext.Speakers.ToList();
            return View(speakers);
        }


        public async Task<IActionResult> Details(int id)
        {
            var speaker = await _dbContext.Speakers
                .Include(s => s.SpeakerTopics)
                    .ThenInclude(st => st.Topic)
                .Include(s => s.Episodes)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (speaker == null)
                return NotFound();

            return View(speaker);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var speaker= await _dbContext.Speakers.FirstOrDefaultAsync(x=>x.Id == id);
            if (speaker == null)
            return NotFound();

            return View(speaker);

        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var speaker= await _dbContext.Speakers.FirstOrDefaultAsync(x=>x.Id==id);
            if (speaker == null) return NotFound();

            _dbContext.Speakers.Remove(speaker);
            await _dbContext.SaveChangesAsync();
            return  RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {

            var speaker= await _dbContext.Speakers.FindAsync(id);
            if (speaker == null) return NotFound();
        
            var viewmodel = new SpeakerEditViewModel
            {
                Id = speaker.Id,
                UserName = speaker.UserName,
                ExistingCoverImageUrl = speaker.CoverImageUrl
            };
            return View(viewmodel);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SpeakerEditViewModel model)
        {
            if (id !=model.Id) return 
                    BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var existingSpeaker = await _dbContext.Speakers.FindAsync(id);
            if (existingSpeaker == null) return NotFound();

            existingSpeaker.UserName = model.UserName;
            if (model.NewCoverImageFile != null && model.NewCoverImageFile.Length > 0)
            {
                var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.NewCoverImageFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", newFileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.NewCoverImageFile.CopyToAsync(stream);
                }

                existingSpeaker.CoverImageUrl = newFileName;
            }
            if (!string.IsNullOrEmpty(existingSpeaker.CoverImageUrl))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", existingSpeaker.CoverImageUrl);
                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SpeakerCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string? fileName = null;
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);
            }

            var speaker = new Speaker 
            {
                UserName = model.UserName,
                CoverImageUrl=fileName
            
            };

            await _dbContext.Speakers.AddAsync(speaker);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
