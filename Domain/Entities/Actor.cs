using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTogether.Domain.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int KinopoiskId { get; set; }
        public List<Movie>? Movies { get; set; }
    }
}
