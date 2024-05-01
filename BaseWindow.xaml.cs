using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProjectFilm.Model;
using ProjectFilm.Api;

namespace ProjectFilm
{
	/// <summary>
	/// Interaction logic for BaseWindow.xaml
	/// </summary>
	public partial class BaseWindow : Window
	{
		private int currentPage = 1;
		private int pageSize = 5;
		string pathImage = "https://image.tmdb.org/t/p/w600_and_h900_bestv2";

		public BaseWindow()
		{
			InitializeComponent();
			PreLoadGenre();
			LoadInitialMovies();
		}

		private async void CategoryButton_Click(object sender, RoutedEventArgs e)
		{
			Button clickedButton = sender as Button;
			string selectedCategory = clickedButton.Tag.ToString();

			currentPage = 1;
			LoadMoviesForCategory(selectedCategory);
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
					Button[] dataButtons = [FilmButton0, FilmButton1, FilmButton2, FilmButton3];
					int index = 0;
					foreach(var butFilm in dataButtons)
					{
						Movie movie = new Movie
						{
							title = movies.Results[index].title + " / " + movies.Results[index].original_title,
							poster_path = pathImage + movies.Results[index].poster_path,
							id = movies.Results[index].id
						};

						butFilm.DataContext = movie;

						index++;
					}
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
					Button[] dataButtons = [FilmButton0, FilmButton1, FilmButton2, FilmButton3];
					int index = 0;
					foreach(var butFilm in dataButtons)
					{
						Movie movie = new Movie
						{
							title = movies.Results[index].title + " / " + movies.Results[index].original_title,
							poster_path = pathImage + movies.Results[index].poster_path,
							id = movies.Results[index].id
						};

						butFilm.DataContext = movie;

						index++;
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
					//DataGrid[] dataGrids = { FilmDataGrid2, FilmDataGrid3 };
					//foreach(var dataGrid in dataGrids)
					//{
					//	dataGrid.Items.Clear();
					//	dataGrid.ItemsSource = movies.Results;
					//}
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
