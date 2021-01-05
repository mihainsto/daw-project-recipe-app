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
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("/favorites/add")]
        public string Add()
        {
            var keys = Request.Form.Keys;
            var id = "";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(usr => usr.Id == userId).First();

            foreach (var key in keys)
            {
                var json = JObject.Parse(key);
                if (json.ContainsKey("id"))
                {
                    id = json["id"].ToString();
                }
            }

            var recipe = _context.Recipes.Where(rcp => rcp.Id.ToString() == id).First();

            if (user.Favorites == null)
            {
                user.Favorites = new List<Favorite>();
            }

            if (recipe.Favorites == null)
            {
                recipe.Favorites = new List<Favorite>();
            }

            var fav = new Favorite {Recipe = recipe, User = user};
            user.Favorites.Add(fav);
            recipe.Favorites.Add(fav);

            _context.SaveChanges();
            return id;
        }

        [HttpPost("/favorites/get")]
        public List<Int32> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(usr => usr.Id == userId).Include(usr => usr.Favorites).First();
            var favorites = _context.Favorites.Where(fav => fav.UserId == user.Id);
            var response = new List<Int32>();
            foreach (var userFavorite in favorites)
            {
                response.Add(userFavorite.RecipeId);
            }

            return response;
        }

        [HttpPost("/favorites/remove")]
        public string Remove()
        {
            var keys = Request.Form.Keys;
            var id = "";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(usr => usr.Id == userId).First();
        
            foreach (var key in keys)
            {
                var json = JObject.Parse(key);
                if (json.ContainsKey("id"))
                {
                    id = json["id"].ToString();
                }
            }
        
            var favorite = _context.Favorites
                .Where(fav => fav.RecipeId.ToString() == id && fav.UserId == user.Id).First();
            _context.Favorites.Remove(favorite);
        
            _context.SaveChanges();
            return id;
        }
    }
}