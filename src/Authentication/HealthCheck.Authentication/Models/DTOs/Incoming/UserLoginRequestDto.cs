using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.Authentication.Models.DTOs.Incoming
{
    public class UserLoginRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
