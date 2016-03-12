using PollSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Collections;
using BLL;
using DAL;
using Newtonsoft.Json;

namespace PollSystem.Controllers
{
    [Authorize]
    public class ManageController  :  Controller
    {
        IPollService pollservice;
        IPollQuestionService pollquestionservice;
        IPollQuesAnswersService pollquesanswerservice;

        public ManageController(IPollService pollservice, IPollQuestionService pollquestionservice, IPollQuesAnswersService pollquesanswerservice)
        {
            this.pollservice = pollservice;
            this.pollquestionservice = pollquestionservice;
            this.pollquesanswerservice = pollquesanswerservice;
        }

        public ActionResult Index()
        {
            if (Session[Constants.LoginUser] != null)
                return View();

            return RedirectToAction("LogOut", "Account");
        }

        public ActionResult ShowAddPollPopup()
        {
            ViewBag.SurveyPopupHeadline = "Add Survey";
            ViewBag.PollId = "";
            ViewBag.Title = "";
            ViewBag.Description = "";
            return PartialView("~/Views/Shared/_AddSurvey.cshtml");
        }

        public ActionResult ShowEditPollPopup(string pollid, string title, string description)
        {
            try
            {
                ViewBag.SurveyPopupHeadline = "Edit Survey";
                ViewBag.PollId = pollid;
                ViewBag.Title = title;
                ViewBag.Description = description;
            }
            catch (Exception ex) { }

            return PartialView("~/Views/Shared/_AddSurvey.cshtml");
        }

        public ActionResult AddUpdatePoll(FormCollection collecion)
        {
            try
            {
                if (collecion != null)
                {
                    Poll poll = new Poll();
                    poll.Title = collecion["Title"];
                    poll.Description = collecion["Description"];
                    poll.IsDeleted = false;

                    if (collecion["PollId"] != "" && collecion["PollId"] != "0")
                    {
                        poll.Id = Convert.ToInt16(collecion["PollId"]);
                        pollservice.UpdatePoll(poll);
                    }
                    else
                        pollservice.InsertPoll(poll);

                    return Json(true);
                }
            }
            catch (Exception ex) { }

            return Json(false);
        }

