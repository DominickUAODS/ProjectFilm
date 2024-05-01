using Microsoft.EntityFrameworkCore.Storage;
using ProjectFilm.Helpers;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ProjectFilm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ProjectFilm.Data
{
    public class DbInit
    {

        public static DbContextOptions<ApplicationDbContext> ConnectToJason() //new
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("Connection.json");
            var config = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var options = optionsBuilder
                .UseSqlServer(config.GetConnectionString("DefaultConnection"))
                .Options;
            return options; 
        }
        public static async Task EnsurePopulate()
        {
            var context = new ApplicationDbContext(ConnectToJason());
            if (!(context.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
            {
                
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            if (!await context.Users.AnyAsync())
            {
                string salt = SecurityHelper.GenerateSalt(70);
                string hashedPassword = SecurityHelper.HashPassword("qwerty", salt, 10101, 70);
                context.Users.Add(new User
                {
                    UserName = "Admin",
                    Email = "admin@gmail.com",
                    Salt = salt,
                    HashedPassword = hashedPassword

                });

                salt = SecurityHelper.GenerateSalt(70);
                hashedPassword = SecurityHelper.HashPassword("192837", salt, 10101, 70);
                context.Users.Add(new User
                {
                    UserName = "Alex",
                    Email = "alex@gmail.com",
                    Salt = salt,
                    HashedPassword = hashedPassword

                });
                await context.SaveChangesAsync();
            }
        }
    }
}
