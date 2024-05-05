namespace ProjectFilm.Model
{
	public class ImageForBase
	{
		public Guid Id { get; set; }
		public byte[] Data { get; set; }
		public List<User>? user { get; set; }
	}
}
