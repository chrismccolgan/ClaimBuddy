using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ClaimBuddy.Models
{
    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDateTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        public bool IsDeleted { get; set; }

        [DisplayName("Category")]
        public int CategoryId { get; set; }

        //public Category Category { get; set; }

        public int UserProfileId { get; set; }
    }
}
