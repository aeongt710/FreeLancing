using Microsoft.AspNetCore.Mvc;

namespace FreeLancing.Areas.Homepage.Controllers
{
    public class HomePageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
