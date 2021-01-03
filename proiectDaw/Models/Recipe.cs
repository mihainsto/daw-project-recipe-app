using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace proiectDaw.Models
{
    public class Recipe
    {
        [Key] public int Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Description { get; set; }

        [Required] public int Time { get; set; }

        [Required] public int Kcal { get; set; }

        [Required] public string MealType { get; set; }

        [Required] public string Difficulty { get; set; }

        [Required] public string[] Steps { get; set; }

        [Required] public List<Ingredient> Ingredients { get; set; }

        [JsonProperty(PropertyName = "url")] public string CoverPhotoUrl { get; set; }
    }
}