using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RunGroops.Helpers;
using RunGroops.Interfaces;
using RunGroops.Models;
using RunGroops.ViewModels;
using System.Diagnostics;
using System.Globalization;
using System.Net;

namespace RunGroops.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IClubRepository _clubRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IClubRepository clubRepository, IHttpContextAccessor httpContextAccessor) {
            _logger = logger;
            this._clubRepository = clubRepository;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index() {
            var ipInfo = new IPInfo();
            var homeViewModel = new HomeViewModel();
            bool isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
            try {
                string url = "https://ipinfo.io?token=db2790fdc3ef2d";
                var info = new WebClient().DownloadString(url);
                ipInfo = JsonConvert.DeserializeObject<IPInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
                homeViewModel.Country = ipInfo.Country;
                homeViewModel.City = ipInfo.City;
                homeViewModel.State = ipInfo.Region;
                homeViewModel.IsAuthenticated = isAuthenticated;
                if (homeViewModel.City != null) {
                    homeViewModel.Clubs = await _clubRepository.GetClubByCity(homeViewModel.City);
                }
                else {
                    homeViewModel.Clubs = null;
                }
                return View(homeViewModel);
            }
            catch (Exception e) {
                homeViewModel.Clubs = null;
            }
            return View(homeViewModel);
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}