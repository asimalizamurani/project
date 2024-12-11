using Microsoft.AspNetCore.Mvc;

namespace Handlingform.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}
