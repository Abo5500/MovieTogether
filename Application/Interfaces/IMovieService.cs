using Application.DTOs.Movie;
using MovieTogether.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMovieService
    {
        Task<MovieDTO> GetByIdAsync(int id);
        Task<PagedMovieDTO> GetByTitleAsync(string title, MovieFilterDTO filter);
        Task<PagedMovieDTO> GetAllAsync(MovieFilterDTO filter);
        Task<PagedMovieDTO> GetByActorIdAsync(int actorId, MovieFilterDTO filter);
        Task<PagedMovieDTO> GetByDirectorIdAsync(int directorId, MovieFilterDTO filter);
        Task<PagedMovieDTO> GetLikedAsync(string userName, MovieFilterDTO filter);
        Task<bool> SetIsLiked(int movieId, bool isLiked); 
    }
}
