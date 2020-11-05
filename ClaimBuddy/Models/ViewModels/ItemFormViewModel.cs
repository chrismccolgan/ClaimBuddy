using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;

namespace ClaimBuddy.Models.ViewModels
{
    public class ItemFormViewModel
    {
        public Item Item { get; set; }
        public List<Category> Categories { get; set; }
        [DisplayName("Item Image")]
        public IFormFile ImageFile { get; set; }
        [DisplayName("Receipt Image")]
        public IFormFile ReceiptImageFile { get; set; }
    }
}
