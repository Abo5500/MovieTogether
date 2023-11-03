using MovieTogether.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Movie
{
    public class MovieDTO
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
        public List<string>? Actors { get; set; }
        public List<string>? Genres { get; set; }
        public List<string>? Directors { get; set; }
        public List<string>? Countries { get; set; }
        public bool IsLiked { get; set; }
    }
}
