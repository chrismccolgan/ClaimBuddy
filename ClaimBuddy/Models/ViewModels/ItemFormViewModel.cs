using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ClaimBuddy.Models.ViewModels
{
    public class ItemFormViewModel
    {
        public Item Item { get; set; }
        public List<Category> Categories { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
