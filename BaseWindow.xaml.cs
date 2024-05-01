using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProjectFilm.Model;
using ProjectFilm.Api;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ProjectFilm
{
	/// <summary>
	/// Interaction logic for BaseWindow.xaml
	/// </summary>
	public partial class BaseWindow : Window
	{
		private int currentPage = 1;
		private int filmsPage = 1;
		private int index = 0;
		private int pageSize = 0;
		private string? selectedCategory = null;
		//private string pathImage = "https://image.tmdb.org/t/p/w600_and_h900_bestv2";
		private string pathImage = GetImagePath();

		public BaseWindow()
		{
			InitializeComponent();
			PreLoadGenre();
			LoadTopMovies();
		}

		private async void CategoryButton_Click(object sender, RoutedEventArgs e)
		{
			Button clickedButton = sender as Button;
			selectedCategory = clickedButton.Tag.ToString();

			currentPage = 1;
			LoadMoviesForCategory(selectedCategory);
		}

		private async void PreviousPage_Click(object sender, RoutedEventArgs e)
		{
			if(currentPage > 1)
			{
				if((currentPage - 1) % 5 == 0)
				{
					filmsPage--;
					pageSize -= 5;
				}
				currentPage--;
				index = (currentPage - 1 - pageSize) * 4;
				await LoadCurrentPage();
			}
		}

		private async void NextPage_Click(object sender, RoutedEventArgs e)
		{
			if(currentPage % 5 == 0)
			{
				filmsPage++;
				index = 0;
				pageSize += 5;
			}
			currentPage++;
			index = (currentPage - 1 - pageSize) * 4;
			await LoadCurrentPage();
		}

		private async Task LoadCurrentPage()
		{
			if(!string.IsNullOrEmpty(SearchTextBox.Text))
			{
				SearchMoviesByName(SearchTextBox.Text, index);
			}
			else if(selectedCategory != null)
			{
				LoadMoviesForCategory(selectedCategory, index);
			}
			else
			{
				LoadTopMovies(index);
			}

			PageInfo.Text = $"Page {currentPage}";
		}

		//private string GetSelectedCategory()
		//{
		//	return "Category 1";
		//}

		//private async Task LoadFilmsForCategory(string category)
		//{
		//	//List<Film> films = await LoadFilmsFromApiAsync(category, currentPage, pageSize);

		//	//FilmDataGrid.ItemsSource = films;

		//	PageInfo.Text = $"Page {currentPage}";
		//}

		//private void FilmDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		//{
		//	FilmWindow filmWindow = new FilmWindow();
		//	filmWindow.Left = this.Left;
		//	filmWindow.Top = this.Top;
		//	this.Hide();
		//	filmWindow.Show();
		//}

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

		private async void LoadTopMovies(int index = 0)
		{
			try
			{
				Movies movies = await MovieApi.GetPopularMovies(filmsPage);

				if(movies != null && movies.Results != null)
				{
					FillButtonInfo(movies, index);

					//Button[] dataButtons = [FilmButton0, FilmButton1, FilmButton2, FilmButton3];
					//int index = 0;
					//foreach(var butFilm in dataButtons)
					//{
					//	Movie movie = new Movie
					//	{
					//		title = movies.Results[index].title + " / " + movies.Results[index].original_title,
					//		poster_path = pathImage + movies.Results[index].poster_path,
					//		id = movies.Results[index].id
					//	};

					//	butFilm.DataContext = movie;

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

		private async void LoadMoviesForCategory(string categoryId, int index = 0)
		{
			try
			{
				Movies movies = await MovieApi.GetMoviesByGenre(int.Parse(categoryId), filmsPage);

				if(movies != null && movies.Results != null)
				{
					FillButtonInfo(movies, index);

					//Button[] dataButtons = [FilmButton0, FilmButton1, FilmButton2, FilmButton3];
					//int index = 0;
					//foreach(var butFilm in dataButtons)
					//{
					//	Movie movie = new Movie
					//	{
					//		title = movies.Results[index].title + " / " + movies.Results[index].original_title,
					//		poster_path = pathImage + movies.Results[index].poster_path,
					//		id = movies.Results[index].id
					//	};

					//	butFilm.DataContext = movie;

					//	index++;
					//}
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

		private async void SearchMoviesByName(string name, int index = 0)
		{
			try
			{
				Movies movies = await MovieApi.GetMoviesByName(name, filmsPage);

				if(movies != null && movies.Results != null)
				{
					FillButtonInfo(movies, index);

					//Button[] dataButtons = [FilmButton0, FilmButton1, FilmButton2, FilmButton3];
					//int index = 0;
					//foreach(var butFilm in dataButtons)
					//{
					//	butFilm.DataContext = null;
					//	Movie movie = new Movie
					//	{
					//		title = movies.Results[index].title + " / " + movies.Results[index].original_title,
					//		poster_path = pathImage + movies.Results[index].poster_path,
					//		id = movies.Results[index].id
					//	};

					//	butFilm.DataContext = movie;

					//	index++;
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

		private void FillButtonInfo(Movies movies, int index = 0)
		{
			Button[] dataButtons = [FilmButton0, FilmButton1, FilmButton2, FilmButton3];
			//index = 0;
			foreach(var butFilm in dataButtons)
			{
				butFilm.DataContext = null;
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

		private static string GetImagePath()
		{
			var builder = new ConfigurationBuilder();
			// установка пути к текущему каталогу 
			builder.SetBasePath(Directory.GetCurrentDirectory());
			// получаем конфигурацию из файла appsettings.json 
			builder.AddJsonFile("appsettings.json");
			// создаем конфигурацию 
			var config = builder.Build();
			// получаем строку подключения 
			var connectionString = config.GetSection("ImagePaths:PosterPath");
			return connectionString.Value;
		}

		private void FilmButton_Click(object sender, RoutedEventArgs e)
		{
			Button clickedButton = sender as Button;
			Movie movie = clickedButton.DataContext as Movie;
			if(movie != null)
			{
				FilmWindow filmWindow = new FilmWindow(movie.id);
				filmWindow.Left = this.Left;
				filmWindow.Top = this.Top;
				this.Hide();
				filmWindow.Show();
			}
		}
	}
}
