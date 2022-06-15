using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreeLancing.Models.ApiModels
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        [Column(Order = 1)]
        public string? SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        [Required]
        [Column(Order = 2)]
        public string? ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }
    }
}
