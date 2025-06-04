using Microsoft.AspNetCore.Mvc;

namespace PodTalk.Areas.Admin.Controllers
{
    public class EpisodeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
