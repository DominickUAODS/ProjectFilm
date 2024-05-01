using ProjectFilm.Data;
using ProjectFilm.Helpers;
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

        public SignInForm()
        {
            InitializeComponent();
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
                Password = txtBoxForPaasword.Password

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
                MessageBox.Show("Ok");
                //переход на форму
            }


        }

        private void ResetLabel(TextBlock label)
        {
            label.Foreground = Brushes.Red;
            label.FontSize = 16;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.Height = 40;
        }

    }
}
