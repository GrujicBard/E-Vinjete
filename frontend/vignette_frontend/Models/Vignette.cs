using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vignette_frontend.Models
{
    public class Vignette
    {
        public string userId { get; set; }
        [Required]
        [Display(Name = "Registration")]
        public string registration { get; set; }
        [Required]
        public string type { get; set; }
        public string dateCreated { get; set; }
        public string dateValid { get; set; }
    }
}
