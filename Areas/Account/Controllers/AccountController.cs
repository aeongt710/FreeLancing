using FreeLancing.Models.VMs;
using FreeLancing.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeLancing.Areas.Account.Controllers
{
    public class AccountController : Controller
    {
        private IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IActionResult LoginSignup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginSignup(RegisterVM registerVM)
        {

            if (ModelState.IsValid)
            {
                string result = await _accountService.RegisterNewUser(registerVM);
                if (result != null)
                    TempData["error"] = result.ToString();
                else
                    return RedirectToAction("Index", "Homepage", new { area = "Homepage" });
            }
            return View(registerVM);
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberme)
        {
            var result = await _accountService.Login(new LoginVM() { Email = email, Password = password, RememberMe = rememberme });
            if (result)
                return RedirectToAction("Index", "Homepage", new { area = "Homepage" });
            else
                TempData["error"] = "Login Failed";
            return RedirectToAction(nameof(LoginSignup));
        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await _accountService.Logout();
            return RedirectToAction("Index", "Homepage", new { area = "Homepage" });
        }
    }
}
