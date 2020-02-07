using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AppNiZiAPI.Models.Dietarymanagement
{
    public class DietaryManagementModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty("Description")]
        public string Description { get; set; }

        [Required]
        [JsonProperty("Amount")]
        public int Amount { get; set; }

        [Required]
        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [Required]
        [JsonProperty("Patient")]
        public int PatientId { get; set; }
    }
}
