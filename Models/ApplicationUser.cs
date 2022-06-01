using Microsoft.AspNetCore.Identity;

namespace FreeLancing.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public string ImgSrc { get; set; }
    }
}
