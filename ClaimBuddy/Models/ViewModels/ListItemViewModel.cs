using System.Collections.Generic;

namespace ClaimBuddy.Models.ViewModels
{
    public class ListItemViewModel
    {
        public MyList MyList { get; set; }
        public List<int> SelectedItemIds { get; set; }
        public List<Item> AllItems { get; set; }
    }
}