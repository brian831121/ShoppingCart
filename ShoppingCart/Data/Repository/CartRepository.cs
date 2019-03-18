using ShoppingCart.Data.Interfaces;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext db) : base(db)
        {
        }

        public IEnumerable<Cart> GetCartsByUserId(string id)
        {
            return _db.Cart.Where(c => c.ApplicationUserId.Equals(id));
        }
    }
}
