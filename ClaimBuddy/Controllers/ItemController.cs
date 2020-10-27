using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClaimBuddy.Models;
using ClaimBuddy.Models.ViewModels;
using ClaimBuddy.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace ClaimBuddy.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ItemController(IItemRepository itemRepository, ICategoryRepository categoryRepository)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: ItemController
        public ActionResult Index()
        {
            List<Item> items = _itemRepository.GetAll();
            return View(items);
        }

        // GET: ItemController/Details/5
        public ActionResult Details(int id)
        {
            Item item = _itemRepository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // GET: ItemController/Create
        public ActionResult Create()
        {
            var vm = new ItemFormViewModel
            {
                Item = new Item(),
                Categories = _categoryRepository.GetAll()
            };
            return View(vm);
        }

        // POST: ItemController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item)
        {
            try
            {
                item.CreateDateTime = DateAndTime.Now;
                item.IsDeleted = false;
                item.UserProfileId = 1;
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

        // GET: ItemController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ItemController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ItemController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ItemController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
