namespace ProjectFilm.Model
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }

	public class ChatMessageViewModel
	{
		public DateTime ChatDate { get; set; }
		public string ChatUserName { get; set; }
		public Guid ChatUserId { get; set; }
		public string ChatText { get; set; }
	}
}
