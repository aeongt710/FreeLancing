using FreeLancing.Models;
using FreeLancing.Models.VMs;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeLancing.Services.IServices
{
    public interface IJobService
    {
        public Task<bool> PostNewJob(PostNewJobVM postNewJobVM);
        public IList<CustomTag> GetTagList();
        public IList<Job> GetPostedJobsNotAssigned(string organizationEmail);
        public IList<Job> GetInProgressJobs(string organizationEmail);

        public IList<Bid> GetBidsOnJob(int jobId);
        public Bid ApproveBid(int bidId);
        public Job MarkComplete(int jobId);
        public IList<Job> GetCompleted(string organizationEmail);
        public IList<Job> GetAllNavOfJobs(string organizationEmail);
    }
}
