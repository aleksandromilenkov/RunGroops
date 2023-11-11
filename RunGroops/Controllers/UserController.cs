using Microsoft.AspNetCore.Mvc;
using RunGroops.Interfaces;
using RunGroops.ViewModels;

namespace RunGroops.Controllers {
    public class UserController : Controller {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository) {
            this._userRepository = userRepository;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index() {
            var users = await _userRepository.GetAllUsers();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users) {
                var userViewModel = new UserViewModel() {
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    Mileage = user.Mileage,
                    ProfileImageUrl = user.ProfileImageUrl
                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        public async Task<IActionResult> Detail(string id) {
            var user = await _userRepository.GetUserById(id);
            var userDetailViewModel = new UserDetailViewModel() {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Mileage = user.Mileage
            };
            return View(userDetailViewModel);
        }
    }
}
