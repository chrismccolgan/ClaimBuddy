using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ClaimBuddy.Models;
using ClaimBuddy.Models.ViewModels;
using ClaimBuddy.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClaimBuddy.Controllers
{
    [Authorize]
    public class MyListController : Controller
    {
        private readonly IMyListRepository _myListRepository;
        private readonly IItemRepository _itemRepository;

        public MyListController(IMyListRepository myListRepository, IItemRepository itemRepository)
        {
            _myListRepository = myListRepository;
            _itemRepository = itemRepository;
        }

        private int CurrentUserProfileId
        {
            get
            {
                return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
        }

        public ActionResult Index()
        {
            List<MyList> myLists = _myListRepository.GetAll(CurrentUserProfileId);
            return View(myLists);
        }

        public ActionResult Details(int id)
        {
            MyList myList = _myListRepository.GetById(id, CurrentUserProfileId);
            return View(myList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MyList myList)
        {
            try
            {
                myList.UserProfileId = CurrentUserProfileId;
                _myListRepository.Add(myList);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(myList);
            }
        }

        public ActionResult Edit(int id)
        {
            MyList myList = _myListRepository.GetById(id, CurrentUserProfileId);
            List<int> selectedItemIds = myList.Items.Select(m => m.Id).ToList();
            List<Item> allItems = _itemRepository.GetAll(CurrentUserProfileId);
            ListItemViewModel vm = new ListItemViewModel()
            {
                MyList = myList,
                SelectedItemIds = selectedItemIds,
                AllItems = allItems,
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ListItemViewModel vm)
        {
            try
            {
                _myListRepository.Update(vm.MyList, vm.SelectedItemIds);
                return RedirectToAction(nameof(Details), new { id = vm.MyList.Id });
            }
            catch
            {
                return View(vm);
            }
        }

        public ActionResult Delete(int id)
        {
            MyList myList = _myListRepository.GetById(id, CurrentUserProfileId);
            if (myList == null)
            {
                return NotFound();
            }
            return View(myList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, MyList myList)
        {
            try
            {
                _myListRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(myList);
            }
        }
    }
}
