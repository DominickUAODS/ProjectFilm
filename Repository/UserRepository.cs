﻿using ProjectFilm.Data;
using ProjectFilm.Helpers;
using ProjectFilm.Interfaces;
using ProjectFilm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectFilm.Model;
using Microsoft.EntityFrameworkCore;

namespace ProjectFilm.Repository
{
    public class UserRepository : IUser
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ImageForBase> GetRandomImageAsync()
        {
            var images = await _context.ImagesForBase.ToListAsync();
            if (images.Count > 0)
            {
                Random random = new Random();
                int index = random.Next(0, images.Count);
                return images[index];
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> RegisterAsync(RegisterViewModel user)
        {
            var salt = SecurityHelper.GenerateSalt(70);
            string hashedPassword = SecurityHelper.HashPassword(user.Password, salt, 10101, 70);

            User newUser = new User
            {
                Email = user.Email,
                Salt = salt,
                HashedPassword = hashedPassword,
                UserName = user.UserName
            };

            var randomImage = await GetRandomImageAsync();
            if (randomImage != null)
            {
                newUser.ImageForBase = randomImage;
            }

            _context.Users.Add(newUser);
            int result = await _context.SaveChangesAsync();

            return result > 0; 
        }

        public async Task<bool> SignInAsync(UserViewModel user)
        {
            var salt = await _context.Users.Where(e => e.Email.Equals(user.Email)).Select(e => new
            {
                Salt = e.Salt,
                HashPassword = e.HashedPassword
            }).FirstOrDefaultAsync();
            if (salt?.Salt != null && salt?.HashPassword != null)
            {
                string hashedPassword = SecurityHelper.HashPassword(user.Password, salt.Salt, 10101, 70);
                return salt.HashPassword.Equals(hashedPassword);
            }
            return false;
        }
    }
}
