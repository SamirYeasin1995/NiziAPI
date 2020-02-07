using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppNiZiAPI.Models.Dietarymanagement
{
    public class DietaryView
    {
        [Required]
        [JsonProperty("Restrictions")]
        public List<DietaryRestriction> restrictions { get; set; }

        [Required]
        [JsonProperty("Dietarymanagement")]
        public List<DietaryManagementModel> DietaryManagements { get; set; }
    }
}
