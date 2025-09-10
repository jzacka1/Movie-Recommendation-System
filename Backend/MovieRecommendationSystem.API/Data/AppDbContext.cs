using Microsoft.EntityFrameworkCore;
using MovieRecommendationSystem.API.Models;
using System.Collections.Generic;

namespace MovieRecommendationSystem.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
