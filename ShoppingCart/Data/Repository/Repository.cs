using ShoppingCart.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _db;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
        }

        public int Count(Func<T, bool> predicate)
        {
            return _db.Set<T>().Where(predicate).Count();
        }

        public void Create(T entity)
        {
            _db.Add(entity);
            _db.SaveChanges();
        }
        public void CreateRange(IEnumerable<T> entity)
        {
            _db.AddRange(entity);
            _db.SaveChanges();
        }


        public void Delete(T entity)
        {
            _db.Remove(entity);
            _db.SaveChanges();
        }

        public void DeleteRange(IEnumerable<T> entity)
        {
            _db.RemoveRange(entity);
            _db.SaveChanges();
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _db.Set<T>().Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _db.Set<T>();
        }

        public T GetById(int id)
        {
            return _db.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            _db.SaveChanges();
        }
    }
}
