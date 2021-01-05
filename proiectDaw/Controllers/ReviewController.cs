using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using proiectDaw.Data;
using proiectDaw.Models;

namespace proiectDaw.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost("/review/reviews")]
        public List<Review> Reviews()
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

            var reviewsDB = _context.Reviews.Where(rv => rv.RecipeId.ToString() == id);
            return reviewsDB.ToList();
        }

        [Authorize]
        [HttpPost("/review/create")]
        public Int32 Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(usr => usr.Id == userId).First();

            var keys = Request.Form.Keys;
            List<Review> reviews = new List<Review>();
            foreach (var key in keys)
            {
                var json = JObject.Parse(key);
                var reviewText = json["reviewText"].ToString();
                Console.WriteLine(reviewText);
                var recipeId = json["recipeId"].ToString();
                var recipeDb = _context.Recipes.Where(r => r.Id.ToString() == recipeId)
                    .Include(r => r.Reviews).First();
                
                var review = new Review {userEmail = user.Email, Text = reviewText};

                recipeDb.Reviews.Add(review);
                _context.SaveChanges();
                
                return review.Id;
            };

            return 0;
        }
        
        [HttpPost("/review/update")]
        public  Int32 Update()
        {
            var keys = Request.Form.Keys;
            var id = "";
            var newText = "";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(usr => usr.Id == userId).First();
            
            foreach (var key in keys)
            {
                var json = JObject.Parse(key);
                if (json.ContainsKey("id"))
                {
                    id = json["id"].ToString();
                }
                if (json.ContainsKey("reviewText"))
                {
                    newText = json["reviewText"].ToString();
                }
            }

            var reviewsDB = _context.Reviews.Where(rv => rv.Id.ToString() == id);
            var review = reviewsDB.First();
            
            if (review.userEmail != user.Email)
            {
                return 0;
            }
            
            if (newText != "")
            {
                review.Text = newText;

            }
            
            _context.SaveChanges();
            return review.Id;
        }
        
        [HttpPost("/review/delete")]
        public  Int32 Delete()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(usr => usr.Id == userId).First();
            
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

            var reviewsDB = _context.Reviews.Where(rv => rv.Id.ToString() == id);
            var review = reviewsDB.First();

            if (review.userEmail != user.Email)
            {
                return 0;
            }
            _context.Reviews.Remove(review);
            _context.SaveChanges();
            return review.Id;
            
        }
    }
}