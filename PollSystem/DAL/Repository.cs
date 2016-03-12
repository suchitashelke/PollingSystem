using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Repository<T> : IRepository<T>, IDisposable where T : class
    {
        protected IPollSystemEntities _context;
        private bool isDisposed;

        public Repository(IPollSystemEntities context)
        {
            _context = context;
        }

        public IList<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }      

        public T Find(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().SingleOrDefault(match);
        }

        public IList<T> FindAll(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().Where(match).ToList();
        }
        
        public int Add(T t)
        {
            _context.Set<T>().Add(t);
            return _context.SaveChanges();
        }

        public int Update(T updated, int key)
        {
            if (updated != null)
            {
                T existing = _context.Set<T>().Find(key);
                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(updated);
                    return _context.SaveChanges();
                }
            }

            return 0;
        }        

        public void Delete(T t)
        {
            _context.Set<T>().Remove(t);
            _context.SaveChanges();
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public T Insert(T t)
        {
            _context.Set<T>().Add(t);
            _context.SaveChanges();
            return t;
        }

        public int MultipleInsert(ICollection<T> t)
        {
            foreach (T t1 in t)
            {
                _context.Set<T>().Add(t1);
            }
           
            return _context.SaveChanges();
        }

        public int ExecuteSqlCommand(string sql, object[] parameters)
        {
            return _context.ExecuteSqlCommand(sql, parameters);
        }

        public IList<T> ExecuteSqlQuery<T>(string sql, object[] parameters)
        {
            return _context.ExecuteSqlQuery<T>(sql, parameters);
        }

        public virtual void Dispose(bool isManuallyDisposing)
        {
            if (!isDisposed)
            {
                if (isManuallyDisposing)
                    _context.Dispose();
            }

            isDisposed = true;
        }

        public void Dispose()
        {
            if (this._context != null)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }           
        }

        ~Repository()
        {
            Dispose(false);
        }
    }
}
