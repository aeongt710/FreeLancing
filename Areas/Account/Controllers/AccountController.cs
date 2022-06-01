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
                string result=await _accountService.RegisterNewUser(registerVM);
                if (result != null)
                    TempData["error"] = result.ToString();
                else
                    return RedirectToAction("Index", "Homepage", new { area = "Homepage" });
            }
            return View(registerVM);
        }
    }
}
