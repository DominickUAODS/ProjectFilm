using ProjectFilm.Model;
using ProjectFilm.ViewModels;

namespace ProjectFilm.Interfaces
{
	public interface IUser
	{
		Task<bool> SignInAsync(UserViewModel user);
		Task<bool> RegisterAsync(RegisterViewModel user);
		Task<ImageForBase> GetRandomImageAsync();
		Task<User> GetUserByEmailAsync(string email);
		//Task<byte[]> GetUserImageAsync(Guid userId);
	}
}
