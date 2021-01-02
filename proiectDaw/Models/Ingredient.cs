using System.ComponentModel.DataAnnotations;

namespace proiectDaw.Models
{
    public class Ingredient
    {
        [Key] public int Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Quantity { get; set; }

        [Required] public string Type { get; set; }

        [Required] public int RecipeId { get; set; }

        [Required] public Recipe Recipe { get; set; }
    }
}