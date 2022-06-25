﻿using FreeLancing.Models;
using FreeLancing.Models.VMs;
using FreeLancing.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeLancing.Areas.Organization.Controllers
{
    [Authorize(Roles ="Organization")]
    public class JobsController : Controller
    {
        private readonly IJobService _jobService;
        private readonly IChattingService _chattingService;

        public JobsController(IJobService jobService, IChattingService chattingService)
        {
            _jobService = jobService;
            _chattingService = chattingService;
        }

        public IActionResult Index()
        {
            var postedJobs=_jobService.GetPostedJobs(HttpContext.User.Identity.Name);
            TempData["success"] = "Index Loaded";
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

        public IActionResult BidsOnJob(int jobId)
        {
            List<Bid> bids=new List<Bid>();
            bids = (List<Bid>)_jobService.GetBidsOnJob(jobId);
            return View(bids);
        }
        public IActionResult Chat(string email)
        {
            if (_chattingService.UserExists(email))
            {
                return View(nameof(Chat), email);
            }
            return NotFound("User Not Found!");
        }
        [HttpPost]
        public async Task<IActionResult> ApproveBidAsync(int bidId)
        {
            var result = _jobService.ApproveBid(bidId);
            if(result!=null)
            {
                await _chattingService.SendNotificationToUser(result.BidderId, HttpContext.User.Identity.Name + " approved your bid on " + result.Job.Title);
                TempData["success"] = "Job Approved Sucessfully";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(BidsOnJob),new {jobId = result.JobId});
        }
        public IActionResult test()
        {
            return View();
        }

    }
}
