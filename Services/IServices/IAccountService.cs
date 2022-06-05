using FreeLancing.Models.VMs;
using System.Threading.Tasks;

namespace FreeLancing.Services.IServices
{
    public interface IAccountService
    {
        public Task<string> RegisterNewUser(RegisterVM registerVM);
        public Task<bool> Login(LoginVM loginVM);
        public Task Logout();
    }
}
