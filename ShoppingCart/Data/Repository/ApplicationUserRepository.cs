using ShoppingCart.Data.Interfaces;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
        }

        public ApplicationUser GetApplicationUserByEmail(string email)
        {
            return _db.AppplicationUser.Where(a => a.Email.Equals(email)).FirstOrDefault();
        }
        public ApplicationUser GetApplicationUserById(string id)
        {
            return _db.AppplicationUser.Where(a => a.Id.Equals(id)).FirstOrDefault();
        }

    }
}