        public ActionResult DeletePoll(string pollid)
        {
            try
            {
                if (!string.IsNullOrEmpty(pollid))
                {
                    pollservice.DeletePoll(Convert.ToInt16(pollid));
                    return Json(true, JsonRequestBehavior.AllowGet);
                } 
            }
            catch (Exception ex) { }          

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPollGridData(int page, int rows, string sidx, string sord)
        {
            try
            {
                List<Poll> listPoll = pollservice.GetPolls();

                if (listPoll != null)
                {
                    var pollGridData = new pollGridObject()
                    {
                        Data = listPoll.AsQueryable().OrderBy(sidx + " " + sord).Skip((page - 1) * rows).Take(rows).ToList(),
                        Page = page.ToString(),
                        PageSize = 5,
                        SortColumn = sidx,
                        SortOrder = sord
                    };
                    var sb = JsonConvert.SerializeObject(pollGridData,
                                Formatting.None,
                                new JsonSerializerSettings()
                                {
                                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                                });

                    return Json(JsonConvert.DeserializeObject<pollGridObject>(sb.ToString()), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex) { }

            return null;            
        }

        public ActionResult GetQuestionGridData(int page, int rows, string sidx, string sord, short pollid)
        {
            try
            {
                List<PollQuestion> list = pollquestionservice.GetQuestionsByPollId(pollid);

                if (list != null)
                {
                    var pollQuesGridData = new pollQuesGridObject()
                    {
                        Data = list.AsQueryable().OrderBy(sidx + " " + sord).Skip((page - 1) * rows).Take(rows).ToList(),
                        Page = page.ToString(),
                        PageSize = 5,
                        SortColumn = sidx,
                        SortOrder = sord
                    };

                    var sb = JsonConvert.SerializeObject(pollQuesGridData,
                               Formatting.None,
                               new JsonSerializerSettings()
                               {
                                   ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                               });

                    return Json(JsonConvert.DeserializeObject<pollQuesGridObject>(sb.ToString()), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex) { }

            return null;
        }

        public ActionResult GetAnswerGridData(int page, int rows, string sidx, string sord, short questionid)
        {
            try
            {
                List<PollQuesAnswer> list = pollquesanswerservice.GetAnswersByQuestionId(questionid);

                if (list != null)
                {
                    var pollQuesAnsGridData = new pollQuesAnswerGridObject()
                    {
                        Data = list.AsQueryable().OrderBy(sidx + " " + sord).Skip((page - 1) * rows).Take(rows).ToList(),
                        Page = page.ToString(),
                        PageSize = 5,
                        SortColumn = sidx,
                        SortOrder = sord
                    };

                    var sb = JsonConvert.SerializeObject(pollQuesAnsGridData,
                               Formatting.None,
                               new JsonSerializerSettings()
                               {
                                   ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                               });

                    return Json(JsonConvert.DeserializeObject<pollQuesAnswerGridObject>(sb.ToString()), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex) { }

            return null;
        }

        public ActionResult ShowAddQuestionPopup(string pollid)
        {
            ViewBag.PollId = pollid;
            return PartialView("~/Views/Shared/_AddQuestion.cshtml");
        }

        public ActionResult AddQuestion(FormCollection collecion)
        {
            try
            {
                if (collecion != null)
                {
                    pollquestionservice.InsertQuestionAnswers(Convert.ToInt16(collecion["PollId"]), collecion["Question"], collecion["Option_1"], collecion["Option_2"], collecion["Option_3"], collecion["Option_4"]);
                    return Json(true);
                }
            }
            catch (Exception ex) { }
            return Json(false);
        }

        public ActionResult ShowEditQuestionPopup(string id, string pollid, string question)
        {
            try
            {
                ViewBag.PollId = pollid;
                ViewBag.QuestionId = id;
                ViewBag.Question = question;
            }
            catch (Exception ex) { }
            return PartialView("~/Views/Shared/_EditQuestion.cshtml");
        }

        public ActionResult ShowEditAnswerPopup(string id,string questionid, string answer)
        {
            try
            {
                ViewBag.QuestionId = questionid;
                ViewBag.AnswerId = id;
                ViewBag.Option = answer;
            }
            catch (Exception ex) { }
            return PartialView("~/Views/Shared/_EditAnswer.cshtml");
        }

        public ActionResult UpdateQuestion(FormCollection collecion)
        {
            try
            {
                if (collecion != null)
                {
                    PollQuestion question = new PollQuestion();
                    question.Id = Convert.ToInt16(collecion["QuestionId"]);
                    question.Question = collecion["Question"];
                    question.PollId = Convert.ToInt16(collecion["PollId"]);
                    question.IsDeleted = false;

                    pollquestionservice.UpdateQuestion(question);
                    return Json(true);
                }
            }
            catch (Exception ex) { }
            return Json(false);
        }

        public ActionResult DeleteQuestion(string questionid)
        {
            try
            {
                if (!string.IsNullOrEmpty(questionid))
                {
                    pollquestionservice.DeleteQuestionAnswers(Convert.ToInt16(questionid));
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex) { }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateAnswer(FormCollection collecion)
        {
            try
            {
                if (collecion != null)
                {
                    PollQuesAnswer answer = new PollQuesAnswer();
                    answer.Id = Convert.ToInt16(collecion["AnswerId"]);
                    answer.AnswerText = collecion["Option"];
                    answer.QuestionId = Convert.ToInt16(collecion["QuestionId"]);
                    answer.IsDeleted = false;

                    pollquesanswerservice.UpdateAnswer(answer);
                    return Json(true);
                }
            }
            catch (Exception ex) { }
            return Json(false);
        }

        public ActionResult CheckQuestionLimit(string pollid)
        {
            try
            {
                if (!string.IsNullOrEmpty(pollid))
                {
                    List<PollQuestion> listQuestion = pollquestionservice.GetQuestionsByPollId(Convert.ToInt16(pollid));
                    if (listQuestion != null && listQuestion.Count >= 4)
                        return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex) { }
            return Json(false, JsonRequestBehavior.AllowGet);

        }
    }
}