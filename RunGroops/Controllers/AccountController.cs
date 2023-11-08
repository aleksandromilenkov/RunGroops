using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroops.Data;
using RunGroops.Models;
using RunGroops.ViewModels;

namespace RunGroops.Controllers {
    public class AccountController : Controller {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        public IActionResult Login() {
            var response = new LoginViewModel(); // if you reload the page while trying to log in, the data in the inputs will be restored
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel) {
            if (!ModelState.IsValid) {
                return View(loginViewModel);
            }
            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);
            if (user != null) {
                // User is found, check the pass:
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck) {
                    // pass is correct, sign the user in:
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded) {
                        return RedirectToAction("Index", "Race");
                    }
                }
                // pass is incorect:
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(loginViewModel);
            }
            // User not found:
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginViewModel);
        }
    }
}
