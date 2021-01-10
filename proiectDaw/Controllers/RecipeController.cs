using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using proiectDaw.Data;
using proiectDaw.Models;

namespace proiectDaw.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecipeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        [HttpPost("/recipe/recipes")]
        public List<Recipe> Recipes()
        {
            var keys = Request.Form.Keys;
            var query = "";
            List<string> ingredientTypes = new List<string>();

            foreach (var key in keys)
            {
                var json = JObject.Parse(key);
                if (json.ContainsKey("query"))
                {
                    query = json["query"].ToString();
                }

                if (json.ContainsKey("ingredientTypes"))
                {
                    Dictionary<string, string> dictObj = json["ingredientTypes"].ToObject<Dictionary<string, string>>();

                    foreach (var keyValuePair in dictObj)
                    {
                        if (keyValuePair.Value == "True")
                        {
                            ingredientTypes.Add(keyValuePair.Key);
                        }
                    }
                }
            }
            var recipesDB =
                _context.Recipes.Include(e => e.Ingredients)
                    .Where(rcp => rcp.Name.ToLower().Contains(query.ToLower()))
                    .Where(rcp =>
                        rcp.Ingredients.Any(ing => ingredientTypes.Contains(ing.Type)
                                                   || ingredientTypes.Count == 0));

            return recipesDB.ToList();
        }

        [HttpPost("/recipe/recipe")]
        public Recipe Recipe()
        {
            var keys = Request.Form.Keys;
            var id = "";

            foreach (var key in keys)
            {
                var json = JObject.Parse(key);
                if (json.ContainsKey("id"))
                {
                    id = json["id"].ToString();
                }
            }

            var recipesDB =
                _context.Recipes.Include(e => e.Ingredients)
                    .Where(rcp => rcp.Id.ToString() == id);

            return recipesDB.First();
        }

        [Authorize]
        [RequestFormLimits(KeyLengthLimit = 30_000, ValueCountLimit = 1500, ValueLengthLimit = 100_000)]
        [HttpPost("/recipe/update")]
        public Boolean Update()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(usr => usr.Id == userId).First();
            if (user.Role != "ADMIN")
            {
                Problem("Only admins can do this");
            }
            
            var keys = Request.Form.Keys;
            
            foreach (var key in keys)
            {
                var json = JObject.Parse(key);
                var recipesDB =
                    _context.Recipes.Include(e => e.Ingredients)
                        .Where(rcp => rcp.Id.ToString() == json["id"].ToString());
                var recipe = recipesDB.First();

                var name = json["name"].ToString();
                var description = json["description"].ToString();
                var difficulty = json["difficulty"].ToString();
                var kcal = json["kcal"].ToObject<int>();
                var steps = json["steps"].ToObject<string[]>();
                var time = json["time"].ToObject<int>();
                var mealType = json["mealType"].ToString();
                var ingredientsNames = json["ingredientsName"].ToObject<string[]>();
                var ingredientsQuantity = json["ingredientsQuantity"].ToObject<string[]>();
                var ingredientsTypes = json["ingredientsTypes"].ToObject<string[]>();
                var imageUrl = json["coverPhotoUrl"];

                recipe.Name = name;
                recipe.Description = description;
                recipe.Difficulty = difficulty;
                recipe.Kcal = kcal;
                recipe.Steps = steps;
                recipe.Time = time;
                recipe.MealType = mealType;
                if (imageUrl != null)
                {
                    var convertedPhoto = imageUrl.ToString().Replace("EQUAL", "=");
                    recipe.CoverPhotoUrl = Base64Decode(convertedPhoto);
                }

                for (int i = 0; i < ingredientsNames.Length; i++)
                {
                    recipe.Ingredients[i].Name = ingredientsNames[i];
                    recipe.Ingredients[i].Quantity = ingredientsQuantity[i];
                    recipe.Ingredients[i].Type = ingredientsTypes[i];
                }

                if (ingredientsNames.Length > recipe.Ingredients.Count)
                {
                    for (int i = recipe.Ingredients.Count; i < ingredientsNames.Length; i++)
                    {
                        var ing = new Ingredient
                            {Name = ingredientsNames[i], Quantity = ingredientsQuantity[i], Type = ingredientsTypes[i]};
                        recipe.Ingredients.Add(ing);
                    }
                }

                if (ingredientsNames.Length < recipe.Ingredients.Count)
                {
                    recipe.Ingredients.RemoveRange(ingredientsNames.Length,
                        recipe.Ingredients.Count - ingredientsNames.Length);
                }


                _context.SaveChanges();

                return true;
            }


            return true;
        }

        [Authorize]
        [RequestFormLimits(KeyLengthLimit = 30_000, ValueCountLimit = 1500, ValueLengthLimit = 100_000)]
        [HttpPost("/recipe/create")]
        public Boolean Create()
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(usr => usr.Id == userId).First();
            if (user.Role != "ADMIN")
            {
                Problem("Only admins can do this");
            }
            
            var keys = Request.Form.Keys;

            foreach (var key in keys)
            {
                var json = JObject.Parse(key);

                var name = json["name"].ToString();
                var description = json["description"].ToString();
                var difficulty = json["difficulty"].ToString();
                var kcal = json["kcal"].ToObject<int>();
                var steps = json["steps"].ToObject<string[]>();
                var time = json["time"].ToObject<int>();
                var mealType = json["mealType"].ToString();
                var ingredientsNames = json["ingredientsName"].ToObject<string[]>();
                var ingredientsQuantity = json["ingredientsQuantity"].ToObject<string[]>();
                var ingredientsTypes = json["ingredientsTypes"].ToObject<string[]>();
                var imageUrl = json["coverPhotoUrl"];

                var recipe = new Recipe();
                recipe.Name = name;
                recipe.Description = description;
                recipe.Difficulty = difficulty;
                recipe.Kcal = kcal;
                recipe.Steps = steps;
                recipe.Time = time;
                recipe.MealType = mealType;
                recipe.Ingredients = new List<Ingredient>();
                recipe.Reviews = new List<Review>();
                if (imageUrl != null)
                {
                    var convertedPhoto = imageUrl.ToString().Replace("EQUAL", "=");
                    recipe.CoverPhotoUrl = Base64Decode(convertedPhoto);
                }

                for (int i = 0; i < ingredientsNames.Length; i++)
                {
                    var ing = new Ingredient
                        {Name = ingredientsNames[i], Quantity = ingredientsQuantity[i], Type = ingredientsTypes[i]};
                    recipe.Ingredients.Add(ing);
                }

                _context.Add(recipe);
                _context.SaveChanges();
                return true;
            }

            return true;
        }

        [Authorize]
        [HttpPost("/recipe/delete")]
        public Boolean Delete()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(usr => usr.Id == userId).First();
            if (user.Role != "ADMIN")
            {
                Problem("Only admins can do this");
            }
            var keys = Request.Form.Keys;
            var id = "";

            foreach (var key in keys)
            {
                var json = JObject.Parse(key);
                if (json.ContainsKey("id"))
                {
                    id = json["id"].ToString();
                }
            }

            var recipesDB =
                _context.Recipes.Include(e => e.Ingredients)
                    .Where(rcp => rcp.Id.ToString() == id);

            _context.Recipes.Remove(recipesDB.First());
            _context.SaveChanges();
            return true;
        }
    }
}