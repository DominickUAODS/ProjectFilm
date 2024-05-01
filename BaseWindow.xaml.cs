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
using ProjectFilm.Model;
using ProjectFilm.Api;
using Microsoft.IdentityModel.Tokens;


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
			PreLoadGenre();
			LoadInitialMovies();
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
			if(!string.IsNullOrEmpty(SearchTextBox.Text))
			SearchMoviesByName(SearchTextBox.Text);
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
						Button categoryButton = new Button
						{
							Content = genre.name, Tag = genre.id
						};

						categoryButton.Click += CategoryButton_Click;

						CategoryPanel.Children.Add(categoryButton);
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

		private async void LoadInitialMovies()
		{
			try
			{
				Movies movies = await MovieApi.GetPopularMovies(currentPage);

				if(movies != null && movies.Results != null)
				{
					Movie movie = new Movie
					{
						title = "Film Title",
						poster_path = "https://image.tmdb.org/t/p/w200/poster.jpg"
					};
					FilmDataGrid0.DataContext = movie;

					//Image imageControl = FilmDataGrid0.Content as Image;
					//TextBlock textBlock = FilmDataGrid0.Content as TextBlock;

					//if(imageControl != null)
					//{
					//	imageControl.Source = new BitmapImage(new Uri(movies.Results[1].poster_path));
					//}

					//if(textBlock != null)
					//{
					//	textBlock.Text = movies.Results[1].title;// + "/" + movies.Results[1].original_title;
					//}

					//int index = 0;
					//DataGrid[] dataGrids = { FilmDataGrid2, FilmDataGrid3 };
					//foreach(var dataGrid in dataGrids)
					//{
					//	dataGrid.Items.Clear();
					//	dataGrid.ItemsSource = movies.Results[index].original_title;
					//	index++;
					//}
				}
				else
				{
					MessageBox.Show("Failed to load initial movies.");
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show($"Error loading initial movies: {ex.Message}");
			}
		}

		private async void LoadMoviesForCategory(string categoryId)
		{
			try
			{
				Movies movies = await MovieApi.GetMoviesByGenre(int.Parse(categoryId), currentPage);

				if(movies != null && movies.Results != null)
				{


					Movie movie = new Movie
					{
						title = "Film Title",
						poster_path = "https://image.tmdb.org/t/p/w200/poster.jpg" // Пример URL изображения
					};

					// Установите значения `Image` и `TextBlock` вручную
					Image imageControl = FilmDataGrid0.Content as Image;
					TextBlock textBlock = FilmDataGrid0.Content as TextBlock;

					if(imageControl != null)
					{
						imageControl.Source = new BitmapImage(new Uri(movie.poster_path));
					}

					if(textBlock != null)
					{
						textBlock.Text = movie.title;
					}

					DataGrid[] dataGrids = { FilmDataGrid2, FilmDataGrid3 };
					foreach(var dataGrid in dataGrids)
					{
						dataGrid.Items.Clear();
						dataGrid.ItemsSource = movies.Results;
					}
				}
				else
				{
					MessageBox.Show("Failed to load movies for category.");
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show($"Error loading movies for category: {ex.Message}");
			}
		}

		private async void SearchMoviesByName(string name)
		{
			try
			{
				Movies movies = await MovieApi.GetMoviesByName(name, currentPage);

				if(movies != null && movies.Results != null)
				{
					DataGrid[] dataGrids = { FilmDataGrid2, FilmDataGrid3 };
					foreach(var dataGrid in dataGrids)
					{
						dataGrid.Items.Clear();
						dataGrid.ItemsSource = movies.Results;
					}
				}
				else
				{
					MessageBox.Show("No movies found for the given name.");
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show($"Error searching movies by name: {ex.Message}");
			}
		}
	}
}
