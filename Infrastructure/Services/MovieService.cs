using Application.DTOs.Movie;
using Application.Exceptions;
using Application.Interfaces;
using Application.Mapping;
using Infrastructure.Contexts;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTogether.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpContext _httpContext;
        private readonly IMovieMappingService _mappingService;

        public MovieService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMovieMappingService mappingService)
        {
            _context = context;
            _httpContext = httpContextAccessor.HttpContext;
            _mappingService = mappingService;
        }

        public async Task<PagedMovieDTO> GetByActorIdAsync(int actorId, MovieFilterDTO filter)
        {
            PagedMovieDTO dto = new();
            var actor = await _context.Actors.FirstOrDefaultAsync(x => x.Id == actorId)
                ?? throw new AccountException("Incorrect actorId");
            var query = _context.Movies
                .Include(x => x.Actors).Include(x => x.Directors).Include(x => x.Countries).Include(x => x.Genres)
                .Where(x => x.Actors.Contains(actor));

            dto.EntityCount = await query.GetWithFilterCount(filter);
            dto.PageCount = GetPageCount(dto.EntityCount, dto.PageSize);
            List<Movie> movies = new();
            List<MovieDTO> moviesDto = new();
            movies = await query.GetWithFilterPaged(filter, dto.PageSize).ToListAsync();
            moviesDto = _mappingService.MapMoviesToDto(movies);
            await SetLikedMovies(moviesDto);

            dto.MovieDTOs = moviesDto;
            dto.Page = filter.Page;
            return dto;
        }

        public async Task<PagedMovieDTO> GetByDirectorIdAsync(int directorId, MovieFilterDTO filter)
        {
            PagedMovieDTO dto = new();
            var director = await _context.Directors.FirstOrDefaultAsync(x => x.Id == directorId)
                ?? throw new AccountException("Incorrect directorId");
            var query = _context.Movies
                .Include(x => x.Actors).Include(x => x.Directors).Include(x => x.Countries).Include(x => x.Genres)
                .Where(x => x.Directors.Contains(director));

            dto.EntityCount = await query.GetWithFilterCount(filter);
            dto.PageCount = GetPageCount(dto.EntityCount, dto.PageSize);
            List<Movie> movies = new();
            List<MovieDTO> moviesDto = new();
            movies = await query.GetWithFilterPaged(filter, dto.PageSize).ToListAsync();
            moviesDto = _mappingService.MapMoviesToDto(movies);
            await SetLikedMovies(moviesDto);
            
            dto.MovieDTOs = moviesDto;
            dto.Page = filter.Page;
            return dto;
        }

        public async Task<MovieDTO?> GetByIdAsync(int id)
        {
            Movie movie = await _context.Movies
                .Include(x => x.Actors).Include(x => x.Directors).Include(x => x.Countries).Include(x => x.Genres)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (movie is null) return null;
            MovieDTO movieDto = _mappingService.MapMovieToDto(movie);
            
            await SetLikedMovie(movieDto);

            return movieDto;
        }

        public async Task<PagedMovieDTO> GetByTitleAsync(string title, MovieFilterDTO filter)
        {
            PagedMovieDTO dto = new();
            
            var query = _context.Movies
                .Include(x => x.Actors).Include(x => x.Directors).Include(x => x.Countries).Include(x => x.Genres)
                .Where(x => x.Title.ToLower().Contains(title.ToLower()));

            dto.EntityCount = await query.GetWithFilterCount(filter);
            dto.PageCount = GetPageCount(dto.EntityCount, dto.PageSize);
            List<Movie> movies = new();
            List<MovieDTO> moviesDto = new();
            if(dto.EntityCount != 0)
            {
                movies = await query.GetWithFilterPaged(filter, dto.PageSize).ToListAsync();
                moviesDto = _mappingService.MapMoviesToDto(movies);
                await SetLikedMovies(moviesDto);
            }
            dto.MovieDTOs = moviesDto;
            dto.Page = filter.Page;
            return dto;
        }

        public async Task<PagedMovieDTO> GetAllAsync(MovieFilterDTO filter)
        {
            PagedMovieDTO dto = new();
            var movies = await _context.Movies.GetWithFilterPaged(filter, dto.PageSize)
                .Include(x => x.Actors).Include(x => x.Directors).Include(x => x.Countries).Include(x => x.Genres).ToListAsync();
            var moviesDto = _mappingService.MapMoviesToDto(movies);
            await SetLikedMovies(moviesDto);
            dto.EntityCount = await _context.Movies.GetWithFilterCount(filter);
            dto.PageCount = GetPageCount(dto.EntityCount, dto.PageSize);
            dto.MovieDTOs = moviesDto;
            dto.Page = filter.Page;
            return dto;
        }
        public async Task<PagedMovieDTO> GetLikedAsync(string username, MovieFilterDTO filter)
        {
            ApplicationUser user = await _context.Users.Include(x => x.LikedMovies)
                .FirstOrDefaultAsync(x => x.UserName == username) ??
                throw new ApiException($"Пользователя с ником \"{username}\" не существует");
            
            PagedMovieDTO dto = new();
            if(user.LikedMovies != null)
            {
                dto.EntityCount = user.LikedMovies.Count;
                var movies = await _context.Movies
                    .Include(x => x.Actors).Include(x => x.Genres).Include(x => x.Countries).Include(x => x.Directors)
                    .Where(x => user.LikedMovies.Select(y => y.Id).Contains(x.Id)).ToListAsync();
                dto.MovieDTOs = _mappingService.MapMoviesToDto(movies);
                dto.PageCount = GetPageCount(dto.EntityCount, dto.PageSize);
                dto.Page = filter.Page;
            }
            return dto;
        }
        public async Task<bool> SetIsLiked(int movieId, bool isLiked)
        {
            var userId = GetCurrentUserId();
            var user = await _context.Users.Include(x => x.LikedMovies).FirstOrDefaultAsync(x => x.Id == userId) 
                ?? throw new AccountApiException();
            var movie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == movieId) 
                ?? throw new ApiException();
            if (isLiked)
            {
                if (user.LikedMovies != null && !user.LikedMovies.Contains(movie))
                {
                    user.LikedMovies.Add(movie);
                }
                else
                {
                    user.LikedMovies = new List<Movie>() { movie };
                }
            }
            else
            {
                if (user.LikedMovies != null && user.LikedMovies.Contains(movie))
                {
                    user.LikedMovies.Remove(movie);
                }
            }
            await _context.SaveChangesAsync();
            return isLiked;
        }
        public async Task<List<MovieDTO>> GetMovieCoincidencesAsync(List<string> usernames)
        {
            List<MovieDTO> coincidenceMovieDTOs = new List<MovieDTO>();
            Dictionary<int, int> coincidences = new();

            foreach(var username in usernames)
            {
                ApplicationUser user = await _context.Users.Include(x => x.LikedMovies)
                .FirstOrDefaultAsync(x => x.UserName == username) ??
                throw new ApiException($"Пользователя с ником \"{username}\" не существует");
                if (user.LikedMovies != null)
                {
                    foreach(var movie in user.LikedMovies)
                    {
                        if (coincidences.ContainsKey(movie.Id))
                        {
                            coincidences[movie.Id]++;
                        }
                        else
                        {
                            coincidences.Add(movie.Id, 1);
                        }
                    }
                }
            }
            foreach(var coincidence in coincidences)
            {
                if(coincidence.Value != 1)
                {
                    MovieDTO movie = await GetByIdAsync(coincidence.Key);
                    movie.CoincidenceCount = coincidence.Value;
                    coincidenceMovieDTOs.Add(movie);
                }
            }
            return coincidenceMovieDTOs;
        }

        private static int GetPageCount(int count, int pageSize)
        {
            return (int)Math.Ceiling((double)count / pageSize);
        }
        private async Task SetLikedMovie(MovieDTO movieDto, List<int> likedMoviesId = null)
        {
            likedMoviesId ??= await GetLikedMoviesIdAsync();
            if (likedMoviesId.Any())
            {
                if (likedMoviesId.Contains(movieDto.Id))
                {
                    movieDto.IsLiked = true;
                }
            }
        }
        private async Task SetLikedMovies(List<MovieDTO> moviesDto)
        {
            if (moviesDto.Any())
            {
                var likedMoviesId = await GetLikedMoviesIdAsync();
                foreach (var movie in moviesDto)
                {
                    await SetLikedMovie(movie, likedMoviesId);
                }
            }
        }
        private async Task<List<int>> GetLikedMoviesIdAsync()
        {
            List<int> ids = new();
            string userId = GetCurrentUserId();
            if(userId != null)
            {
                ApplicationUser user = await _context.Users.Include(x => x.LikedMovies).FirstOrDefaultAsync(x => x.Id == userId);
                if(user != null && user.LikedMovies != null)
                {
                    foreach (var movie in user.LikedMovies)
                    {
                        ids.Add(movie.Id);
                    }
                }
            }

            return ids;
        }
        private string? GetCurrentUserId()
        {
            string? userId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }

    }
}
