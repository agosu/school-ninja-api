﻿using SchoolNinjaAPI.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace SchoolNinjaAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(2)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(2)]
        public string Lastname { get; set; }

        [Required]
        [RegularExpression(Validation.Email)]
        public string Email { get; set; }

        [Required]
        [RegularExpression("student|teacher")]
        public string Type { get; set; }

        [IntegerValidator(MinValue = 1, MaxValue = 12)]
        public int? Grade { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
