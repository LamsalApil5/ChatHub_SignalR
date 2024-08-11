using System.ComponentModel.DataAnnotations;

namespace HostelSoftware.Models
{
    public class UserRegistrationViewModel
    {
        [Required]
        public string Fullname { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string? Number { get; set; }
    }
}
