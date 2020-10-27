using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimBuddy.Auth.Models
{
    public class FirebaseRequest
    {
        public FirebaseRequest(string email, string password)
        {
            Email = email;
            Password = password;
            ReturnSecureToken = true;
        }

        public string Email { get; }
        public string Password { get; }
        public bool ReturnSecureToken { get; }
    }
}
