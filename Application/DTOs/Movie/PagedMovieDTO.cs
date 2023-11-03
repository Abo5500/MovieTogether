using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Movie
{
    public class PagedMovieDTO : PagedBase
    {
        public List<MovieDTO> MovieDTOs { get; set; } = new();
    }
}
