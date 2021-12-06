using System.ComponentModel.DataAnnotations;

namespace OktaDemo.Models
{
    public class UpdateUserRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
