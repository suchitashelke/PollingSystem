using DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IPollQuestionService
    {
        List<PollQuestion> GetQuestionsByPollId(short pollid);
        PollQuestion GetQuestionById(short questionid);
        int UpdateQuestion(PollQuestion question);
        int InsertQuestionAnswers(short pollid, string question, string option1, string option2, string option3, string option4);
        int DeleteQuestionAnswers(short questionid);
    }

    public class PollQuestionService : IPollQuestionService
    {
        IRepository<PollQuestion> repository;

        public PollQuestionService(IRepository<PollQuestion> repository)
        {
            this.repository = repository;
        }

        public List<PollQuestion> GetQuestionsByPollId(short pollid)
        {
            return repository.FindAll(q => q.PollId == pollid && q.IsDeleted == false) as List<PollQuestion>;
        }

        public PollQuestion GetQuestionById(short questionid)
        {
            return repository.Get(questionid);
        }

        public int UpdateQuestion(PollQuestion question)
        {
            return repository.Update(question, question.Id);
        }

        public int InsertQuestionAnswers(short pollid, string question, string option1, string option2, string option3, string option4)
        {
            SqlParameter pollidParam = new SqlParameter("@PollId", pollid);
            SqlParameter questionParam = new SqlParameter("@Question", question);
            SqlParameter option1Param = new SqlParameter("@Option1", option1);
            SqlParameter option2Param = new SqlParameter("@Option2", option2);
            SqlParameter option3Param = new SqlParameter("@Option3", option3);
            SqlParameter option4Param = new SqlParameter("@Option4", option4);
            object[] parameters = new object[] { pollidParam, questionParam, option1Param, option2Param, option3Param, option4Param };
            return repository.ExecuteSqlCommand("exec spInsertQuestionAnswers @PollId, @Question, @Option1,@Option2, @Option3, @Option4", parameters); 
        }

        public int DeleteQuestionAnswers(short questionid)
        {
            SqlParameter questionParam = new SqlParameter("@QuestionId", questionid);
            object[] parameters = new object[] { questionParam };
            return repository.ExecuteSqlCommand("exec spDeleteQuestionAnswers @QuestionId", parameters);
        }
    }
}
