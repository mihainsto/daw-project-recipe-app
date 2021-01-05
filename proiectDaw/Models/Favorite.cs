using System.ComponentModel.DataAnnotations;

namespace proiectDaw.Models
{
    public class Favorite
    {
        [Key] public int Id { get; set; } 
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}