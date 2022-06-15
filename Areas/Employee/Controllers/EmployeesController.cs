using FreeLancing.Models;
using FreeLancing.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeLancing.Areas.Employee.Controllers
{
    [Authorize(Roles ="Employee")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        public async Task<IActionResult> Index()
        {
            var jobs = await _employeeService.GetAvailableJobs(HttpContext.User.Identity.Name);
            return View(jobs);
        }
        public IActionResult BidNow(int jobId)
        {

            if (jobId == 0)
                return NotFound();
            var bid=new Bid() { JobId = jobId };
            return View(bid);
        }
        [HttpPost]
        [ActionName("BidNow")]
        public async Task<IActionResult> BidNowPost(Bid bid)
        {
            if(ModelState.IsValid)
            {
                var result= await _employeeService.AddNewBid(bid,HttpContext.User.Identity.Name);
                if(result)
                {
                    TempData["success"] = "Successfully Bidded";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(bid);
        }
    }
}
