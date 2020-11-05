using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaimBuddy.Models
{
    public class MyList
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int UserProfileId { get; set; }
        public List<Item> Items { get; set; }
    }
}
