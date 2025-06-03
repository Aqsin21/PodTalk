using Microsoft.AspNetCore.Mvc;

namespace PodTalk.Areas.Admin.Controllers
{
    public class SpeakerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
