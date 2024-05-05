using System.ComponentModel.DataAnnotations;

namespace ProjectFilm.ViewModels
{
	public class UserViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
