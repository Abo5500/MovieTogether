using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PagedBase
    {
        public int EntityCount { get; set; } = 0;
        public int Page { get; set; } = 1;
        public int PageCount { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
