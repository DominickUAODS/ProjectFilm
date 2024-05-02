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
	/// Interaction logic for UserWindow.xaml
	/// </summary>
	public partial class UserWindow : Window
	{
		public UserWindow()
		{
			InitializeComponent();
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			BaseWindow baseWindow = new BaseWindow();
			baseWindow.Left = this.Left;
			baseWindow.Top = this.Top;
			this.Hide();
			baseWindow.Show();
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
