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
        [DisplayName("Value")]
        public decimal Price { get; set; }

        [DisplayName("Receipt Image")]
        public string ReceiptImage { get; set; }

        public string Model { get; set; }

        [DisplayName("Item Image")]
        public string Image { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Created")]
        public DateTime CreateDateTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Purchased")]
        public DateTime PurchaseDateTime { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int UserProfileId { get; set; }
    }
}