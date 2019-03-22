using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data.Interfaces;
using ShoppingCart.Models;
using ShoppingCart.Utilities;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        [BindProperty]
        public OrderItemCartViewModel orderItemCartVM { get; set; }
        public CartController(IApplicationUserRepository applicationUserRepository,ICartRepository cartRepository, IProductRepository productRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _applicationUserRepository = applicationUserRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }
        public IActionResult Index()
        {
            orderItemCartVM = new OrderItemCartViewModel()
            {
                Order = new Order()
            };

            orderItemCartVM.Order.OrderTotal = 0;
            orderItemCartVM.Order.OrderTotalOriginal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var carts = _cartRepository.GetCartsByUserId(claim.Value);

            if (carts != null)
            {
                orderItemCartVM.CartList = carts.ToList();
            }

            foreach (var list in orderItemCartVM.CartList)
            {
                list.Product = _productRepository.GetById(list.ProductId);
                orderItemCartVM.Order.OrderTotal += list.Product.Price * list.Count;
            }

            return View(orderItemCartVM); 
        }

        public IActionResult Summary()
        {
            orderItemCartVM = new OrderItemCartViewModel()
            {
                Order = new Order()
            };

            orderItemCartVM.Order.OrderTotal = 0;
            orderItemCartVM.Order.OrderTotalOriginal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var carts = _cartRepository.GetCartsByUserId(claim.Value);

            if (carts != null)
            {
                orderItemCartVM.CartList = carts.ToList();
            }

            foreach (var list in orderItemCartVM.CartList)
            { 
                list.Product = _productRepository.GetById(list.ProductId);
                orderItemCartVM.Order.OrderTotal += list.Product.Price * list.Count;
            }

            var user = _applicationUserRepository.GetApplicationUserById(claim.Value);
            orderItemCartVM.Order.PickupName = user.Name;
            orderItemCartVM.Order.PhoneNumber = user.PhoneNumber;
            return View(orderItemCartVM);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Summary")]
        public IActionResult SummaryPost()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _applicationUserRepository.GetApplicationUserById(claim.Value);
            var carts = _cartRepository.GetCartsByUserId(claim.Value);
            var products = _productRepository.GetAll();

            orderItemCartVM.CartList = carts.ToList();
            orderItemCartVM.Order.ApplicationUserId = claim.Value;
            orderItemCartVM.Order.Status = OrderUtility.PaymentStatusPending;
            orderItemCartVM.Order.PaymentStatus = OrderUtility.PaymentStatusPending;
            orderItemCartVM.Order.OrderDate = DateTime.Now;
            orderItemCartVM.Order.PickupName = user.Name;
            orderItemCartVM.Order.PhoneNumber = user.PhoneNumber;
            _orderRepository.Create(orderItemCartVM.Order);

            var orderItemList = new List<OrderItem>();

            foreach (var cart in orderItemCartVM.CartList)
            {
                var product = products.FirstOrDefault(p => p.Id.Equals(cart.ProductId));
                var orderItem = new OrderItem
                {
                    OrderId = orderItemCartVM.Order.Id,
                    ProductId = product.Id,
                    Count = cart.Count,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price
                };
                orderItemCartVM.Order.OrderTotal += orderItem.Count * orderItem.Price;
                orderItemList.Add(orderItem);
            }

            _orderItemRepository.CreateRange(orderItemList);
            _cartRepository.DeleteRange(orderItemCartVM.CartList);
            //return View(orderItemCartVM);
            return RedirectToAction("Confirm", "Order", new { id = orderItemCartVM.Order.Id });
        }

        public IActionResult Plus(int cartId)
        {
            var cart = _cartRepository.GetById(cartId);
            cart.Count += 1;
            _cartRepository.Update(cart);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _cartRepository.GetById(cartId);
            if (cart.Count == 1)
            {
                _cartRepository.Delete(cart);

                var cartCount = _cartRepository.GetCartsByUserId(cart.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(SessionUtility.CartCount, cartCount);
            }
            else
            {
                cart.Count -= 1;
                _cartRepository.Update(cart);
            }
            _cartRepository.Update(cart);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartid)
        {
            var cart = _cartRepository.GetById(cartid);
            _cartRepository.Delete(cart);
            var cartCount = _cartRepository.GetCartsByUserId(cart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32(SessionUtility.CartCount, cartCount);
            return RedirectToAction(nameof(Index));
        }
    }
}