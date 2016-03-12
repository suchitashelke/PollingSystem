using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public partial class PollSystemEntities : DbContext, IPollSystemEntities
    {
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public int ExecuteSqlCommand(string sql, object[] parameters)
        {
            return Database.ExecuteSqlCommand(sql, parameters);
        }

        public IList<TEntity> ExecuteSqlQuery<TEntity>(string sql, object[] parameters)
        {
            return Database.SqlQuery<TEntity>(sql, parameters).ToList<TEntity>();
        }
    }
}
