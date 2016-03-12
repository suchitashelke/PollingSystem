using DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IUserAnswerService
    {
        int InsertUserAnswers(short userid, string answerids);        
    }

    public class UserAnswerService : IUserAnswerService
    {
        IRepository<UserAnswer> repository;

        public UserAnswerService(IRepository<UserAnswer> repository)
        {
            this.repository = repository;
        }

        public int InsertUserAnswers(short userid, string answerids)
        {
            SqlParameter useridParam = new SqlParameter("@UserId", userid); 
            SqlParameter answersParam = new SqlParameter("@AnswerIds", answerids);
            object[] parameters = new object[] { useridParam, answersParam };
            return repository.ExecuteSqlCommand("exec spInsertUserAnswers @UserId,@AnswerIds ", parameters);
        }
    }
}
