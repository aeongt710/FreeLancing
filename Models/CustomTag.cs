﻿using System.ComponentModel.DataAnnotations;

namespace FreeLancing.Models
{
    public class CustomTag
    {
        public int Id { get; set; }
        [Required]
        public string TagText { get; set; }
    }
}
