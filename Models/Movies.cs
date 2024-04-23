using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFilm.Model
{
    public class Movies

    {

        public int Page { get; set; }

        public List<Movie> Results { get; set; }

        public int Total_Pages { get; set; }

        public int Total_Results { get; set; }

    }
}
