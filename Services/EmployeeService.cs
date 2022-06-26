using FreeLancing.Data;
using FreeLancing.Models;
using FreeLancing.Services.IServices;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FreeLancing.Services
{
    public class EmployeeService : IEmployeeService
    {
        public readonly ApplicationDbContext _dbContext;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public EmployeeService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
            , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = db;
        }

        public async Task<bool> AddNewBid(Bid bid, string bidderEmail)
        {
            var user = await _userManager.FindByNameAsync(bidderEmail);
            bid.BidderId = user.Id;
            _dbContext.Add(bid);
            var result = _dbContext.SaveChanges();
            if (result > 0)
                return true;
            return false;
        }

        public async Task<List<Job>> GetAvailableJobs(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            var bids = _dbContext.Bids
                .Where(a => a.BidderId == user.Id)
                    .Select(c => c.JobId)
                        .ToList();
            return _dbContext.Jobs
                .Include(a => a.Organization)
                    .Include(b => b.Tag)
                        .Include(d => d.JobBids)
                            .Where(c => (c.IsAssigned == false) && !bids.Contains(c.Id))
                                .ToList();
        }
        public async Task<List<Job>> GetAvailableJobsSearch(string email, string query)
        {
            var user = await _userManager.FindByNameAsync(email);
            var bids = _dbContext.Bids
                .Where(a => a.BidderId == user.Id)
                    .Select(c => c.JobId)
                        .ToList();
            return _dbContext.Jobs
                .Include(a => a.Organization)
                    .Include(b => b.Tag)
                        .Include(d => d.JobBids)
                            .Where(e => (e.Title.ToLower().Contains(query.ToLower()) || query.ToLower().Contains(e.Title.ToLower()) || e.Tag.TagText.ToLower().Contains(query.ToLower()) || query.ToLower().Contains(e.Tag.TagText.ToLower())))
                                .Where(c => (c.IsAssigned == false) && !bids.Contains(c.Id))
                                    .ToList();
        }

        public async Task<List<Bid>> GetCurrentBids(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            return _dbContext.Bids
                .Include(a => a.Job).ThenInclude(b => b.Organization)
                    .Include(c => c.Job).ThenInclude(d => d.Tag)
                        .Include(e => e.Bidder)
                            .Where(x => x.BidderId == user.Id && x.Job.IsAssigned==false).ToList();
        }
        public Bid GetBidById(int Id)
        {
            return _dbContext.Bids.Include(a => a.Job).FirstOrDefault(a => a.Id == Id);
        }

        public List<Bid> GetInProgressBids(string email)
        {
            return _dbContext.Bids.Include(b => b.Bidder)
                .Include(e => e.Job)
                    .ThenInclude(f => f.JobBids)
                        .Include(a => a.Job.Organization)
                            .Where(c => c.Bidder.Email == email && c.Job.IsAssigned == true &&  c.Job.IsCompleted==false)
                                .ToList();
        }

        public Job SubmitJob(int jobId)
        {
            var job = _dbContext.Jobs.FirstOrDefault(a => a.Id == jobId);
            job.IsSubmitted = true;
            _dbContext.Jobs.Update(job);
            var result = _dbContext.SaveChanges();
            if(result > 0)
                return job;
            return null;
        }

        public IList<Bid> GetAllNavOfBids(string bidderEmail)
        {
            return _dbContext.Bids.Include(a => a.Bidder).Include(c => c.Job)
                .Where(b => b.Bidder.Email == bidderEmail).ToList();
                
        }

        public IList<Job> GetCompletedJobs(string bidderEmail)
        {
            return _dbContext.Bids.Include(a => a.Bidder)
                .Include(c => c.Job)
                    .ThenInclude(x=>x.JobBids)
                        .Where(b => b.Bidder.Email == bidderEmail&&b.Job.IsCompleted==true)
                            .Select(a=>a.Job)
                                .ToList();
        }
    }
}
