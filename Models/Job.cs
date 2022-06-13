using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeLancing.Models
{
    public class Job
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(10, int.MaxValue)]
        public int Salary { get; set; }
        [Required]
        public string Durtion { get; set; }
        public bool IsAssigned { get; set; }
        public string SubmittedText { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsCompleted { get; set; }
        public int TagId { get; set; }
        public CustomTag Tag { get; set; }
        public string OrganizationId { get; set; }
        public ApplicationUser Organization { get; set; }

        public List<Bid> JobBids { get; set; }
    }
}
