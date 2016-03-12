using DAL;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BLL
{
    public interface IPollService
    {
        List<Poll> GetPolls();
        int InsertPoll(Poll poll);
        int UpdatePoll(Poll poll);
        Poll GetPollById(short id);
        int DeletePoll(short pollid);
        List<Poll> GetUserPoll(short userid);
    }

    public class PollService : IPollService
    {
        IRepository<Poll> repository;

        public PollService(IRepository<Poll> repository)
        {
            this.repository = repository;
        }

        public List<Poll> GetPolls()
        {
            return repository.FindAll(p => p.IsDeleted == false) as List<Poll>;
        }

        public int InsertPoll(Poll poll)
        {
            return repository.Add(poll);
        }

        public int UpdatePoll(Poll poll)
        {
            return repository.Update(poll, poll.Id);
        }

        public Poll GetPollById(short id)
        {
            return repository.Get(id);
        }

        public int DeletePoll(short pollid)
        {
            SqlParameter pollidParam = new SqlParameter("@PollId", pollid);
            object[] parameters = new object[] { pollidParam };
            return repository.ExecuteSqlCommand("exec spDeletePoll @PollId", parameters);
        }

        public List<Poll> GetUserPoll(short userid)
        {
            SqlParameter useridParam = new SqlParameter("@UserId", userid);
            object[] parameters = new object[] { useridParam };
            return repository.ExecuteSqlQuery<Poll>("exec spGetUserPoll @UserId", parameters) as List<Poll>;
        }
    }
}
