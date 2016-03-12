using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public interface IUserService
    {
        User GetUserByEmailPassword(string emailid, string password);
        bool CheckEmailIdExists(string emailid);
        User GetUserDetailsByEmailId(string emailid);
        int InsertUser(User user);        
    }

    public class UserService : IUserService
    {
        IRepository<User> repository;

        public UserService(IRepository<User> repository)
        {
            this.repository = repository;
        }

        public User GetUserByEmailPassword(string emailid, string password)
        {
            return repository.Find(u => u.EmailId == emailid.Trim() && u.Password == password.Trim() && u.IsDeleted == false);
        }

        public bool CheckEmailIdExists(string emailid)
        {
            User user = repository.Find(u => u.EmailId == emailid.Trim() && u.IsDeleted == false);
            return user != null ? true : false;
        }

        public User GetUserDetailsByEmailId(string emailid)
        {
            return repository.Find(u => u.EmailId == emailid.Trim() && u.IsDeleted == false);
        }

        public int InsertUser(User user)
        {
            return repository.Add(user);
        }
    }
}
