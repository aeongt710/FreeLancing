using FreeLancing.Data;
using FreeLancing.Models;
using FreeLancing.Models.VMs;
using FreeLancing.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FreeLancing.Services
{
    public class JobService : IJobService
    {
        private readonly ApplicationDbContext _dbContext;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public JobService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
            , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public IList<Job> GetPostedJobsNotAssigned(string organizationEmail)
        {
            return _dbContext.Jobs
                .Include(a => a.Organization)
                    .Include(c => c.Tag)
                        .Include(d => d.JobBids)
                            .Where(b => (b.Organization.Email == organizationEmail && b.IsAssigned == false))
                                .ToList();
        }

        public IList<Job> GetInProgressJobs(string organizationEmail)
        {
            return _dbContext.Jobs
                .Include(a => a.Organization)
                    .Include(c => c.Tag)
                        .Include(d => d.JobBids)
                            .ThenInclude(e=>e.Bidder)
                                .Where(b => (b.Organization.Email == organizationEmail && b.IsAssigned == true && b.IsCompleted== false))
                                    .ToList();
        }

        public IList<CustomTag> GetTagList()
        {
            return _dbContext.CustomTags.ToList();
        }

        public async Task<bool> PostNewJob(PostNewJobVM postNewJobVM)
        {
            var user = await _userManager.FindByEmailAsync(postNewJobVM.OrganizationEmail);
            _dbContext.Add(new Job()
            {
                Title = postNewJobVM.Title,
                TagId = postNewJobVM.TagId,
                Description = postNewJobVM.Description,
                Salary = postNewJobVM.Salary,
                OrganizationId = user.Id,
                Durtion = postNewJobVM.Durtion,
                IsAssigned = false,
                IsCompleted = false,
                IsSubmitted = false

            });
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
                return true;
            return false;
        }
        public IList<Bid> GetBidsOnJob(int jobId)
        {
            return _dbContext.Jobs
                .Include(a => a.Organization)
                    .Include(c => c.Tag)
                        .Include(d => d.JobBids)
                            .Where(b => (b.Id == jobId))
                                .Include(e => e.JobBids)
                                    .ThenInclude(f => f.Bidder)
                                        .Include(h => h.JobBids)
                                            .ThenInclude(g => g.Job)
                                                .Select(e => e.JobBids)
                                                    .FirstOrDefault();
        }

        public Bid ApproveBid(int bidId)
        {
            var bid = _dbContext.Bids.FirstOrDefault(a => a.Id == bidId);
            var deleteBids = _dbContext.Bids.Where(a => a.JobId == bid.JobId && a.Id != bid.Id).ToList();
            if (deleteBids != null)
                _dbContext.Bids.RemoveRange(deleteBids);
            var job = _dbContext.Jobs.FirstOrDefault(a => a.Id == bid.JobId);
            job.IsAssigned = true;
            var result = _dbContext.SaveChanges();
            if (result > 0)
                return _dbContext.Bids.Include(a => a.Job).FirstOrDefault(a => a.Id == bidId);
            return null;
        }
        public Job MarkComplete(int jobId)
        {
            var job = _dbContext.Jobs.Include(a=>a.JobBids).FirstOrDefault(a => a.Id == jobId);
            job.IsCompleted = true;
            _dbContext.Jobs.Update(job);
            var result = _dbContext.SaveChanges();
            if (result > 0)
                return job;
            return null;
        }
        public IList<Job> GetCompleted(string organizationEmail)
        {
            return _dbContext.Jobs
                .Include(a => a.Organization)
                    .Include(c => c.Tag)
                        .Include(d => d.JobBids)
                            .ThenInclude(e => e.Bidder)
                                .Where(b => (b.Organization.Email == organizationEmail && b.IsAssigned == true && b.IsCompleted == true))
                                    .ToList();
        }
        public IList<Job> GetAllNavOfJobs(string organizationEmail)
        {
            return _dbContext.Jobs
                .Include(a => a.Organization)
                    .Include(c => c.Tag)
                        .Include(d => d.JobBids)
                            .ThenInclude(e => e.Bidder)
                                .Where(b => (b.Organization.Email == organizationEmail))
                                    .ToList();
        }
    }
}
