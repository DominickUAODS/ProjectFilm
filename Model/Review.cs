﻿namespace ProjectFilm.Model
{
	public class Review
	{
		public Guid Id { get; set; }
		public string MovieName { get; set; }
		public DateTime DateOfPost { get; set; }
		public User User { get; set; }
		public Guid UserId { get; set; }
		public string TextOfReview { get; set; } //добавить ограничение по символам 
	}
}
