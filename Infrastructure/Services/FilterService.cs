using Application.DTOs.Filter;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class FilterService : IFilterService
    {
        public Task<PagedActorDTO> GetActorsByNameAsync(string fullName, int page = 1)
        {
            throw new NotImplementedException();
        }

        public List<CountryDTO> GetCountries()
        {
            throw new NotImplementedException();
        }

        public Task<PagedDirectorDTO> GetDirectorsByNameAsync(string fullName, int page = 1)
        {
            throw new NotImplementedException();
        }

        public List<GenreDTO> GetGenres()
        {
            throw new NotImplementedException();
        }

        public List<MovieSortDTO> GetMovieSorts()
        {
            throw new NotImplementedException();
        }
    }
}
