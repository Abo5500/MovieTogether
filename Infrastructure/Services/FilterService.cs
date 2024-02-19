using Application.DTOs.Filter;
using Application.Enums;
using Application.Interfaces;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using MovieTogether.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class FilterService : IFilterService
    {
        private readonly ApplicationDbContext _context;

        public FilterService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedActorDTO> GetActorsByNameAsync(string fullName, int page)
        {
            PagedActorDTO pagedActorDTO = new ();
            var query = _context.Actors.Include(x => x.Movies).Where(x => x.FullName.ToLower().Contains(fullName.ToLower()));
            pagedActorDTO.EntityCount = query.Count();
            List<ActorDTO> actorsDto = new();
            if(pagedActorDTO.EntityCount > 0) 
            {
                var actors = await query.OrderByDescending(x => x.Movies.Select(y => y.RateCount).Sum()).GetPaged(page, pagedActorDTO.PageSize).ToListAsync();
                foreach (var actor in actors)
                {
                    ActorDTO actorDto = new()
                    {
                        Id = actor.Id,
                        FullName = actor.FullName,
                        TopMovies = actor.Movies.OrderByDescending(x => x.RateCount).Take(10).Select(x => x.Title).ToList()
                    };
                    actorDto.MovieCount = actorDto.TopMovies.Count;
                    actorsDto.Add(actorDto);
                }
            }
            pagedActorDTO.Actors = actorsDto;
            pagedActorDTO.Page = page;
            pagedActorDTO.PageCount = GetPageCount(page, pagedActorDTO.PageSize);

            return pagedActorDTO;
        }

        public async Task<List<CountryDTO>> GetCountriesAsync()
        {
            List<Country> countries = await _context.Countries.OrderBy(x => x.Name).ToListAsync();
            List<CountryDTO> countriesDto = new();
            foreach (var country in countries)
            {
                CountryDTO countryDto = new()
                {
                    Id = country.Id,
                    Name = country.Name,
                };
                countriesDto.Add(countryDto);
            }
            return countriesDto;
        }

        public async Task<PagedDirectorDTO> GetDirectorsByNameAsync(string fullName, int page)
        {
            PagedDirectorDTO pagedDirectorDto = new();
            var query = _context.Directors.Include(x => x.Movies).Where(x => x.FullName.ToLower().Contains(fullName.ToLower()));
            pagedDirectorDto.EntityCount = query.Count();
            List<DirectorDTO> directorsDto = new();
            if (pagedDirectorDto.EntityCount > 0)
            {
                var directors = await query.OrderByDescending(x => x.Movies.Select(y => y.RateCount).Sum()).GetPaged(page, pagedDirectorDto.PageSize).ToListAsync();
                foreach (var director in directors)
                {
                    DirectorDTO directorDto = new()
                    {
                        Id = director.Id,
                        FullName = director.FullName,
                        TopMovies = director.Movies.OrderByDescending(x => x.RateCount).Take(10).Select(x => x.Title).ToList()
                    };
                    directorDto.MovieCount = directorDto.TopMovies.Count;
                    directorsDto.Add(directorDto);
                }
            }
            pagedDirectorDto.Directors = directorsDto;
            pagedDirectorDto.Page = page;
            pagedDirectorDto.PageCount = GetPageCount(page, pagedDirectorDto.PageSize);

            return pagedDirectorDto;
        }

        public async Task<List<GenreDTO>> GetGenresAsync()
        {
            List<Genre> genres = await _context.Genres.OrderBy(x => x.Name).ToListAsync();
            List<GenreDTO> genresDto = new();
            foreach(var genre in genres)
            {
                GenreDTO genreDto = new()
                {
                    Id = genre.Id,
                    Name = genre.Name
                };
                genresDto.Add(genreDto);
            }
            return genresDto;
        }

        public List<MovieSortDTO> GetMovieSorts()
        {
            int length = Enum.GetValues(typeof(MovieSorts)).Length;
            List<MovieSortDTO> sorts = new();
            for(int i = 0; i < length; i++)
            {
                MovieSortDTO movieSortDto = new()
                {
                    Id = i,
                    Name = ((MovieSorts)i).ToString()
                };
                sorts.Add(movieSortDto);    
            }
            return sorts;
        }

        public async Task<PagedActorDTO> GetActorsAsync(int page)
        {
            PagedActorDTO pagedActorDto = new();
            var query = _context.Actors.Include(x => x.Movies).OrderByDescending(x => x.Movies.Select(y => y.RateCount).Sum());
            pagedActorDto.Page = page;
            pagedActorDto.EntityCount = await query.CountAsync();
            pagedActorDto.PageCount = GetPageCount(pagedActorDto.EntityCount, pagedActorDto.PageSize);
            List<Actor> actors = await query.GetPaged(page, pagedActorDto.PageSize).ToListAsync();

            foreach(var actor in actors)
            {
                ActorDTO actorDTO = new ActorDTO()
                {
                    Id = actor.Id,
                    FullName = actor.FullName,
                    MovieCount = actor.Movies.Count(),
                    TopMovies = actor.Movies.OrderByDescending(x => x.RateCount).Take(10).Select(x => x.Title).ToList()
                };
                pagedActorDto.Actors.Add(actorDTO);
            }
            return pagedActorDto;
        }

        public async Task<PagedDirectorDTO> GetDirectorsAsync(int page)
        {
            PagedDirectorDTO pagedDirectorDto = new();
            var query = _context.Directors.Include(x => x.Movies).OrderByDescending(x => x.Movies.Select(y => y.RateCount).Sum());
            pagedDirectorDto.Page = page;
            pagedDirectorDto.EntityCount = await query.CountAsync();
            pagedDirectorDto.PageCount = GetPageCount(pagedDirectorDto.EntityCount, pagedDirectorDto.PageSize);
            List<Director> directors = await query.GetPaged(page, pagedDirectorDto.PageSize).ToListAsync();

            foreach (var director in directors)
            {
                DirectorDTO actorDTO = new DirectorDTO()
                {
                    Id = director.Id,
                    FullName = director.FullName,
                    MovieCount = director.Movies.Count(),
                    TopMovies = director.Movies.OrderByDescending(x => x.RateCount).Take(10).Select(x => x.Title).ToList()
                };
                pagedDirectorDto.Directors.Add(actorDTO);
            }
            return pagedDirectorDto;
        }

        private static int GetPageCount(int count, int pageSize)
        {
            return (int)Math.Ceiling((double)count / pageSize);
        }
    }
}
