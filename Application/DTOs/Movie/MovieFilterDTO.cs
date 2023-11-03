using Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Movie
{
    public class MovieFilterDTO
    {
        public List<int> CountryIds { get; set; } = new List<int>();
        public List<int> GenreIds { get; set; } = new List<int>();
        public List<int> YearsRange { get; set; } = new List<int>();
        public MovieSorts MovieSort { get; set; } = MovieSorts.RateCountDescending;
        public int Page { get; set; } = 1;
    }
}
