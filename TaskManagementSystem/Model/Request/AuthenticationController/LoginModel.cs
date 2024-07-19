using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Model.Request.AuthenticationController
{
    public class LoginModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}