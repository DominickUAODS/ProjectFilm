using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ProjectFilm.Data;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjectFilm.Helpers;
using ProjectFilm.ViewModels;
using System.Diagnostics.Eventing.Reader;
using ProjectFilm.Data;
using ProjectFilm.Repository;
using System.Windows.Navigation;

namespace ProjectFilm
{
    /// <summary>
    /// Interaction logic for RegistrationForm.xaml
    /// </summary>
    public partial class RegistrationForm : Window
    {
        public static ApplicationDbContext context = new ApplicationDbContext(DbInit.ConnectToJason());
        ValidationHelper helper = new ValidationHelper(context);
        UserRepository userRepository = new UserRepository(context);

        public RegistrationForm()
        {
            InitializeComponent();
            DbInit.EnsurePopulate().GetAwaiter();
        }

        public async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            if (helper.IsEnglishLettersAndNumbers(txtBoxForLoginl.Text))
            {
                RegisterViewModel uv = new RegisterViewModel
                {
                    Email = txtBoxForEmail.Text,
                    Password = txtBoxForPassword.Text,
                    UserName = txtBoxForLoginl.Text
                };

                if (uv != null)
                {
                    if (await helper.IsEquelLogin(uv))
                    {
                        labelEnterLogin.Text = "This login already exists in our system.";
                        ResetLabel(labelEnterLogin);
                    }
                    else if (await helper.IsAlreadyInBaseByEmail(uv))
                    {
                        labelEnterEmail.Text = "This email already exists in our system.";
                        ResetLabel(labelEnterEmail);
                    }
                    else if (!helper.IsValidEmail(txtBoxForEmail.Text))
                    {
                        labelEnterEmail.Text = "Please, enter valid email.";
                        ResetLabel(labelEnterEmail);
                    }
                    else
                    {
                        await userRepository.RegisterAsync(uv);
                        BaseWindow registration = new BaseWindow();
                        registration.Show();
                        Hide();
                    }
                }
            }
            else
            {
                labelEnterLogin.Text = "Invalid LogIn.Please use English letters and numbers.";
                ResetLabel(labelEnterLogin);
            }
        }

        private void ResetLabel(TextBlock label)
        {
            label.Foreground = Brushes.Red;
            label.FontSize = 12;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.Height = 40;
        }

        public void GoToLogInButton_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();

        }

        public void buttonSee_Click(object sender, RoutedEventArgs e)
        {

            if (psBoxForPassword.Visibility == Visibility.Visible)
            {
                psBoxForPassword.Visibility = Visibility.Collapsed;
                txtBoxForPassword.Visibility = Visibility.Visible;
                txtBoxForPassword.Text = psBoxForPassword.Password;
            }
            else
            {
                psBoxForPassword.Visibility = Visibility.Visible;
                txtBoxForPassword.Visibility = Visibility.Collapsed;
                psBoxForPassword.Password = txtBoxForPassword.Text;
            }
        }
    }
}    
 

    

