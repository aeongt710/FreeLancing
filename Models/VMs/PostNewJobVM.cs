using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FreeLancing.Models.VMs
{
    public class PostNewJobVM
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
        [Required]
        public int TagId { get; set; }
        [ValidateNever]
        public string OrganizationEmail { get; set; }
    }
}
