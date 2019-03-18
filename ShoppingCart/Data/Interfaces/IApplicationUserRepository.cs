using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Interfaces
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        ApplicationUser GetApplicationUserByEmail(string email);
    }
}
