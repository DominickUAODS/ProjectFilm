namespace ProjectFilm.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public ImageForBase? ImageForBase { get; set; }
        public Guid ImageId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public DateTime RegisterDate { get; set; }
        public List<ChatMessage> Messages { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
