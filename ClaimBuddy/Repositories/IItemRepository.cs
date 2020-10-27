using ClaimBuddy.Models;
using System.Collections.Generic;

namespace ClaimBuddy.Repositories
{
    public interface IItemRepository
    {
        void Add(Item item);
        List<Item> GetAll();
        Item GetById(int id);
    }
}