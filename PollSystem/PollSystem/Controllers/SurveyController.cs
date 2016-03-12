using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PollSystem.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        IPollService pollservice;
        IPollQuestionService pollquestionservice;
        IUserAnswerService useranswerservice;
        IUserPollService userpollservice;

        public SurveyController(IPollService pollservice, IPollQuestionService pollquestionservice, IUserAnswerService useranswerservice, IUserPollService userpollservice)
        {
            this.pollservice = pollservice;
            this.pollquestionservice = pollquestionservice;
            this.useranswerservice = useranswerservice;
            this.userpollservice = userpollservice;
        }
       
        public ActionResult Index()
        {
            try
            {
                if (Session[Constants.LoginUser] != null)
                {
                    User user = (User)Session[Constants.LoginUser];
                    ViewBag.PollList = GetPollList(user.Id);
                    return View();
                }
            }
            catch (Exception ex) { }

            return RedirectToAction("LogOut", "Account");
        }

        private List<SelectListItem> GetPollList(short userid)
        {
            try
            {
                List<Poll> listPoll = pollservice.GetUserPoll(userid);

                List<SelectListItem> ddlPollList = listPoll
                                                    .Select(item => new SelectListItem
                                                    {
                                                        Value = item.Id.ToString(),
                                                        Text = item.Title,
                                                        Selected = "-1" == item.Id.ToString() ? true : false
                                                    })
                                                    .ToList();

                return ddlPollList;
            }
            catch (Exception ex) { }
            return null;
        }

        public ActionResult GetQuestion(string pollid)
        {
            try
            {
                if (!string.IsNullOrEmpty(pollid))
                {
                    List<PollQuestion> listQuestion = pollquestionservice.GetQuestionsByPollId(Convert.ToInt16(pollid));
                    ViewBag.listPollQuestion = listQuestion;
                    return PartialView("~/Views/Shared/_SurveyQuestion.cshtml");
                }
            }
            catch (Exception ex) { }

            return null;
            
        }

        public ActionResult submitSurvey(FormCollection collection)
        {
            try
            {
                if (collection != null)
                {
                    User user = (User)Session[Constants.LoginUser];
                    StringBuilder sb = new StringBuilder();

                    foreach (var key in collection.Keys)
                    {
                        if (key.ToString().ToLower().StartsWith("ansfor"))
                            sb.Append(collection[key.ToString()] + ",");
                    }
                    userpollservice.Insert(user.Id, Convert.ToInt16(collection["PollId"]));
                    useranswerservice.InsertUserAnswers(user.Id, sb.ToString());
                    return Json(true);
                }
            }
            catch (Exception ex) { }

            return Json(false);            
        }

    }
}