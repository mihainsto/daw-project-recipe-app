using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost("/recipe/recipes")]
        public List<Recipe> Recipes()
        {
            var keys = Request.Form.Keys;
            foreach (var key in keys)
            {
                Console.WriteLine(key);
            }
            
            var recipesDB = _context.Recipes.Include(e => e.Ingredients);

            return recipesDB.ToList();
        }

        [HttpGet("/recipe/create")]
        public int Create()
        {
            return 10;
        }
    }
}