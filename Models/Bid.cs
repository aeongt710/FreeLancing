using System.ComponentModel.DataAnnotations;

namespace FreeLancing.Models
{
    public class Bid
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Bid Amount")]
        public int BidAmount { get; set; }
        public string Description { get; set; }
        public string BidderId { get; set; }
        public ApplicationUser Bidder { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }
    }
}
