using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using ProjectFilm.Data;
using ProjectFilm.Helpers;
using ProjectFilm.Model;
using ProjectFilm.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectFilm
{
	/// <summary>
	/// Interaction logic for UserWindow.xaml
	/// </summary>
	public partial class UserWindow : Window
	{
        public static ApplicationDbContext context = new ApplicationDbContext(DbInit.ConnectToJason());
        ValidationHelper helper = new ValidationHelper(context);
        UserRepository userRepository = new UserRepository(context);
		Model.User user = new Model.User();
        public UserWindow()
		{
			InitializeComponent();
			GetValidUser();
			ShowInformation().GetAwaiter();
		}

		public void GetValidUser()
		{
			Model.User LogInUser = SignInForm.GetUser();
			Model.User SignInUser = RegistrationForm.GetUser();
			if(LogInUser == null)
			{
				user = SignInUser;
			}
			else
			{
				user = LogInUser;
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			BaseWindow baseWindow = new BaseWindow();
			baseWindow.Left = this.Left;
			baseWindow.Top = this.Top;
			this.Hide();
			baseWindow.Show();
        }
		public async Task ShowInformation()
		{
			Guid? userPhotoId = await userRepository.GetUserPhotoIdAsync(user.Id);
			if (userPhotoId != null)
			{
				// Получаем фотографию по айди
				var userPhoto = await context.ImagesForBase.FirstOrDefaultAsync(up => up.Id == userPhotoId);
				if (userPhoto != null && userPhoto.Data != null)
				{
					BitmapImage userImage = new BitmapImage();
					using (MemoryStream memoryStream = new MemoryStream(userPhoto.Data))
					{
						userImage.BeginInit();
						userImage.StreamSource = memoryStream;
						userImage.CacheOption = BitmapCacheOption.OnLoad;
						userImage.EndInit();
					}
					ProfileImg.Source = userImage;
				}

			}
		}
		private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditLoginButton_Click(object sender, RoutedEventArgs e)
        {
			MakeVisibleToEdit();
            EnterOldLabel.Content = "Enter old Login";
			EnterNewLabel.Content = "Enter new Login";


        }

		public void MakeVisibleToEdit()
		{
            EnterOldLabel.Visibility = Visibility.Visible;
            EnterOldTxtBox.Visibility = Visibility.Visible;
			EnterNewLabel.Visibility = Visibility.Visible;
			EnterNewTextBox.Visibility = Visibility.Visible;	

        }
    }
}
