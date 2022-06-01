using FreeLancing.Models.VMs;
using System.Threading.Tasks;

namespace FreeLancing.Services.IServices
{
    public interface IAccountService
    {
        public Task<string> RegisterNewUser(RegisterVM registerVM);
    }
}
