using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{

    public interface IPollQuesAnswersService
    {
        List<PollQuesAnswer> GetAnswersByQuestionId(short questionid);
        PollQuesAnswer GetAnswerById(short answerid);
        int UpdateAnswer(PollQuesAnswer answer);
    }

    public class PollQuesAnswersService : IPollQuesAnswersService
    {
        IRepository<PollQuesAnswer> repository;

        public PollQuesAnswersService(IRepository<PollQuesAnswer> repository)
        {
            this.repository = repository;
        }

        public List<PollQuesAnswer> GetAnswersByQuestionId(short questionid)
        {
            return repository.FindAll(a => a.QuestionId == questionid && a.IsDeleted == false) as List<PollQuesAnswer>;
        }

        public PollQuesAnswer GetAnswerById(short answerid)
        {
            return repository.Get(answerid);
        }

        public int UpdateAnswer(PollQuesAnswer answer)
        {
            return repository.Update(answer, answer.Id);
        }
    }
}
