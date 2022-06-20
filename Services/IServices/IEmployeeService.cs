using FreeLancing.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeLancing.Services.IServices
{
    public interface IEmployeeService
    {
        public Task<bool> AddNewBid(Bid bid, string bidderEmail);
        public Task<List<Job>> GetAvailableJobs(string email);
        public Task<List<Job>> GetAvailableJobsSearch(string email, string query);
        public Task<List<Bid>> GetCurrentBids(string email);
    }
}
