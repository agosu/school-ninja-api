using System.ComponentModel.DataAnnotations;

namespace SchoolNinjaAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }
    }
}
