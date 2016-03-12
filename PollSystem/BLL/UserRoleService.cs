using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
     public interface IUserRoleService
    {
       short GetRoleIdByRole(string role);
       string GetRoleById(short roleid);
    }

     public class UserRoleService : IUserRoleService
     {
         IRepository<UserRole> repository;

         public UserRoleService(IRepository<UserRole> repository)
         {
             this.repository = repository;
         }  

         public short GetRoleIdByRole(string role)
         {
             UserRole userrole = repository.Find(x => x.Role.ToLower() == role.ToLower().Trim());
             
             if (userrole != null)
                 return userrole.Id;

             return 0;
         }

         public string GetRoleById(short roleid)
         {
             UserRole userrole = repository.Find(x => x.Id == roleid);

             if (userrole != null)
                 return userrole.Role;

             return string.Empty;
         }
     }
}
