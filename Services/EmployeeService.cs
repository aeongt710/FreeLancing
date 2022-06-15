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
            var user =await _userManager.FindByNameAsync(bidderEmail);
            bid.BidderId = user.Id;
            _dbContext.Add(bid);
            var result=_dbContext.SaveChanges();
            if(result >0)
                return true;
            return false;
        }

        public async Task<List<Job>> GetAvailableJobs(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            var bids = _dbContext.Bids
                .Where(a=>a.BidderId==user.Id)
                    .Select(c => c.JobId)
                        .ToList(); 
            return _dbContext.Jobs
                .Include(a => a.Organization)
                    .Include(b => b.Tag)
                        .Include(d => d.JobBids)
                            .Where(c => (c.IsAssigned == false) && !bids.Contains(c.Id))
                                .ToList();
        }


    }
}
