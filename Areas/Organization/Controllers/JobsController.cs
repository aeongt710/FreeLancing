using FreeLancing.Models.VMs;
using FreeLancing.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace FreeLancing.Areas.Organization.Controllers
{
    [Authorize(Roles ="Organization")]
    public class JobsController : Controller
    {
        private readonly IJobService _jobService;
        public JobsController(IJobService jobService)
        {
            _jobService=jobService;
        }

        public IActionResult Index()
        {
            var postedJobs=_jobService.GetPostedJobs(HttpContext.User.Identity.Name);
            TempData["success"] = "Job Posted Sucessfully";
            return View(postedJobs);
        }
        public IActionResult PostNewJob()
        {
            ViewBag.TagList = _jobService.GetTagList().Select(p => new SelectListItem { Text = p.TagText, Value = p.Id.ToString() }).ToList(); 
            return View();
        }
        [HttpPost]
        [ActionName("PostNewJob")]
        public async Task<IActionResult> PostNewJobAsync(PostNewJobVM postNewJobVM)
        {
            if(ModelState.IsValid)
            {
                postNewJobVM.OrganizationEmail = HttpContext.User.Identity.Name;
                var result=await _jobService.PostNewJob(postNewJobVM);
                if (result)
                {
                    TempData["success"] = "Job Posted Sucessfully";
                    return RedirectToAction(nameof(Index));
                }
                    
            }
            ViewBag.TagList = _jobService.GetTagList().Select(p => new SelectListItem { Text = p.TagText, Value = p.Id.ToString() }).ToList();
            return View();
        }

        public IActionResult test()
        {

            return View();
        }

    }
}
