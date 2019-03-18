using ShoppingCart.Data.Interfaces;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Repository
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationDbContext db) : base(db)
        {
        }

        public IEnumerable<OrderItem> GetOrderItemsByOrderId(int id)
        {
            return _db.OrderItem.Where(o => o.OrderId.Equals(id));
        }
    }
}
