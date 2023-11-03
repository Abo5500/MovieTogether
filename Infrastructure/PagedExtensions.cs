using Application.DTOs.Movie;
using Application.Enums;
using MovieTogether.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class PagedExtensions
    {
        public static IQueryable<T> GetPaged<T>(this IQueryable<T> values, int page, int pageSize)
        {
            return values.Skip((page - 1) * pageSize).Take(pageSize);
        }
        public static int GetWithFilterCount(this IQueryable<Movie>? movies, MovieFilterDTO filter)
        {
            movies = movies.GetWithFilter(filter);
            if (movies != null)
            {
                return movies.Count();
            }
            return 0;
        }
        public static IQueryable<Movie>? GetWithFilterPaged(this IQueryable<Movie>? movies, MovieFilterDTO filter, int pageSize)
        {
            return movies.GetWithFilter(filter)?.GetPaged(filter.Page, pageSize);
        }
        public static IQueryable<Movie>? GetWithFilter(this IQueryable<Movie>? movies, MovieFilterDTO filter)
        {
            if(movies == null) return null;
            if (filter.YearsRange.Any())
            {
                movies = movies.Where(x => filter.YearsRange.Contains(x.Year));
            }
            if (filter.CountryIds.Any())
            {
                movies = movies.Where(x => x.Countries.Where(y => filter.CountryIds.Contains(y.Id)).Any());
            }
            if (filter.GenreIds.Any())
            {
                movies = movies.Where(x => x.Genres.Where(y => filter.GenreIds.Contains(y.Id)).Any());
            }
            switch (filter.MovieSort)
            {
                case MovieSorts.RateCountDescending:
                    movies = movies.OrderByDescending(x => x.RateCount);
                    break;
                case MovieSorts.RatingDescending:
                    movies = movies.OrderByDescending(x => x.Rating);
                    break;
                case MovieSorts.YearDescending:
                    movies = movies.OrderByDescending(x => x.Year);
                    break;
                case MovieSorts.TitleDescending:
                    movies = movies.OrderByDescending(x => x.Title);
                    break;
                case MovieSorts.TimeInMinutesDescending:
                    movies = movies.OrderByDescending(x => x.TimeInMinutes);
                    break;
                case MovieSorts.RateCount:
                    movies = movies.OrderBy(x => x.RateCount);
                    break;
                case MovieSorts.Rating:
                    movies = movies.OrderBy(x => x.Rating);
                    break;
                case MovieSorts.Title:
                    movies = movies.OrderBy(x => x.Title);
                    break;
                case MovieSorts.Year:
                    movies = movies.OrderBy(x => x.Year);
                    break;
                case MovieSorts.TimeInMinutes:
                    movies = movies.OrderBy(x => x.TimeInMinutes);
                    break;
            }
            return movies;
        }
    }
}
