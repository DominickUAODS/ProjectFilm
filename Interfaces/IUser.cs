using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectFilm.Model;
using ProjectFilm.ViewModels;

namespace ProjectFilm.Interfaces
{
    public interface IUser
    {
        Task<bool> SignInAsync(UserViewModel user);
        Task<bool> RegisterAsync(RegisterViewModel user);
        Task<ImageForBase> GetRandomImageAsync();
    }
}
