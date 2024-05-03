using Microsoft.Extensions.Configuration;
using ProjectFilm.Api;
using ProjectFilm.Model;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using ProjectFilm.Data;
using Microsoft.EntityFrameworkCore;

namespace ProjectFilm
{
	/// <summary>
	/// Interaction logic for FilmWindow.xaml
	/// </summary>
	public partial class FilmWindow : Window
	{
		private int filmId;
		private string pathImage = GetImagePath();
		private Movie movie;
		private Guid UserId;
		//public static ApplicationDbContext context = new ApplicationDbContext(DbInit.ConnectToJason());

		public FilmWindow(int id)
		{
			InitializeComponent();
			filmId = id;
			InitializeAsync();
			//LoadFilmDetailsAsync();
			//LoadReviews();

		}

		private async void InitializeAsync()
		{
			await LoadFilmDetailsAsync();
			await LoadReviews();
		}

		private async Task LoadFilmDetailsAsync()
		{
			try
			{
				movie = await MovieApi.GetMovieById(filmId);

				if(movie != null)
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
					if(movie.genres != null)
					{
						foreach(var genre in movie.genres)
						{
							genreStr = $"{genreStr} {genre.name}, ";
						}
						genreStr = genreStr.TrimEnd([' ', ',']);
					}
					FilmInfoLabel.Content = $"{releaseDate:dd/MM/yyyy} {genreStr}";

					OverviewLabel.Text = $"{movie.overview}";

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

		private async void ReviewAddButton_Click(object sender, RoutedEventArgs e)
		{
			if(!string.IsNullOrEmpty(ReviewsTextBox.Text))
			{
				Review newReview = new Review
				{
					Id = Guid.NewGuid(),
					MovieName = movie.original_title,
					DateOfPost = DateTime.Now,
					TextOfReview = ReviewsTextBox.Text,
					//UserId = UserId
					UserId = Guid.NewGuid()
				};

				using(var context = new ApplicationDbContext(DbInit.ConnectToJason()))
				{
					context.Reviews.Add(newReview);
					await context.SaveChangesAsync();
				}

				ReviewsTextBox.Clear();
				await LoadReviews();
			}
			else
			{
				MessageBox.Show($"Review is empty. Please add your review.");
			}
		}

		private async Task LoadReviews()
		{
			ReviewsListBox.Items.Clear();
			using(var context = new ApplicationDbContext(DbInit.ConnectToJason()))
			{
				var reviews = await context.Reviews.Where(r=>r.MovieName == movie.original_title).ToListAsync();

				if(reviews.Count != 0)
				{
					ReviewsListBox.ItemsSource = reviews;
				}
				else
				{
					ReviewsListBox.Items.Add($"No reviews for current film");
				}
			}
		}
	}
}
