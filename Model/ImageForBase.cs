using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFilm.Model
{
    public  class ImageForBase
    {
        public Guid Id { get; set; }
        public byte[] Data { get; set; }
        public List<User>? user { get; set;}
    }
}
