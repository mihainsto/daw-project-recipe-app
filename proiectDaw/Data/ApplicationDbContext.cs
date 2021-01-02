using proiectDaw.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace proiectDaw.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     
        //     modelBuilder.Entity<Ingredient>()
        //         .HasOne(p => p.Recipe)
        //         .WithMany(b => b.Ingredients);
        // }
        
        public DbSet<Recipe> Recipes { get; set; }
        
        public DbSet<Ingredient> Ingredients { get; set; }
    }
}
