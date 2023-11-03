using Application.DTOs.Movie;
using Application.Mapping;
using MovieTogether.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class MovieMappingService : IMovieMappingService
    {
        public List<MovieDTO>? MapMoviesToDto(List<Movie> movies)
        {
            if(movies is null)
            {
                return null;
            }
            List<MovieDTO> movieDTOs = new();
            foreach (Movie movie in movies)
            {
                movieDTOs.Add(MapMovieToDto(movie));
            }
            return movieDTOs;
        }

        public MovieDTO? MapMovieToDto(Movie movie)
        {
            if(movie is null)
            {
                return null;
            }
            return new MovieDTO()
            {
                Actors = movie.Actors?.Select(x => x.FullName).ToList(),
                Countries = movie.Countries?.Select(x => x.Name).ToList(),
                RateCount = movie.RateCount,
                Description = movie.Description,
                Directors = movie.Directors?.Select(x => x.FullName).ToList(),
                Genres = movie.Genres?.Select(x => x.Name).ToList(),
                Id = movie.Id,
                KinopoiskId = movie.KinopoiskId,
                PosterUrl = movie.PosterUrl,
                Rating = movie.Rating,
                TimeInMinutes = movie.TimeInMinutes,
                Title = movie.Title,
                Year = movie.Year,
                IsLiked = false
            };
        }
    }
}
