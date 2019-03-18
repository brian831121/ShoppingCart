using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Count = 1;
        }
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }

        [NotMapped]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int ProductId { get; set; }

        [NotMapped]
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "輸入的值須大於{1}")]
        public int Count { get; set; }

    }
}
