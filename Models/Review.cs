using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectFilm.Model;

namespace ProjectFilm.Model
{
    public class Review
    {
        public Guid Id { get; set; }
        public Movie Movie { get; set; }
        public Guid MovieId { get; set; }
        public DateTime DateOfPost { get; set; }
        public User user { get; set; }
        public Guid UserId { get; set; }
        public string TextOfReview { get; set; } //добавить ограничение по символам 



    }
}
