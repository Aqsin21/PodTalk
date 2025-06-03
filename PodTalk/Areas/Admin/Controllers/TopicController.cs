using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PodTalk.DataContext;
using PodTalk.DataContext.Entities;

namespace PodTalk.Areas.Admin.Controllers
{
    public class TopicController : AdminController
    {

        private readonly AppDbContext _dbContext;

        public TopicController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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

            return View(topic);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Topic updatedTopic, IFormFile? ImageFile)
        {
            if (id != updatedTopic.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(updatedTopic);

            var existingTopic = await _dbContext.Topics.FindAsync(id);
            if (existingTopic == null)
                return NotFound();

            existingTopic.Name = updatedTopic.Name;
            existingTopic.EpisodeCount = updatedTopic.EpisodeCount;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "admin", "images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                existingTopic.ImageUrl = fileName;
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}