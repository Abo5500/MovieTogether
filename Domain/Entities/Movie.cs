using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTogether.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }
        public int KinopoiskId { get; set; }
        public string? PosterUrl { get; set; }
        public double? Rating { get; set; }
        public int RateCount { get; set; }
        public int TimeInMinutes { get; set; }
        public List<Actor>? Actors { get; set; }
        public List<Genre>? Genres { get; set; }
        public List<Director>? Directors { get; set;}
        public List<Country>? Countries { get; set; }
    }
}
