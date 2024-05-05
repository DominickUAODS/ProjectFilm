using ProjectFilm.Model;
using ProjectFilm.Data;
using ProjectFilm.Helpers;
using ProjectFilm.Repository;
using System.Windows;

namespace ProjectFilm
{
	/// <summary>
	/// Interaction logic for ForgetPassword.xaml
	/// </summary>
	public partial class ForgetPassword : Window
	{
		private User user;
		public static ApplicationDbContext context = new ApplicationDbContext(DbInit.ConnectToJason());
		ValidationHelper helper = new ValidationHelper(context);
		UserRepository userRepository = new UserRepository(context);
		public static string recoveryCode = SecurityHelper.GenerateRandomCode(4);

		public ForgetPassword()
		{
			InitializeComponent();
		}

		private async void ButtonSubmitCode_Click(object sender, RoutedEventArgs e)
		{
			if(!string.IsNullOrWhiteSpace(EnterCodeBox.Text))
			{
				if(recoveryCode == EnterCodeBox.Text)
				{
					string newPassword = SecurityHelper.GenerateRandomPassword(7);
					user.HashedPassword = SecurityHelper.HashPassword(newPassword, user.Salt, 10101, 70);
					await context.SaveChangesAsync();
					SecurityHelper.SendNewPassword(user.Email, newPassword);
					MessageBox.Show("Check your gmail!");
					SignInForm baseWindow = new SignInForm();
					this.Hide();
					baseWindow.Show();
				}
				else
				{
					EnterCodeLabel.Content = "Please try again";
				}
			}
			else
			{
				MessageBox.Show("Please enter code");
			}
		}

		public void IsUserExist()
		{
			User? u = context.Users.FirstOrDefault(u => u.Email == EnterGmailBox.Text && u.UserName == EnterLoginBox.Text);
			if(u != null)
			{
				user = u;
			}
			else
			{
				EnterGmailLabel.Content = "Not valid email. Reenter.";
				EnterLoginLabel.Content = "Not valid login. Reenter.";
			}
		}

		private void ButtonSendCode_Click(object sender, RoutedEventArgs e)
		{
			IsUserExist();
			if(user != null)
			{
				SecurityHelper.SendRecoveryCode(user.Email, recoveryCode);
				EnterCodeLabel.Visibility = Visibility.Visible;
				EnterCodeBox.Visibility = Visibility.Visible;
				SubmitButton.Visibility = Visibility.Visible;
			}
			else
			{
				EnterGmailLabel.Content = "Not valid email. Reenter.";
			}
		}
	}
}
