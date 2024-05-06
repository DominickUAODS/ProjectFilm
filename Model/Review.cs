using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFilm.Model
{
	public class Review
	{
		public Guid Id { get; set; }
		public string MovieName { get; set; }
		public DateTime DateOfPost { get; set; }
		public User? user { get; set; }
		public Guid UserId { get; set; }
		public string TextOfReview { get; set; } //добавить ограничение по символам 
	}
}
