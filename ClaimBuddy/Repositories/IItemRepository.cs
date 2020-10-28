using ClaimBuddy.Models;
using System.Collections.Generic;

namespace ClaimBuddy.Repositories
{
    public interface IItemRepository
    {
        void Add(Item item);
        void Delete(int id);
        List<Item> GetAll(int userProfileId);
        Item GetById(int id, int userProfileId);
        void Update(Item item);
    }
}