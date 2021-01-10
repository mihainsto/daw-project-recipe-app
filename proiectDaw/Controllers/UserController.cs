using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using proiectDaw.Data;
using proiectDaw.Models;

namespace proiectDaw.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController: ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpPost("/user/changeRole")]
        public Boolean ChangeRole()
        {
            var keys = Request.Form.Keys;
            var id = "";
            var role = "";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(usr => usr.Id == userId).First();
            if (user.Role != "ADMIN")
            {
                Problem("Only admins can change roles");
            }
            foreach (var key in keys)
            {
                var json = JObject.Parse(key);
                if (json.ContainsKey("id"))
                {
                    id = json["id"].ToString();
                }
                
                if (json.ContainsKey("role"))
                {
                    role = json["role"].ToString();
                }
            }

            var userToChange = _context.Users.Where(usr => usr.Id == id).First();
            userToChange.Role = role;

            _context.SaveChanges();
            return true;
        }
        
        [HttpPost("/user/get")]
        public List<ApplicationUser> Get()
        {
            var keys = Request.Form.Keys;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(usr => usr.Id == userId).First();
            if (user.Role != "ADMIN")
            {
                Problem("Only admins can change get users");
            }
            
            var users = _context.Users;
            return users.ToList();
        }
        
        [HttpPost("/user/getRole")]
        public object GetRole()
        {
            var keys = Request.Form.Keys;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(usr => usr.Id == userId).First();
            var obj = new {role = user.Role};
            return obj;
        }
    }
}