using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class AccountException: Exception
    {
        public AccountException() : base() { }
        public AccountException(string message) : base(message) { }
    }
}
