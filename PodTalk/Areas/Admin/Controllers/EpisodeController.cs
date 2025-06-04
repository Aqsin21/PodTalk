using Microsoft.AspNetCore.Mvc;
using PodTalk.DataContext;

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
    }
}
