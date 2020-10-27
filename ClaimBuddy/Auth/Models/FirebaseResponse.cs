using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimBuddy.Auth.Models
{
    public class FirebaseResponse
    {
        // A Firebase Auth ID token for the authenticated user.
        public string IdToken { get; set; }

        // The email for the authenticated user.
        public string Email { get; set; }

        // A Firebase Auth refresh token for the authenticated user.
        public string RefreshToken { get; set; }

        // The number of seconds in which the ID token expires.
        public string ExpiresIn { get; set; }

        // The uid of the authenticated user.
        public string LocalId { get; set; }
    }
}
