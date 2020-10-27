using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimBuddy.Auth.Models
{
    public class FirebaseUser
    {
        public string Email { get; }
        public string FirebaseUserId { get; }

        public FirebaseUser(string email, string firebaseUserId)
        {
            Email = email;
            FirebaseUserId = firebaseUserId;
        }
    }
}
