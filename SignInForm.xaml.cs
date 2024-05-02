using ProjectFilm.Data;
using ProjectFilm.Helpers;
using ProjectFilm.Model;
using ProjectFilm.Repository;
using ProjectFilm.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {
        public static ApplicationDbContext context = new ApplicationDbContext(DbInit.ConnectToJason());
        UserRepository userRepository = new UserRepository(context);
        public static User user = new User();

        public SignInForm()
        {
            InitializeComponent();
        }

        public static ProjectFilm.Model.User GetUser()
        {
                return user;
        }
        public void GoToSignInButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationForm registration= new RegistrationForm();
            registration.Show();
            Hide();

        }

        public async void buttonLogIn_Click(object sender, RoutedEventArgs e)
        {
            UserViewModel uv = new UserViewModel
            {
                Email = txtBoxForEmail.Text,
                Password = txtBoxForPaasword.Text

            };

            bool flag = await userRepository.SignInAsync(uv);
            if(!flag)
            {
                string invalid = "Invalid email or password!";
                labelForEmail.Text = invalid;
                labelForPassword.Text = invalid;
                ResetLabel(labelForEmail);
                ResetLabel(labelForPassword);
            }
            else
            {
                user = await userRepository.GetUserByEmailAsync(uv.Email);

                if (user != null)
                {
                    BaseWindow registration = new BaseWindow();
                    registration.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Failed to retrieve user information.");
                }
            }


        }

        private void ResetLabel(TextBlock label)
        {
            label.Foreground = Brushes.Red;
            label.FontSize = 16;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.Height = 40;
        }

        public void buttonSee_Click(object sender, RoutedEventArgs e)
        {

            if (psBoxForPaasword.Visibility == Visibility.Visible)
            {
                psBoxForPaasword.Visibility = Visibility.Collapsed;
                txtBoxForPaasword.Visibility = Visibility.Visible;
                txtBoxForPaasword.Text = psBoxForPaasword.Password;
            }
            else
            {
                psBoxForPaasword.Visibility = Visibility.Visible;
                txtBoxForPaasword.Visibility = Visibility.Collapsed;
                psBoxForPaasword.Password = txtBoxForPaasword.Text;
            }
        }
    }
}
