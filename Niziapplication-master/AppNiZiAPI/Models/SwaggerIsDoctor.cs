using System.ComponentModel.DataAnnotations;

namespace AppNiZiAPI.Models
{
    class SwaggerIsDoctor
    {
        [Required]
        public int PatientId { get; set; }
        public string Role { get; set; }
    }
}
