using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClaimBuddy.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ClaimBuddy.Repositories;

namespace ClaimBuddy.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;
        
        private int GetCurrentUserProfileId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public HomeController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public IActionResult Index()
        {
            var userProfileId = GetCurrentUserProfileId();
            var userProfile = _userProfileRepository.GetById(userProfileId);
            return View(userProfile);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
