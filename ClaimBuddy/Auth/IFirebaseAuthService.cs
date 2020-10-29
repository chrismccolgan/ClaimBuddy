using ClaimBuddy.Auth.Models;
using System.Threading.Tasks;

namespace ClaimBuddy.Auth
{
    public interface IFirebaseAuthService
    {
        Task<FirebaseUser> Login(Credentials credentials);
        Task<FirebaseUser> Register(Registration registration);
    }
}
