using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MurkyPluse.Controllers;
using MurkyPluse.Models;

namespace MurkyPluse.Entities
{
    public class MurkyDbContext : IdentityDbContext<IdentityUser>
    {
        public MurkyDbContext(DbContextOptions<MurkyDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> Posts { get; set; }
        public DbSet<BlogCategory> Categories { get; set; }
        public DbSet<BlogComment>   Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Call the Seed method to populate initial data
            Seed(modelBuilder);
        }

        public static void Seed(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<BlogCategory>().HasData(
                new BlogCategory { Id = 1, Name = "Technology" },
                new BlogCategory { Id = 2, Name = "Health" },
                new BlogCategory { Id = 3, Name = "Lifestyle" }
            );

            // Seed Posts with static values for PublishedDate
            modelBuilder.Entity<BlogPost>().HasData(
                new BlogPost
                {
                    Id = 1,
                    Title = "Tech Post 1",
                    Content = "Content of Tech Post 1",
                    Author = "John Doe",
                    PublishedDate = new DateTime(2023, 1, 1), // Static date instead of DateTime.Now
                    CategoryId = 1,
                    FeatureImagePath = "tech_image.jpg", // Sample image path
                },
                new BlogPost
                {
                    Id = 2,
                    Title = "Health Post 1",
                    Content = "Content of Health Post 1",
                    Author = "Jane Doe",
                    PublishedDate = new DateTime(2023, 1, 1), // Static date
                    CategoryId = 2,
                    FeatureImagePath = "health_image.jpg", // Sample image path
                },
                new BlogPost
                {
                    Id = 3,
                    Title = "Lifestyle Post 1",
                    Content = "Content of Lifestyle Post 1",
                    Author = "Alex Smith",
                    PublishedDate = new DateTime(2023, 1, 1), // Static date
                    CategoryId = 3,
                    FeatureImagePath = "lifestyle_image.jpg", // Sample image path
                }
            );
        }

    }


}


