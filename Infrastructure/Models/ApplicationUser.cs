using Domain.Entities.Account;
using Microsoft.AspNetCore.Identity;
using MovieTogether.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class ApplicationUser: IdentityUser
    {
        public List<Movie>? LikedMovies { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
