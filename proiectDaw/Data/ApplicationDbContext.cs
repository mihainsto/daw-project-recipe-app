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
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Ingredient>()
                .HasOne(p => p.Recipe)
                .WithMany(b => b.Ingredients)
                .HasForeignKey(p => p.RecipeId);
            
            modelBuilder.Entity<Review>()
                .HasOne(p => p.Recipe)
                .WithMany(b => b.Reviews)
                .HasForeignKey(p => p.RecipeId);
        }
        
        public DbSet<Recipe> Recipes { get; set; }
        
        public DbSet<Ingredient> Ingredients { get; set; }
        
        public DbSet<Review> Reviews { get; set; }
    }
}
