using Application.DTOs.Movie;
using MovieTogether.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{
    public interface IMovieMappingService
    {
        MovieDTO? MapMovieToDto(Movie movie);
        List<MovieDTO>? MapMoviesToDto(List<Movie> movies);
    }
}
