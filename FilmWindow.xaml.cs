using Microsoft.Extensions.Configuration;
using ProjectFilm.Api;
using ProjectFilm.Model;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProjectFilm
{
	/// <summary>
	/// Interaction logic for FilmWindow.xaml
	/// </summary>
	public partial class FilmWindow : Window
	{
		private int filmId;
		private string pathImage = GetImagePath();

		public FilmWindow(int id)
		{
			filmId = id;
			LoadFilmDetailsAsync();
			InitializeComponent();
		}

		private async void LoadFilmDetailsAsync()
		{
			try
			{
				Movie movie = await MovieApi.GetMovieById(filmId);

				if (movie != null)
				{
					FilmButton.DataContext = null;
					Movie currentMovie = new Movie
					{
						poster_path = pathImage + movie.poster_path,
						id = movie.id
					};
					FilmButton.DataContext = currentMovie;

					string imagePath = $"{pathImage}{movie.backdrop_path}";
					BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
					BackDropImage.ImageSource = bitmapImage;

					OriginalTitleLabel.Content = $"{movie.original_title}";

					DateTime releaseDate;
					DateTime.TryParse(movie.release_date, out releaseDate);

					ReleaseYearLabel.Content = $"{(releaseDate.Year.ToString())}";

					string genreStr = string.Empty;
					if (movie.genres != null)
					{
						foreach(var genre in movie.genres)
						{
							genreStr = $"{genreStr} {genre.name}, ";
						}
						genreStr = genreStr.TrimEnd([' ', ',']);
					}
					FilmInfoLabel.Content = $"{releaseDate:dd/MM/yyyy} {genreStr}";

					OverviewLabel.Content = $"{movie.overview}";

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

		//public FilmWindow()
		//{
		//	InitializeComponent();
		//}

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
	}
}
