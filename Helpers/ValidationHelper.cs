using Microsoft.EntityFrameworkCore;
using ProjectFilm.Data;
using ProjectFilm.Model;
using ProjectFilm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFilm.Helpers
{
    public class ValidationHelper
    {
        private readonly ApplicationDbContext _context;

        public ValidationHelper(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool IsEnglishLettersAndNumbers(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c) || char.IsUpper(c))
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> IsAlreadyInBaseByEmail(RegisterViewModel user)
        {
            var myUser = await _context.Users.Where(e => e.Email.Equals(user.Email)).FirstOrDefaultAsync();
            if (myUser != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsEquelLogin(RegisterViewModel user)
        {
            var myUser = await _context.Users.Where(e => e.UserName.Equals(user.UserName)).FirstOrDefaultAsync();
            if (myUser != null)
            {
                return true;
            }
            return false;

        }


    }


}



