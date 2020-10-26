using ClaimBuddy.Models;
using System.Collections.Generic;

namespace ClaimBuddy.Repositories
{
    public interface IItemRepository
    {
        List<Item> GetAll();
    }
}