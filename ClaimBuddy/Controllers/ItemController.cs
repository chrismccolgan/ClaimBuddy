using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using ClaimBuddy.Models;
using ClaimBuddy.Models.ViewModels;
using ClaimBuddy.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ClaimBuddy.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ItemController(IItemRepository itemRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
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
        public ActionResult Create(ItemFormViewModel vm)
        {
            try
            {
                if (vm.ImageFile != null)
                {
                    string uniqueFileName = UploadedFile(vm);
                    vm.Item.Image = uniqueFileName;
                }
                if (vm.ReceiptImageFile != null)
                {
                    string uniqueFileName2 = UploadedFile2(vm);
                    vm.Item.ReceiptImage = uniqueFileName2;
                }
                vm.Item.CreateDateTime = DateTime.Now;
                vm.Item.IsDeleted = false;
                vm.Item.UserProfileId = CurrentUserProfileId;
                _itemRepository.Add(vm.Item);
                return RedirectToAction(nameof(Details), new { id = vm.Item.Id });
            }
            catch
            {
                vm.Categories = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        private string UploadedFile(ItemFormViewModel vm)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.ImageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                vm.ImageFile.CopyTo(fileStream);
            }        
            return uniqueFileName;
        }

        private string UploadedFile2(ItemFormViewModel vm)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            string uniqueFileName2 = Guid.NewGuid().ToString() + "_" + vm.ReceiptImageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName2);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                vm.ReceiptImageFile.CopyTo(fileStream);
            }
            return uniqueFileName2;
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
        public ActionResult Edit(ItemFormViewModel vm)
        {
            try
            {
                if (vm.ImageFile != null)
                {
                    string uniqueFileName = UploadedFile(vm);
                    vm.Item.Image = uniqueFileName;
                }
                if (vm.ReceiptImageFile != null)
                {
                    string uniqueFileName2 = UploadedFile2(vm);
                    vm.Item.ReceiptImage = uniqueFileName2;
                }
                _itemRepository.Update(vm.Item);
                return RedirectToAction(nameof(Details), new { id = vm.Item.Id });
            }
            catch
            {
                return View(vm);
            }
        }

        public ActionResult Delete(int id)
        {
            Item item = _itemRepository.GetById(id, CurrentUserProfileId);
            if (item == null)
            {
                return NotFound();
            }
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
