using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AppNiZiAPI.Models.Dietarymanagement
{
    public class DietaryRestriction
    {
        [Required]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty("Description")]
        public string Description { get; set; }
    }
}
