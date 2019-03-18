using ShoppingCart.Data.Interfaces;
using ShoppingCart.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext db) : base(db)
        {

        }

        public IEnumerable<Product> GetAllWithCategory()
        {
            return _db.Product.Include(p => p.Category);
        }

        public Product GetWithCategoryById(int id)
        {
            return _db.Product.Include(p => p.Category).FirstOrDefault(m => m.Id.Equals(id));
        }

        public IEnumerable<Product> GetProductByCategoryId(int id)
        {
            return _db.Product.Where(p => p.CategoryId.Equals(id)).Include(p => p.Category);
        }
    }
}
