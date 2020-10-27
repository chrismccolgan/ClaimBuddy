using ClaimBuddy.Models;
using System.Collections.Generic;

namespace ClaimBuddy.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
    }
}