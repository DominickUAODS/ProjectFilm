using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ProjectFilm.Model;

namespace ProjectFilm.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {

        }

    


        public DbSet<User> Users { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet <Review> Reviews { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(e => e.RegisterDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<ChatMessage>().Property(e => e.Date).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>()
               .HasMany(m => m.Messages)
               .WithOne(u => u.User);
            modelBuilder.Entity<User>()
                .HasMany(r => r.Reviews)
                .WithOne(u=>u.user);
        }

    }
}
