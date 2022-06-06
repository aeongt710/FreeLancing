using FreeLancing.Data;
using FreeLancing.Models;
using FreeLancing.Models.VMs;
using FreeLancing.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
                IsAssigned =false,
                IsCompleted=false,
                IsSubmitted=false
                
            });
            var result=await _dbContext.SaveChangesAsync();
            if (result > 0)
                return true;
            return false;
        }
    }
}
