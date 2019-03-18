using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.ViewModels
{
    public class OrderItemCartViewModel
    {
        public List<Cart> CardList { get; set; }
        public Order Order { get; set; }
    }
}
