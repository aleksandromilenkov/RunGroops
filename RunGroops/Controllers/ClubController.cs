using Microsoft.AspNetCore.Mvc;

namespace RunGroops.Controllers {
    public class ClubController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
