using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Filter
{
    public class DirectorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<string>? TopMovies { get; set; }
        public int MovieCount { get; set; }
    }
}
