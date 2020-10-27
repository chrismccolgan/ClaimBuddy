using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimBuddy.Models.ViewModels
{
    public class ItemFormViewModel
    {
        public Item Item { get; set; }
        public List<Category> Categories { get; set; }
    }
}
