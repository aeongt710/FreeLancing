using FreeLancing.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeLancing.Services.IServices
{
    public interface IEmployeeService
    {
        public Task<bool> AddNewBid(Bid bid, string bidderEmail);
        public List<Job> GetAvailableJobs(string email);
    }
}
