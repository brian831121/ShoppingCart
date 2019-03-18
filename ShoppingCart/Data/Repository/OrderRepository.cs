using ShoppingCart.Data.Interfaces;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext db) : base(db)
        {
        }

        public IEnumerable<Order> GetOrdersByUserId(string id)
        {
            return _db.Order.Where(o => o.ApplicationUserId.Equals(id));
        }
    }
}
