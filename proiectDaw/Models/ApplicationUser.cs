using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using proiectDaw.Data;

namespace proiectDaw.Models
{
    public class ApplicationUser : IdentityUser
    {
        [JsonIgnore]  public ICollection<Favorite> Favorites { get; set; }
        
    }
}
