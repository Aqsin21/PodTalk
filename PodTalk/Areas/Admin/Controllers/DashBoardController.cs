using Microsoft.AspNetCore.Mvc;

namespace PodTalk.Areas.Admin.Controllers
{
    public class DashBoardController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }



    }
}
