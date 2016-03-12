using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IPollSystemEntities
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        DbEntityEntry Entry(object o);
        int ExecuteSqlCommand(string sql, object[] parameters);
        IList<TEntity> ExecuteSqlQuery<TEntity>(string sql, object[] parameters);
        void Dispose();
    }
}
