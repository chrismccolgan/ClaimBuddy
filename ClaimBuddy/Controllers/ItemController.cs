using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClaimBuddy.Models;
using ClaimBuddy.Models.ViewModels;
using ClaimBuddy.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace ClaimBuddy.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ItemController(IItemRepository itemRepository, ICategoryRepository categoryRepository)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
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
            List<Item> items = _itemRepository.GetAll(CurrentUserProfileId);
            return View(items);
        }

        public ActionResult Details(int id)
        {
            Item item = _itemRepository.GetById(id, CurrentUserProfileId);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        public ActionResult Create()
        {
            var vm = new ItemFormViewModel
            {
                Item = new Item(),
                Categories = _categoryRepository.GetAll()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item)
        {
            try
            {
                item.CreateDateTime = DateAndTime.Now;
                item.IsDeleted = false;
                item.UserProfileId = CurrentUserProfileId;
                _itemRepository.Add(item);
                return RedirectToAction(nameof(Details), new { id = item.Id });
            }
            catch
            {
                ItemFormViewModel vm = new ItemFormViewModel()
                {
                    Item = item,
                    Categories = _categoryRepository.GetAll()
                };
                return View(vm);
            }
        }

        public ActionResult Edit(int id)
        {
            Item item  = _itemRepository.GetById(id, CurrentUserProfileId); 
            if (item == null)
            {
                return NotFound();
            }
            var vm = new ItemFormViewModel
            {
                Item = item,
                Categories = _categoryRepository.GetAll()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            try
            {
                _itemRepository.Update(item);
                return RedirectToAction(nameof(Details), new { id = item.Id });
            }
            catch
            {
                return View(item);
            }
        }

        public ActionResult Delete(int id)
        {
            Item item = _itemRepository.GetById(id, CurrentUserProfileId);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Item item)
        {
            try
            {
                _itemRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(item);
            }
        }
    }
}
