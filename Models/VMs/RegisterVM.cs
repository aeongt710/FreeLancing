using System.ComponentModel.DataAnnotations;

namespace FreeLancing.Models.VMs
{
    public class RegisterVM
    {
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Enter strong password!", MinimumLength = 6)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password Does Not Match!")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Role")]
        public bool isIndividual { get; set; }
    }
}
