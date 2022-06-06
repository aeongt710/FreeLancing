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
    }
}
