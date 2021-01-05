using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using proiectDaw.Data;

namespace proiectDaw.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Recipe> Favorites { get; set; }
        

    }
}
