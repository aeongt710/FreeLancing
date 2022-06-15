using System;
using System.ComponentModel.DataAnnotations;

namespace FreeLancing.Models.ApiModels
{
    public class GlobalMessage
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
    }
}
