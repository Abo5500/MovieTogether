using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Filter
{
    public class PagedActorDTO : PagedBase
    {
        public List<ActorDTO> Actors { get; set; } = new();
        
    }
}
