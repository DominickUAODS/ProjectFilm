using System.ComponentModel.DataAnnotations;

namespace ProjectFilm.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [MinLength(3)]
        public string UserName { get; set; }
    }
}
