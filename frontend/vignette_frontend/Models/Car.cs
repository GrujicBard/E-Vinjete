
using System.ComponentModel.DataAnnotations;

namespace vignette_frontend.Models
{
    public class Car
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public string Registration { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Country { get; set; }

    }
}
