using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace proiectDaw.Models
{
    public class Ingredient
    {
        [Key] public int Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Quantity { get; set; }

        [Required] public string Type { get; set; }

        [Required] public int RecipeId { get; set; }

        [JsonIgnore] [Required] public Recipe Recipe { get; set; }
    }
}