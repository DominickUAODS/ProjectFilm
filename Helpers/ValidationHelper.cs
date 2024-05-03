using Microsoft.EntityFrameworkCore;
using ProjectFilm.Data;
using ProjectFilm.ViewModels;
using System.Text.RegularExpressions;

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
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c)) 
                {
                    return false;
                }
            }

            return true;
        }

        public  bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);
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
