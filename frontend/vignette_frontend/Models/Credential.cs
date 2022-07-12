using System.ComponentModel.DataAnnotations;

namespace vignette_frontend.Models
{
    public class Credential
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
