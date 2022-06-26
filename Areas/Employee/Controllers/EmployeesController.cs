using FreeLancing.Models;
using FreeLancing.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeLancing.Areas.Employee.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IChattingService _chattingService;
        public EmployeesController(IEmployeeService employeeService, IChattingService chattingService)
        {
            _employeeService = employeeService;
            _chattingService = chattingService;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> AvialableJobs(string query)
        {
            if (query != null)
            {
                var queryJobs = await _employeeService.GetAvailableJobsSearch(HttpContext.User.Identity.Name, query);
                return View(queryJobs);
            }
            var jobs = await _employeeService.GetAvailableJobs(HttpContext.User.Identity.Name);
            return View(jobs);
        }

        public IActionResult InProgressJobs()
        {
            var inprogress = _employeeService.GetInProgressBids(HttpContext.User.Identity.Name);
            return View(inprogress);
        }
        public IActionResult BidNow(int jobId)
        {
            if (jobId == 0)
                return NotFound();
            var bid = new Bid() { JobId = jobId };
            return View(bid);
        }
        [HttpPost]
        [ActionName("BidNow")]
        public async Task<IActionResult> BidNowPost(Bid bid)
        {
            if (ModelState.IsValid)
            {
                var result = await _employeeService.AddNewBid(bid, HttpContext.User.Identity.Name);
                if (result)
                {
                    Bid x = _employeeService.GetBidById(bid.Id);
                    await _chattingService.SendNotificationToUser(bid.Job.OrganizationId, HttpContext.User.Identity.Name + " bidded on your job.");
                    TempData["success"] = "Successfully Bidded";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(bid);
        }
        public async Task<IActionResult> Bids()
        {
            var bids = await _employeeService.GetCurrentBids(HttpContext.User.Identity.Name);
            return View(bids);
        }
        public async Task<IActionResult> SubmitJob(int jobId)
        {
            var result =  _employeeService.SubmitJob(jobId);
            if(result!=null)
            {
                TempData["success"] = "Sucessfully Submitted";
                await _chattingService.SendNotificationToUser(result.OrganizationId, HttpContext.User.Identity.Name + " submitted your job "+result.Title);
            }
            return RedirectToAction(nameof(InProgressJobs));
        }
        public IActionResult Chat(string email)
        {
            if (_chattingService.UserExists(email))
            {
                return View(nameof(Chat), email);
            }
            return NotFound("User Not Found!");
        }
    }
}
