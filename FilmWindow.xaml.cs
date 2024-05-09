﻿using Microsoft.Extensions.Configuration;
using ProjectFilm.Api;
using ProjectFilm.Model;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using ProjectFilm.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Net;

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
		//private Guid UserId;
		private User _user;
		//public static ApplicationDbContext context = new ApplicationDbContext(DbInit.ConnectToJason());

		/// chat data 
		private UdpClient udpClient;
		private IPEndPoint remoteEndPoint;
		private string serverIp = "127.0.0.1";
		private int serverPort = 9090;

		public FilmWindow(int id)
		{
			InitializeComponent();
			filmId = id;
			InitializeAsync();
			//LoadFilmDetailsAsync();
			//LoadReviews();
			InitializeChat();
		}

		private async void InitializeAsync()
		{
			await LoadFilmDetailsAsync();
			GetValidUser();
			await LoadReviews();
		}

		public void GetValidUser()
		{
			User LogInUser = SignInForm.GetUser();
			User SignInUser = RegistrationForm.GetUser();
			if(LogInUser == null)
			{
				_user = SignInUser;
			}
			else
			{
				_user = LogInUser;
			}
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

					ReleaseYearLabel.Content = $"({releaseDate.Year.ToString()})";

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
					UserId = _user.Id
					//UserId = Guid.NewGuid()
				};

				//using(var context = new ApplicationDbContext(DbInit.ConnectToJason()))
				//{
				//	context.Reviews.Add(newReview);
				//	await context.SaveChangesAsync();
				//}

				using(var context = new ApplicationDbContext(DbInit.ConnectToJason()))
				{
					await context.Users.FindAsync(_user.Id);
					if(_user != null)
					{
						newReview.UserId = _user.Id;
						context.Reviews.Add(newReview);
						await context.SaveChangesAsync();
					}
					else
					{
						MessageBox.Show("Error");
					}
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
					foreach(var review in reviews)
					{
						string userName = await GetUserNameById(review.UserId);
						//ReviewsListBox.ItemsSource = review.User.UserName;
						//ReviewsListBox.ItemsSource = _user.whew;
						ReviewsListBox.Items.Add($"{review.DateOfPost} {userName}");
						ReviewsListBox.Items.Add(review.TextOfReview);
					}
				}
				else
				{
					ReviewsListBox.Items.Add($"No reviews for current film");
				}
			}
		}

		private void InitializeChat()
		{
			udpClient = new UdpClient();
			remoteEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
			StartListening();
		}

		public static async Task<string> GetUserNameById(Guid userId)
		{
			using(var context = new ApplicationDbContext(DbInit.ConnectToJason()))
			{
				User _user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
				return _user.UserName;
			}
		}

		//private async Task StartListening()
		//{
		//	while(true)
		//	{
		//		try
		//		{
		//			using(var context = new ApplicationDbContext(DbInit.ConnectToJason()))
		//			{
		//				var chatMessages = await context.Messages.OrderBy(m=>m.Date).ToListAsync();

		//				Dispatcher.Invoke(async () =>
		//				{
		//					LiveChatListBox.Items.Clear();

		//					foreach(var chatMessage in chatMessages)
		//					{
		//						string userName = await GetUserNameById(chatMessage.UserId);

		//						var listItem = new ChatMessageViewModel
		//						{
		//							ChatDate = chatMessage.Date,
		//							ChatUserName = userName,
		//							ChatUserId = chatMessage.UserId,
		//							ChatText = chatMessage.Text
		//						};
		//						//string displayedMessage = $"{chatMessage.Date} {userName} : {chatMessage.Text}";
		//						//LiveChatListBox.Items.Add($"{chatMessage.Date} {userName}");
		//						//LiveChatListBox.Items.Add(chatMessage.Text);
		//						//LiveChatListBox.Items.Add(displayedMessage);
		//						LiveChatListBox.Items.Add(listItem);
		//					}
		//				});
		//			}
		//		}
		//		catch(Exception ex)
		//		{
		//			MessageBox.Show($"Error receiving message: {ex.Message}");
		//		}

		//		//await Task.Delay(TimeSpan.FromSeconds(2));
		//		await Task.Delay(2000);
		//	}
		//}

		private async void SendButton_Click(object sender, RoutedEventArgs e)
		{
			if(!string.IsNullOrEmpty(MessageTextBox.Text))
			{
				ChatMessage newChatMessage = new ChatMessage
				{
					Id = Guid.NewGuid(),
					Text = MessageTextBox.Text,
					Date = DateTime.Now,
					UserId = _user.Id
				};

				using(var context = new ApplicationDbContext(DbInit.ConnectToJason()))
				{
					context.Messages.Add(newChatMessage);
					await context.SaveChangesAsync();
				}

				string userName = await GetUserNameById(newChatMessage.UserId);

				//string newMessage = $"{newChatMessage.Date} {userName} : {newChatMessage.Text}";

				var listItem = new ChatMessageViewModel
				{
					ChatDate = newChatMessage.Date,
					ChatUserName = userName,
					ChatUserId = newChatMessage.UserId,
					ChatText = newChatMessage.Text
				};

				//LiveChatListBox.Items.Add(newMessage);
				LiveChatListBox.Items.Add(listItem);

				MessageTextBox.Clear();
			}
		}

		private async void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			if(LiveChatListBox.SelectedIndex >= 0)
			{
				//string selectedMessage = LiveChatListBox.SelectedItem.ToString();
				//string[] parts = selectedMessage.Split(' ');
				int index = LiveChatListBox.SelectedIndex;
				var selectedMessage = LiveChatListBox.SelectedItem as ChatMessageViewModel;

				if(selectedMessage != null)
				{
					var ChatUserId = selectedMessage.ChatUserId;
					var ChatText = selectedMessage.ChatText;

					if(ChatUserId == _user.Id)
					{
						using(var context = new ApplicationDbContext(DbInit.ConnectToJason()))
						{
							var chatMessage = await context.Messages.FirstOrDefaultAsync(msg => msg.UserId == ChatUserId && msg.Text == ChatText);

							if(chatMessage != null)
							{
								context.Messages.Remove(chatMessage);
								await context.SaveChangesAsync();
							}
						}
						//LiveChatListBox.Items.RemoveAt(LiveChatListBox.SelectedIndex);
						LiveChatListBox.Items.RemoveAt(index);
					}
					else
					{
						MessageBox.Show("You can only delete your own messages.");
					}
				}

				//if(parts.Length >= 2)
				//{
				//	string selectedUserId = parts[0].Trim();
				//	string selectedText = parts[1].Trim();

				//	if(selectedUserId == _user.Id.ToString())
				//	{
				//		using(var context = new ApplicationDbContext(DbInit.ConnectToJason()))
				//		{
				//			var chatMessage = await context.Messages
				//				.FirstOrDefaultAsync(msg => msg.UserId.ToString() == selectedUserId && msg.Text == selectedText);

				//			if(chatMessage != null)
				//			{
				//				context.Messages.Remove(chatMessage);
				//				await context.SaveChangesAsync();
				//			}
				//		}

				//		LiveChatListBox.Items.RemoveAt(LiveChatListBox.SelectedIndex);
				//	}
				//	else
				//	{
				//		MessageBox.Show("You can only delete your own messages.");
				//	}
				//}
			}
		}

		public event EventHandler<ChatMessage> NewMessageAdded;

		protected void OnNewMessageAdded(ChatMessage newMessage)
		{
			NewMessageAdded?.Invoke(this, newMessage);
		}

		public async Task AddMessage(ChatMessage newMessage)
		{
			using(var context = new ApplicationDbContext(DbInit.ConnectToJason()))
			{
				context.Messages.Add(newMessage);
				await context.SaveChangesAsync();
			}
			OnNewMessageAdded(newMessage);
		}

		private async Task StartListening()
		{
			try
			{
				await LoadMessages();

				using(var context = new ApplicationDbContext(DbInit.ConnectToJason()))
				{
					context.NewMessageAdded += async (sender, newMessage) =>
					{
						string userName = await GetUserNameById(newMessage.UserId);

						var listItem = new ChatMessageViewModel
						{
							ChatDate = newMessage.Date,
							ChatUserName = userName,
							ChatUserId = newMessage.UserId,
							ChatText = newMessage.Text
						};

						LiveChatListBox.Items.Add(listItem);
					};
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show($"Error receiving message: {ex.Message}");
			}
		}

		private async Task LoadMessages()
		{
			try
			{
				using(var context = new ApplicationDbContext(DbInit.ConnectToJason()))
				{
					var chatMessages = await context.Messages.OrderBy(m=>m.Date).ToListAsync();

					Dispatcher.Invoke(async () =>
					{
						LiveChatListBox.Items.Clear();

						foreach(var chatMessage in chatMessages)
						{
							string userName = await GetUserNameById(chatMessage.UserId);

							var listItem = new ChatMessageViewModel
							{
								ChatDate = chatMessage.Date,
								ChatUserName = userName,
								ChatUserId = chatMessage.UserId,
								ChatText = chatMessage.Text
							};

							LiveChatListBox.Items.Add(listItem);
						}
					});
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show($"Error loading messages: {ex.Message}");
			}
		}


	}
}
