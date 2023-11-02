using Microsoft.AspNetCore.Mvc;

namespace RunGroops.Controllers {
    public class RaceController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
