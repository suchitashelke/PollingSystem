using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetAll();
        T Get(int id);
        T Find(Expression<Func<T, bool>> match);
        IList<T> FindAll(Expression<Func<T, bool>> match);
        int Add(T t);
        int Update(T updated, int key);
        void Delete(T t);
        int Count();
        T Insert(T t);
        int MultipleInsert(ICollection<T> t);
        int ExecuteSqlCommand(string sql, object[] parameters);
        IList<T> ExecuteSqlQuery<T>(string sql, object[] parameters);
    }
}
