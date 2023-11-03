using Application.DTOs.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFilterService
    {
        List<MovieSortDTO> GetMovieSorts();
        List<GenreDTO> GetGenres();
        List<CountryDTO> GetCountries();
        Task<PagedActorDTO> GetActorsByNameAsync(string fullName, int page = 1);
        Task<PagedDirectorDTO> GetDirectorsByNameAsync(string fullName, int page = 1);   
    }
}
