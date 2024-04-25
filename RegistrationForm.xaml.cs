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
            if (!helper.IsEnglishLettersAndNumbers(txtBoxForLoginl.Text))
            {
                RegisterViewModel uv = new RegisterViewModel
                {
                    Email = txtBoxForEmail.Text,
                    Password = txtBoxForPassword.Text,
                    UserName = txtBoxForLoginl.Text
                };

                if(uv != null)
                {
                    if(await helper.IsEquelLogin(uv))
                    {
                        labelEnterLogin.Text = "This login already exists in our system. Please come up with another one.";
                        labelEnterLogin.Background = Brushes.Red;
                    }
                    else if (await helper.IsAlreadyInBaseByEmail(uv))
                    {
                        labelEnterLogin.Text = "This email already exists in our system. You want to log in?";
                        labelEnterLogin.Background = Brushes.Red;
                    }
                    else
                    {
                       await userRepository.RegisterAsync(uv);
                        MessageBox.Show("Welcome!");
                    }
                }
                
            }
            else 
            {
                labelEnterLogin.Text = "Invalid LogIn. Please use English letters and numbers.";
                labelEnterLogin.Background = Brushes.Red;
            
            }
        }
    }
}    
 

    

