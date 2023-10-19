using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Account
{
    public class RefreshTokensResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
