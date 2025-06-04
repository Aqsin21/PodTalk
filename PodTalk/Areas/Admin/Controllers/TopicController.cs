using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PodTalk.Areas.Admin.Data;
using PodTalk.DataContext;
using PodTalk.DataContext.Entities;

namespace PodTalk.Areas.Admin.Controllers
{
    public class TopicController(AppDbContext dbContext) : AdminController
    {

        private readonly AppDbContext _dbContext = dbContext;

        public IActionResult Index()
        {
            var topics = _dbContext.Topics.ToList();
            return View(topics);
        }


        public async Task<IActionResult> Details(int id)
        {
            var topic = await _dbContext.Topics
           .Include(t => t.Episodes)
           .Include(t => t.SpeakerTopics)
           .ThenInclude(st => st.Speaker)
           .FirstOrDefaultAsync(x => x.Id == id);

            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var topic = await _dbContext.Topics.FirstOrDefaultAsync(x => x.Id == id);
            if (topic == null) return NotFound();

            return View(topic);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topic = await _dbContext.Topics.FirstOrDefaultAsync(x => x.Id == id);
            if (topic == null) return NotFound();

            _dbContext.Topics.Remove(topic);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            var topic = await _dbContext.Topics.FindAsync(id);
            if (topic == null)
                return NotFound();
            var viewModel = new TopicEditViewModel
            {
                Id = topic.Id,
                Name = topic.Name,
                EpisodeCount = topic.EpisodeCount,
                ExistingImageUrl = topic.ImageUrl
            };
                return View(viewModel);
        }
        

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TopicEditViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var existingTopic = await _dbContext.Topics.FindAsync(id);
            if (existingTopic == null)
                return NotFound();

            
            existingTopic.Name = model.Name;
            existingTopic.EpisodeCount = model.EpisodeCount;

            if (model.NewImageFile != null && model.NewImageFile.Length >0)
            {
                var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.NewImageFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", newFileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.NewImageFile.CopyToAsync(stream);
                }

                existingTopic.ImageUrl = newFileName;
            }
            if (!string.IsNullOrEmpty(existingTopic.ImageUrl))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", existingTopic.ImageUrl);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TopicCreateViewModel model)
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


            var topic = new Topic
            {
                Name = model.Name,
                EpisodeCount = model.EpisodeCount,
                ImageUrl = fileName 
            };

            await _dbContext.Topics.AddAsync(topic);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));


        }
    }
}
