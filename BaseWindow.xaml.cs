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
using static System.Reflection.Metadata.BlobBuilder;

namespace ProjectFilm
{
	/// <summary>
	/// Interaction logic for BaseWindow.xaml
	/// </summary>
	public partial class BaseWindow : Window
	{
		private int currentPage = 1;
		private int pageSize = 5;

		public BaseWindow()
		{
			InitializeComponent();
		}

		private async void CategoryButton_Click(object sender, RoutedEventArgs e)
		{
			Button clickedButton = sender as Button;
			string selectedCategory = clickedButton.Content.ToString();

			currentPage = 1;
			await LoadFilmsForCategory(selectedCategory);
		}

		private async void PreviousPage_Click(object sender, RoutedEventArgs e)
		{
			if(currentPage > 1)
			{
				currentPage--;
				await LoadCurrentPage();
			}
		}

		private async void NextPage_Click(object sender, RoutedEventArgs e)
		{
			currentPage++;
			await LoadCurrentPage();
		}

		private async Task LoadCurrentPage()
		{
			string selectedCategory = GetSelectedCategory();
			if(selectedCategory != null)
			{
				await LoadFilmsForCategory(selectedCategory);
			}
		}

		private string GetSelectedCategory()
		{
			return "Category 1";
		}

		private async Task LoadFilmsForCategory(string category)
		{
			//List<Film> films = await LoadFilmsFromApiAsync(category, currentPage, pageSize);

			//FilmDataGrid.ItemsSource = films;

			PageInfo.Text = $"Page {currentPage}";
		}

		private void FilmDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			FilmWindow filmWindow = new FilmWindow();
			filmWindow.Left = this.Left;
			filmWindow.Top = this.Top;
			this.Hide();
			filmWindow.Show();
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void UserProfileButton_Click(object sender, RoutedEventArgs e)
		{
			UserWindow userWindow = new UserWindow();
			userWindow.Left = this.Left;
			userWindow.Top = this.Top;
			this.Hide();
			userWindow.Show();
		}

		private async void PreLoadGenre()
		{
			try
			{
				Genres genres = await MovieApi.GetGenreList();
				if(genres != null && genres.genres != null)
				{
					foreach(Genre genre in genres.genres)
					{
						CategoryPanel.Children.Add(new ComboBoxItem { Text = genre.name, Value = genre.id });
					}
				}
				else
				{
					MessageBox.Show("Failed to load genres.");
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show($"Error loading genres: {ex.Message}");
			}
		}
	}
}
