using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Filter
{
    public class ActorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<string>? Movies { get; set; }
    }
}
