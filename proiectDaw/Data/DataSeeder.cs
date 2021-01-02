using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using proiectDaw.Models;

namespace proiectDaw.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DataSeeder> _logger;

        public DataSeeder(ApplicationDbContext context, ILogger<DataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Seed()
        {
            var recipeDb = _context.Recipes.Find(22);

            Console.WriteLine("Seeding database");
            _logger.LogInformation("Seeding database");

            // Cleaning the db
            _context.Recipes.RemoveRange(_context.Recipes);
            _context.SaveChanges();

            var rcp = new Recipe();
            rcp.Name = "Simple Carbonara";
            rcp.Description =
                "Humble ingredients—eggs, noodles, cheese, and pork—combine to create glossy, glorious pasta carbonara. It's the no-food-in-the-house dinner of our dreams.";
            rcp.Kcal = 400;
            rcp.Difficulty = "EASY";
            rcp.Time = 40;
            rcp.MealType = "Pasta";
            
            var ing = new Ingredient();
            ing.Name = "Parmesan";
            ing.Quantity = "2 oz";
            ing.Recipe = rcp;

            rcp.Ingredients = new List<Ingredient>();
            rcp.Ingredients.Add(ing);

            _context.Recipes.Add(rcp);
            _context.Ingredients.Add(ing);
            _context.SaveChanges();
            //Seeding the db
            Console.WriteLine("Database seeded");


            _logger.LogInformation($"Database seeded");
        }
    }
}