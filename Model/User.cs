using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProjectFilm.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public ImageForBase? ImageForBase { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public DateTime RegisterDate { get; set; }
        public IEnumerable<ChatMessage> Messages { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}
