﻿namespace ProjectFilm.Model
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
