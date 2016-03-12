
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PollSystem.Models
{
    public class pollGridObject
    {
        public string Page { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public List<Poll> Data { get; set; }
    }

    public class pollQuesGridObject
    {
        public string Page { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public List<PollQuestion> Data { get; set; }
    }

    public class pollQuesAnswerGridObject
    {
        public string Page { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public List<PollQuesAnswer> Data { get; set; }
    }
}