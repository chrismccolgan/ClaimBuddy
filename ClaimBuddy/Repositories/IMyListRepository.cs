using ClaimBuddy.Models;
using System.Collections.Generic;

namespace ClaimBuddy.Repositories
{
    public interface IMyListRepository
    {
        void Add(MyList myList);
        void Delete(int myListId);
        List<MyList> GetAll(int userProfileId);
        MyList GetById(int id, int userProfileId);
        void Update(MyList myList, List<int> selectedItemIds);
    }
}