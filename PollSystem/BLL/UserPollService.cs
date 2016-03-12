using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{        
    public interface IUserPollService
    {
        List<UserPoll> GetPollIdsByUserId(short userid);
        int Insert(short userid, short pollid);
    }

    public class UserPollService : IUserPollService
    {
        IRepository<UserPoll> repository;

        public UserPollService(IRepository<UserPoll> repository)
        {
            this.repository = repository;
        }

        public List<UserPoll> GetPollIdsByUserId(short userid)
        {
            return repository.FindAll(up => up.UserId == userid) as List<UserPoll>;
        }

        public int Insert(short userid, short pollid)
        {
            UserPoll userpoll = new UserPoll();
            userpoll.UserId = userid;
            userpoll.PollId = pollid;

            return repository.Add(userpoll);
        }
    }
}
