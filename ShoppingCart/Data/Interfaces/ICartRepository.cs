﻿using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        IEnumerable<Cart> GetCartsByUserId(string id);
    }
}
