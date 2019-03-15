using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetAllWithCategory();
        Product GetWithCategoryById(int id);
        IEnumerable<Product> GetProductByCategoryId(int id);
    }
}
