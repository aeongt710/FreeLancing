using FreeLancing.Data;
using FreeLancing.Models;
using FreeLancing.Models.VMs;
using FreeLancing.Services.IServices;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FreeLancing.Services
{
    public class AccountService : IAccountService
    {
        public readonly ApplicationDbContext _dbContext;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public AccountService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
            , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = db;
        }

        public async Task<bool> Login(LoginVM vm)
        {

            var signInResult = await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, vm.RememberMe, false);
            if (signInResult.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> RegisterNewUser(RegisterVM registerVM)
    {
        checkRoles();
        var _user = new ApplicationUser
        {
            Email = registerVM.Email,
            FullName = registerVM.Name,
            UserName = registerVM.Email
        };
        var result = _userManager.CreateAsync(_user, registerVM.Password).Result;
        if (result.Succeeded)
        {
            if (registerVM.isIndividual)
                await _userManager.AddToRoleAsync(_user, Utility.Helper.RoleEmployee);
            else
                await _userManager.AddToRoleAsync(_user, Utility.Helper.RoleOrganization);
            await _signInManager.SignInAsync(_user, isPersistent: false);

        }
        else
        {
            string output = string.Empty;
            foreach (var item in result.Errors)
            {
                output = output + item.Description;
            }
            return output;
        }


        return null;
    }
    private void checkRoles()
    {
        if (!_roleManager.RoleExistsAsync(Utility.Helper.RoleEmployee).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(Utility.Helper.RoleEmployee)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Utility.Helper.RoleOrganization)).GetAwaiter().GetResult();
        }
    }

}
}
