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
	/// Interaction logic for FilmWindow.xaml
	/// </summary>
	public partial class FilmWindow : Window
	{
		public FilmWindow()
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

		private void UserProfileButton_Click(object sender, RoutedEventArgs e)
		{
			UserWindow userWindow = new UserWindow();
			userWindow.Left = this.Left;
			userWindow.Top = this.Top;
			this.Hide();
			userWindow.Show();
		}
	}
}
