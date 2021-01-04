using System;
using System.Collections.Generic;
using System.Linq;
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
        public string LoggedInUser => User.Identity.Name;
        
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
    }
}