using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace proiectDaw.Models
{
    public class Review
    {
        [Key] public int Id { get; set; }

        [Required] public string Text { get; set; }
        
        [Required] public int RecipeId { get; set; }

        [JsonIgnore] [Required] public Recipe Recipe { get; set; }
    }
}