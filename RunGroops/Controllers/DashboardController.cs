using Microsoft.AspNetCore.Mvc;

namespace RunGroops.Controllers {
    public class DashboardController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
