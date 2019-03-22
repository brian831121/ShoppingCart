using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data.Interfaces;
using ShoppingCart.Models;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderController(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Confirm(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderItemViewModel orderItemVM = new OrderItemViewModel
            {
                Order = _orderRepository.GetOrdersByUserId(claim.Value).FirstOrDefault(o => o.Id.Equals(id)),
                OrderItems = _orderItemRepository.GetOrderItemsByOrderId(id).ToList()
            };

            return View(orderItemVM);
        }

        [Authorize]
        public IActionResult OrderHistory()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<OrderItemViewModel> orderItemVMList = new List<OrderItemViewModel>();
            List<Order> orderList = _orderRepository.GetOrdersByUserId(claim.Value).ToList();
            var orderItems = _orderItemRepository.GetAll();

            foreach (var order in orderList)
            {
                OrderItemViewModel orderItemVM = new OrderItemViewModel
                {
                    Order = order,
                    OrderItems = orderItems.Where(o => o.OrderId.Equals(order.Id)).ToList()
                };
            }

            return View(orderItemVMList);
        }

        public IActionResult GetOrderItems(int id)
        {
            OrderItemViewModel orderItemVM = new OrderItemViewModel
            {
                Order = _orderRepository.GetById(id),
                OrderItems = _orderItemRepository.GetOrderItemsByOrderId(id).ToList()
            };

            return PartialView("_IndivitaulOrderItems", orderItemVM);
        }
    }
}