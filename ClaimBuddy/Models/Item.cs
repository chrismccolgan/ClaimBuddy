using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClaimBuddy.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Notes { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Image { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Created")]
        public DateTime CreateDateTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Purchased")]
        public DateTime PurchaseDate { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int UserProfileId { get; set; }
    }
}
