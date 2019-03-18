using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        public OrderItemCartViewModel orderItemCartVM { get; set; }
        public CartController()
        {

        }
        public IActionResult Index()
        {
            orderItemCartVM = new OrderItemCartViewModel()
            {
                Order = new Order()
            };

            orderItemCartVM.Order.OrderTotal = 0;
            orderItemCartVM.Order.OrderTotalOriginal = 0;
        }
    }
}