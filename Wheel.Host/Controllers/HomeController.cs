using Microsoft.AspNetCore.Mvc;

namespace Wheel.Host.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
