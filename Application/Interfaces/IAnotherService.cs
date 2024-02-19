using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAnotherService
    {
        Task<bool> RemoveAllActors();
        Task<bool> StartWork();
        Task<bool> RemoveNoneActor();
        Task<bool> RenameCharsActors();
        Task<bool> RenameCharsDirectors();
        Task<bool> FindDirectors();
    }
}
