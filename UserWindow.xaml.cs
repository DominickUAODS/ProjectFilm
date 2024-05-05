using Microsoft.EntityFrameworkCore;
using ProjectFilm.Data;
using ProjectFilm.Helpers;
using ProjectFilm.Model;
using ProjectFilm.Repository;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

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
		User user = new User();

		public UserWindow()
		{
			InitializeComponent();
			GetValidUser();
			ShowInformation().GetAwaiter();
		}

		public void GetValidUser()
		{
			User LogInUser = SignInForm.GetUser();
			User SignInUser = RegistrationForm.GetUser();
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

			if(userPhotoId != null)
			{
				var userPhoto = await context.ImagesForBase.FirstOrDefaultAsync(up => up.Id == userPhotoId);
				if(userPhoto != null && userPhoto.Data != null)
				{
					BitmapImage userImage = new BitmapImage();
					using(MemoryStream memoryStream = new MemoryStream(userPhoto.Data))
					{
						userImage.BeginInit();
						userImage.StreamSource = memoryStream;
						userImage.CacheOption = BitmapCacheOption.OnLoad;
						userImage.EndInit();
					}
					ProfileImg.Source = userImage;
				}
			}

			loginUsertxtBox.Text = user.UserName;
			emailUsertxtBox.Text = user.Email;
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			if(EnterNewLabel.Content == "Enter new Login")
			{
				IsRightLogin();
				string newUserName = EnterNewTextBox.Text;
				var userToUpdate = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
				if(userToUpdate != null)
				{
					userToUpdate.UserName = newUserName;
					await context.SaveChangesAsync();
					MessageBox.Show("User updated successfully!");

				}
				loginUsertxtBox.Text = newUserName;
			}
			else if(EnterNewLabel.Content == "Enter new email")
			{
				IsRightEmail();
				string newUserEmail = EnterNewTextBox.Text;
				var userToUpdate = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
				if(userToUpdate != null)
				{
					userToUpdate.Email = newUserEmail;
					await context.SaveChangesAsync();
					MessageBox.Show("User updated successfully!");

				}
				emailUsertxtBox.Text = newUserEmail;
			}
			else if(EnterNewLabel.Content == "Enter new password")
			{
				await IsRightPassword();
				var salt = await context.Users.Where(u => u.Email.Equals(user.Email))
											  .Select(u => new { Salt = u.Salt, HashPassword = u.HashedPassword })
											  .FirstOrDefaultAsync();

				if(salt != null && salt.Salt != null && salt.HashPassword != null)
				{
					string newHashedPassword = SecurityHelper.HashPassword(EnterNewTextBox.Text, salt.Salt, 10101, 70);
					user.HashedPassword = newHashedPassword;
					await context.SaveChangesAsync();
				}
				MessageBox.Show("User updated successfully!");
			}
		}

		private async Task IsRightPassword()
		{
			string enteredOldPasswordHash = SecurityHelper.HashPassword(EnterOldTxtBox.Text, user.Salt, 10101, 70);
			var user2 = await context.Users.FirstOrDefaultAsync(u => u.HashedPassword == enteredOldPasswordHash);

			if(user != null)
			{

				SaveButton.IsEnabled = true;
			}
			else
			{
				EnterOldLabel.Content = "Wrong password.";
				SaveButton.IsEnabled = false;
			}
		}

		private void IsRightEmail()
		{
			User u = context.Users.FirstOrDefault(u => u.Email == EnterOldTxtBox.Text);
			if(u != null)
			{
				if(helper.IsValidEmail(EnterNewTextBox.Text))
				{
					SaveButton.IsEnabled = true;
				}
				else
				{
					EnterNewLabel.Content = "Not valid email. Reenter.";
					SaveButton.IsEnabled = false;
				}
			}
			else
			{
				EnterOldLabel.Content = "Wrong email.";
				SaveButton.IsEnabled = false;
			}
		}

		private void EditLoginButton_Click(object sender, RoutedEventArgs e)
		{
			MakeVisibleToEdit();
			EnterOldLabel.Content = "Enter old Login";
			EnterNewLabel.Content = "Enter new Login";
		}

		public void IsRightLogin()
		{
			ProjectFilm.Model.User u = context.Users.FirstOrDefault(u => u.UserName == EnterOldTxtBox.Text);
			if(u != null)
			{
				if(helper.IsEnglishLettersAndNumbers(EnterNewTextBox.Text))
				{
					SaveButton.IsEnabled = true;
				}
				else
				{
					EnterNewLabel.Content = "Not valid login. Enter english letter.";
					SaveButton.IsEnabled = false;
				}
			}
			else
			{
				EnterOldLabel.Content = "Wrong login.";
				SaveButton.IsEnabled = false;
			}
		}

		public void MakeVisibleToEdit()
		{
			EnterOldLabel.Visibility = Visibility.Visible;
			EnterOldTxtBox.Visibility = Visibility.Visible;
			EnterNewLabel.Visibility = Visibility.Visible;
			EnterNewTextBox.Visibility = Visibility.Visible;
		}

		private void EditEmailButton_Click(object sender, RoutedEventArgs e)
		{
			MakeVisibleToEdit();
			EnterOldLabel.Content = "Enter old email";
			EnterNewLabel.Content = "Enter new email";
		}

		private void EditPasswordButton_Click(object sender, RoutedEventArgs e)
		{
			MakeVisibleToEdit();
			EnterOldLabel.Content = "Enter old password";
			EnterNewLabel.Content = "Enter new password";
			User u = context.Users.FirstOrDefault(u => u.HashedPassword == EnterOldTxtBox.Text);
		}
	}
}
